#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.3.1"

#load ./build/settings.cake

using System.Xml;
using System.Text.RegularExpressions;

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
		packageVersion = gitVersion.LegacySemVerPadded;

		// Full release versions and PRs need no further handling
		int dash = packageVersion.IndexOf('-');
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

			packageVersion = gitVersion.MajorMinorPatch + suffix;
		}

		if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(packageVersion + "-" + AppVeyor.Environment.Build.Number);
	}

    Information("Building {0} version {1} of TestCentric GUI.", configuration, packageVersion);
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
    CleanDirectory(BIN_DIR);
});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does(() =>
{
    NuGetRestore(SOLUTION, nugetRestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    if(usingXBuild)
    {
        XBuild(SOLUTION, xBuildSettings);
    }
    else
    {
        MSBuild(SOLUTION, msBuildSettings);
    }
});

//////////////////////////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////////////////////////

var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more unit tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

// Run tests using NUnitLite
private void RunNUnitLite(string testName, string framework, string directory)
{
	bool isDotNetCore = framework.StartsWith("netcoreapp");
	string ext = isDotNetCore ? ".dll" : ".exe";
	string testPath = directory + testName + ext;

	Information($"Trying to run {testPath}");

	int rc = isDotNetCore
		? StartProcess("dotnet", testPath)
		: StartProcess(testPath);

	if (rc > 0)
		ErrorDetail.Add(string.Format($"{testName}: {rc} tests failed running under {framework}"));
	else if (rc < 0)
		ErrorDetail.Add(string.Format($"{testName} returned rc = {rc} running under {framework}"));
}

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE
//////////////////////////////////////////////////////////////////////

var testEngineTask = Task("TestEngine")
	.Description("Tests the TestCentric Engine");

foreach (var runtime in ENGINE_RUNTIMES)
{
	var task = Task("TestEngine_" + runtime)
		.Description("Tests the Engine on " + runtime)
		.IsDependentOn("Build")
		.OnError(exception => { ErrorDetail.Add(exception.Message); })
		.Does(() =>
		{
			RunNUnitLite(ENGINE_TESTS, runtime, BIN_DIR + $"engine-tests/{runtime}/");
		});

	testEngineTask.IsDependentOn(task);	
}

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

var testEngineCoreTask = Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core");

foreach (var runtime in ENGINE_CORE_RUNTIMES)
{
	var task = Task("TestEngineCore_" + runtime)
		.Description("Tests the Engine Core on " + runtime)
		.IsDependentOn("Build")
		.OnError(exception => { ErrorDetail.Add(exception.Message); })
		.Does(() =>
		{
			RunNUnitLite(ENGINE_CORE_TESTS, runtime, BIN_DIR + $"engine-tests/{runtime}/");
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
    NUnit3(BIN_DIR + ALL_TESTS, new NUnit3Settings {
        NoResults = true
        });
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
    BIN_DIR + "testcentric.exe",
    BIN_DIR + "testcentric.exe.config",
    BIN_DIR + "tc-next.exe",
    BIN_DIR + "tc-next.exe.config",
    BIN_DIR + "TestCentric.Common.dll",
    BIN_DIR + "TestCentric.Gui.Components.dll",
    BIN_DIR + "TestCentric.Gui.Runner.dll",
    BIN_DIR + "Experimental.Gui.Runner.dll",
    BIN_DIR + "nunit.uiexception.dll",
    BIN_DIR + "TestCentric.Gui.Model.dll",
    BIN_DIR + "testcentric.engine.api.dll",
    BIN_DIR + "testcentric.engine.metadata.dll",
    BIN_DIR + "testcentric.engine.core.dll",
    BIN_DIR + "testcentric.engine.dll",
    BIN_DIR + "Mono.Cecil.dll"
};

var PdbFiles = new string[]
{
    BIN_DIR + "testcentric.pdb",
    BIN_DIR + "tc-next.pdb",
    BIN_DIR + "TestCentric.Common.pdb",
    BIN_DIR + "TestCentric.Gui.Components.pdb",
    BIN_DIR + "TestCentric.Gui.Runner.pdb",
    BIN_DIR + "Experimental.Gui.Runner.pdb",
    BIN_DIR + "nunit.uiexception.pdb",
    BIN_DIR + "TestCentric.Gui.Model.pdb",
    BIN_DIR + "testcentric.engine.api.pdb",
    BIN_DIR + "testcentric.engine.metadata.pdb",
    BIN_DIR + "testcentric.engine.core.pdb",
    BIN_DIR + "testcentric.engine.pdb",
};

//////////////////////////////////////////////////////////////////////
// CREATE PACKAGE IMAGE
//////////////////////////////////////////////////////////////////////

Task("CreateImage")
	.IsDependentOn("Build")
    .Description("Copies all files into the image directory")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        CleanDirectory(CurrentImageDir);
        CopyFiles(RootFiles, CurrentImageDir);

        string imageBinDir = CurrentImageDir + "bin/";
        CreateDirectory(imageBinDir);
		CopyFiles(baseFiles, imageBinDir);
		if (!usingXBuild)
			CopyFiles(PdbFiles, imageBinDir);

		CopyDirectory(BIN_DIR + "Images", imageBinDir + "Images");

		foreach (var runtime in AGENT_RUNTIMES)
        {
            var targetDir = imageBinDir + "agents/" + Directory(runtime);
            var sourceDir = BIN_DIR + "agents/" + Directory(runtime);
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
		Information("Creating package " + ZipPackage);

        var zipFiles = GetFiles(CurrentImageDir + "**/*.*");
        Zip(CurrentImageDir, File(ZipPackage), zipFiles);
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE FOR NUGET.ORG
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.IsDependentOn("CreateImage")
	.Does(() =>
	{
		Information("Creating package " + NuGetPackage);

        var content = new List<NuSpecContent>();
		int index = CurrentImageDir.Length;

		foreach (var file in GetFiles(CurrentImageDir + "**/*.*"))
		{
			var source = file.FullPath;
			var target = System.IO.Path.GetDirectoryName(file.FullPath.Substring(index));

			if (target == "bin")
				target = "tools";
			else if (target.StartsWith("bin" + System.IO.Path.DirectorySeparatorChar))
				target = "tools" + target.Substring(3);

			content.Add(new NuSpecContent() { Source = file.FullPath, Target = target });
		}

		// Icon goes in the root
		content.Add(new NuSpecContent() { Source = PROJECT_DIR + "testcentric.png" });

		// Use addins file tailored for nuget install
		content.Add(new NuSpecContent() { Source = NUGET_DIR + "testcentric-gui.addins", Target = "tools" });
		foreach (string runtime in AGENT_RUNTIMES)
			content.Add(new NuSpecContent() {Source = NUGET_DIR + "testcentric-agent.addins", Target = $"tools/agents/{runtime}" }); 

        NuGetPack(NUGET_DIR + NUGET_PACKAGE_NAME + ".nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            OutputDirectory = PACKAGE_DIR,
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
		Information("Creating package " + ChocolateyPackage);

        var content = new List<ChocolateyNuSpecContent>();
		int index = CurrentImageDir.Length;

		foreach (var file in GetFiles(CurrentImageDir + "**/*.*"))
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
			new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "VERIFICATION.txt", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "testcentric-agent.exe.ignore", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "testcentric-agent-x86.exe.ignore", Target = "tools" },
			new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "testcentric.choco.addins", Target = "tools" }
		});
			
		ChocolateyPack(CHOCO_DIR + PACKAGE_NAME + ".nuspec", 
            new ChocolateyPackSettings()
            {
                Version = packageVersion,
                OutputDirectory = PACKAGE_DIR,
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
		Information("Testing package " + ZipPackage);

		CleanDirectory(ZIP_TEST_DIR);

		Unzip(File(ZipPackage), ZIP_TEST_DIR);
		CopyTestFiles(BIN_DIR, ZIP_TEST_DIR + "bin/");

		NUnit3(ZIP_TEST_DIR + "bin/" + ALL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestNuGetPackage")
	.IsDependentOn("PackageNuGet")
	.Does(() =>
	{
		Information("Testing package " + NuGetPackage);

		CleanDirectory(NUGET_TEST_DIR);
		Unzip(File(NuGetPackage), NUGET_TEST_DIR);

		CheckNuGetContent(NUGET_TEST_DIR);

		CopyTestFiles(BIN_DIR, NUGET_TEST_DIR + "tools/");

		NUnit3(NUGET_TEST_DIR + "tools/" + ALL_TESTS);
	});

private void CheckNuGetContent(string nugetDir)
{
	if (!DirectoryExists(nugetDir))
		throw new Exception($"Directory {nugetDir} not found!");
		
	string addinsFile = nugetDir + "tools/testcentric-gui.addins";
	if (!FileExists(addinsFile))
		throw new Exception($"File {addinsFile} not found in the package.");
}

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE TEST
//////////////////////////////////////////////////////////////////////

Task("TestChocolateyPackage")
	.IsDependentOn("PackageChocolatey")
	.Does(() =>
	{
		Information("Testing package " + ChocolateyPackage);

		CleanDirectory(CHOCO_TEST_DIR);

		Unzip(File(ChocolateyPackage), CHOCO_TEST_DIR);
		CopyTestFiles(BIN_DIR, CHOCO_TEST_DIR + "tools/");

		NUnit3(CHOCO_TEST_DIR + "tools/" + ALL_TESTS);
	});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL COMMANDS FOR PUBLISHING PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PublishToMyGet")
	.Does(() =>
	{
		PublishToMyGet(NuGetPackage);
		PublishToMyGet(ChocolateyPackage);
	});

Task("PublishToNuGet")
	.Does(() =>
	{
		PublishToNuGet(NuGetPackage);
	});

Task("PublishToChocolatey")
	.Does(() =>
	{
		PublishToChocolatey(ChocolateyPackage);
	});

	string MYGET_API_KEY = EnvironmentVariable("MYGET_API_KEY");
	string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
	string NUGET_API_KEY = EnvironmentVariable("NUGET_API_KEY");
	string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";
	string CHOCO_API_KEY = EnvironmentVariable("CHOCO_API_KEY");
	string CHOCO_PUSH_URL = "https://push.chocolatey.org/";

	private void PublishToMyGet(string packageName)
	{
		EnsurePackageExists(packageName);

		Information($"Publishing {packageName} to myget.org.");
		NuGetPush(packageName, new NuGetPushSettings() { ApiKey=MYGET_API_KEY, Source=MYGET_PUSH_URL });
	}

	private void PublishToNuGet(string packageName)
	{
		EnsurePackageExists(packageName);

		Information($"Publishing {packageName} to nuget.org.");
		NuGetPush(packageName, new NuGetPushSettings() { ApiKey=NUGET_API_KEY, Source=NUGET_PUSH_URL });
	}

	private void PublishToChocolatey(string packageName)
	{
		EnsurePackageExists(packageName);
		EnsureKeyIsSet(CHOCO_API_KEY);

		Information($"Publishing {packageName} to chocolatey.");
		ChocolateyPush(packageName, new ChocolateyPushSettings() { ApiKey=CHOCO_API_KEY, Source=CHOCO_PUSH_URL });
	}

	private void EnsurePackageExists(string path)
	{
		if (!FileExists(path))
		{
			var packageName = System.IO.Path.GetFileName(path);
			throw new InvalidOperationException(
			  $"Package not found: {packageName}.\nCode may have changed since package was last built.");
		}
	}

	private void EnsureKeyIsSet(string apiKey)
	{
		if (string.IsNullOrEmpty(apiKey))
			throw new InvalidOperationException("The Api Key has not been set.");
	}

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
		StartProcess(BIN_DIR + GUI_RUNNER, BIN_DIR + GUI_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// EXPERIMENTAL GUI TEST
//////////////////////////////////////////////////////////////////////

Task("ExperimentalGuiTest")
    .IsDependentOn("Build")
    .Does(() =>
{
		StartProcess(BIN_DIR + EXPERIMENTAL_RUNNER, BIN_DIR + EXPERIMENTAL_TESTS + " --run");
		CheckTestResult("TestResult.xml");
});

//////////////////////////////////////////////////////////////////////
// ZIP GUI TEST (USES ZIP PACKAGE)
//////////////////////////////////////////////////////////////////////

Task("ZipGuiTest")
	.IsDependentOn("PackageZip")
	.Does(() =>
	{
		CleanDirectory(ZIP_TEST_DIR);

		Unzip(File(ZipPackage), ZIP_TEST_DIR);
		CopyTestFiles(BIN_DIR, ZIP_TEST_DIR + "tools/");

		StartProcess(ZIP_TEST_DIR + GUI_RUNNER, ZIP_TEST_DIR + GUI_TESTS + " --run");
		CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// ZIP EXPERIMENTAL GUI TEST (USES ZIP PACKAGE)
//////////////////////////////////////////////////////////////////////

Task("ZipExperimentalGuiTest")
	.IsDependentOn("PackageZip")
	.Does(() =>
	{
		CleanDirectory(ZIP_TEST_DIR);

		Unzip(File(PACKAGE_DIR + "TestCentric.GuiRunner-" + packageVersion + ".nupkg"), ZIP_TEST_DIR);
		CopyTestFiles(BIN_DIR, ZIP_TEST_DIR + "tools/");

		StartProcess(ZIP_TEST_DIR + EXPERIMENTAL_RUNNER, ZIP_TEST_DIR + EXPERIMENTAL_TESTS + " --run");
		CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY INSTALL (MUST RUN AS ADMIN)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyInstall")
	.Does(() =>
	{
		if (StartProcess("choco", $"install -f -y -s {PACKAGE_DIR} {PACKAGE_NAME}") != 0)
			throw new Exception("Failed to install package. Must run this command as administrator.");
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY TEST (AFTER INSTALL)
//////////////////////////////////////////////////////////////////////

Task("ChocolateyTest")
	.IsDependentOn("PackageChocolatey")
	.Does(() =>
	{
		CleanDirectory(CHOCO_TEST_DIR);

		Unzip(File(ChocolateyPackage), CHOCO_TEST_DIR);
		CopyTestFiles(BIN_DIR, CHOCO_TEST_DIR + "tools/");

		// TODO: When starting the commands that chocolatey has shimmed, the StartProcess
		// call returns immediately, so we can't check the test result. For now, we just
		// run the tests and inspect manually but we need to figure out how to wait for
		// the process to complete.
		StartProcess("testcentric", CHOCO_TEST_DIR + "TestCentric.Gui.Tests.dll --run");
		StartProcess("tc-next", CHOCO_TEST_DIR + "Experimental.Gui.Tests.dll --run");
		//CheckTestResult("TestResult.xml");
	});

//////////////////////////////////////////////////////////////////////
// HELPER METHODS - TESTS
//////////////////////////////////////////////////////////////////////

// Copy all files needed to run tests from one directory to another
private void CopyTestFiles(string fromDir, string toDir)
{
	CopyFiles(fromDir + "*.Tests.*", toDir);
	CopyFiles(fromDir + "nunit.framework.*", toDir);
	CopyFiles(fromDir + "mock-assembly.*", toDir);
	CopyFiles(fromDir + "test-utilities.*", toDir);
	CopyFiles(fromDir + "System.Threading.Tasks.*", toDir);
	CopyFiles(fromDir + "NSubstitute.*", toDir);
	CopyFiles(fromDir + "Castle.Core.*", toDir);
}

// Examine the result file to make sure a test run passed
private void CheckTestResult(string resultFile)
{
	var doc = new XmlDocument();
	doc.Load(resultFile);

	XmlNode testRun = doc.DocumentElement;
	if (testRun.Name != "test-run")
		throw new Exception("The test-run element was not found.");

	string result = testRun.Attributes["result"]?.Value;
	if (result == null)
		throw new Exception("The test-run element has no result attribute.");

	if (result == "Failed")
	{
		string msg = "The test run failed.";
		string failed = testRun.Attributes["failed"]?.Value;
		if (failed != null)
			msg += $" {int.Parse(failed)} tests failed";
		throw new Exception(msg);
	}
}

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

RunTarget(target);
