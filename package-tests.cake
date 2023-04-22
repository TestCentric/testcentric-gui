public static List<PackageTest> PackageTests = new List<PackageTest>();

public static void DefinePackageTests()
{
    if (BuildSettings.Context == null)
        throw new Exception("Trying to use BuildSettings before it is initialized");

	// Define Package Tests
    //   Level 1 tests are run each time we build the packages
    //   Level 2 tests are run for PRs and when packages will be published
    //   Level 3 tests are run only when publishing a release

	// Tests of single assemblies targeting each runtime we support

	PackageTests.Add(new PackageTest(1, "Net462Test", "Run net462 mock-assembly.dll under .NET 4.6.2",
		"net462/mock-assembly.dll",
		MockAssemblyExpectedResult("Net462AgentLauncher")));

	PackageTests.Add(new PackageTest(1, "Net35Test", "Run net35 mock-assembly.dll under .NET 4.6.2",
	"net35/mock-assembly.dll",
        MockAssemblyExpectedResult("Net462AgentLauncher")));

	PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run .NET Core 2.1 mock-assembly.dll under .NET Core 3.1",
        "netcoreapp2.1/mock-assembly.dll",
        MockAssemblyExpectedResult("Net60AgentLauncher")));

    PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll under .NET Core 3.1",
        "netcoreapp3.1/mock-assembly.dll",
        MockAssemblyExpectedResult("Net60AgentLauncher")));

    //    PackageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
    //        "netcoreapp1.1/mock-assembly.dll",
    //        new ExpectedResult("Failed")
    //        {
    //Total = 41,
    //Passed = 22,
    //Failed = 7,
    //Warnings = 0,
    //Inconclusive = 5,
    //Skipped = 7,
    //Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "netcore-1.1") }
    //        }));

    PackageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll under .NET 5.0",
        "net5.0/mock-assembly.dll",
        MockAssemblyExpectedResult("Net60AgentLauncher")));

    PackageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll under .NET 6.0",
        "net6.0/mock-assembly.dll",
        MockAssemblyExpectedResult("Net60AgentLauncher")));

    PackageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll under .NET 7.0",
        "net7.0/mock-assembly.dll",
        MockAssemblyExpectedResult("Net70AgentLauncher")));

    // AspNetCore tests

	PackageTests.Add(new PackageTest(1, "AspNetCore31Test", "Run test using AspNetCore under .NET Core 3.1",
        "netcoreapp3.1/aspnetcore-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
        }));

    PackageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
        "net5.0/aspnetcore-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
        }));

    PackageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
        "net6.0/aspnetcore-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
        }));

    // TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
    if (!BuildSettings.IsRunningOnAppVeyor)
        PackageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
            "net7.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
            }));

	// Windows Forms Tests

	// TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
    if (!BuildSettings.IsRunningOnAppVeyor)
        PackageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
            "net5.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
            }));

    PackageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
        "net6.0-windows/windows-forms-test.dll",
        new ExpectedResult("Passed")
        {
            Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
        }));

    if (!BuildSettings.IsRunningOnAppVeyor)
        PackageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
            "net7.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
            }));

	// Multiple assembly tests

    PackageTests.Add(new PackageTest(1, "Net462PlusNet35Test", "Run net462 and net35 builds of mock-assembly.dll together",
        "net462/mock-assembly.dll net35/mock-assembly.dll",
        MockAssemblyExpectedResult("Net462AgentLauncher", "Net462AgentLauncher")));

    PackageTests.Add(new PackageTest(1, "Net462PlusNet60Test", "Run different builds of mock-assembly.dll together",
        "net462/mock-assembly.dll net6.0/mock-assembly.dll",
        MockAssemblyExpectedResult("Net462AgentLauncher", "Net60AgentLauncher")));

    // Level 2 tests are run for PRs and when packages will be published

    // NOTE: Package tests using a pluggable agent must be run after all tests
    // that assume no pluggable agents are installed!

	PackageTests.Add(new PackageTest(2, "Net20PluggableAgentTest", "Run net35 mock-assembly.dll under .NET 2.0 pluggable agent",
	    "net35/mock-assembly.dll",
        MockAssemblyExpectedResult("Net20AgentLauncher"),
        EngineExtensions.Net20PluggableAgent.SetVersion("2.1.0-dev00018")));

	PackageTests.Add(new PackageTest(2, "NetCore21PluggableAgentTest", "Run .NET Core 2.1 mock-assembly.dll under .NET Core 2.1 pluggable agent",
		"netcoreapp2.1/mock-assembly.dll",
        MockAssemblyExpectedResult("NetCore21AgentLauncher"),
		EngineExtensions.NetCore21PluggableAgent));

	// TODO: Suppress V2 tests until driver is working
    //PackageTests.Add(new PackageTest(2, "NUnitV2Test", "Run mock-assembly.dll built for NUnit V2",
    //	"v2-tests/mock-assembly.dll",
    //	new ExpectedResult("Failed")
    //	{
    //		Total = 28,
    //		Passed = 18,
    //		Failed = 5,
    //		Warnings = 0,
    //		Inconclusive = 1,
    //		Skipped = 4
    //	},
    //	EngineExtensions.NUnitV2Driver));

    // TODO: Use --config option when it's supported by the extension.
    // Current test relies on the fact that the Release config appears
    // first in the project file.
    if (BuildSettings.Configuration == "Release")
    {
        PackageTests.Add(new PackageTest(2, "NUnitProjectTest", "Run an NUnit project",
            "../../TestProject.nunit",
            MockAssemblyExpectedResult(
                "Net20AgentLauncher", "Net462AgentLauncher", "Net60AgentLauncher", "Net60AgentLauncher"),
            EngineExtensions.NUnitProjectLoader));
    }

	// TODO: Make this work on AppVeyor
	const string NET80_MOCK_ASSEMBLY = "../../../net80-pluggable-agent/bin/Release/tests/net8.0/mock-assembly.dll";
	if (BuildSettings.IsLocalBuild && BuildSettings.Context.FileExists(BuildSettings.OutputDirectory + NET80_MOCK_ASSEMBLY))
		PackageTests.Add(new PackageTest(2, "NetCore80PluggableAgentTest", "Run mock-assembly.dll targeting Net 8.0 using NetCore80PluggableAgent",
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
			EngineExtensions.Net80PluggableAgent));

    ExpectedResult MockAssemblyExpectedResult(params string[] agentNames)
    {
        int ncopies = agentNames.Length;

        var assemblies = new ExpectedAssemblyResult[ncopies];
        for (int i = 0; i < ncopies; i++)
            assemblies[i] = new ExpectedAssemblyResult("mock-assembly.dll", agentNames[i]);

        return new ExpectedResult("Failed")
        {
            Total = 41 * ncopies,
            Passed = 22 * ncopies,
            Failed = 7 * ncopies,
            Warnings = 0 * ncopies,
            Inconclusive = 5 * ncopies,
            Skipped = 7 * ncopies,
            Assemblies = assemblies
        };
    }
}
