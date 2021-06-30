#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool nuget:?package=GitReleaseManager&version=0.11.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.8.0"
#tool nuget:?package=Wyam&version=2.2.9

#addin nuget:?package=Cake.Git&version=0.22.0
#addin nuget:?package=Cake.Wyam&version=2.2.9

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
// --testLevel=LEVEL
//     Specifies the level of package testing, which is normally set
//     automatically for different types of builds like CI, PR, etc.
//     Used by developers to test packages locally without creating
//     a PR or publishing the package. Defined levels are
//       1 = Normal CI tests run every time you build a package
//       2 = Adds more tests for PRs and Dev builds uploaded to MyGet
//       3 = Adds even more tests prior to publishing a release
//
// NOTE: Cake syntax requires the `=` character. Neither a space
//       nor a colon will work!
//////////////////////////////////////////////////////////////////////

using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

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

Task("CleanAll")
	.IsDependentOn("Clean")
	.Does<BuildParameters>((parameters) =>
	{
		DeleteObjectDirectories(parameters);
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
	parameters.BuildVersion.PatchAssemblyInfo("src/CommonAssemblyInfo.cs");
    parameters.BuildVersion.PatchAssemblyInfo("src/TestEngine/CommonEngineAssemblyInfo.cs");
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
	.IsDependentOn("UpdateAssemblyInfo")
	.IsDependentOn("CheckHeaders")
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

static var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE
//////////////////////////////////////////////////////////////////////

Task("TestEngine")
	.Description("Tests the TestCentric Engine")
	.IsDependentOn("Build")
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
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.SupportedCoreRuntimes)
			RunNUnitLite("testcentric.engine.core.tests", runtime, $"{parameters.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.AGENT.CORE
//////////////////////////////////////////////////////////////////////

Task("TestAgentCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.SupportedCoreRuntimes)
			RunNUnitLite("testcentric.agent.core.tests", runtime, $"{parameters.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF THE GUI
//////////////////////////////////////////////////////////////////////

Task("TestGui")
    .IsDependentOn("Build")
    .Does<BuildParameters>((parameters) =>
	{
		var guiTests = GetFiles(parameters.OutputDirectory + GUI_TESTS);
		var args = new StringBuilder();
		foreach (var test in guiTests)
			args.Append($"\"{test}\" ");

		var guiTester = new GuiTester(parameters);
		guiTester.RunGuiUnattended(parameters.OutputDirectory + GUI_RUNNER, args.ToString());
		var result = new ActualResult(parameters.OutputDirectory + "TestResult.xml");

		new ConsoleReporter(result).Display();

		if (result.OverallResult == "Failed")
			throw new System.Exception("There were test failures or errors. See listing.");
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

Task("BuildZipPackage")
    .IsDependentOn("CreateImage")
    .Does<BuildParameters>((parameters) =>
    {
		Information("Creating package " + parameters.ZipPackageName);

		// TODO: We copy in and then delete zip-specific addins files because Zip command
		// requires all files to be in the directory that is zipped. Ideally, the image
		// directory should be used exclusively for the zip package to avoid having to
		// add and delete these files.
		try
		{
			CopyFileToDirectory(parameters.ZipDirectory + "testcentric.zip.addins", parameters.ImageDirectory + "bin/");
			foreach (string runtime in parameters.SupportedAgentRuntimes)
				CopyFileToDirectory(parameters.ZipDirectory + "testcentric-agent.zip.addins", $"{parameters.ImageDirectory}bin/agents/{runtime}");

			var zipFiles = GetFiles(parameters.ImageDirectory + "**/*.*");
			Zip(parameters.ImageDirectory, parameters.ZipPackage, zipFiles);
		}
		finally
		{
			DeleteFile(parameters.ImageDirectory + "bin/testcentric.zip.addins");
			foreach (string runtime in parameters.SupportedAgentRuntimes)
				DeleteFile($"{parameters.ImageDirectory}bin/agents/{runtime}/testcentric-agent.zip.addins");
		}
	});

Task("TestZipPackage")
	.IsDependentOn("BuildZipPackage")
	.Does<BuildParameters>((parameters) =>
	{
		new ZipPackageTester(parameters).RunAllTests();
	});

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildNuGetPackage")
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
		content.Add(new NuSpecContent() { Source = "testcentric.nuget.addins", Target = "tools" });
		foreach (string runtime in parameters.SupportedAgentRuntimes)
			content.Add(new NuSpecContent() {Source = "testcentric-agent.nuget.addins", Target = $"tools/agents/{runtime}" }); 

        NuGetPack($"{parameters.NuGetDirectory}/{NUGET_PACKAGE_NAME}.nuspec", new NuGetPackSettings()
        {
            Version = parameters.PackageVersion,
            OutputDirectory = parameters.PackageDirectory,
            NoPackageAnalysis = true,
			Files = content
        });
	});

Task("TestNuGetPackage")
	.IsDependentOn("BuildNuGetPackage")
	.Does<BuildParameters>((parameters) =>
	{
		new NuGetPackageTester(parameters).RunAllTests();
	});

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildChocolateyPackage")
    .IsDependentOn("CreateImage")
	.WithCriteria(IsRunningOnWindows())
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

		foreach (string runtime in parameters.SupportedAgentRuntimes)
			content.Add(new ChocolateyNuSpecContent() {Source = "testcentric-agent.choco.addins", Target = $"tools/agents/{runtime}" }); 
			
		ChocolateyPack($"{parameters.ChocoDirectory}/{PACKAGE_NAME}.nuspec", 
            new ChocolateyPackSettings()
            {
                Version = parameters.PackageVersion,
                OutputDirectory = parameters.PackageDirectory,
                Files = content
            });
    });

Task("TestChocolateyPackage")
	.IsDependentOn("BuildChocolateyPackage")
	.WithCriteria(IsRunningOnWindows())
	.Does<BuildParameters>((parameters) =>
	{
		new ChocolateyPackageTester(parameters).RunAllTests();
	});

//////////////////////////////////////////////////////////////////////
// ENGINE CORE PACKAGE
//////////////////////////////////////////////////////////////////////

// NOTE: The testcentric.engine.core assembly and its dependencies are
// included in all the main packages. It is also published separately 
// as a nuget package for use in creating pluggable agents and for any
// other projects, which may want to make use of it.

Task("BuildEngineCorePackage")
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		NuGetPack($"{parameters.NuGetDirectory}/TestCentric.Engine.Core.nuspec", new NuGetPackSettings()
		{
			Version = parameters.PackageVersion,
			OutputDirectory = parameters.PackageDirectory,
			NoPackageAnalysis = true
		});
	});

