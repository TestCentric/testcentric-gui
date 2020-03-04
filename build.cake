#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.3.1"

#load "./build/parameters.cake"
#load "./build/helpers.cake"

using System.Xml;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////

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

BuildParameters Parameters;

Setup<BuildParameters>((context) =>
{
	var parameters = new BuildParameters(context);


	if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(parameters.PackageVersion + "-" + AppVeyor.Environment.Build.Number);

    Information("Building {0} version {1} of TestCentric GUI.", parameters.Configuration, parameters.PackageVersion);

	return parameters;
});

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

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
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
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

//////////////////////////////////////////////////////////////////////
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
// PACKAGE ZIP
//////////////////////////////////////////////////////////////////////

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does<BuildParameters>((parameters) =>
    {
		Information("Creating package " + parameters.ZipPackage);

        var zipFiles = GetFiles(parameters.ImageDirectory + "**/*.*");
        Zip(parameters.ImageDirectory, parameters.ZipPackage, zipFiles);
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE FOR NUGET.ORG
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.IsDependentOn("CreateImage")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Creating package " + parameters.NuGetPackage);

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

//////////////////////////////////////////////////////////////////////
// PACKAGE FOR CHOCOLATEY
//////////////////////////////////////////////////////////////////////

Task("PackageChocolatey")
    .IsDependentOn("CreateImage")
    .Does<BuildParameters>((parameters) =>
    {
		Information("Creating package " + parameters.ChocolateyPackage);

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

//////////////////////////////////////////////////////////////////////
// ZIP PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestZipPackage")
	.IsDependentOn("PackageZip")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.ZipPackage);

		CleanDirectory(parameters.ZipTestDirectory);

		Unzip(parameters.ZipPackage, parameters.ZipTestDirectory);
		CopyTestFiles(parameters.OutputDirectory, parameters.ZipTestDirectory + "bin/");

		NUnit3(parameters.ZipTestDirectory + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestNuGetPackage")
	.IsDependentOn("PackageNuGet")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.NuGetPackage);

		CleanDirectory(parameters.NuGetTestDirectory);
		Unzip(parameters.NuGetPackage, parameters.NuGetTestDirectory);

		CheckNuGetContent(parameters.NuGetTestDirectory);

		CopyTestFiles(parameters.OutputDirectory, parameters.NuGetTestDirectory + "tools/");

		NUnit3(parameters.NuGetTestDirectory + "tools/" + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestChocolateyPackage")
	.IsDependentOn("PackageChocolatey")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Testing package " + parameters.ChocolateyPackage);

		CleanDirectory(parameters.ChocolateyTestDirectory);

		Unzip(parameters.ChocolateyPackage, parameters.ChocolateyTestDirectory);
		CopyTestFiles(parameters.OutputDirectory, parameters.ChocolateyTestDirectory + "tools/");

		NUnit3(parameters.ChocolateyTestDirectory + "tools/" + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL COMMANDS FOR PUBLISHING PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PublishToMyGet")
	.Does<BuildParameters>((parameters) =>
	{
		PublishToMyGet(parameters.NuGetPackage);
		PublishToMyGet(parameters.ChocolateyPackage);
	});

Task("PublishToNuGet")
	.Does<BuildParameters>((parameters) =>
	{
		PublishToNuGet(parameters.NuGetPackage);
	});

Task("PublishToChocolatey")
	.Does<BuildParameters>((parameters) =>
	{
		PublishToChocolatey(parameters.ChocolateyPackage);
	});

//////////////////////////////////////////////////////////////////////
// INTERACTIVE TESTS FOR USE IN DEVELOPMENT
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// GUI TEST
//////////////////////////////////////////////////////////////////////

Task("GuiTest")
    .IsDependentOn("Build")
    .Does<BuildParameters>((parameters) =>
{
		StartProcess(
		  parameters.OutputDirectory + GUI_RUNNER, 
		  parameters.OutputDirectory + GUI_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL GUI TEST
//////////////////////////////////////////////////////////////////////

Task("ExperimentalGuiTest")
    .IsDependentOn("Build")
    .Does<BuildParameters>((parameters) =>
{
		StartProcess(
		  parameters.OutputDirectory + EXPERIMENTAL_RUNNER, 
		  parameters.OutputDirectory + EXPERIMENTAL_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// ZIP GUI TEST (USES ZIP PACKAGE)
//////////////////////////////////////////////////////////////////////

Task("ZipGuiTest")
	.IsDependentOn("PackageZip")
	.Does<BuildParameters>((parameters) =>
	{
		var testDir = parameters.ZipTestDirectory;
		CleanDirectory(testDir);

		Unzip(parameters.ZipPackage, testDir);
		CopyTestFiles(parameters.OutputDirectory, testDir + "tools/");

		StartProcess(
		  testDir + GUI_RUNNER, 
		  testDir + GUI_TESTS + " --run");

		CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// ZIP EXPERIMENTAL GUI TEST (USES ZIP PACKAGE)
//////////////////////////////////////////////////////////////////////

Task("ZipExperimentalGuiTest")
	.IsDependentOn("PackageZip")
	.Does<BuildParameters>((parameters) =>
	{
		CleanDirectory(parameters.ZipTestDirectory);

		Unzip(parameters.NuGetPackage, parameters.ZipTestDirectory);
		CopyTestFiles(parameters.OutputDirectory, parameters.ZipTestDirectory + "tools/");

		StartProcess(
		  parameters.ZipTestDirectory + EXPERIMENTAL_RUNNER,
		  parameters.ZipTestDirectory + EXPERIMENTAL_TESTS + " --run");

		CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY INSTALL (MUST RUN AS ADMIN)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyInstall")
	.Does<BuildParameters>((parameters) =>
	{
		if (StartProcess("choco", $"install -f -y -s {parameters.PackageDirectory} {PACKAGE_NAME}") != 0)
			throw new Exception("Failed to install package. Must run this command as administrator.");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY TEST (AFTER INSTALL)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyTest")
	.IsDependentOn("PackageChocolatey")
	.Does<BuildParameters>((parameters) =>
	{
		var testDir = parameters.ChocolateyTestDirectory;
		CleanDirectory(testDir);

		Unzip(parameters.ChocolateyPackage, testDir);
		CopyTestFiles(parameters.OutputDirectory, testDir + "tools/");

		// TODO: When starting the commands that chocolatey has shimmed, the StartProcess
		// call returns immediately, so we can't check the test result. For now, we just
		// run the tests and inspect manually but we need to figure out how to wait for
		// the process to complete.
		StartProcess(GUI_RUNNER, testDir + GUI_TESTS + " --run");
		StartProcess(EXPERIMENTAL_RUNNER, testDir + EXPERIMENTAL_TESTS + " --run");
		//CheckTestResult("TestResult.xml");
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

Task("TestPackages")
	.IsDependentOn("TestZipPackage")
	.IsDependentOn("TestNuGetPackage")
	.IsDependentOn("TestChocolateyPackage");

Task("Appveyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
	.IsDependentOn("TestPackages");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("PackageZip")
	.IsDependentOn("TestZipPackage");

Task("All")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
	.IsDependentOn("TestPackages");

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
