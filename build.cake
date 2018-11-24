#tool nuget:?package=NUnit.ConsoleRunner&version=3.8.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

var version = "1.0";
var modifier = "-alpha2";
var dbgSuffix = configuration == "Debug" ? "-dbg" : "";

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// Directories
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";
var CHOCO_DIR = PROJECT_DIR + "choco/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";

// Packaging
var PACKAGE_NAME = "testcentric-gui";
var PACKAGE_VERSION = version + modifier + dbgSuffix;

// Solution
var SOLUTION = "testcentric-gui.sln";

// Test Assembly Pattern
var ALL_TESTS = BIN_DIR + "*.Tests.dll";

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(BIN_DIR);
});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(SOLUTION);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(SOLUTION, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(SOLUTION, settings =>
        settings.SetConfiguration(configuration));
    }

    // Temporary hack... needs update if we update the engine
    CopyFileToDirectory("packages/NUnit.Engine.3.9.0/lib/nunit-agent.exe.config", BIN_DIR);
    CopyFileToDirectory("packages/NUnit.Engine.3.9.0/lib/nunit-agent-x86.exe.config", BIN_DIR);
});

//////////////////////////////////////////////////////////////////////
// TEST
//////////////////////////////////////////////////////////////////////

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(ALL_TESTS, new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// PACKAGE ZIP
//////////////////////////////////////////////////////////////////////

Task("PackageZip")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        CopyFileToDirectory("LICENSE.txt", BIN_DIR);
        CopyFileToDirectory("NOTICES.txt", BIN_DIR);
        CopyFileToDirectory("CHANGES.txt", BIN_DIR);

        var zipFiles = new FilePath[]
        {
            BIN_DIR + "LICENSE.txt",
            BIN_DIR + "NOTICES.txt",
            BIN_DIR + "CHANGES.txt",
            BIN_DIR + "testcentric.exe",
            BIN_DIR + "testcentric.exe.config",
            BIN_DIR + "TestCentric.Common.dll",
            BIN_DIR + "TestCentric.Gui.Components.dll",
            BIN_DIR + "TestCentric.Gui.Runner.dll",
            BIN_DIR + "nunit.uiexception.dll",
            BIN_DIR + "TestCentric.Gui.Model.dll",
            BIN_DIR + "nunit.engine.api.dll",
            BIN_DIR + "nunit.engine.dll",
            BIN_DIR + "Mono.Cecil.dll",
            BIN_DIR + "nunit-agent.exe",
            BIN_DIR + "nunit-agent.exe.config",
            BIN_DIR + "nunit-agent-x86.exe",
            BIN_DIR + "nunit-agent-x86.exe.config"
        };

        Zip(BIN_DIR, File(PACKAGE_DIR + PACKAGE_NAME + "-" + PACKAGE_VERSION + ".zip"), zipFiles);
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE CHOCOLATEY
//////////////////////////////////////////////////////////////////////

Task("PackageChocolatey")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        ChocolateyPack("choco/" + PACKAGE_NAME + ".nuspec", 
            new ChocolateyPackSettings()
            {
                Version = PACKAGE_VERSION,
                OutputDirectory = PACKAGE_DIR,
                Files = new ChocolateyNuSpecContent[]
                {
                    new ChocolateyNuSpecContent() { Source = PROJECT_DIR + "LICENSE.txt", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = PROJECT_DIR + "NOTICES.txt", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = PROJECT_DIR + "CHANGES.txt", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "VERIFICATION.txt", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "testcentric.exe", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "testcentric.exe.config", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Common.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Gui.Components.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Gui.Runner.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit.uiexception.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Gui.Model.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit.engine.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit.engine.api.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "Mono.Cecil.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit-agent.exe", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit-agent.exe.config", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "nunit-agent.exe.ignore", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit-agent-x86.exe", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "nunit-agent-x86.exe.config", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "nunit-agent-x86.exe.ignore", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = CHOCO_DIR + "nunit.choco.addins", Target="tools" }
                }
            });
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
    .IsDependentOn("PackageZip")
    .IsDependentOn("PackageChocolatey");

Task("Appveyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("PackageZip");

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
