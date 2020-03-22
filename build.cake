#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.3.1"

#load "./build/parameters.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS (In addition to the standard Cake arguments)
//
// --asVersion=VERSION
//     Specifies the full package version, incliding any pre-release
//     suffix. This version is used directly instead of the default
//     version from the script or that calculated by GitVersion.
//     Note that all other versions (AssemblyVersion, etc.) are
//     derived from the package version.
//
// NOTE: Cake syntax requires the `=` character. Neither a space
//       nor a colon will work!
//////////////////////////////////////////////////////////////////////

using System.Xml;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////

// NOTE: Since GitVersion is only used when running under
// Windows, the default version should be updated to the 
// next version after each release.
const string DEFAULT_VERSION = "1.3.3";
const string DEFAULT_CONFIGURATION = "Release";

const string SOLUTION = "testcentric-gui.sln";

const string PACKAGE_NAME = "testcentric-gui";
const string NUGET_PACKAGE_NAME = "TestCentric.GuiRunner";

const string GUI_RUNNER = "testcentric.exe";
const string GUI_TESTS = "TestCentric.Gui.Tests.dll";
const string EXPERIMENTAL_RUNNER = "tc-next.exe";
const string EXPERIMENTAL_TESTS = "Experimental.Gui.Tests.dll";
const string MODEL_TESTS = "TestCentric.Gui.Model.Tests.dll";
const string ALL_TESTS = "*.Tests.dll";

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN
//////////////////////////////////////////////////////////////////////

Setup<BuildParameters>((context) =>
{
	var parameters = BuildParameters.Create(context);


	if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(parameters.PackageVersion + "-" + AppVeyor.Environment.Build.Number);

    Information("Building {0} version {1} of TestCentric GUI.", parameters.Configuration, parameters.PackageVersion);

	return parameters;
});

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// DUMP SETTINGS
//////////////////////////////////////////////////////////////////////

Task("DumpSettings")
	.Does<BuildParameters>((parameters) =>
{
	parameters.DumpSettings();
});

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildParameters>((parameters) =>
{
    CleanDirectory(parameters.OutputDirectory);
});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does<BuildParameters>((parameters) =>
{
    NuGetRestore(SOLUTION, parameters.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// UPDATE ASSEMBLYINFO
//////////////////////////////////////////////////////////////////////

Task("UpdateAssemblyInfo")
	.Does<BuildParameters>((parameters) =>
{
	var major = new Version(parameters.AssemblyVersion).Major;
	parameters.Versions.PatchAssemblyInfo("src/CommonAssemblyInfo.cs");
    parameters.Versions.PatchAssemblyInfo("src/TestEngine/CommonEngineAssemblyInfo.cs");
    parameters.Versions.PatchAssemblyInfo("src/TestEngine/testcentric.engine.api/Properties/AssemblyInfo.cs", major + ".0.0.0");
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
	.IsDependentOn("UpdateAssemblyInfo")
    .Does<BuildParameters>((parameters) =>
{
    if(parameters.UsingXBuild)
        XBuild(SOLUTION, parameters.XBuildSettings);
    else
        MSBuild(SOLUTION, parameters.MSBuildSettings);
});

//////////////////////////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////////////////////////

var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE
//////////////////////////////////////////////////////////////////////

Task("TestEngine")
	.Description("Tests the TestCentric Engine")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.SupportedEngineRuntimes)
			RunNUnitLite("testcentric.engine.tests", runtime, $"{parameters.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.SupportedCoreRuntimes)
			RunNUnitLite("testcentric.engine.core.tests", runtime, $"{parameters.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF THE GUI
//////////////////////////////////////////////////////////////////////

Task("TestGui")
    .IsDependentOn("Build")
    .Does<BuildParameters>((parameters) =>
	{
		NUnit3(
			parameters.OutputDirectory + ALL_TESTS,
			new NUnit3Settings { NoResults = true }
		);
	});

/////////////////////////////////////////////////////////////////////
// PACKAGING
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// CREATE PACKAGE IMAGE
//////////////////////////////////////////////////////////////////////

Task("CreateImage")
	.IsDependentOn("Build")
    .Description("Copies all files into the image directory")
    .Does<BuildParameters>((parameters) =>
    {
        CreateDirectory(parameters.PackageDirectory);

		CreateImage(parameters);
    });

//////////////////////////////////////////////////////////////////////
// ZIP PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does<BuildParameters>((parameters) =>
    {
		Information("Creating package " + parameters.ZipPackageName);

        var zipFiles = GetFiles(parameters.ImageDirectory + "**/*.*");
        Zip(parameters.ImageDirectory, parameters.ZipPackage, zipFiles);
    });

Task("CreateZipTestDirectory")
	.IsDependentOn("PackageZip")
	.Does<BuildParameters>((parameters) =>
	{
		CleanDirectory(parameters.ZipTestDirectory);
		Unzip(parameters.ZipPackage, parameters.ZipTestDirectory);
	});

Task("CheckZipPackage")
	.IsDependentOn("CreateZipTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Checking package " + parameters.ZipPackageName);

		var checker = new PackageChecker(parameters.ZipPackageName, parameters.ZipTestDirectory);

		if (!checker.RunChecks(
			HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
			HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES),
			HasDirectory("bin/agents/net20").WithFiles(AGENT_FILES),
			HasDirectory("bin/agents/net40").WithFiles(AGENT_FILES),
			HasDirectory("bin/Images").WithFiles("DebugTests.png", "RunTests.png"),
			HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
			HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
			HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
			HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)))
		{
			ErrorDetail.Add($"Package check failed for {parameters.ZipPackageName}");
		}
	});

Task("TestZipPackage")
	.IsDependentOn("CreateZipTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.ZipPackageName);

		RunGuiAndReportResults(parameters.ZipTestDirectory + "bin/" + GUI_RUNNER, MODEL_TESTS, parameters.OutputDirectory);
	});

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.IsDependentOn("CreateImage")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Creating package " + parameters.NuGetPackageName);

        var content = new List<NuSpecContent>();
		int index = parameters.ImageDirectory.Length;

		foreach (var file in GetFiles(parameters.ImageDirectory + "**/*.*"))
		{
			var source = file.FullPath;
			var target = System.IO.Path.GetDirectoryName(source.Substring(index));

			if (target == "bin")
				target = "tools";
			else if (target.StartsWith("bin" + System.IO.Path.DirectorySeparatorChar))
				target = "tools" + target.Substring(3);

			if (target.IndexOf("Visual") != -1)
				Console.WriteLine($"Adding Source = {file.FullPath}\nTarget = {target}");

			content.Add(new NuSpecContent() { Source = file.FullPath, Target = target });
		}

		// Icon goes in the root
		content.Add(new NuSpecContent() { Source = "../testcentric.png" });

		// Use addins file tailored for nuget install
		content.Add(new NuSpecContent() { Source = "testcentric-gui.addins", Target = "tools" });
		foreach (string runtime in parameters.SupportedAgentRuntimes)
			content.Add(new NuSpecContent() {Source = "testcentric-agent.addins", Target = $"tools/agents/{runtime}" }); 

        NuGetPack($"{parameters.NuGetDirectory}/{NUGET_PACKAGE_NAME}.nuspec", new NuGetPackSettings()
        {
            Version = parameters.PackageVersion,
            OutputDirectory = parameters.PackageDirectory,
            NoPackageAnalysis = true,
			Files = content
        });
	});

