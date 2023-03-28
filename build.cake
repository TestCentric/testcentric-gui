#tool NuGet.CommandLine&version=6.0.0
#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1

const string SOLUTION = "testcentric-engine.sln";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-engine";

const string DEFAULT_VERSION = "2.0.0";
static readonly string[] VALID_CONFIGS = { "Release", "Debug" };

const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

const string TEST_BED_EXE = "test-bed.exe";

// Load scripts after defining constants
#load "./cake/building.cake"
#load "./cake/build-settings.cake"
#load "./cake/check-headers.cake"
#load "../TestCentric.Cake.Recipe/recipe/constants.cake"
#load "./cake/local-targets.cake"
#load "./cake/package-checks.cake"
#load "./cake/package-definitions.cake"
#load "./cake/package-tests.cake"
#load "./cake/test-reports.cake"
#load "./cake/test-results.cake"
#load "./cake/packaging.cake"
#load "./cake/publishing.cake"
#load "./cake/releasing.cake"
#load "./cake/testing.cake"
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
