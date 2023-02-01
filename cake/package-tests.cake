
//////////////////////////////////////////////////////////////////////
// TESTING HELPER METHODS
//////////////////////////////////////////////////////////////////////

static void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

private void RunNUnitLite(string testName, string framework, string directory)
{
	bool isDotNetCore = framework.StartsWith("netcoreapp");
	string ext = isDotNetCore ? ".dll" : ".exe";
	string testPath = directory + testName + ext;

	Information("==================================================");
	Information("Running tests under " + framework);
	Information("==================================================");

	int rc = isDotNetCore
		? StartProcess("dotnet", testPath)
		: StartProcess(testPath);

	if (rc > 0)
		ErrorDetail.Add($"{testName}: {rc} tests failed running under {framework}");
	else if (rc < 0)
		ErrorDetail.Add($"{testName} returned rc = {rc} running under {framework}");
}

// Class that knows how to run tests against either GUI
public class GuiTester
{
	private BuildParameters _parameters;

	public GuiTester(BuildParameters parameters)
	{
		_parameters = parameters;
	}

	public int RunGuiUnattended(string runnerPath, string arguments)
	{
		if (!arguments.Contains(" --run"))
			arguments += " --run";
		if (!arguments.Contains(" --unattended"))
			arguments += " --unattended";
		if (!arguments.Contains(" --full-gui"))
			arguments += " --full-gui";

		return RunGui(runnerPath, arguments);
	}

	public int RunGui(string runnerPath, string arguments)
	{
		return _parameters.Context.StartProcess(runnerPath, new ProcessSettings()
		{
			Arguments = arguments,
			WorkingDirectory = _parameters.OutputDirectory
		});
	}
}

// Representation of a single test to be run against a pre-built package.
// Each test has a Level, with the following values defined...
//  0 Do not run - used for temporarily disabling a test
//  1 Run for all CI tests - that is every time we test packages
//  2 Run only on PRs, dev builds and when publishing
//  3 Run only when publishing
public struct PackageTest
{
	public int Level;
	public string Name;
	public string Description;
	public string Arguments;
	public ExpectedResult ExpectedResult;
	public string[] ExtensionsNeeded;
	
	public PackageTest(int level, string name, string description, string arguments, ExpectedResult expectedResult, params string[] extensionsNeeded)
	{
		Level = level;
		Name = name;
		Description = description;
		Arguments = arguments;
		ExpectedResult = expectedResult;
		ExtensionsNeeded = extensionsNeeded;
	}
}

// Abstract base for all package testers. Currently, we only
// have one package of each type (Zip, NuGet, Chocolatey).
public abstract class PackageTester : GuiTester
{
	protected BuildParameters _parameters;
	private ICakeContext _context;

