// Load the recipe
//#load nuget:?package=TestCentric.Cake.Recipe&version=1.3.2-dev00003
// Comment out above line and uncomment below for local tests of recipe changes
#load ../TestCentric.Cake.Recipe/recipe/*.cake

#load "./package-tests.cake"

//////////////////////////////////////////////////////////////////////
// INITIALIZATION
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
    context: Context,
    title: "TestCentric.GuiRunner",
    solutionFile: "testcentric-gui.sln",
    githubRepository: "testcentric-gui",
    // The V1 build uses two different sets of headers for engine vs gui.
    // The current build of TestCentric.Cake.Recipe doesn't support this.
    suppressHeaderCheck: true
);

//////////////////////////////////////////////////////////////////////
// COMMON DEFINITIONS USED IN ALL PACKAGES
//////////////////////////////////////////////////////////////////////

DefinePackageTests();

static readonly FilePath[] ENGINE_FILES = {
        "testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll" };
static readonly FilePath[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "nunit.uiexception.dll", "TestCentric.Gui.Components.dll",
        "TestCentric.Common.dll", "TestCentric.Gui.Runner.dll", "TestCentric.Gui.Model.dll" };
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
    "\r\n\nProjects with tests to be run under **TestCentric** must already have some version of the NUnit framework installed separately.";

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGES
//////////////////////////////////////////////////////////////////////

var nugetPackage = new NuGetPackage(
    id: "TestCentric.GuiRunner",
    description: GUI_DESCRIPTION,
    source: "nuget/TestCentric.GuiRunner.nuspec",
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
    source: "choco/testcentric-gui.nuspec",
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
// PACKAGE TEST RUNNER
//////////////////////////////////////////////////////////////////////

public class GuiSelfTester : TestRunner, IPackageTestRunner
{
    private FilePath _executablePath;

    // NOTE: When constructed as an argument to BuildSettings.Initialize(),
    // the executable path is not yet known and should not be provided.
    public GuiSelfTester(string executablePath = null)
    {
        _executablePath = executablePath;
    }

    public int RunPackageTest(string arguments)
    {
        if (!arguments.Contains(" --run"))
            arguments += " --run";
        if (!arguments.Contains(" --unattended"))
            arguments += " --unattended";

        if (_executablePath == null)
            _executablePath = BuildSettings.OutputDirectory + "testcentric.exe";

        Console.WriteLine($"Running {_executablePath} with arguments {arguments}");
        return base.RunTest(_executablePath, arguments);
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
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.Run();
