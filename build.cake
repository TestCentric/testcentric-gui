#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1
#tool nuget:?package=NuGet.CommandLine&version=6.0.0

const string SOLUTION = "testcentric-gui.sln";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-gui";

const string DEFAULT_VERSION = "2.0.0";
const string DEFAULT_CONFIGURATION = "Release";
static string[] VALID_CONFIGS = new [] { "Release", "Debug" };

// NOTE: This must match what is actually referenced by
// the GUI test model project. Hopefully, this is a temporary
// fix, which we can get rid of in the future.
const string REF_ENGINE_VERSION = "2.0.0-dev00053";

const string PACKAGE_NAME = "testcentric-gui";
const string NUGET_PACKAGE_NAME = "TestCentric.GuiRunner";

const string GUI_RUNNER = "testcentric.exe";
const string GUI_TESTS = "*.Tests.dll";

// Load scripts after defining constants
#load "./cake/build-settings.cake"

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

    Information("Building {0} version {1} of TestCentric GUI.", settings.Configuration, settings.PackageVersion);

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
    MSBuild(SOLUTION, settings.MSBuildSettings.WithProperty("Version", settings.PackageVersion));

	// The package does not restore correctly. As a temporary
	// fix, we install a local copy and then copy agents and
	// content to the output directory.

	string tempEngineInstall = settings.ProjectDirectory + "tempEngineInstall/";

	CleanDirectory(tempEngineInstall);

	NuGetInstall("TestCentric.Engine", new NuGetInstallSettings()
	{
		Version = REF_ENGINE_VERSION,
		OutputDirectory = tempEngineInstall,
		ExcludeVersion = true
	});

	CopyFileToDirectory(
		tempEngineInstall + "TestCentric.Engine/content/testcentric.nuget.addins",
		settings.OutputDirectory);
	Information("Copied testcentric.nuget.addins");
	CopyDirectory(
		tempEngineInstall + "TestCentric.Engine/tools",
		settings.OutputDirectory);
	Information("Copied engine files");

});

//////////////////////////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////////////////////////

static var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TESTS OF THE GUI
//////////////////////////////////////////////////////////////////////

Task("Test")
	.IsDependentOn("Build")
	.Does<BuildSettings>((settings) =>
	{
		var guiTests = GetFiles(settings.OutputDirectory + GUI_TESTS);
		var args = new StringBuilder();
		foreach (var test in guiTests)
			args.Append($"\"{test}\" ");

		var guiTester = new GuiTester(settings);
		Information ($"Running {settings.OutputDirectory + GUI_RUNNER} with arguments {args}");
		guiTester.RunGuiUnattended(settings.OutputDirectory + GUI_RUNNER, args.ToString());
		var result = new ActualResult(settings.OutputDirectory + "TestResult.xml");

		new ConsoleReporter(result).Display();

		if (result.OverallResult == "Failed")
			throw new System.Exception("There were test failures or errors. See listing.");
	});

////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST EACH PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.Description("Build and Test the NuGet Package")
	.Does<BuildSettings>(settings =>
	{
		settings.NuGetPackage.BuildVerifyAndTest();
	});

Task("PackageChocolatey")
	.Description("Build and Test the Chocolatey Package")
	.Does<BuildSettings>(settings =>
	{
		settings.ChocolateyPackage.BuildVerifyAndTest();
	});

Task("CreateZipImage")
	.Description("Create image used for zip package")
	.Does<BuildSettings>(settings => {
		Information("Creating Zip Image Directory");

		CreateDirectory(settings.PackageDirectory);
		CreateZipImage(settings);
	});

Task("PackageZip")
	.Description("Build and Test the Zip Package")
	.IsDependentOn("CreateZipImage")
	.Does<BuildSettings>(settings =>
	{
		settings.ZipPackage.BuildVerifyAndTest();
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
	.Does<BuildSettings>((settings) =>
	{
        if (!settings.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
		else if (settings.NoPush)
			Information("NoPush option suppressing publication to MyGet");
        else
            try
			{
				PushNuGetPackage(settings.NuGetPackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
				PushChocolateyPackage(settings.ChocolateyPackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
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
		else
			try
			{
				PushNuGetPackage(settings.NuGetPackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
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
	.Does<BuildSettings>((settings) =>
	{
		if (!settings.ShouldPublishToChocolatey)
			Information("Nothing to publish to Chocolatey from this run.");
		else
			try
			{
				PushChocolateyPackage(settings.ChocolateyPackage.PackageFilePath, settings.ChocolateyApiKey, settings.ChocolateyPushUrl);
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
			string releaseName = $"TestCentric {milestone}";

			Information($"Creating draft release for {releaseName}");

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

//////////////////////////////////////////////////////////////////////
// CREATE A PRODUCTION RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateProductionRelease")
	.Does<BuildSettings>((settings) =>
	{
		if (settings.IsProductionRelease)
		{
			// Exit if any PackageTests failed
			CheckTestErrors(ref ErrorDetail);

			string token = settings.GitHubAccessToken;
			string tagName = settings.PackageVersion;
			string assets = settings.GitHubReleaseAssets;

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
	.IsDependentOn("PackageExistingBuild");

Task("PackageExistingBuild")
	.IsDependentOn("PackageNuGet")
	.IsDependentOn("PackageChocolatey")
	.IsDependentOn("PackageZip");

//Task("PackageZip")
//	.IsDependentOn("BuildZipPackage")
//	.IsDependentOn("VerifyZipPackage")
//	.IsDependentOn("TestZipPackage");

//Task("PackageNuGet")
//	.IsDependentOn("BuildNuGetPackage")
//	.IsDependentOn("VerifyNuGetPackage")
//	.IsDependentOn("TestNuGetPackage");

//Task("PackageChocolatey")
//	.IsDependentOn("BuildChocolateyPackage")
//	.IsDependentOn("VerifyChocolateyPackage")
//	.IsDependentOn("TestChocolateyPackage");

Task("AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("BuildTestAndPackage")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", Argument("t", "Default")));
