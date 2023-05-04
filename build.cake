#tool nuget:?package=GitVersion.CommandLine&version=5.6.3

using System.Xml.Serialization;

const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-gui";

const string DEFAULT_VERSION = "2.0.0";
static string[] VALID_CONFIGS = new [] { "Release", "Debug" };

// NOTE: This must match what is actually referenced by
// the GUI test model project. Hopefully, this is a temporary
// fix, which we can get rid of in the future.
const string REF_ENGINE_VERSION = "2.0.0-dev00007";

const string PACKAGE_NAME = "testcentric-gui";
const string NUGET_PACKAGE_NAME = "TestCentric.GuiRunner";

const string GUI_RUNNER = "testcentric.exe";
const string GUI_TESTS = "*.Tests.dll";

// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.0.0-dev00061
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

#load "./package-tests.cake"

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
	githubRepository: "testcentric-gui",
	exemptFiles: new [] { "Resource.cs", "TextCode.cs" }
);

DefinePackageTests();

static readonly FilePath[] ENGINE_FILES = {
        "testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll"};
static readonly FilePath[] ENGINE_CORE_FILES = {
        "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll" };
static readonly FilePath[] NET_FRAMEWORK_AGENT_FILES = {
        "testcentric-agent.exe", "testcentric-agent.exe.config", "testcentric-agent-x86.exe", "testcentric-agent-x86.exe.config" };
static readonly FilePath[] NET_CORE_AGENT_FILES = {
        "testcentric-agent.dll", "testcentric-agent.dll.config" };
static readonly FilePath[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "nunit.uiexception.dll",
        "TestCentric.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "Mono.Options.dll" };
static readonly FilePath[] TREE_ICONS_JPG = {
        "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
static readonly FilePath[] TREE_ICONS_PNG = {
        "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

var nugetPackage = new NuGetPackage(
	id: "TestCentric.GuiRunner",
	source: "nuget/TestCentric.GuiRunner.nuspec",
	basePath: BuildSettings.OutputDirectory,
	testRunner: new GuiSelfTester(BuildSettings.NuGetTestDirectory + "TestCentric.GuiRunner/tools/testcentric.exe"),
	checks: new PackageCheck[] {
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

var chocolateyPackage = new ChocolateyPackage(
	id: "testcentric-gui",
	source: BuildSettings.ChocolateyDirectory + "testcentric-gui.nuspec",
	basePath: BuildSettings.OutputDirectory,
	testRunner: new GuiSelfTester(BuildSettings.ChocolateyTestDirectory + "testcentric-gui/tools/testcentric.exe"),
	checks: new PackageCheck[] {
		HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

var zipPackage = new ZipPackage(
	id: "TestCentric.GuiRunner",
	source: BuildSettings.ZipDirectory + "TestCentric.GuiRunner.zspec",
	basePath: BuildSettings.OutputDirectory,
	testRunner: new GuiSelfTester(BuildSettings.ZipTestDirectory + "TestCentric.GuiRunner/bin/testcentric.exe"),
	checks: new PackageCheck[] {
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
		HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.zip.addins"),
		HasDirectory("bin/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES),
		HasDirectory("bin/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES),
		HasDirectory("bin/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
		HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests);

BuildSettings.Packages.Add(nugetPackage);
BuildSettings.Packages.Add(chocolateyPackage);
BuildSettings.Packages.Add(zipPackage);

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
// PACKAGE TEST RUNNER
//////////////////////////////////////////////////////////////////////

public class GuiSelfTester : TestRunner
{
	// NOTE: When constructed as an argument to BuildSettings.Initialize(),
	// the executable path is not yet known and should not be provided.
	public GuiSelfTester(string executablePath = null)
	{
		ExecutablePath = executablePath;
	}

	public override int Run(string arguments)
	{


		if (!arguments.Contains(" --run"))
			arguments += " --run";
		if (!arguments.Contains(" --unattended"))
			arguments += " --unattended";
		if (!arguments.Contains(" --full-gui"))
			arguments += " --full-gui";

		if (ExecutablePath == null)
			ExecutablePath = BuildSettings.OutputDirectory + "testcentric.exe";

		return base.Run(arguments);
	}
}

//////////////////////////////////////////////////////////////////////
// INDIVIDUAL TEST RUNS
//////////////////////////////////////////////////////////////////////

Task("RunTestCentricGuiTests")
	.IsDependentOn("Build")
	.Does(() =>
	{
		new GuiSelfTester().Run(BuildSettings.OutputDirectory + "TestCentric.Gui.Tests.dll");

		var result = new ActualResult(BuildSettings.OutputDirectory + "TestResult.xml");

		new ConsoleReporter(result).Display();

		if (result.OverallResult == "Failed")
			throw new System.Exception("There were test failures or errors. See listing.");
	});

//////////////////////////////////////////////////////////////////////
// INDIVIDUAL PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.Does(() =>
	{
		nugetPackage.BuildVerifyAndTest();
	});

Task("PackageChocolatey")
	.Does(() =>
	{
		chocolateyPackage.BuildVerifyAndTest();
	});

Task("PackageZip")
	.Does(() =>
	{
		zipPackage.BuildVerifyAndTest();
	});

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

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", Argument("t", "Default")));
