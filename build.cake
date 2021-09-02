#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool nuget:?package=GitReleaseManager&version=0.11.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.8.0"

#load "./build/parameters.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS (In addition to the standard Cake arguments)
//
// --asVersion=VERSION
//     Specifies the full package version, including any pre-release
//     suffix. This version is used directly instead of the default
//     version from the script or that calculated by GitVersion.
//     Note that all other versions (AssemblyVersion, etc.) are
//     derived from the package version.
//     
//     NOTE: We can't use "version" since that's an argument to Cake itself.
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
// NOTE: Cake syntax now requires the `=` character. Neither a space
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
	Information("Deleting " + parameters.OutputDirectory);
	CleanDirectory(parameters.OutputDirectory);
});

Task("CleanAll")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Deleting all output directories");
		CleanDirectory(parameters.ProjectDirectory + "bin/");

		Information("Deleting object directories");
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
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
	.IsDependentOn("CheckHeaders")
    .Does<BuildParameters>((parameters) =>
{
    if(parameters.UsingXBuild)
        XBuild(SOLUTION, parameters.XBuildSettings.WithProperty("Version", parameters.PackageVersion));
    else
        MSBuild(SOLUTION, parameters.MSBuildSettings.WithProperty("Version", parameters.PackageVersion));
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

////////////////////////////////////////////////////////////////////
// PACKAGING
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// ZIP PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildZipPackage")
    .Does<BuildParameters>((parameters) =>
    {
		CreateDirectory(parameters.PackageDirectory);
		CreateZipImage(parameters);

		Information("Creating package " + parameters.ZipPackageName);

		var zipFiles = GetFiles(parameters.ZipImageDirectory + "**/*.*");
		Zip(parameters.ZipImageDirectory, parameters.ZipPackage, zipFiles);
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
	.Does<BuildParameters>((parameters) =>
	{
		CreateDirectory(parameters.PackageDirectory);

		Information("Creating package " + parameters.NuGetPackageName);

		NuGetPack($"{parameters.NuGetDirectory}/{NUGET_PACKAGE_NAME}.nuspec", new NuGetPackSettings()
        {
            Version = parameters.PackageVersion,
			BasePath = parameters.OutputDirectory,
            OutputDirectory = parameters.PackageDirectory,
            NoPackageAnalysis = true
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
	.WithCriteria(IsRunningOnWindows())
    .Does<BuildParameters>((parameters) =>
    {
		CreateDirectory(parameters.PackageDirectory);

		Information("Creating package " + parameters.ChocolateyPackageName);

		ChocolateyPack($"{parameters.ChocoDirectory}/{PACKAGE_NAME}.nuspec", 
            new ChocolateyPackSettings()
			{
				Version = parameters.PackageVersion,
				WorkingDirectory = parameters.OutputDirectory,
				OutputDirectory = parameters.PackageDirectory,
				ArgumentCustomization = args => args.Append($"BIN={parameters.OutputDirectory}")
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

static bool hadPublishingErrors = false;

Task("PublishPackages")
	.Description("Publish nuget and chocolatey packages according to the current settings")
	.IsDependentOn("PublishToMyGet")
	.IsDependentOn("PublishToNuGet")
	.IsDependentOn("PublishToChocolatey")
	.Does(() =>
	{
		if (hadPublishingErrors)
			throw new Exception("One of the publishing steps failed.");
	});

// This task may either be run by the PublishPackages task,
// which depends on it, or directly when recovering from errors.
Task("PublishToMyGet")
	.Description("Publish packages to MyGet")
	.Does<BuildParameters>((parameters) =>
	{
        if (!parameters.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
        else
            try
			{
				PushNuGetPackage(parameters.NuGetPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushChocolateyPackage(parameters.ChocolateyPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			}
			catch(Exception)
			{
				hadPublishingErrors = true;
			}
	});

// This task may either be run by the PublishPackages task,
// which depends on it, or directly when recovering from errors.
Task("PublishToNuGet")
	.Description("Publish packages to NuGet")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.ShouldPublishToNuGet)
			Information("Nothing to publish to NuGet from this run.");
		else
			try
			{
				PushNuGetPackage(parameters.NuGetPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
			}
			catch(Exception)
            {
				hadPublishingErrors = true;
			}
	});

// This task may either be run by the PublishPackages task,
// which depends on it, or directly when recovering from errors.
Task("PublishToChocolatey")
	.Description("Publish packages to Chocolatey")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.ShouldPublishToChocolatey)
			Information("Nothing to publish to Chocolatey from this run.");
		else
			try
			{
				PushChocolateyPackage(parameters.ChocolateyPackage, parameters.ChocolateyApiKey, parameters.ChocolateyPushUrl);
			}
			catch(Exception)
            {
				hadPublishingErrors = true;
			}
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
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
	.IsDependentOn("Build")
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
