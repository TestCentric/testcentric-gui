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
#load "../TestCentric.Cake.Recipe/recipe/build-settings.cake"
#load "../TestCentric.Cake.Recipe/recipe/check-headers.cake"
#load "../TestCentric.Cake.Recipe/recipe/constants.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-checks.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-definition.cake"
#load "../TestCentric.Cake.Recipe/recipe/package-tests.cake"
#load "../TestCentric.Cake.Recipe/recipe/packaging.cake"
#load "../TestCentric.Cake.Recipe/recipe/publishing.cake"
#load "../TestCentric.Cake.Recipe/recipe/releasing.cake"
#load "../TestCentric.Cake.Recipe/recipe/testcentric-gui.cake"
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
// DEFINE PACKAGE TESTS
//////////////////////////////////////////////////////////////////////

//   Level 1 tests are run each time we build the packages
//   Level 2 tests are run for PRs and when packages will be published
//   Level 3 tests are run only when publishing a release

var packageTests = new List<PackageTest>();

// Tests of single assemblies targeting each runtime we support

packageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll targeting .NET 4.6.2",
    "engine-tests/net462/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll targeting .NET 3.5",
    "engine-tests/net35/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll targeting .NET Core 2.1",
    "engine-tests/netcoreapp2.1/mock-assembly.dll",
    MockAssemblyExpectedResult("NetCore31AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll targeting .NET Core 3.1",
    "engine-tests/netcoreapp3.1/mock-assembly.dll",
    MockAssemblyExpectedResult("NetCore31AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
    "engine-tests/netcoreapp1.1/mock-assembly.dll",
    MockAssemblyExpectedResult("NetCore31AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll targeting .NET 5.0",
    "engine-tests/net5.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net50AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll targeting .NET 6.0",
    "engine-tests/net6.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll targeting .NET 7.0",
    "engine-tests/net7.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net70AgentLauncher")));

static ExpectedResult MockAssemblyExpectedResult(params string[] agentNames)
{
    int ncopies = agentNames.Length;

    var assemblies = new ExpectedAssemblyResult[ncopies];
    for (int i = 0; i < ncopies; i++)
        assemblies[i] = new ExpectedAssemblyResult("mock-assembly.dll", agentNames[i]);

    return new ExpectedResult("Failed")
    {
        Total = 36 * ncopies,
        Passed = 23 * ncopies,
        Failed = 5 * ncopies,
        Warnings = 1 * ncopies,
        Inconclusive = 1 * ncopies,
        Skipped = 7 * ncopies,
        Assemblies = assemblies
    };
}

// AspNetCore Tests

packageTests.Add(new PackageTest(1, "AspNetCore31Test", "Run test using AspNetCore under .NET Core 3.1",
    "engine-tests/netcoreapp3.1/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "NetCore31AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
    "engine-tests/net5.0/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net50AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
    "engine-tests/net6.0/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
    }));

// TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
if (!BuildSettings.IsRunningOnAppVeyor)
    packageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
        "engine-tests/net7.0/aspnetcore-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
        }));

// Windows Forms Tests

// TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
if (!BuildSettings.IsRunningOnAppVeyor)
    packageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
        "engine-tests/net5.0-windows/windows-forms-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net50AgentLauncher") }
        }));

packageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
    "engine-tests/net6.0-windows/windows-forms-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
    }));

// TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
if (!BuildSettings.IsRunningOnAppVeyor)
    packageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
        "engine-tests/net7.0-windows/windows-forms-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
        }));

// Multiple Assembly Tests

packageTests.Add(new PackageTest(1, "Net35PlusNetCore21Test", "Run different builds of mock-assembly.dll together",
    "engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp2.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher", "NetCore31AgentLauncher")));

// TODO: Use --config option when it's supported by the extension.
// Current test relies on the fact that the Release config appears
// first in the project file.
if (BuildSettings.Configuration == "Release")
{
    packageTests.Add(new PackageTest(1, "NUnitProjectTest", "Run an NUnit project",
        "TestProject.nunit",
        new ExpectedResult("Failed")
        {
            Assemblies = new[] {
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "NetCore31AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net50AgentLauncher") }
        },
        BuildSettings.NUnitProjectLoader));
}

// NOTE: Package tests using a pluggable agent must be run after all tests
// that assume no pluggable agents are installed!

// TODO: Disabling Net20PluggableAgentTest until the agent is updated
//packageTests.Add(new PackageTest(1, "Net20PluggableAgentTest", "Run mock-assembly.dll targeting net35 using Net20PluggableAgent",
//    "engine-tests/net35/mock-assembly.dll",
//    new ExpectedResult("Failed")
//    {
//        Total = 36,
//        Passed = 23,
//        Failed = 5,
//        Warnings = 1,
//        Inconclusive = 1,
//        Skipped = 7,
//        Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "Net20AgentLauncher") }
//    },
//    Net20PluggableAgent));

