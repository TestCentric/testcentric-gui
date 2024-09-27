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
    PackageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll under .NET 4.6.2",
        "engine-tests/net462/mock-assembly.dll",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "net-4.6.2") }
        }));
    PackageTests.Add(new PackageTest(1, "Net462X86Test", "Run mock-assembly-x86.dll under .NET 4.6.2",
        "engine-tests/net462/mock-assembly-x86.dll",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly-x86.dll", "net-4.6.2") }
        }));
    PackageTests.Add(new PackageTest(1, "Net462TestInProcess", "Run mock-assembly.dll under .NET 4.6.2 in process",
        "engine-tests/net462/mock-assembly.dll --inprocess",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "inprocess") }
        }));
    PackageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll under .NET 3.5",
        "engine-tests/net35/mock-assembly.dll",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "net-2.0") }
        }));
    PackageTests.Add(new PackageTest(1, "Net35X86Test", "Run mock-assembly-x86.dll under .NET 3.5",
        "engine-tests/net35/mock-assembly-x86.dll",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly-x86.dll", "net-2.0") }
        }));
    PackageTests.Add(new PackageTest(1, "Net35TestInProcess", "Run mock-assembly.dll under .NET 3.5 in process",
        "engine-tests/net35/mock-assembly.dll --inprocess",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "inprocess") }
        }));
    PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll under .NET Core 3.1",
        "engine-tests/netcoreapp3.1/mock-assembly.dll --trace:Debug",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "netcore-3.1") }
        }));
    PackageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll under .NET 6.0",
        "engine-tests/net6.0/mock-assembly.dll",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "netcore-6.0") }
        }));
    PackageTests.Add(new PackageTest(1, "Net80Test", "Run mock-assembly.dll under .NET 8.0",
        "engine-tests/net8.0/mock-assembly.dll --trace:Debug",
        new ExpectedResult("Failed")
        {
            Total = 31,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 7,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "netcore-8.0") }
        }));

    // TODO:
    // AspNetCore Tests
    // Windows Forms Tests

    // Multiple Assembly Tests
    PackageTests.Add(new PackageTest(1, "Net35PlusNetCore31Test", "Run different builds of mock-assembly.dll together",
        "engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp3.1/mock-assembly.dll",
        new ExpectedResult("Failed")
        {
            Total = 62,
            Passed = 36,
            Failed = 10,
            Warnings = 0,
            Inconclusive = 2,
            Skipped = 14,
            Assemblies = new[] {
                new ExpectedAssemblyResult("mock-assembly.dll", "net-2.0"),
                new ExpectedAssemblyResult("mock-assembly.dll", "netcore-3.1")
            }
        }));

    // Tests using engine extensions
    PackageTests.Add(new PackageTest(1, "NUnitV2Net462Test", "Run mock-assembly-v2.dll under net462",
        "v2-tests/net462/mock-assembly-v2.dll",
        new ExpectedResult("Failed")
        {
            Total = 28,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 4,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly-v2.dll", "net4.6.2") }
        },
        KnownExtensions.NUnitV2Driver));
    PackageTests.Add(new PackageTest(1, "NUnitV2Net462TestInProcess", "Run mock-assembly-v2.dll in process under net462",
        "v2-tests/net462/mock-assembly-v2.dll --inprocess --trace:Debug",
        new ExpectedResult("Failed")
        {
            Total = 28,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 4,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly-v2.dll", "inprocess") }
        },
        KnownExtensions.NUnitV2Driver));
    PackageTests.Add(new PackageTest(1, "NUnitV2Net35Test", "Run mock-assembly-v2.dll under net35",
        "v2-tests/net35/mock-assembly-v2.dll",
        new ExpectedResult("Failed")
        {
            Total = 28,
            Passed = 18,
            Failed = 5,
            Warnings = 0,
            Inconclusive = 1,
            Skipped = 4,
            Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly-v2.dll", "net-2.0") }
        },
        KnownExtensions.NUnitV2Driver));

    // Level 2 - Run GUI Tests under the GUI
    PackageTests.Add(new PackageTest(2, "NUnitProjectTest", "Run GUI Tests under the GUI",
        "TestCentric.Gui.Tests.exe",
        new ExpectedResult("Passed")
        {
            Assemblies = new[] { new ExpectedAssemblyResult("TestCentric.Gui.Tests.exe", "net-4.6.2") }
        }));
}