Task("CreateNuGetTestDirectory")
	.IsDependentOn("PackageNuGet")
	.Does<BuildParameters>((parameters) =>
	{
		CleanDirectory(parameters.NuGetTestDirectory);
		Unzip(parameters.NuGetPackage, parameters.NuGetTestDirectory);
	});

Task("CheckNuGetPackage")
	.IsDependentOn("CreateNuGetTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Checking package " + parameters.NuGetPackageName);

		CleanDirectory(parameters.NuGetTestDirectory);
		Unzip(parameters.NuGetPackage, parameters.NuGetTestDirectory);

		var checker = new PackageChecker(parameters.NuGetPackageName, parameters.NuGetTestDirectory);

		if (!checker.RunChecks(
			HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
			HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFiles("testcentric-gui.addins"),
			HasDirectory("tools/agents/net20").WithFiles(AGENT_FILES),
			HasDirectory("tools/agents/net40").WithFiles(AGENT_FILES),
			HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png"),
			HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
			HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
			HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
			HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)))
		{
			ErrorDetail.Add($"Package check failed for {parameters.NuGetPackageName}");
		}
	});

Task("TestNuGetPackage")
	.IsDependentOn("CreateNuGetTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.NuGetPackageName);

		RunGuiAndReportResults(parameters.NuGetTestDirectory + "tools/" + GUI_RUNNER, MODEL_TESTS, parameters.OutputDirectory);
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageChocolatey")
    .IsDependentOn("CreateImage")
    .Does<BuildParameters>((parameters) =>
    {
		Information("Creating package " + parameters.ChocolateyPackageName);

        var content = new List<ChocolateyNuSpecContent>();
		int index = parameters.ImageDirectory.Length;

		foreach (var file in GetFiles(parameters.ImageDirectory + "**/*.*"))
		{
			var source = file.FullPath;
			var target = System.IO.Path.GetDirectoryName(file.FullPath.Substring(index));

			if (target == "" || target == "bin")
				target = "tools";
			else if (target.StartsWith("bin" + System.IO.Path.DirectorySeparatorChar))
				target = "tools" + target.Substring(3);

			if (target.IndexOf("Visual") != -1)
				Console.WriteLine($"Adding Source = {file.FullPath}\nTarget = {target}");

			content.Add(new ChocolateyNuSpecContent() { Source = file.FullPath, Target = target });
		}

		content.AddRange(new ChocolateyNuSpecContent[]
		{
			new ChocolateyNuSpecContent() { Source = "VERIFICATION.txt", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = "testcentric-agent.exe.ignore", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = "testcentric-agent-x86.exe.ignore", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = "testcentric.choco.addins", Target = "tools" }
		});
			
		ChocolateyPack($"{parameters.ChocoDirectory}/{PACKAGE_NAME}.nuspec", 
            new ChocolateyPackSettings()
            {
                Version = parameters.PackageVersion,
                OutputDirectory = parameters.PackageDirectory,
                Files = content
            });
    });

Task("CreateChocolateyTestDirectory")
	.IsDependentOn("PackageChocolatey")
	.Does<BuildParameters>((parameters) =>
	{
		CleanDirectory(parameters.ChocolateyTestDirectory);
		Unzip(parameters.ChocolateyPackage, parameters.ChocolateyTestDirectory);
	});

Task("CheckChocolateyPackage")
	.IsDependentOn("CreateChocolateyTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Checking package " + parameters.ChocolateyPackageName);

		CleanDirectory(parameters.ChocolateyTestDirectory);
		Unzip(parameters.ChocolateyPackage, parameters.ChocolateyTestDirectory);

		var checker = new PackageChecker(parameters.ChocolateyPackageName, parameters.ChocolateyTestDirectory);

		if (!checker.RunChecks(
			HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES),
			HasDirectory("tools/agents/net20").WithFiles(AGENT_FILES),
			HasDirectory("tools/agents/net40").WithFiles(AGENT_FILES),
			HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png"),
			HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
			HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
			HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
			HasDirectory("tools/Images/Tree/Visual%20Studio").WithFiles(TREE_ICONS_PNG)))
		{
			ErrorDetail.Add($"Package check failed for {parameters.ChocolateyPackageName}");
		}
	});