packageTests.Add(new PackageTest(1, "NetCore21PluggableAgentTest", "Run mock-assembly.dll targeting Net Core 2.1 using NetCore21PluggableAgent",
    "engine-tests/netcoreapp2.1/mock-assembly.dll",
    new ExpectedResult("Failed")
    {
        Total = 36,
        Passed = 23,
        Failed = 5,
        Warnings = 1,
        Inconclusive = 1,
        Skipped = 7,
        Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "NetCore21AgentLauncher") }
    },
    BuildSettings.NetCore21PluggableAgent));

const string NET80_MOCK_ASSEMBLY = "../../../net80-pluggable-agent/bin/Release/tests/net8.0/mock-assembly.dll";
if (BuildSettings.IsLocalBuild && Context.FileExists(BuildSettings.OutputDirectory + NET80_MOCK_ASSEMBLY))
	packageTests.Add(new PackageTest(1, "NetCore80PluggableAgentTest", "Run mock-assembly.dll targeting Net 8.0 using NetCore80PluggableAgent",
		NET80_MOCK_ASSEMBLY,
		new ExpectedResult("Failed")
		{
			Total = 36,
			Passed = 23,
			Failed = 5,
			Warnings = 1,
			Inconclusive = 1,
			Skipped = 7,
			Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "Net80AgentLauncher") }
		},
		BuildSettings.Net80PluggableAgent));

// TODO: Disabling NUnitV2Test until the driver works
//packageTests.Add(new PackageTest(1, "NUnitV2Test", "Run tests using the V2 framework driver",
//	"v2-tests/net35/v2-test-assembly.dll",
//	new ExpectedResult("Failed")
//	{
//		Total = 28,
//		Passed = 18,
//		Failed = 5,
//		Warnings = 0,
//		Inconclusive = 1,
//		Skipped = 4
//	},
//	NUnitV2Driver));

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGES
//////////////////////////////////////////////////////////////////////

var EnginePackage = new NuGetPackage(
	id: "TestCentric.Engine",
	source: BuildSettings.NuGetDirectory + "TestCentric.Engine.nuspec",
	basePath: BuildSettings.OutputDirectory,
	testRunner: new TestCentricEngineTestBed(),
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(
			"testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"testcentric.engine.pdb", "testcentric.engine.core.pdb", "test-bed.exe", "test-bed.addins"),
		HasDirectory("content").WithFile("testcentric.nuget.addins"),
		HasDirectory("tools/agents/net462").WithFiles(
			"testcentric-agent.exe", "testcentric-agent.pdb", "testcentric-agent.exe.config",
			"testcentric-agent-x86.exe", "testcentric-agent-x86.pdb", "testcentric-agent-x86.exe.config",
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
			"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/netcoreapp3.1").WithFiles(
			"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
			"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net5.0").WithFiles(
			"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
			"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net6.0").WithFiles(
			"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
			"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net7.0").WithFiles(
			"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
			"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins")
	},
	tests: packageTests);

var EngineCorePackage = new NuGetPackage(
	id: "TestCentric.Engine.Core",
	source: BuildSettings.NuGetDirectory + "TestCentric.Engine.Core.nuspec",
	basePath: BuildSettings.ProjectDirectory,
	checks:new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib/net20").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
		HasDirectory("lib/net462").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
		HasDirectory("lib/netstandard2.0").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
		HasDirectory("lib/netcoreapp3.1").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"Microsoft.Extensions.DependencyModel.dll")
	});

var EngineApiPackage = new NuGetPackage(
	id: "TestCentric.Engine.Api",
	source: BuildSettings.NuGetDirectory + "TestCentric.Engine.Api.nuspec",
	basePath: BuildSettings.ProjectDirectory,
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb")
	});

BuildSettings.Packages.AddRange(new [] {
	EnginePackage,
	EngineCorePackage,
	EngineApiPackage
});

//////////////////////////////////////////////////////////////////////
// RUN TESTS OF TESTCENTRIC.ENGINE SEPARATELY
//////////////////////////////////////////////////////////////////////

Task("TestEngine")
	.Description("Tests the TestCentric Engine")
	.IsDependentOn("Build")
	.Does(() =>
	{
		NUnitLite.RunUnitTests("**/testcentric.engine.tests.exe");
	});

//////////////////////////////////////////////////////////////////////
// RUN TESTS OF TESTCENTRIC.ENGINE.CORE SEPARATELY
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does(() =>
	{
		NUnitLite.RunUnitTests("**/testcentric.engine.core.tests.exe|**/testcentric.engine.core.tests.dll");
	});

//////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST INDIVIDUAL PACKAGES
//////////////////////////////////////////////////////////////////////

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does(() =>
	{
		EnginePackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does(() =>
	{
		EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does(() =>
	{
		EngineApiPackage.BuildVerifyAndTest();
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
