#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1
#tool nuget:?package=NuGet.CommandLine&version=6.0.0

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
#load "./cake/BuildSettings.cake"
#load "../TestCentric.Cake.Recipe/recipe/check-headers.cake"
#load "../TestCentric.Cake.Recipe/recipe/ConsoleReporter.cake"
#load "../TestCentric.Cake.Recipe/recipe/constants.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-checks.cake"
#load "./cake/package-definitions.cake"
#load "../TestCentric.Cake.Recipe/recipe/PackageTest.cake"
#load "./cake/packaging.cake"
#load "../TestCentric.Cake.Recipe/recipe/publishing.cake"
#load "../TestCentric.Cake.Recipe/recipe/releasing.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-reports.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-results.cake"
#load "../TestCentric.Cake.Recipe/recipe/testing.cake"
#load "../TestCentric.Cake.Recipe/recipe/test-runners.cake"
#load "../TestCentric.Cake.Recipe/recipe/utilities.cake"
#load "../TestCentric.Cake.Recipe/recipe/versioning.cake"

#load "./cake/package-tests.cake"

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
// INITIALIZATION
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
	context: Context,
	title: "TestCentric.GuiRunner",
	solutionFile: "testcentric-gui.sln",
	unitTestRunner: new GuiSelfTester(),
	exemptFiles: new [] { "Resource.cs", "TextCode.cs" }
);

DefinePackageTests();

var NuGetPackage = new NuGetPackageDefinition(
	id: "TestCentric.GuiRunner",
	source: BuildSettings.NuGetDirectory + "TestCentric.GuiRunner.nuspec",
	basePath: BuildSettings.OutputDirectory,
	executable: "tools/testcentric.exe",
	checks: new PackageCheck[] {
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

var ChocolateyPackage = new ChocolateyPackageDefinition(
	id: "testcentric-gui",
	source: BuildSettings.ChocolateyDirectory + "testcentric-gui.nuspec",
	basePath: BuildSettings.OutputDirectory,
	executable: "tools/testcentric.exe",
	checks: new PackageCheck[] {
		HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual%20Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

var ZipPackage = new ZipPackageDefinition(
	id: "TestCentric.Gui.Runner",
	source: BuildSettings.NuGetDirectory + "TestCentric.Gui.Runner.nuspec",
	basePath: BuildSettings.OutputDirectory,
	executable: "bin/testcentric.exe",
	checks: new PackageCheck[] {
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
		HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.zip.addins"),
		HasDirectory("bin/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES),
		HasDirectory("bin/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

BuildSettings.Packages.Add(NuGetPackage);
BuildSettings.Packages.Add(ChocolateyPackage);
BuildSettings.Packages.Add(ZipPackage);

//////////////////////////////////////////////////////////////////////
// POST-BUILD ACTION
//////////////////////////////////////////////////////////////////////

// The engine package does not restore correctly. As a temporary
// fix, we install a local copy and then copy agents and
// content to the output directory.
TaskTeardown(context =>
{
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
// UNIT TEST RUNNER
//////////////////////////////////////////////////////////////////////

public class GuiSelfTester : TestRunner
{
	public override int Run(string arguments)
	{
		if (!arguments.Contains(" --run"))
			arguments += " --run";
		if (!arguments.Contains(" --unattended"))
			arguments += " --unattended";
		if (!arguments.Contains(" --full-gui"))
			arguments += " --full-gui";

		ExecutablePath = BuildSettings.OutputDirectory + "testcentric.exe";

		return base.Run(arguments);
	}
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package")
	.IsDependentOn("Publish")
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
