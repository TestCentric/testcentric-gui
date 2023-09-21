#tool NuGet.CommandLine&version=6.0.0

const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

const string TEST_BED_EXE = "test-bed.exe";

// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-dev00048
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

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
	githubRepository: "testcentric-engine",
	unitTests: "**/*.tests.exe|**/*.tests.dll"
);

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGE TESTS
//////////////////////////////////////////////////////////////////////

//   Level 1 tests are run each time we build the packages
//   Level 2 tests are run for PRs and when packages will be published
//   Level 3 tests are run only when publishing a release

var packageTests = new List<PackageTest>();

// Tests of single assemblies targeting each runtime we support

packageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll targeting .NET 4.6.2",
    "engine-tests/net462/mock-assembly.dll --trace:Debug",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll targeting .NET 3.5",
    "engine-tests/net35/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll targeting .NET Core 2.1",
    "engine-tests/netcoreapp2.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll targeting .NET Core 3.1",
    "engine-tests/netcoreapp3.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
    "engine-tests/netcoreapp1.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll targeting .NET 5.0",
    "engine-tests/net5.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll targeting .NET 6.0",
    "engine-tests/net6.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll targeting .NET 7.0",
    "engine-tests/net7.0/mock-assembly.dll --trace:Debug",
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
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
    "engine-tests/net5.0/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
    "engine-tests/net6.0/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
    "engine-tests/net7.0/aspnetcore-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
    }));

// Windows Forms Tests

packageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
    "engine-tests/net5.0-windows/windows-forms-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
    "engine-tests/net6.0-windows/windows-forms-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
    }));

packageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
    "engine-tests/net7.0-windows/windows-forms-test.dll",
    new ExpectedResult("Passed")
    {
        Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
    }));

// Multiple Assembly Tests

packageTests.Add(new PackageTest(1, "Net35PlusNetCore21Test", "Run different builds of mock-assembly.dll together",
    "engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp2.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher", "Net60AgentLauncher")));

// TODO: Use --config option when it's supported by the extension.
// Current test relies on the fact that the Release config appears
// first in the project file.
if (BuildSettings.Configuration == "Release")
{
    packageTests.Add(new PackageTest(2, "NUnitProjectTest", "Run an NUnit project",
        "TestProject.nunit",
        new ExpectedResult("Failed")
        {
            Assemblies = new[] {
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net60AgentLauncher"),
                            new ExpectedAssemblyResult("mock-assembly.dll", "Net60AgentLauncher") }
        },
        NUnitProjectLoader));
}

// NOTE: Package tests using a pluggable agent must be run after all tests
// that assume no pluggable agents are installed!

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
	//source: "src/TestEngine/testcentric.engine/testcentric.engine.csproj",
	description: "This package provides the TestCentric Engine, used by runner applications to load and excute NUnit tests.",
	packageContent: new PackageContent(
		new FilePath[] { "../../LICENSE.txt", "../../testcentric.png" },
		new DirectoryContent("tools").WithFiles(
			"testcentric.engine.dll", "testcentric.engine.core.dll", "testcentric.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "nunit.engine.api.dll",
			"testcentric.engine.pdb", "testcentric.engine.core.pdb", "test-bed.exe",
			"test-bed.addins", "../../testcentric.nuget.addins")),
	testRunner: new TestCentricEngineTestBed(),
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(
			"testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
			"testcentric.engine.pdb", "testcentric.engine.core.pdb", "test-bed.exe",
			"test-bed.addins", "testcentric.nuget.addins")
	},
	tests: packageTests,
	preloadedExtensions: new [] {
		new PackageReference("TestCentric.Extension.Net462PluggableAgent", "2.3.0-dev00007"),
		new PackageReference("TestCentric.Extension.Net60PluggableAgent", "2.3.0-dev00003"),
		new PackageReference("TestCentric.Extension.Net70PluggableAgent", "2.3.0-dev00004") }
);

