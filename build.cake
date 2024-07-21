// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.2.1-dev00010
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
	githubRepository: "testcentric-engine"
	//unitTests: "**/*.tests.exe|**/*.tests.dll"
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
    "engine-tests/net462/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll targeting .NET 3.5",
    "engine-tests/net35/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher")));

packageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll targeting .NET Core 3.1",
    "engine-tests/netcoreapp3.1/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

if (!BuildSettings.IsRunningOnAppVeyor)
    packageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll targeting .NET Core 2.1",
        "engine-tests/netcoreapp2.1/mock-assembly.dll",
        MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll targeting .NET 5.0",
    "engine-tests/net5.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll targeting .NET 6.0",
    "engine-tests/net6.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net60AgentLauncher")));

packageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll targeting .NET 7.0",
    "engine-tests/net7.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net70AgentLauncher")));

if (!BuildSettings.IsRunningOnAppVeyor)
    packageTests.Add(new PackageTest(1, "Net80Test", "Run mock-assembly.dll targeting .NET 8.0",
        "engine-tests/net8.0/mock-assembly.dll",
        MockAssemblyExpectedResult("Net80AgentLauncher")));

static ExpectedResult MockAssemblyExpectedResult(params string[] agentNames)
{
    int ncopies = agentNames.Length;

    var assemblies = new ExpectedAssemblyResult[ncopies];
    for (int i = 0; i < ncopies; i++)
        assemblies[i] = new ExpectedAssemblyResult("mock-assembly.dll", agentNames[i]);

    return new ExpectedResult("Failed")
    {
        Total = 37 * ncopies,
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
    "engine-tests/net35/mock-assembly.dll engine-tests/net6.0/mock-assembly.dll",
    MockAssemblyExpectedResult("Net462AgentLauncher", "Net60AgentLauncher")));

// TODO: Use --config option when it's supported by the extension.
// Current test relies on the fact that the Release config appears
// first in the project file.
// Disabling entirely for now
//if (BuildSettings.Configuration == "Release")
//{
//    packageTests.Add(new PackageTest(1, "NUnitProjectTest", "Run an NUnit project",
//        "TestProject.nunit",
//        new ExpectedResult("Failed")
//        {
//            Assemblies = new[] {
//                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
//                            new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
//                            new ExpectedAssemblyResult("mock-assembly.dll", "Net60AgentLauncher"),
//                            new ExpectedAssemblyResult("mock-assembly.dll", "Net60AgentLauncher") }
//        },
//        KnownExtensions.NUnitProjectLoader));
//}

// NOTE: Package tests using a pluggable agent must be run after all tests
// that assume no pluggable agents are installed!

// TODO: Disabling NUnitV2Test until the driver works
//packageTests.Add(new PackageTest(1, "NUnitV2Test", "Run tests using the V2 framework driver",
//	"v2-tests/net35/v2-test-assembly.dll --trace:Debug",
//	new ExpectedResult("Failed")
//	{
//		Total = 28,
//		Passed = 18,
//		Failed = 5,
//		Warnings = 0,
//		Inconclusive = 1,
//		Skipped = 4
//	},
//	KnownExtensions.NUnitV2Driver));

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGE
//////////////////////////////////////////////////////////////////////

BuildSettings.Packages.Add(new NuGetPackage(
	id: "TestCentric.Engine",
	//source: "src/TestEngine/testcentric.engine/testcentric.engine.csproj",
	description: "This package provides the TestCentric Engine, used by runner applications to load and excute NUnit tests.",
	packageContent: new PackageContent(
		new FilePath[] { "../../LICENSE.txt", "../../testcentric.png" },
		new DirectoryContent("lib").WithFiles(
			"testcentric.engine.dll", "testcentric.engine.api.dll", "nunit.engine.api.dll",
			"testcentric.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll", "TestCentric.InternalTrace.dll",
			"testcentric.engine.pdb", "test-bed.exe", "test-bed.exe.config",
			"test-bed.addins", "../../testcentric.nuget.addins")),
	testRunner: new TestCentricEngineTestBed(),
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib").WithFiles(
			"testcentric.engine.dll", "testcentric.engine.api.dll", "nunit.engine.api.dll",
			"testcentric.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll", "TestCentric.InternalTrace.dll",
			"testcentric.engine.pdb", "test-bed.exe", "test-bed.exe.config",
			"test-bed.addins", "testcentric.nuget.addins")
	},
	tests: packageTests,
	preloadedExtensions: new [] {
        KnownExtensions.Net462PluggableAgent.NuGetPackage.LatestDevBuild,
        KnownExtensions.Net60PluggableAgent.NuGetPackage.LatestRelease,
        KnownExtensions.Net70PluggableAgent.NuGetPackage.LatestDevBuild,
        KnownExtensions.Net80PluggableAgent.NuGetPackage.LatestDevBuild }
));

//////////////////////////////////////////////////////////////////////
// TEST BED RUNNER
//////////////////////////////////////////////////////////////////////

public class TestCentricEngineTestBed : TestRunner, IPackageTestRunner
{
    private FilePath _executablePath;

	public TestCentricEngineTestBed()
	{
		_executablePath = BuildSettings.NuGetTestDirectory + "TestCentric.Engine." + BuildSettings.PackageVersion + "/lib/test-bed.exe";
	}

	public int RunPackageTest(string arguments)
	{
		return BuildSettings.Context.StartProcess(_executablePath, new ProcessSettings()
		{
			Arguments = arguments,
			WorkingDirectory = BuildSettings.OutputDirectory
		});
	}
}

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.Run();
