// This file contains both constants and static readonly values, which
// are used as constants. The latter must not depend in any way on the
// contents of other cake files, which are loaded after this one.

// NOTE: Since GitVersion is only used when running under
// Windows, the default version should be updated to the 
// next version after each release.
const string DEFAULT_VERSION = "1.6.2";
const string DEFAULT_CONFIGURATION = "Release";

const string SOLUTION = "testcentric-gui.sln";

const string PACKAGE_NAME = "testcentric-gui";
const string NUGET_PACKAGE_NAME = "TestCentric.GuiRunner";
const string METADATA_PACKAGE_NAME = "TestCentric.Metadata";

const string GUI_RUNNER = "testcentric.exe";
const string EXPERIMENTAL_RUNNER = "tc-next.exe";
const string ALL_TESTS = "*.Tests.dll";

const string DEFAULT_TEST_RESULT_FILE = "TestResult.xml";

// URLs for uploading packages
private const string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
private const string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";
private const string CHOCO_PUSH_URL = "https://push.chocolatey.org/";

// Environment Variable names holding API keys
private const string MYGET_API_KEY = "MYGET_API_KEY";
private const string NUGET_API_KEY = "NUGET_API_KEY";
private const string CHOCO_API_KEY = "CHOCO_API_KEY";

// Environment Variable names holding GitHub identity of user
private const string GITHUB_OWNER = "TestCentric";
private const string GITHUB_REPO = "testcentric-gui";	
// Access token is used by GitReleaseManager
private const string GITHUB_ACCESS_TOKEN = "GITHUB_ACCESS_TOKEN";

// Pre-release labels that we publish
private static readonly string[] LABELS_WE_PUBLISH_ON_MYGET = { "dev", "pre" };
private static readonly string[] LABELS_WE_PUBLISH_ON_NUGET = { "alpha", "beta", "rc" };
private static readonly string[] LABELS_WE_PUBLISH_ON_CHOCOLATEY = { "alpha", "beta", "rc" };