//////////////////////////////////////////////////////////////////////
// ENGINE API PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildEngineApiPackage")
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		NuGetPack($"{parameters.NuGetDirectory}/TestCentric.Engine.Api.nuspec", new NuGetPackSettings()
		{
			Version = parameters.PackageVersion,
			OutputDirectory = parameters.PackageDirectory,
			NoPackageAnalysis = true
		});
	});

//////////////////////////////////////////////////////////////////////
// PUBLISH PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PublishPackages")
	.Description("Publish nuget and chocolatey packages according to the current settings")
	.Does<BuildParameters>((parameters) =>
	{
		bool nothingToPublish = true;

		if (parameters.ShouldPublishToMyGet)
		{
			PushNuGetPackage(parameters.NuGetPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			PushNuGetPackage(parameters.EngineCorePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			PushNuGetPackage(parameters.EngineApiPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			PushChocolateyPackage(parameters.ChocolateyPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			nothingToPublish = false;
		}

		if (parameters.ShouldPublishToNuGet)
		{
			PushNuGetPackage(parameters.NuGetPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
			PushNuGetPackage(parameters.EngineCorePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
			PushNuGetPackage(parameters.EngineApiPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
			nothingToPublish = false;
		}

		if (parameters.ShouldPublishToChocolatey)
		{
			PushChocolateyPackage(parameters.ChocolateyPackage, parameters.ChocolateyApiKey, parameters.ChocolateyPushUrl);
			nothingToPublish = false;
		}

		if (nothingToPublish)
			Information("Nothing to publish from this run.");
	});

//////////////////////////////////////////////////////////////////////
// CREATE A DRAFT RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateDraftRelease")
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.IsReleaseBranch)
		{
			// Exit if any PackageTests failed
			CheckTestErrors(ref ErrorDetail);

			// NOTE: Since this is a release branch, the pre-release label
			// is "pre", which we don't want to use for the draft release.
			// The branch name contains the full information to be used
			// for both the name of the draft release and the milestone,
			// i.e. release-2.0.0, release-2.0.0-beta2, etc.
			string milestone = parameters.BranchName.Substring(8);
			string releaseName = $"TestCentric {milestone}";

			Information($"Creating draft release for {releaseName}");

			try
			{
				GitReleaseManagerCreate(parameters.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, new GitReleaseManagerCreateSettings()
				{
					Name = releaseName,
					Milestone = milestone
				});
			}
			catch
            {
				Error($"Unable to create draft release for {releaseName}.");
				Error($"Check that there is a {milestone} milestone with at least one closed issue.");
				Error("");
				throw;
            }

			GitReleaseManagerExport(parameters.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, "DraftRelease.md",
				new GitReleaseManagerExportSettings() { TagName = milestone });
		}
		else
		{
			Information("Skipping Release creation because this is not a release branch");
		}
	});

//////////////////////////////////////////////////////////////////////
// CREATE A PRODUCTION RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateProductionRelease")
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.IsProductionRelease)
		{
			// Exit if any PackageTests failed
			CheckTestErrors(ref ErrorDetail);

			string token = parameters.GitHubAccessToken;
			string tagName = parameters.PackageVersion;
			string assets = parameters.GitHubReleaseAssets;

			Information($"Publishing release {tagName} to GitHub");

			GitReleaseManagerAddAssets(token, GITHUB_OWNER, GITHUB_REPO, tagName, assets);
			GitReleaseManagerClose(token, GITHUB_OWNER, GITHUB_REPO, tagName);
		}
		else
		{
			Information("Skipping CreateProductionRelease because this is not a production release");
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

Task("Package")
	.IsDependentOn("BuildPackages")
	.IsDependentOn("TestPackages");

Task("Test")
	.IsDependentOn("TestEngineCore")
	.IsDependentOn("TestAgentCore")
	.IsDependentOn("TestEngine")
	.IsDependentOn("TestGui");

Task("BuildPackages")
	.IsDependentOn("CheckTestErrors")
    .IsDependentOn("BuildZipPackage")
	.IsDependentOn("BuildNuGetPackage")
    .IsDependentOn("BuildChocolateyPackage")
	.IsDependentOn("BuildEngineCorePackage")
	.IsDependentOn("BuildEngineApiPackage");

Task("TestPackages")
	.IsDependentOn("BuildPackages")
    .IsDependentOn("TestZipPackage")
	.IsDependentOn("TestNuGetPackage")
    .IsDependentOn("TestChocolateyPackage");

Task("PackageZip")
	.IsDependentOn("BuildZipPackage")
	.IsDependentOn("TestZipPackage");

Task("PackageNuGet")
	.IsDependentOn("BuildNuGetPackage")
	.IsDependentOn("TestNuGetPackage");

Task("PackageChocolatey")
	.IsDependentOn("BuildChocolateyPackage")
	.IsDependentOn("TestChocolateyPackage");

Task("AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("BuildPackages")
	.IsDependentOn("TestPackages")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("All")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("BuildPackages")
	.IsDependentOn("TestPackages");

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
