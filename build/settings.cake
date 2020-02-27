//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

string target = Argument("target", "Default");
string configuration = Argument("configuration", "Release");
string packageVersion = Argument("packageVersion", "1.0.0");

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

int dash = packageVersion.IndexOf('-');
string version = dash > 0
    ? packageVersion.Substring(0, dash)
    : packageVersion;

if (configuration == "Debug")
    packageVersion += "-dbg";

//////////////////////////////////////////////////////////////////////
// DETERMINE BUILD ENVIRONMENT
//////////////////////////////////////////////////////////////////////

bool usingXBuild = EnvironmentVariable("USE_XBUILD") != null;

var msBuildSettings = new MSBuildSettings {
    Verbosity = Verbosity.Minimal,
    ToolVersion = MSBuildToolVersion.Default,//The highest available MSBuild tool version//VS2017
    Configuration = configuration,
    PlatformTarget = PlatformTarget.MSIL,
    MSBuildPlatform = MSBuildPlatform.Automatic,
    DetailedSummary = true,
};

var xBuildSettings = new XBuildSettings {
    Verbosity = Verbosity.Minimal,
    ToolVersion = XBuildToolVersion.Default,//The highest available XBuild tool version//NET40
    Configuration = configuration,
};

var nugetRestoreSettings = new NuGetRestoreSettings();
// Older Mono version was not picking up the testcentric source
if (usingXBuild)
    nugetRestoreSettings.Source = new string [] {
        "https://www.myget.org/F/testcentric/api/v2/",
        "https://www.myget.org/F/testcentric/api/v3/index.json",
        "https://www.nuget.org/api/v2/",
        "https://api.nuget.org/v3/index.json",
		"https://www.myget.org/F/nunit/api/v2/",
		"https://www.myget.org/F/nunit/api/v3/index.json"
    };

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// Directories
string PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
string PACKAGE_DIR = PROJECT_DIR + "package/";
string PACKAGE_IMAGE_DIR = PACKAGE_DIR + "images/";
string NUGET_DIR = PROJECT_DIR + "nuget/";
string CHOCO_DIR = PROJECT_DIR + "choco/";
string BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
string BIN_DIR_NET20 = BIN_DIR + "net20/";
string BIN_DIR_NET35 = BIN_DIR + "net35/";

// Packaging
string PACKAGE_NAME = "testcentric-gui";
string NUGET_PACKAGE_NAME = "TestCentric.GuiRunner";
string ZIP_TEST_DIR = PACKAGE_DIR + "test/zip/";
string NUGET_TEST_DIR = PACKAGE_DIR + "test/nuget/";
string CHOCO_TEST_DIR = PACKAGE_DIR + "test/choco/";

// Properties use the packageVersion, which may be changed in SetUp
string ZipPackage => $"{PACKAGE_DIR}{PACKAGE_NAME}-{packageVersion}.zip";
string NuGetPackage => $"{PACKAGE_DIR}{NUGET_PACKAGE_NAME}.{packageVersion}.nupkg";
string ChocolateyPackage => $"{PACKAGE_DIR}{PACKAGE_NAME}.{packageVersion}.nupkg";
string CurrentImageDir => $"{PACKAGE_IMAGE_DIR}{PACKAGE_NAME}-{packageVersion}/";

// Solution
string SOLUTION = "testcentric-gui.sln";

// GUI Testing
string GUI_RUNNER = "testcentric.exe";
string EXPERIMENTAL_RUNNER = "tc-next.exe";
string GUI_TESTS = "TestCentric.Gui.Tests.dll";
string EXPERIMENTAL_TESTS = "Experimental.Gui.Tests.dll";
string ALL_TESTS = "*.Tests.dll";

// Engine Testing
string ENGINE_TESTS = "testcentric.engine.tests";
string[] ENGINE_RUNTIMES = new string[] {"net40", "netcoreapp2.1"};
string ENGINE_CORE_TESTS = "testcentric.engine.core.tests";
string[] ENGINE_CORE_RUNTIMES = IsRunningOnWindows()
	? new string[] {"net40", "net35", "netcoreapp2.1", "netcoreapp1.1"}
	: new string[] {"net40", "net35", "netcoreapp2.1"};
string[] AGENT_RUNTIMES =new string[] { "net20" };

