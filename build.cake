#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool nuget:?package=GitReleaseManager&version=0.11.0
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.8.0"

const string SOLUTION = "testcentric-engine.sln";
const string NUGET_ID = "TestCentric.Engine";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-engine";
const string DEFAULT_VERSION = "2.0.0";
const string DEFAULT_CONFIGURATION = "Release";

const string PACKAGE_NAME = "testcentric-engine";
const string ENGINE_PACKAGE_NAME = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_NAME = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_NAME = "TestCentric.Engine.Api";

static readonly string TEST_RUNNER_EXE = "test-runner.exe";

// Load scripts after defining constants
#load "./cake/parameters.cake"

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

    Information("Building {0} version {1} of TestCentric Engine.", parameters.Configuration, parameters.PackageVersion);

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
		Information("Cleaning " + parameters.OutputDirectory);
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
	.IsDependentOn("CheckHeaders")
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.UsingXBuild)
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
// ENGINE PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		CreateDirectory(parameters.PackageDirectory);

		Information("Creating package " + parameters.EnginePackageName);

        NuGetPack($"{parameters.NuGetDirectory}/{ENGINE_PACKAGE_NAME}.nuspec", new NuGetPackSettings()
        {
            Version = parameters.PackageVersion,
            BasePath = parameters.OutputDirectory,
            OutputDirectory = parameters.PackageDirectory,
            NoPackageAnalysis = true
        });
    });

Task("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
        CleanDirectory(parameters.NuGetTestDirectory);
        Unzip(parameters.EnginePackage, parameters.NuGetTestDirectory);

        Information($"Installed {parameters.EnginePackageName} at { parameters.NuGetTestDirectory}");
    });

Task("VerifyEnginePackage")
	.IsDependentOn("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		Check.That(parameters.NuGetTestDirectory,
			HasFiles("LICENSE.txt", "testcentric.png"),
			HasDirectory("lib").WithFiles(
				"testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll",
				"testcentric.engine.pdb", "testcentric.engine.core.pdb"),
			HasDirectory("content").WithFile("testcentric.nuget.addins"),
			HasDirectory("agents/net40").WithFiles(
				"testcentric-agent.exe", "testcentric-agent.pdb", "testcentric-agent.exe.config",
				"testcentric-agent-x86.exe", "testcentric-agent-x86.pdb", "testcentric-agent-x86.exe.config",
				"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
				"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric-agent.nuget.addins"),
			HasDirectory("agents/netcoreapp3.1").WithFiles(
				"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
				"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
				"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric-agent.nuget.addins"),
			HasDirectory("agents/net5.0").WithFiles(
				"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
				"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
				"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric-agent.nuget.addins"));

			Information("Verification was successful!");
	});

Task("TestEnginePackage")
	.IsDependentOn("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		new NuGetPackageTester(parameters).RunAllTests();
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

Task("VerifyEngineCorePackage")
	.Does<BuildParameters>((parameters) =>
	{
		string dirName = $"{System.Guid.NewGuid()}/";

		try
		{
			Unzip(parameters.EngineCorePackage, dirName);

			Check.That(dirName,
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/net20").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll"),
				HasDirectory("lib/net40").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll"),
				HasDirectory("lib/netstandard2.0").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll"));
		}
		finally
		{
			DeleteDirectory(dirName, new DeleteDirectorySettings()
			{
				Recursive = true
			});
		}

		Information("Verification was successful!");
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

Task("VerifyEngineApiPackage")
	.Does<BuildParameters>((parameters) =>
	{
		string dirName = $"{System.Guid.NewGuid()}/";

		try
		{
			Unzip(parameters.EngineApiPackage, dirName);

			Check.That(dirName,
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/net20").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"),
				HasDirectory("lib/net40").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"),
				HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"));
		}
		finally
		{
			DeleteDirectory(dirName, new DeleteDirectorySettings()
			{
				Recursive = true
			});
		}

		Information("Verification was successful!");
	});

//////////////////////////////////////////////////////////////////////
// PUBLISH PACKAGES
//////////////////////////////////////////////////////////////////////

static bool hadPublishingErrors = false;

Task("PublishPackages")
	.Description("Publish packages according to the current settings")
	.IsDependentOn("PublishToMyGet")
    .IsDependentOn("PublishToNuGet")
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
				PushNuGetPackage(parameters.EnginePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
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
				PushNuGetPackage(parameters.EnginePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
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
            string releaseName = $"TestCentric Engine {milestone}";

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
        }
        else
        {
            Information("Skipping Release creation because this is not a release branch");
        }
    });

////////////////////////////////////////////////////////////////////////
//// CREATE A PRODUCTION RELEASE
////////////////////////////////////////////////////////////////////////

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
	.IsDependentOn("PackageEngine")
	.IsDependentOn("PackageEngineCore")
	.IsDependentOn("PackageEngineApi");

Task("PackageEngine")
	.IsDependentOn("BuildEnginePackage")
	.IsDependentOn("VerifyEnginePackage")
	.IsDependentOn("TestEnginePackage");

Task("PackageEngineCore")
	.IsDependentOn("BuildEngineCorePackage")
	.IsDependentOn("VerifyEngineCorePackage");

Task("PackageEngineApi")
	.IsDependentOn("BuildEngineApiPackage")
	.IsDependentOn("VerifyEngineApiPackage");

Task("Test")
	.IsDependentOn("TestEngineCore")
	.IsDependentOn("TestEngine");

Task("AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("CheckTestErrors")
	.IsDependentOn("Package")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Full")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
	.IsDependentOn("CheckTestErrors")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
