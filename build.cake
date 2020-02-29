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
// BUILD PARAMETERS
//////////////////////////////////////////////////////////////////////

var Parameters = new BuildParameters(Context);

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN
//////////////////////////////////////////////////////////////////////

Setup(context =>
{
	// TODO: Make GitVersion work on Linux
	if (IsRunningOnWindows())
	{
		var gitVersion = GitVersion();

		Information("GitVersion Properties:");

		string branchName = gitVersion.BranchName;
		// We don't currently use this pattern, but check in case we do later.
		if (branchName.StartsWith ("feature/"))
			branchName = branchName.Substring(8);

		// Default based on GitVersion.yml. This gives us a tag of dev
		// for master, ci for features, pr for pull requests and rc
		// for release branches.
		Parameters.PackageVersion = gitVersion.LegacySemVerPadded;

		// Full release versions and PRs need no further handling
		int dash = Parameters.PackageVersion.IndexOf('-');
		bool isPreRelease = dash > 0;

		string label = gitVersion.PreReleaseLabel;
		bool isPR = label == "pr"; // Set in our GitVersion.yml

		if (isPreRelease && !isPR)
		{
			// This handles non-standard branch names.
			if (label == branchName)
				label = "ci";

			string suffix = "-" + label + gitVersion.CommitsSinceVersionSourcePadded;

			if (label == "ci")
			{
				branchName = Regex.Replace(branchName, "[^0-9A-Za-z-]+", "-");
				suffix += "-" + branchName;
			}

			// Nuget limits "special version part" to 20 chars. Add one for the hyphen.
			if (suffix.Length > 21)
				suffix = suffix.Substring(0, 21);

			Parameters.PackageVersion = gitVersion.MajorMinorPatch + suffix;
		}

		if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(Parameters.PackageVersion + "-" + AppVeyor.Environment.Build.Number);
	}

    Information("Building {0} version {1} of TestCentric GUI.", Parameters.Configuration, Parameters.PackageVersion);
});

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(Parameters.OutputDirectory);
});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does(() =>
{
    NuGetRestore(SOLUTION, Parameters.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    if(Parameters.UsingXBuild)
    {
        XBuild(SOLUTION, Parameters.XBuildSettings);
    }
    else
    {
        MSBuild(SOLUTION, Parameters.MSBuildSettings);
    }
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

var testEngineTask = Task("TestEngine")
	.Description("Tests the TestCentric Engine");

foreach (var runtime in Parameters.SupportedEngineRuntimes)
{
	var task = Task("TestEngine_" + runtime)
		.Description("Tests the Engine on " + runtime)
		.IsDependentOn("Build")
		.OnError(exception => { ErrorDetail.Add(exception.Message); })
		.Does(() =>
		{
			RunNUnitLite("testcentric.engine.tests", runtime, $"{Parameters.OutputDirectory}engine-tests/{runtime}/");
		});

	testEngineTask.IsDependentOn(task);	
}

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

var testEngineCoreTask = Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core");

foreach (var runtime in Parameters.SupportedCoreRuntimes)
{
	var task = Task("TestEngineCore_" + runtime)
		.Description("Tests the Engine Core on " + runtime)
		.IsDependentOn("Build")
		.OnError(exception => { ErrorDetail.Add(exception.Message); })
		.Does(() =>
		{
			RunNUnitLite("testcentric.engine.core.tests", runtime, $"{Parameters.OutputDirectory}engine-tests/{runtime}/");
		});

	testEngineCoreTask.IsDependentOn(task);	
}

//////////////////////////////////////////////////////////////////////
// TESTS OF THE GUI
//////////////////////////////////////////////////////////////////////

Task("TestGui")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(
	    Parameters.OutputDirectory + ALL_TESTS,
	    new NUnit3Settings { NoResults = true }
	);
});

//////////////////////////////////////////////////////////////////////
// PACKAGING
//////////////////////////////////////////////////////////////////////

var RootFiles = new string[]
{
    "LICENSE.txt",
    "NOTICES.txt",
    "CHANGES.txt"
};

var baseFiles = new string[]
{
    Parameters.OutputDirectory + "testcentric.exe",
    Parameters.OutputDirectory + "testcentric.exe.config",
    Parameters.OutputDirectory + "tc-next.exe",
    Parameters.OutputDirectory + "tc-next.exe.config",
    Parameters.OutputDirectory + "TestCentric.Common.dll",
    Parameters.OutputDirectory + "TestCentric.Gui.Components.dll",
    Parameters.OutputDirectory + "TestCentric.Gui.Runner.dll",
    Parameters.OutputDirectory + "Experimental.Gui.Runner.dll",
    Parameters.OutputDirectory + "nunit.uiexception.dll",
    Parameters.OutputDirectory + "TestCentric.Gui.Model.dll",
    Parameters.OutputDirectory + "testcentric.engine.api.dll",
    Parameters.OutputDirectory + "testcentric.engine.metadata.dll",
    Parameters.OutputDirectory + "testcentric.engine.core.dll",
    Parameters.OutputDirectory + "testcentric.engine.dll",
    Parameters.OutputDirectory + "Mono.Cecil.dll"
};

var PdbFiles = new string[]
{
    Parameters.OutputDirectory + "testcentric.pdb",
    Parameters.OutputDirectory + "tc-next.pdb",
    Parameters.OutputDirectory + "TestCentric.Common.pdb",
    Parameters.OutputDirectory + "TestCentric.Gui.Components.pdb",
    Parameters.OutputDirectory + "TestCentric.Gui.Runner.pdb",
    Parameters.OutputDirectory + "Experimental.Gui.Runner.pdb",
    Parameters.OutputDirectory + "nunit.uiexception.pdb",
    Parameters.OutputDirectory + "TestCentric.Gui.Model.pdb",
    Parameters.OutputDirectory + "testcentric.engine.api.pdb",
    Parameters.OutputDirectory + "testcentric.engine.metadata.pdb",
    Parameters.OutputDirectory + "testcentric.engine.core.pdb",
    Parameters.OutputDirectory + "testcentric.engine.pdb",
};

//////////////////////////////////////////////////////////////////////
// CREATE PACKAGE IMAGE
//////////////////////////////////////////////////////////////////////

Task("CreateImage")
	.IsDependentOn("Build")
    .Description("Copies all files into the image directory")
    .Does(() =>
    {
        CreateDirectory("./package");

        CleanDirectory(Parameters.ImageDirectory);
        CopyFiles(RootFiles, Parameters.ImageDirectory);

        string imageBinDir = Parameters.ImageDirectory + "bin/";
        CreateDirectory(imageBinDir);
		CopyFiles(baseFiles, imageBinDir);
		if (!Parameters.UsingXBuild)
			CopyFiles(PdbFiles, imageBinDir);

		CopyDirectory(Parameters.OutputDirectory + "Images", imageBinDir + "Images");

		foreach (var runtime in Parameters.SupportedAgentRuntimes)
        {
            var targetDir = imageBinDir + "agents/" + Directory(runtime);
            var sourceDir = Parameters.OutputDirectory + "agents/" + Directory(runtime);
            CopyDirectory(sourceDir, targetDir);
		}

		// NOTE: Files specific to a particular package are not copied
		// into the image directory but are added separately.
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE ZIP
//////////////////////////////////////////////////////////////////////

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
		Information("Creating package " + Parameters.ZipPackage);

        var zipFiles = GetFiles(Parameters.ImageDirectory + "**/*.*");
        Zip(Parameters.ImageDirectory, File(Parameters.ZipPackage), zipFiles);
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE FOR NUGET.ORG
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.IsDependentOn("CreateImage")
	.Does(() =>
	{
		Information("Creating package " + Parameters.NuGetPackage);

        var content = new List<NuSpecContent>();
		int index = Parameters.ImageDirectory.Length;

		foreach (var file in GetFiles(Parameters.ImageDirectory + "**/*.*"))
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
		foreach (string runtime in Parameters.SupportedAgentRuntimes)
			content.Add(new NuSpecContent() {Source = "testcentric-agent.addins", Target = $"tools/agents/{runtime}" }); 

        NuGetPack($"{Parameters.NuGetDirectory}/{NUGET_PACKAGE_NAME}.nuspec", new NuGetPackSettings()
        {
            Version = Parameters.PackageVersion,
            OutputDirectory = Parameters.PackageDirectory,
            NoPackageAnalysis = true,
			Files = content
        });
	});

//////////////////////////////////////////////////////////////////////
// PACKAGE FOR CHOCOLATEY
//////////////////////////////////////////////////////////////////////

Task("PackageChocolatey")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
		Information("Creating package " + Parameters.ChocolateyPackage);

        var content = new List<ChocolateyNuSpecContent>();
		int index = Parameters.ImageDirectory.Length;

		foreach (var file in GetFiles(Parameters.ImageDirectory + "**/*.*"))
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
			
		ChocolateyPack($"{Parameters.ChocoDirectory}/{PACKAGE_NAME}.nuspec", 
            new ChocolateyPackSettings()
            {
                Version = Parameters.PackageVersion,
                OutputDirectory = Parameters.PackageDirectory,
                Files = content
            });
    });

//////////////////////////////////////////////////////////////////////
// ZIP PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestZipPackage")
	.IsDependentOn("PackageZip")
	.Does(() =>
	{
		Information("Testing package " + Parameters.ZipPackage);

		CleanDirectory(Parameters.ZipTestDirectory);

		Unzip(File(Parameters.ZipPackage), Parameters.ZipTestDirectory);
		CopyTestFiles(Parameters.OutputDirectory, Parameters.ZipTestDirectory + "bin/");

		NUnit3(Parameters.ZipTestDirectory + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestNuGetPackage")
	.IsDependentOn("PackageNuGet")
	.Does(() =>
	{
		Information("Testing package " + Parameters.NuGetPackage);

		CleanDirectory(Parameters.NuGetTestDirectory);
		Unzip(File(Parameters.NuGetPackage), Parameters.NuGetTestDirectory);

		CheckNuGetContent(Parameters.NuGetTestDirectory);

		CopyTestFiles(Parameters.OutputDirectory, Parameters.NuGetTestDirectory + "tools/");

		NUnit3(Parameters.NuGetTestDirectory + "tools/" + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestChocolateyPackage")
	.IsDependentOn("PackageChocolatey")
	.Does(() =>
	{
		Information("Testing package " + Parameters.ChocolateyPackage);

		CleanDirectory(Parameters.ChocolateyTestDirectory);

		Unzip(File(Parameters.ChocolateyPackage), Parameters.ChocolateyTestDirectory);
		CopyTestFiles(Parameters.OutputDirectory, Parameters.ChocolateyTestDirectory + "tools/");

		NUnit3(Parameters.ChocolateyTestDirectory + "tools/" + MODEL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL COMMANDS FOR PUBLISHING PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PublishToMyGet")
	.Does(() =>
	{
		PublishToMyGet(Parameters.NuGetPackage);
		PublishToMyGet(Parameters.ChocolateyPackage);
	});

Task("PublishToNuGet")
	.Does(() =>
	{
		PublishToNuGet(Parameters.NuGetPackage);
	});

Task("PublishToChocolatey")
	.Does(() =>
	{
		PublishToChocolatey(Parameters.ChocolateyPackage);
	});

//////////////////////////////////////////////////////////////////////
// INTERACTIVE TESTS FOR USE IN DEVELOPMENT
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// GUI TEST
//////////////////////////////////////////////////////////////////////

Task("GuiTest")
    .IsDependentOn("Build")
    .Does(() =>
{
		StartProcess(
		  Parameters.OutputDirectory + GUI_RUNNER, 
		  Parameters.OutputDirectory + GUI_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL GUI TEST
//////////////////////////////////////////////////////////////////////

Task("ExperimentalGuiTest")
    .IsDependentOn("Build")
    .Does(() =>
{
		StartProcess(
		  Parameters.OutputDirectory + EXPERIMENTAL_RUNNER, 
		  Parameters.OutputDirectory + EXPERIMENTAL_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// ZIP GUI TEST (USES ZIP PACKAGE)
//////////////////////////////////////////////////////////////////////

Task("ZipGuiTest")
	.IsDependentOn("PackageZip")
	.Does(() =>
	{
		var testDir = Parameters.ZipTestDirectory;
		CleanDirectory(testDir);

		Unzip(File(Parameters.ZipPackage), testDir);
		CopyTestFiles(Parameters.OutputDirectory, testDir + "tools/");

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
	.Does(() =>
	{
		CleanDirectory(Parameters.ZipTestDirectory);

		Unzip(File(Parameters.NuGetPackage), Parameters.ZipTestDirectory);
		CopyTestFiles(Parameters.OutputDirectory, Parameters.ZipTestDirectory + "tools/");

		StartProcess(
		  Parameters.ZipTestDirectory + EXPERIMENTAL_RUNNER,
		  Parameters.ZipTestDirectory + EXPERIMENTAL_TESTS + " --run");

		CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY INSTALL (MUST RUN AS ADMIN)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyInstall")
	.Does(() =>
	{
		if (StartProcess("choco", $"install -f -y -s {Parameters.PackageDirectory} {PACKAGE_NAME}") != 0)
			throw new Exception("Failed to install package. Must run this command as administrator.");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY TEST (AFTER INSTALL)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyTest")
	.IsDependentOn("PackageChocolatey")
	.Does(() =>
	{
		var testDir = Parameters.ChocolateyTestDirectory;
		CleanDirectory(testDir);

		Unzip(File(Parameters.ChocolateyPackage), testDir);
		CopyTestFiles(Parameters.OutputDirectory, testDir + "tools/");

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

RunTarget(Parameters.Target);