	public PackageTester(BuildParameters parameters)
		: base(parameters) 
	{
		_parameters = parameters;
		_context = parameters.Context;

		PackageTests = new List<PackageTest>();

		// Level 1 tests are run each time we build the packages

		// Tests of single assemblies targeting each runtime we support

		PackageTests.Add(new PackageTest(1, "Net462Test", "Run net462 mock-assembly.dll under .NET 4.6.2",
			"net462/mock-assembly.dll",
			MockAssemblyExpectedResult("Net462AgentLauncher")));

		PackageTests.Add(new PackageTest(1, "Net35Test", "Run net35 mock-assembly.dll under .NET 4.6.2",
		"net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

		PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run .NET Core 2.1 mock-assembly.dll under .NET Core 3.1",
            "netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll under .NET Core 3.1",
            "netcoreapp3.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

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
            MockAssemblyExpectedResult("Net50AgentLauncher")));

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
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "NetCore31AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
            "net5.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net50AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
            "net6.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
                "net7.0/aspnetcore-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
                }));

		// Windows Forms Tests

		// TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
                "net5.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net50AgentLauncher") }
                }));

        PackageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
            "net6.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
            }));

        if (!parameters.IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
                "net7.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
                }));

		// This test installs the .NET 2.0 pluggable agent. All subsequent
		// tests will use that agent for .NET 2.0 through 3.5 tests.
		PackageTests.Add(new PackageTest(1, "Net20PluggableAgentTest", "Run net35 mock-assembly.dll under .NET 2.0 pluggable agent",
			"net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net20AgentLauncher"),
			Net20PluggableAgent));

		// Multiple assembly tests

        PackageTests.Add(new PackageTest(1, "Net462PlusNet35Test", "Run net462 and net35 builds of mock-assembly.dll together",
            "net462/mock-assembly.dll net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "Net20AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net462PlusNet60Test", "Run different builds of mock-assembly.dll together",
            "net462/mock-assembly.dll net6.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "Net60AgentLauncher")));

        // Level 2 tests are run for PRs and when packages will be published

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
        //	NUnitV2Driver));

        // TODO: Use --config option when it's supported by the extension.
        // Current test relies on the fact that the Release config appears
        // first in the project file.
        //if (_parameters.Configuration == "Release")
        //{
        //    PackageTests.Add(new PackageTest(2, "NUnitProjectTest", "Run an NUnit project",
        //        "../../GuiTests.nunit",
        //        new ExpectedResult("Passed")
        //        {
        //            Assemblies = new[] {
        //                    new ExpectedAssemblyResult("TestCentric.Gui.Tests.dll", "net-4.5"),
        //                    new ExpectedAssemblyResult("TestCentric.Gui.Model.Tests.dll", "net-4.5") }
        //        },
        //        NUnitProjectLoader));
        //}
    }

	protected abstract string PackageName { get; }
	protected abstract FilePath PackageUnderTest { get; }
	protected abstract string PackageTestDirectory { get; }
	protected abstract string PackageResultDirectory { get; }
	protected abstract string PackageTestBinDirectory { get; }
	protected abstract string ExtensionInstallDirectory { get; }

	protected virtual string NUnitV2Driver => "NUnit.Extension.NUnitV2Driver";
	protected virtual string NUnitProjectLoader => "NUnit.Extension.NUnitProjectLoader";
	protected virtual string Net20PluggableAgent => "NUnit.Extension.Net20PluggableAgent";

	private List<string> InstalledExtensions { get; } = new List<string>();

	// NOTE: Currently, we use the same tests for all packages. There seems to be
	// no reason for the three packages to differ in capability so the only reason
	// to limit tests on some of them would be efficiency... so far not a problem.
	private List<PackageTest> PackageTests { get; }

	protected string GuiRunner => PackageTestBinDirectory + GUI_RUNNER;

	public void RunAllTests()
	{
		Console.WriteLine("Testing package " + PackageName);

		RunPackageTests(_parameters.PackageTestLevel);

		CheckTestErrors(ref ErrorDetail);
	}

	private void ClearAllExtensions()
    {
		// Ensure we start out each package with no extensions installed.
		// If any package test installs an extension, it remains available
		// for subsequent tests of the same package only.
		foreach (var dirPath in _context.GetDirectories(ExtensionInstallDirectory + "*"))
		{
			string dirName = dirPath.GetDirectoryName();
			if (dirName.StartsWith("NUnit.Extension.") || dirName.StartsWith("nunit-extension-"))
			{
				_context.DeleteDirectory(dirPath, new DeleteDirectorySettings() { Recursive = true });
				Console.WriteLine("Deleted directory " + dirName);
			}
		}
	}

	private void CheckExtensionIsInstalled(string extension)
	{
		bool alreadyInstalled = _context.GetDirectories($"{ExtensionInstallDirectory}{extension}.*").Count > 0;

		if (!alreadyInstalled)
		{
			DisplayBanner($"Installing {extension}");
			InstallEngineExtension(extension);
			InstalledExtensions.Add(extension);
		}
	}

	protected abstract void InstallEngineExtension(string extension);

	private void RunPackageTests(int testLevel)
	{
		var reporter = new ResultReporter(PackageName);

		ClearAllExtensions();

		foreach (var packageTest in PackageTests)
		{
			if (packageTest.Level > 0 && packageTest.Level <= testLevel)
			{
				foreach (string extension in packageTest.ExtensionsNeeded)
					CheckExtensionIsInstalled(extension);

				var workDirectory = PackageResultDirectory + packageTest.Name + "/";
				var resultFile = workDirectory + "TestResult.xml";
				// Delete result file ahead of time so we don't mistakenly
				// read a left-over file from another test run. Leave the
				// file after the run in case we need it to debug a failure.
				if (_context.FileExists(resultFile))
					_context.DeleteFile(resultFile);
				
				DisplayBanner(packageTest.Description);
				DisplayTestEnvironment(packageTest);

				_context.CreateDirectory(workDirectory);

				RunGuiUnattended(GuiRunner, packageTest.Arguments + $" --work:\"{workDirectory}\"");

				try
                {
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

					Console.WriteLine("\nERROR: No result found!");
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

    static ExpectedResult MockAssemblyExpectedResult(params string[] agentNames)
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

	private void DisplayBanner(string message)
	{
		Console.WriteLine("\n========================================");;
		Console.WriteLine(message);
		Console.WriteLine("========================================");
	}

	private void DisplayTestEnvironment(PackageTest test)
	{
		Console.WriteLine("Test Environment");
		Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
		Console.WriteLine($"  CLR Version: {Environment.Version}");
		Console.WriteLine($"       Runner: {GuiRunner}");
		Console.WriteLine($"    Arguments: {test.Arguments}");
		Console.WriteLine();
	}

    protected FileCheck HasFile(string file) => HasFiles(new [] { file });
    protected FileCheck HasFiles(params string[] files) => new FileCheck(files);  

    protected DirectoryCheck HasDirectory(string dir) => new DirectoryCheck(dir);
}

public class ZipPackageTester : PackageTester
{
	public ZipPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.ZipPackageName;
	protected override FilePath PackageUnderTest => _parameters.ZipPackage;
	protected override string PackageTestDirectory => _parameters.ZipTestDirectory;
	protected override string PackageResultDirectory => _parameters.ZipResultDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "bin/";
	protected override string ExtensionInstallDirectory => PackageTestBinDirectory + "addins/";
	
	protected override void InstallEngineExtension(string extension)
	{
		Console.WriteLine($"Installing {extension} to directory {ExtensionInstallDirectory}");

		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings()
			{
				OutputDirectory = ExtensionInstallDirectory,
				Prerelease = true
			});
	}
}

public class NuGetPackageTester : PackageTester
{
	public NuGetPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.NuGetPackageName;
	protected override FilePath PackageUnderTest => _parameters.NuGetPackage;
	protected override string PackageTestDirectory => _parameters.NuGetTestDirectory;
	protected override string PackageResultDirectory => _parameters.NuGetResultDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	protected override string ExtensionInstallDirectory => _parameters.TestDirectory;
	
	protected override void InstallEngineExtension(string extension)
	{
		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings()
			{
				OutputDirectory = ExtensionInstallDirectory,
				Prerelease = true
			});
	}
}

public class ChocolateyPackageTester : PackageTester
{
	public ChocolateyPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.ChocolateyPackageName;
	protected override FilePath PackageUnderTest => _parameters.ChocolateyPackage;
	protected override string PackageTestDirectory => _parameters.ChocolateyTestDirectory;
	protected override string PackageResultDirectory => _parameters.ChocolateyResultDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	protected override string ExtensionInstallDirectory => _parameters.TestDirectory;
	
	// Chocolatey packages have a different naming convention from NuGet
	protected override string NUnitV2Driver => "nunit-extension-nunit-v2-driver";
	protected override string NUnitProjectLoader => "nunit-extension-nunit-project-loader";
    protected override string Net20PluggableAgent => "nunit-extension-net20-pluggable-agent";

    protected override void InstallEngineExtension(string extension)
	{
		// Install with NuGet because choco requires administrator access
		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings()
			{
				Source = new[] { "https://www.myget.org/F/testcentric/api/v3/index.json" },
				OutputDirectory = ExtensionInstallDirectory,
				Prerelease = true
			});
	}
}