Task("TestChocolateyPackage")
	.IsDependentOn("CreateChocolateyTestDirectory")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.ChocolateyPackageName);

		RunGuiAndReportResults(parameters.ChocolateyTestDirectory + "tools/" + GUI_RUNNER, MODEL_TESTS, parameters.OutputDirectory);
	});

//////////////////////////////////////////////////////////////////////
// PUBLISH PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PublishPackages")
	.Description("Publish nuget and chocolatey packages according to the current settings")
	//.IsDependentOn("TestPackages")
	.WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublishPackages)
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.ShouldPublishToMyGet)
		{
			PushNuGetPackage(parameters.NuGetPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			PushChocolateyPackage(parameters.ChocolateyPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
		}

		if (parameters.ShouldPublishToNuGet)
		{
			PushNuGetPackage(parameters.NuGetPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
		}

		if (parameters.ShouldPublishToChocolatey)
		{
			PushChocolateyPackage(parameters.ChocolateyPackage, parameters.ChocolateyApiKey, parameters.ChocolateyPushUrl);
		}
	});

//////////////////////////////////////////////////////////////////////
// INTERACTIVE TESTS FOR USE IN DEVELOPMENT
//////////////////////////////////////////////////////////////////////

Task("MustBeLocalBuild")
	.Description("Throw an exception if this is not a local build")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.IsLocalBuild)
			throw new InvalidOperationException($"The {parameters.Target} task is interactive and may only be run locally.");
	});

//////////////////////////////////////////////////////////////////////
// RUN THE STANDARD GUI
//////////////////////////////////////////////////////////////////////

Task("RunTestCentric")
    .IsDependentOn("MustBeLocalBuild")
    .Does<BuildParameters>((parameters) =>
	{
		StartProcess(parameters.OutputDirectory + GUI_RUNNER);
	});

//////////////////////////////////////////////////////////////////////
// RUN THE EXPERIMENTAL GUI
//////////////////////////////////////////////////////////////////////

Task("RunExperimental")
    .IsDependentOn("MustBeLocalBuild")
    .Does<BuildParameters>((parameters) =>
	{
		StartProcess(parameters.OutputDirectory + EXPERIMENTAL_RUNNER);
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY INSTALL (MUST RUN AS ADMIN)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyInstall")
	.Does<BuildParameters>((parameters) =>
	{
		if (StartProcess("choco", $"install -f -y -s {parameters.PackageDirectory} {PACKAGE_NAME}") != 0)
			throw new InvalidOperationException("Failed to install package. Must run this command as administrator.");
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Test")
	.IsDependentOn("TestEngineCore")
	.IsDependentOn("TestEngine")
	.IsDependentOn("TestGui");

Task("Package")
	.IsDependentOn("CheckTestErrors")
    .IsDependentOn("PackageZip")
	.IsDependentOn("PackageNuget")
    .IsDependentOn("PackageChocolatey");

Task("CheckPackages")
	.IsDependentOn("Package")
	.IsDependentOn("CheckZipPackage")
	.IsDependentOn("CheckNuGetPackage")
	.IsDependentOn("CheckChocolateyPackage");

Task("TestPackages")
	.IsDependentOn("CheckPackages")
	.IsDependentOn("TestZipPackage")
	.IsDependentOn("TestNuGetPackage")
	.IsDependentOn("TestChocolateyPackage");

Task("AppVeyor")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
	.IsDependentOn("CheckPackages")
	.IsDependentOn("TestPackages")
	.IsDependentOn("PublishPackages");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("All")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
	.IsDependentOn("CheckPackages")
	.IsDependentOn("TestPackages")
	.IsDependentOn("PublishPackages");

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
