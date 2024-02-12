// NOTE: This must match what is actually referenced by
// the GUI test model project. Hopefully, this is a temporary
// fix, which we can get rid of in the future.
const string REF_ENGINE_VERSION = "2.0.0-dev00010";

// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-dev00082
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

#load "./package-tests.cake"

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

//////////////////////////////////////////////////////////////////////
// COMMON DEFINITIONS USED IN BOTH PACKAGES
//////////////////////////////////////////////////////////////////////

DefinePackageTests();

static readonly FilePath[] ENGINE_FILES = {
        "testcentric.engine.dll", "testcentric.engine.api.dll", "testcentric.metadata.dll"};
static readonly FilePath[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "nunit.uiexception.dll",
        "TestCentric.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "Mono.Options.dll" };
static readonly FilePath[] TREE_ICONS_JPG = {
        "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
static readonly FilePath[] TREE_ICONS_PNG = {
        "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

private const string GUI_DESCRIPTION =
	"The TestCentric Runner for NUnit (**TestCentric**) is a GUI runner aimed at eventually supporting a range of .NET testing frameworks. In the 1.x release series, we are concentrating on support of NUnit tests. The user interface is based on the layout and feature set of the of the original NUnit GUI, with the internals modified so as to run NUnit 3 tests." +
	"\r\n\nThis package includes the both the standard TestCentric GUI runner (`testcentric.exe`) and an experiental runner (`tc-next.exe`) which is available for... wait for it... experimentation! The package incorporates the TestCentric test engine, a modified version of the NUnit engine." +
	"\r\n\n### Features" +
	"\r\n\nMost features of the NUnit V2 Gui runner are supported. See CHANGES.txt for more detailed information." +
	"\r\n\nNUnit engine extensions are supported but no extensions are bundled with the GUI itself. They must be installed separately **using chocolatey**. In particular, to run NUnit V2 tests, you should install the **NUnit V2 Framework Driver Extension**." + 
	"\r\n\n**Warning:** When using the GUI chocolatey package, **only** chocolatey-packaged extensions will be availble. This is by design." +
	"\r\n\n### Prerequisites" +
	"\r\n\n**TestCentric** requires .NET 4.5 or later in order to function, although your tests may run in a separate process under other framework versions." +
	"\r\n\nProjects with tests to be run under **TestCentric** must already have some version of the NUnit framework installed separtely.";

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGES
//////////////////////////////////////////////////////////////////////

var nugetPackage = new NuGetPackage(
	id: "TestCentric.GuiRunner",
	description: GUI_DESCRIPTION,
	packageContent: new PackageContent()
		.WithRootFiles("../../LICENSE.txt", "../../NOTICES.txt", "../../CHANGES.txt", "../../testcentric.png")
		.WithDirectories(
			new DirectoryContent("tools").WithFiles(
				"testcentric.exe", "testcentric.exe.config", "TestCentric.Gui.Runner.dll",
				"nunit.uiexception.dll", "TestCentric.Gui.Model.dll", "Mono.Options.dll",
				"TestCentric.Engine.dll", "TestCentric.Engine.Api.dll", "TestCentric.InternalTrace.dll",
				"TestCentric.Metadata.dll", "TestCentric.Extensibility.dll", "TestCentric.Extensibility.Api.dll",
				"TestCentric.Engine.pdb", "../../nuget/testcentric.nuget.addins"),
			new DirectoryContent("tools/Images/Tree/Circles").WithFiles(
				"Images/Tree/Circles/Success.jpg", "Images/Tree/Circles/Failure.jpg", "Images/Tree/Circles/Ignored.jpg", "Images/Tree/Circles/Inconclusive.jpg", "Images/Tree/Circles/Skipped.jpg"),
			new DirectoryContent("tools/Images/Tree/Classic").WithFiles(
				"Images/Tree/Classic/Success.jpg", "Images/Tree/Classic/Failure.jpg", "Images/Tree/Classic/Ignored.jpg", "Images/Tree/Classic/Inconclusive.jpg", "Images/Tree/Classic/Skipped.jpg"),
			new DirectoryContent("tools/Images/Tree/Default").WithFiles(
				"Images/Tree/Default/Success.png", "Images/Tree/Default/Failure.png", "Images/Tree/Default/Ignored.png", "Images/Tree/Default/Inconclusive.png", "Images/Tree/Default/Skipped.png"),
			new DirectoryContent("tools/Images/Tree/Visual Studio").WithFiles(
				"Images/Tree/Visual Studio/Success.png", "Images/Tree/Visual Studio/Failure.png", "Images/Tree/Visual Studio/Ignored.png", "Images/Tree/Visual Studio/Inconclusive.png", "Images/Tree/Visual Studio/Skipped.png") )
		.WithDependencies(
			KnownExtensions.Net462PluggableAgent.NuGetPackage.LatestDevBuild,
			KnownExtensions.Net60PluggableAgent.NuGetPackage.LatestDevBuild,
			KnownExtensions.Net70PluggableAgent.NuGetPackage.LatestDevBuild,
			KnownExtensions.Net80PluggableAgent.NuGetPackage.LatestDevBuild
		),
	testRunner: new GuiSelfTester(BuildSettings.NuGetTestDirectory + "TestCentric.GuiRunner." + BuildSettings.PackageVersion + "/tools/testcentric.exe"),
	checks: new PackageCheck[] {
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	},
	tests: PackageTests
);

var chocolateyPackage = new ChocolateyPackage(
	id: "testcentric-gui",
	description: GUI_DESCRIPTION,
	packageContent: new PackageContent()
		.WithDirectories(
			new DirectoryContent("tools").WithFiles(
				"../../LICENSE.txt", "../../NOTICES.txt", "../../CHANGES.txt", "../../testcentric.png",
				"../../choco/VERIFICATION.txt", "../../choco/testcentric.choco.addins",
				"../../choco/testcentric-agent.exe.ignore",	"../../choco/testcentric-agent-x86.exe.ignore",
				"testcentric.exe", "testcentric.exe.config", "TestCentric.Gui.Runner.dll",
				"nunit.uiexception.dll", "TestCentric.Gui.Model.dll", "Mono.Options.dll",
				"TestCentric.Engine.dll", "TestCentric.Engine.Api.dll", "TestCentric.InternalTrace.dll",
				"TestCentric.Metadata.dll", "TestCentric.Extensibility.dll", "TestCentric.Extensibility.Api.dll",
				"testcentric.engine.pdb"),
			new DirectoryContent("tools/Images/Tree/Circles").WithFiles(
				"Images/Tree/Circles/Success.jpg", "Images/Tree/Circles/Failure.jpg", "Images/Tree/Circles/Ignored.jpg", "Images/Tree/Circles/Inconclusive.jpg", "Images/Tree/Circles/Skipped.jpg"),
			new DirectoryContent("tools/Images/Tree/Classic").WithFiles(
				"Images/Tree/Classic/Success.jpg", "Images/Tree/Classic/Failure.jpg", "Images/Tree/Classic/Ignored.jpg", "Images/Tree/Classic/Inconclusive.jpg", "Images/Tree/Classic/Skipped.jpg"),
			new DirectoryContent("tools/Images/Tree/Default").WithFiles(
				"Images/Tree/Default/Success.png", "Images/Tree/Default/Failure.png", "Images/Tree/Default/Ignored.png", "Images/Tree/Default/Inconclusive.png", "Images/Tree/Default/Skipped.png"),
			new DirectoryContent("tools/Images/Tree/Visual Studio").WithFiles(
				"Images/Tree/Visual Studio/Success.png", "Images/Tree/Visual Studio/Failure.png", "Images/Tree/Visual Studio/Ignored.png", "Images/Tree/Visual Studio/Inconclusive.png", "Images/Tree/Visual Studio/Skipped.png") )
		.WithDependencies(
			KnownExtensions.Net462PluggableAgent.ChocoPackage.LatestDevBuild,
			KnownExtensions.Net60PluggableAgent.ChocoPackage.LatestDevBuild,
			KnownExtensions.Net70PluggableAgent.ChocoPackage.LatestDevBuild,
			KnownExtensions.Net80PluggableAgent.ChocoPackage.LatestDevBuild
		),
	testRunner: new GuiSelfTester(BuildSettings.ChocolateyTestDirectory + "testcentric-gui." + BuildSettings.PackageVersion + "/tools/testcentric.exe"),
	checks: new PackageCheck[] {
		HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG),
	},
	tests: PackageTests
);

BuildSettings.Packages.Add(nugetPackage);
BuildSettings.Packages.Add(chocolateyPackage);

//////////////////////////////////////////////////////////////////////
// POST-BUILD ACTION
//////////////////////////////////////////////////////////////////////

// The current engine package does not restore correctly. As a temporary
// fix, we install a local copy and then copy agents and
// content to the output directory.
TaskTeardown(context =>
{
	if (context.Task.Name == "Build")
	{
		string engineInstallDir = BuildSettings.PackageTestDirectory;

		CleanDirectory(engineInstallDir);
		NuGetInstall("TestCentric.Engine", new NuGetInstallSettings()
		{
			Version = REF_ENGINE_VERSION,
			OutputDirectory = engineInstallDir,
			ExcludeVersion = true
		});

		CopyDirectory(
			engineInstallDir + "TestCentric.Engine/tools",
			BuildSettings.OutputDirectory);
		Information("Copied engine files to output directory");
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
// INDIVIDUAL PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PackageNuGet")
	.IsDependentOn("Build")
	.Does(() =>
	{
		nugetPackage.BuildVerifyAndTest();
	});

Task("PackageChocolatey")
	.IsDependentOn("Build")
	.Does(() =>
	{
		chocolateyPackage.BuildVerifyAndTest();
	});

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
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task ("Indirect")
	.IsDependentOn("CreateDraftRelease");
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

RunTarget(CommandLineOptions.Target.Value);
