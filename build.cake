#tool nuget:?package=NUnit.ConsoleRunner&version=3.9.0
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
// DETERMINE BUILD ENVIRONMENT
//////////////////////////////////////////////////////////////////////

string monoVersion = null;

Type type = Type.GetType("Mono.Runtime");
if (type != null)
{
    var displayName = type.GetMethod("GetDisplayName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    if (displayName != null)
        monoVersion = displayName.Invoke(null, null).ToString();
}

// Thanks to Pawel Troka for this idea. See https://github.com/cake-build/cake/issues/1631
bool isMonoButSupportsMsBuild = monoVersion!=null && System.Text.RegularExpressions.Regex.IsMatch(monoVersion,@"^([5-9]|\d{2,})\.\d+\.\d+(\.\d+)?");

var msBuildSettings = new MSBuildSettings {
    Verbosity = Verbosity.Minimal,
    ToolVersion = MSBuildToolVersion.Default,//The highest available MSBuild tool version//VS2017
    Configuration = configuration,
    PlatformTarget = PlatformTarget.MSIL,
    MSBuildPlatform = MSBuildPlatform.Automatic,
    DetailedSummary = true,
};

if(!IsRunningOnWindows() && isMonoButSupportsMsBuild)
{
    msBuildSettings.ToolPath = new FilePath(@"/usr/lib/mono/msbuild/15.0/bin/MSBuild.dll");//hack for Linux bug - missing MSBuild path
}

var xBuildSettings = new XBuildSettings {
    Verbosity = Verbosity.Minimal,
    ToolVersion = XBuildToolVersion.Default,//The highest available XBuild tool version//NET40
    Configuration = configuration,
};

var nugetRestoreSettings = new NuGetRestoreSettings();
// Older Mono version was not picking up the testcentric source
if (!IsRunningOnWindows() && !isMonoButSupportsMsBuild)
    nugetRestoreSettings.Source = new string [] {
        "https://www.myget.org/F/testcentric/api/v2/",
        "https://www.myget.org/F/testcentric/api/v3/index.json",
        "https://www.nuget.org/api/v2/",
        "https://api.nuget.org/v3/index.json"
    };

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// HACK: Engine Version - Must update this manually to match package used
var ENGINE_VERSION = "3.10.0-dev-00002";

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
    .Does(() =>
{
    NuGetRestore(SOLUTION, nugetRestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
        if(IsRunningOnWindows() || isMonoButSupportsMsBuild)
        {
            MSBuild(SOLUTION, msBuildSettings);
        }
        else
        {
            XBuild(SOLUTION, xBuildSettings);
        }

    // Temporary hack... needs update if we update the engine
    CopyFileToDirectory("packages/NUnit.Engine." + ENGINE_VERSION + "/lib/net20/nunit-agent.exe.config", BIN_DIR);
    CopyFileToDirectory("packages/NUnit.Engine." + ENGINE_VERSION + "/lib/net20/nunit-agent-x86.exe.config", BIN_DIR);
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
            BIN_DIR + "tc-next.exe",
            BIN_DIR + "tc-next.exe.config",
            BIN_DIR + "TestCentric.Common.dll",
            BIN_DIR + "TestCentric.Gui.Components.dll",
            BIN_DIR + "TestCentric.Gui.Runner.dll",
            BIN_DIR + "Experimental.Gui.Runner.dll",
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

        ChocolateyPack(CHOCO_DIR + PACKAGE_NAME + ".nuspec", 
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
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "tc-next.exe", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "tc-next.exe.config", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Common.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Gui.Components.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "TestCentric.Gui.Runner.dll", Target="tools" },
                    new ChocolateyNuSpecContent() { Source = BIN_DIR + "Experimental.Gui.Runner.dll", Target="tools" },
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

Task("All")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