var EngineCorePackage = new NuGetPackage(
	id: "TestCentric.Engine.Core",
	title: "TestCentric Engine Core Assembly",
	description: "This package includes the TestCentric engine.core assembly, which forms part of the TestCentric engine. It is provided in a separate package use in creating pluggable agents.",
	basePath: "src/TestEngine/testcentric.engine.core/bin/" + BuildSettings.Configuration,
	packageContent: new PackageContent(
		new FilePath[] { "../../../../../LICENSE.txt", "../../../../../testcentric.png" },
		new DirectoryContent("lib/net20").WithFiles(
			"net20/testcentric.engine.core.dll", "net20/testcentric.engine.core.pdb",
			"net20/testcentric.engine.metadata.dll", "net20/testcentric.extensibility.dll",
			"net20/nunit.engine.api.dll"),
		new DirectoryContent("lib/net462").WithFiles(
			"net462/testcentric.engine.core.dll", "net462/testcentric.engine.core.pdb",
			"net462/testcentric.engine.metadata.dll", "net462/testcentric.extensibility.dll",
			"net462/nunit.engine.api.dll"),
		new DirectoryContent("lib/netstandard2.0").WithFiles(
			"netstandard2.0/testcentric.engine.core.dll", "netstandard2.0/testcentric.engine.core.pdb",
			"netstandard2.0/testcentric.engine.metadata.dll", "netstandard2.0/testcentric.extensibility.dll",
			"netstandard2.0/nunit.engine.api.dll"),
		new DirectoryContent("lib/netcoreapp3.1").WithFiles(
			"netcoreapp3.1/testcentric.engine.core.dll", "netcoreapp3.1/testcentric.engine.core.pdb",
			"netcoreapp3.1/testcentric.engine.metadata.dll", "netcoreapp3.1/testcentric.extensibility.dll",
			"netcoreapp3.1/Microsoft.Extensions.DependencyModel.dll", "netcoreapp3.1/nunit.engine.api.dll")),
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
	title: "TestCentric Engine Api Assembly",
	description: "This package includes the testcentric.agent.api assembly, containing the interfaces used in creating pluggable agents.",
	basePath: "src/TestEngine/testcentric.engine.api/bin/" + BuildSettings.Configuration,
	packageContent: new PackageContent(
		new FilePath[] { "../../../../../LICENSE.txt", "../../../../../testcentric.png" },
		new DirectoryContent("lib/netstandard2.0").WithFiles(
			"netstandard2.0/testcentric.engine.api.dll", "netstandard2.0/testcentric.engine.api.pdb")),
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll")
	});

BuildSettings.Packages.AddRange(new [] {
	EngineApiPackage,
	EngineCorePackage,
	EnginePackage
});

//////////////////////////////////////////////////////////////////////
// TEST BED RUNNER
//////////////////////////////////////////////////////////////////////

public class TestCentricEngineTestBed : TestRunner
{
	public TestCentricEngineTestBed()
	{
		ExecutablePath = BuildSettings.NuGetTestDirectory + "TestCentric.Engine/tools/test-bed.exe";
	}

	public override int Run(string arguments)
	{
		return BuildSettings.Context.StartProcess(ExecutablePath, new ProcessSettings()
		{
			Arguments = arguments,
			WorkingDirectory = BuildSettings.OutputDirectory
		});
	}
}

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

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does(() =>
	{
		EngineApiPackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does(() =>
	{
		EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does(() =>
	{
		EnginePackage.BuildVerifyAndTest();
	});

// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("PackageEngineApi")
	.IsDependentOn("PackageEngineCore")
	.IsDependentOn("PackageEngine")
	.IsDependentOn("Publish")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
	.Description("Targets to run on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

// We can't use the BuildSettings.Target for this because Setup has
// not yet run and the settings have not been initialized.
RunTarget(Argument("target", Argument("t", "Default")));
