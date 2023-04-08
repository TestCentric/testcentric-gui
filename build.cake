#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1
#tool nuget:?package=NuGet.CommandLine&version=6.0.0

const string SOLUTION = "testcentric-gui.sln";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-gui";

const string DEFAULT_VERSION = "2.0.0";
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
#load "../TestCentric.Cake.Recipe/recipe/building.cake"
#load "./cake/build-settings.cake"
#load "../TestCentric.Cake.Recipe/recipe/check-headers.cake"
#load "../TestCentric.Cake.Recipe/recipe/console-reporter.cake"
#load "../TestCentric.Cake.Recipe/recipe/constants.cake"
#load "./cake/gui-tester.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-checks.cake"
#load "./cake/package-definitions.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-tests.cake"
#load "./cake/packaging.cake"
#load "./cake/publishing.cake"
#load "./cake/releasing.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-reports.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-results.cake"
#load "./cake/testing.cake"
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
// TEARDOWN
//////////////////////////////////////////////////////////////////////

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// INITIALIZE BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
	Context,
	solutionFile: SOLUTION,
	exemptFiles: new [] { "Resource.cs", "TextCode.cs" }
);

TaskTeardown(context =>
{
	// The engine package does not restore correctly. As a temporary
	// fix, we install a local copy and then copy agents and
	// content to the output directory.
	if (context.Task.Name == "Build")
	{
		string tempEngineInstall = BuildSettings.ProjectDirectory + "tempEngineInstall/";

		CleanDirectory(tempEngineInstall);

		NuGetInstall("TestCentric.Engine", new NuGetInstallSettings()
		{
			Version = REF_ENGINE_VERSION,
			OutputDirectory = tempEngineInstall,
			ExcludeVersion = true
		});

		CopyFileToDirectory(
			tempEngineInstall + "TestCentric.Engine/content/testcentric.nuget.addins",
			BuildSettings.OutputDirectory);
		Information("Copied testcentric.nuget.addins");
		CopyDirectory(
			tempEngineInstall + "TestCentric.Engine/tools",
			BuildSettings.OutputDirectory);
		Information("Copied engine files");
	}
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

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
