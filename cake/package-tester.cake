const string DEFAULT_TEST_RESULT_FILE = "TestResult.xml";

// Abstract base for all package testers.
public abstract class PackageTester
{
    protected BuildParameters _parameters;
    private ICakeContext _context;

    public PackageTester(BuildParameters parameters)
    {
        _parameters = parameters;
        _context = parameters.SetupContext;

        PackageTests = new List<PackageTest>();

        //Level 1 tests are run each time we build the packages

		// Tests of single assemblies targeting each runtime we support

        PackageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll targeting .NET 4.6.2",
            "engine-tests/net462/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll targeting .NET 3.5",
            "engine-tests/net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll targeting .NET Core 2.1",
            "engine-tests/netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll targeting .NET Core 3.1",
            "engine-tests/netcoreapp3.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
            "engine-tests/netcoreapp1.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll targeting .NET 5.0",
            "engine-tests/net5.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net50AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll targeting .NET 6.0",
            "engine-tests/net6.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net60AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll targeting .NET 7.0",
            "engine-tests/net7.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net70AgentLauncher")));

        // AspNetCore Tests

        PackageTests.Add(new PackageTest(1, "AspNetCore31Test", "Run test using AspNetCore under .NET Core 3.1",
            "engine-tests/netcoreapp3.1/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "NetCore31AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
            "engine-tests/net5.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net50AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
            "engine-tests/net6.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
                "engine-tests/net7.0/aspnetcore-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
                }));

        // Windows Forms Tests

        // TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
                "engine-tests/net5.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net50AgentLauncher") }
                }));

        PackageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
            "engine-tests/net6.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
                "engine-tests/net7.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
                }));

        // Multiple Assembly Tests

        PackageTests.Add(new PackageTest(1, "Net35PlusNetCore21Test", "Run different builds of mock-assembly.dll together",
            "engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "NetCore31AgentLauncher")));

        // Level 2 tests are run for PRs and when packages will be published

        // TODO: Use --config option when it's supported by the extension.
        // Current test relies on the fact that the Release config appears
        // first in the project file.
        if (_parameters.Configuration == "Release")
        {
            PackageTests.Add(new PackageTest(1, "NUnitProjectTest", "Run an NUnit project",
                "TestProject.nunit",
                new ExpectedResult("Failed")
                {
                    Assemblies = new[] {
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "NetCore31AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net50AgentLauncher") }
                },
                NUnitProjectLoader));
        }

        // NOTE: Package tests using a pluggable agent must be run after all tests
        // that assume no pluggable agents are installed!

        PackageTests.Add(new PackageTest(1, "Net20PluggableAgentTest", "Run mock-assembly.dll targeting net35 using Net20PluggableAgent",
            "engine-tests/net35/mock-assembly.dll",
            new ExpectedResult("Failed")
            {
                Total = 36,
                Passed = 23,
                Failed = 5,
                Warnings = 1,
                Inconclusive = 1,
                Skipped = 7,
                Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "Net20AgentLauncher") }
            },
            Net20PluggableAgent));

        PackageTests.Add(new PackageTest(1, "NetCore21PluggableAgentTest", "Run mock-assembly.dll targeting Net Core 2.1 using NetCore21PluggableAgent",
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
            NetCore21PluggableAgent));

        //PackageTests.Add(new PackageTest(1, "NUnitV2Test", "Run tests using the V2 framework driver",
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
    }

    protected abstract string PackageName { get; }
    protected abstract FilePath PackageUnderTest { get; }
    protected abstract string PackageTestDirectory { get; }
	protected abstract string PackageResultDirectory { get; }
    protected abstract string PackageTestBinDirectory { get; }
    protected abstract string ExtensionInstallDirectory { get; }

    protected virtual ExtensionSpecifier NUnitV2Driver => new ExtensionSpecifier("NUnit.Extension.NUnitV2Driver", "3.9.0");
    protected virtual ExtensionSpecifier NUnitProjectLoader => new ExtensionSpecifier("NUnit.Extension.NUnitProjectLoader", "3.7.1");
    protected virtual ExtensionSpecifier Net20PluggableAgent => new ExtensionSpecifier("NUnit.Extension.Net20PluggableAgent", "2.0.0");
    protected virtual ExtensionSpecifier NetCore21PluggableAgent => new ExtensionSpecifier("NUnit.Extension.NetCore21PluggableAgent", "2.0.0");

    // NOTE: Currently, we use the same tests for all packages. There seems to be
    // no reason for the three packages to differ in capability so the only reason
    // to limit tests on some of them would be efficiency... so far not a problem.
    private List<PackageTest> PackageTests { get; }

    public void RunAllTests()
    {
        Console.WriteLine("Testing package " + PackageName);

        RunPackageTests(_parameters.PackageTestLevel);

        CheckTestErrors(ref ErrorDetail);
    }

    private void CheckExtensionIsInstalled(ExtensionSpecifier extension)
    {
        bool alreadyInstalled = _context.GetDirectories($"{ExtensionInstallDirectory}{extension.Id}.*").Count > 0;

        if (!alreadyInstalled)
        {
            DisplayBanner($"Installing {extension.Id} version {extension.Version}");
            InstallEngineExtension(extension);
        }
    }

    protected abstract void InstallEngineExtension(ExtensionSpecifier extension);

    private void RunPackageTests(int testLevel)
    {
        _context.CopyFileToDirectory(
            _parameters.OutputDirectory + TEST_BED_EXE, 
            PackageTestBinDirectory);

        var reporter = new ResultReporter(PackageName);

        foreach (var packageTest in PackageTests)
        {
            if (packageTest.Level > 0 && packageTest.Level <= testLevel)
            {
                foreach (ExtensionSpecifier extension in packageTest.ExtensionsNeeded)
                    CheckExtensionIsInstalled(extension);

				var workDirectory = PackageResultDirectory + packageTest.Name + "/";
				var resultFile = workDirectory + "TestResult.xml";
                // Delete result file ahead of time so we don't mistakenly
                // read a left-over file from another test run. Leave the
                // file after the run in case we need it to debug a failure.
                if (_context.FileExists(resultFile))
                    _context.DeleteFile(resultFile);

                DisplayBanner(packageTest.Description);

				_context.CreateDirectory(workDirectory);

                try
                {
                    Console.WriteLine($"Launching {PackageTestBinDirectory}{TEST_BED_EXE}");
                    _context.StartProcess(PackageTestBinDirectory + TEST_BED_EXE, new ProcessSettings()
                    {
                        Arguments = packageTest.Arguments + $" --work:\"{workDirectory}\"",
                        WorkingDirectory = _parameters.OutputDirectory
                    });

                    var result = new ActualResult(resultFile);
                    var report = new PackageTestReport(packageTest, result);
                    reporter.AddReport(report);

                    Console.WriteLine(report.Errors.Count == 0
                        ? "\nSUCCESS: Test Result matches expected result!"
                        : "\nERROR: Test Result not as expected!");
                }
                catch (Exception ex)
                {
                    reporter.AddReport(new PackageTestReport(packageTest, ex));

                    Console.WriteLine("\nERROR: Unexpected Exception thrown.");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        bool anyErrors = reporter.ReportResults();
        Console.WriteLine();

        // All package tests are run even if one of them fails. If there are
        // any errors,  we stop the run at this point.
        if (anyErrors)
            throw new Exception("One or more package tests had errors!");
    }

    private void DisplayBanner(string message)
    {
        var bar = new string('-', Math.Max(message.Length, 70));
        Console.WriteLine();
        Console.WriteLine(bar);
        Console.WriteLine(message);
        Console.WriteLine(bar);
    }

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
}

public class EnginePackageTester : PackageTester
{
    public EnginePackageTester(BuildParameters parameters) : base(parameters) { }

    protected override string PackageName => _parameters.EnginePackageName;
    protected override FilePath PackageUnderTest => _parameters.EnginePackage;
    protected override string PackageTestDirectory => _parameters.EngineTestDirectory;
    protected override string PackageResultDirectory => _parameters.EngineResultDirectory;
    protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
    protected override string ExtensionInstallDirectory => _parameters.TestDirectory;

    protected override void InstallEngineExtension(ExtensionSpecifier extension)
    {
        _parameters.SetupContext.NuGetInstall(extension.Id,
            new NuGetInstallSettings()
            {
                OutputDirectory = ExtensionInstallDirectory,
                Version = extension.Version
            });
    }
}
