#tool NuGet.CommandLine&version=6.0.0
#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1

const string SOLUTION = "testcentric-engine.sln";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-engine";

const string DEFAULT_VERSION = "2.0.0";
const string DEFAULT_CONFIGURATION = "Release";
static readonly string[] VALID_CONFIGS = { "Release", "Debug" };

const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

const string TEST_BED_EXE = "test-bed.exe";

// Load scripts after defining constants
#load "./cake/build-settings.cake"
#load "./cake/check-headers.cake"
#load "./cake/local-targets.cake"
#load "./cake/package-checks.cake"
#load "./cake/package-definitions.cake"
#load "./cake/package-tests.cake"
#load "./cake/test-reports.cake"
#load "./cake/test-results.cake"
#load "./cake/utilities.cake"
#load "./cake/versioning.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//
// Arguments taking a value may use  `=` or space to separate the name
// from the value. Examples of each are shown here.
//
// --target=TARGET
// -t Target
//
//    The name of the task to be run, e.g. Test. Defaults to Build.
//
// --configuration=CONFIG
// -c CONFIG
//
//     The name of the configuration to build, test and/or package, e.g. Debug.
//     Defaults to Release.
//
// --packageVersion=VERSION
// --package=VERSION
//     Specifies the full package version, including any pre-release
//     suffix. This version is used directly instead of the default
//     version from the script or that calculated by GitVersion.
//     Note that all other versions (AssemblyVersion, etc.) are
//     derived from the package version.
//
//     NOTE: We can't use "version" since that's an argument to Cake itself.
//
// --testLevel=LEVEL
// --level=LEVEL
//     Specifies the level of package testing, which is normally set
//     automatically for different types of builds like CI, PR, etc.
//     Used by developers to test packages locally without creating
//     a PR or publishing the package. Defined levels are
//       1 = Normal CI tests run every time you build a package
//       2 = Adds more tests for PRs and Dev builds uploaded to MyGet
//       3 = Adds even more tests prior to publishing a release
//
// --nopush
//     Indicates that no publishing or releasing should be done. If
//     publish or release targets are run, a message is displayed.
//
//////////////////////////////////////////////////////////////////////

using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN
//////////////////////////////////////////////////////////////////////

Setup<BuildSettings>((context) =>
{
	var settings = BuildSettings.CreateInstance(context);

	if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(settings.PackageVersion + "-" + AppVeyor.Environment.Build.Number);

    Information("Building {0} version {1} of TestCentric Engine.", settings.Configuration, settings.PackageVersion);

	return settings;
});

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// DUMP SETTINGS
//////////////////////////////////////////////////////////////////////

Task("DumpSettings")
	.Does<BuildSettings>((settings) =>
	{
		settings.DumpSettings();
	});

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildSettings>((settings) =>
	{
		Information("Cleaning " + settings.OutputDirectory);
		CleanDirectory(settings.OutputDirectory);

        Information("Cleaning Package Directory");
        CleanDirectory(settings.PackageDirectory);
	});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does<BuildSettings>((settings) =>
{
	NuGetRestore(SOLUTION, settings.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("RestorePackages")
	.IsDependentOn("CheckHeaders")
	.Does<BuildSettings>((settings) =>
	{
		if (settings.UsingXBuild)
			XBuild(SOLUTION, settings.XBuildSettings
				.WithProperty("Version", settings.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
		else
			MSBuild(SOLUTION, settings.MSBuildSettings
				.WithProperty("Version", settings.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
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
	.Does<BuildSettings>((settings) =>
	{
		foreach (var runtime in settings.EngineRuntimes)
			RunNUnitLite("testcentric.engine.tests", runtime, $"{settings.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does<BuildSettings>((settings) =>
	{
		foreach (var runtime in settings.EngineCoreRuntimes)
		{
			// Only .NET Standard we currently build is 2.0
			var testUnder = runtime == "netstandard2.0" ? "netcoreapp2.1" : runtime;
			RunNUnitLite("testcentric.engine.core.tests", testUnder, $"{settings.OutputDirectory}engine-tests/{testUnder}/");
		}
	});

//////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST EACH PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EnginePackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EngineApiPackage.BuildVerifyAndTest();
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
	.Does<BuildSettings>((settings) =>
	{
        if (!settings.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
		else if (settings.NoPush)
			Information("NoPush option suppressing publication to MyGet");
        else
            try
			{
				PushNuGetPackage(settings.EnginePackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
				PushNuGetPackage(settings.EngineCorePackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
				PushNuGetPackage(settings.EngineApiPackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
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
	.Does<BuildSettings>((settings) =>
	{
		if (!settings.ShouldPublishToNuGet)
			Information("Nothing to publish to NuGet from this run.");
		else if (settings.NoPush)
			Information("NoPush option suppressing publication to NuGet");
		else
			try
			{
				PushNuGetPackage(settings.EnginePackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
				PushNuGetPackage(settings.EngineCorePackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
				PushNuGetPackage(settings.EngineApiPackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
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
    .Does<BuildSettings>((settings) =>
    {
        if (settings.IsReleaseBranch)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

            // NOTE: Since this is a release branch, the pre-release label
            // is "pre", which we don't want to use for the draft release.
            // The branch name contains the full information to be used
            // for both the name of the draft release and the milestone,
            // i.e. release-2.0.0, release-2.0.0-beta2, etc.
            string milestone = settings.BranchName.Substring(8);
            string releaseName = $"TestCentric Engine {milestone}";

            Information($"Creating draft release for {releaseName}");

		    if (settings.NoPush)
			    Information("NoPush option suppressed creation of draft release");
			else
				try
				{
					GitReleaseManagerCreate(settings.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, new GitReleaseManagerCreateSettings()
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
    .Does<BuildSettings>((settings) =>
    {
        if (settings.IsProductionRelease)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

			string tagName = settings.PackageVersion;
            Information($"Publishing release {tagName} to GitHub");

            if (settings.NoPush)
            {
                Information("NoPush option suppressed publishing of assets:");
                foreach (var asset in settings.GitHubReleaseAssets)
                    Information("  " + asset);
            }
			else
			{
				string token = settings.GitHubAccessToken;
				string assets = $"\"{string.Join(',', settings.GitHubReleaseAssets)}\"";

				GitReleaseManagerAddAssets(token, GITHUB_OWNER, GITHUB_REPO, tagName, assets);
				GitReleaseManagerClose(token, GITHUB_OWNER, GITHUB_REPO, tagName);
			}
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
	.Description("Build and package all components")
	.IsDependentOn("Build")
	.IsDependentOn("PackageExistingBuild");

Task("PackageExistingBuild")
	.Description("Package all components using existing build")
	.IsDependentOn("PackageEngine")
	.IsDependentOn("PackageEngineCore")
	.IsDependentOn("PackageEngineApi");

Task("Test")
	.Description("Builds and tests engine core and  engine")
	.IsDependentOn("TestEngineCore")
	.IsDependentOn("TestEngine");

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("CheckTestErrors")
	.IsDependentOn("Package")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
	.Description("Targets to run on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("BuildTestAndPackage")
	.Description("Build, Test and Package")
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

// We can't use the BuildSettings.Target for this because Setup has
// not yet run and the settings have not been initialized.
RunTarget(Argument("target", Argument("t", "Default")));
