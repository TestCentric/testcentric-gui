#tool NuGet.CommandLine&version=6.0.0
#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1

const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-engine";

const string DEFAULT_VERSION = "2.0.0";
static readonly string[] VALID_CONFIGS = { "Release", "Debug" };

const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

const string TEST_BED_EXE = "test-bed.exe";

// Load scripts after defining constants
#load "../TestCentric.Cake.Recipe/recipe/building.cake"
#load "./cake/build-settings.cake"
#load "../TestCentric.Cake.Recipe/recipe/check-headers.cake"
#load "../TestCentric.Cake.Recipe/recipe/constants.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-checks.cake"
#load "./cake/package-definitions.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-tests.cake"
#load "../TestCentric.Cake.Recipe/recipe/packaging.cake"
#load "../TestCentric.Cake.Recipe/recipe/publishing.cake"
#load "../TestCentric.Cake.Recipe/recipe/releasing.cake"
#load "../TestCentric.Cake.Recipe/recipe/testing.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-reports.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-results.cake"
#load "../TestCentric.Cake.Recipe/recipe/utilities.cake"
#load "../TestCentric.Cake.Recipe/recipe/versioning.cake"

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
// INITIALIZE BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
	Context,
	"TestCentric.Engine",
	solutionFile: "testcentric-engine.sln",
	unitTests: "engine-tests/**/*.tests.exe|engine-tests/**/*.tests.dll");

if (BuildSystem.IsRunningOnAppVeyor)
		AppVeyor.UpdateBuildVersion(BuildSettings.PackageVersion + "-" + AppVeyor.Environment.Build.Number);

Information("Building {0} version {1} of TestCentric Engine.", BuildSettings.Configuration, BuildSettings.PackageVersion);

//////////////////////////////////////////////////////////////////////
// RUN TESTS OF TESTCENTRIC.ENGINE SEPARATELY
//////////////////////////////////////////////////////////////////////

Task("TestEngine")
	.Description("Tests the TestCentric Engine")
	.IsDependentOn("Build")
	.Does(() =>
	{
		foreach (var runtime in BuildSettings.EngineRuntimes)
			RunNUnitLite("testcentric.engine.tests", runtime, $"{BuildSettings.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// RUN TESTS OF TESTCENTRIC.ENGINE.CORE SEPARATELY
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does(() =>
	{
		foreach (var runtime in BuildSettings.EngineCoreRuntimes)
		{
			// Only .NET Standard we currently build is 2.0
			var testUnder = runtime == "netstandard2.0" ? "netcoreapp2.1" : runtime;
			RunNUnitLite("testcentric.engine.core.tests", testUnder, $"{BuildSettings.OutputDirectory}engine-tests/{testUnder}/");
		}
	});

//////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST INDIVIDUAL PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does(() =>
	{
		BuildSettings.EnginePackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does(() =>
	{
		BuildSettings.EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does(() =>
	{
		BuildSettings.EngineApiPackage.BuildVerifyAndTest();
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package")
	.IsDependentOn("Publish")
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
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

// We can't use the BuildSettings.Target for this because Setup has
// not yet run and the settings have not been initialized.
RunTarget(Argument("target", Argument("t", "Default")));
