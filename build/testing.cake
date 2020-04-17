//////////////////////////////////////////////////////////////////////
// TESTING HELPER METHODS
//////////////////////////////////////////////////////////////////////

const string DEFAULT_RESULT_FILE = "TestResult.xml";

void CheckTestErrors(ref List<string> errorDetail)
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

	public void RunGuiUnattended(string runnerPath, string arguments)
	{
		if (!arguments.Contains(" --run"))
			arguments += " --run";
		if (!arguments.Contains(" --unattended"))
			arguments += " --unattended";

		RunGui(runnerPath, arguments);
	}

	public void RunGui(string runnerPath, string arguments)
	{
		_parameters.Context.StartProcess(runnerPath, new ProcessSettings()
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
	public string Description;
	public string Runner;
	public string Arguments;
	public ExpectedResult ExpectedResult;
	
	public PackageTest(int level, string runner, string arguments, ExpectedResult expectedResult, string description)
	{
		Level = level;
		Description = description;
		Runner = runner;
		Arguments = arguments;
		ExpectedResult = expectedResult;
	}
}

// Abstract base for all package testers. Currently, we only
// have one package of each type (Zip, NuGet, Chocolatey).
public abstract class PackageTester : GuiTester
{
    protected static readonly string[] ENGINE_FILES = { 
        "testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll", "Mono.Cecil.dll"};
    protected static readonly string[] AGENT_FILES = { 
        "testcentric-agent.exe", "testcentric-agent.exe.config", "testcentric-agent-x86.exe", "testcentric-agent-x86.exe.config",
        "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll" };
    protected static readonly string[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "tc-next.exe", "tc-next.exe.config", "nunit.uiexception.dll",
        "TestCentric.Gui.Runner.dll", "Experimental.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "TestCentric.Common.dll" };
    protected static readonly string[] TREE_ICONS_JPG = {
        "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
    protected static readonly string[] TREE_ICONS_PNG = {
        "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

	protected const string GUI_TESTS = "TestCentric.Gui.Tests.dll";
	protected const string EXPERIMENTAL_TESTS = "Experimental.Gui.Tests.dll";
	protected const string MODEL_TESTS = "TestCentric.Gui.Model.Tests.dll";
	protected const string ENGINE_CORE_TESTS = "testcentric.engine.core.tests.dll";

	protected const string V2_MOCK_ASSEMBLY = "v2-tests/mock-assembly.dll";
	protected static readonly ExpectedResult V2_MOCK_ASSEMBLY_RESULT = new ExpectedResult("Failed")
	{
		Total = 28,
		Passed = 18,
		Failed = 5,
		Warnings = 0,
		Inconclusive = 1,
		Skipped = 4
	};

	protected BuildParameters _parameters;

	public PackageTester(BuildParameters parameters)
		: base(parameters) 
	{
		_parameters = parameters;

		PackageTests = new List<PackageTest>();

		PackageTests.Add(new PackageTest(1, StandardRunner,
			MODEL_TESTS,
			ExpectedResult.Success,
			"Run tests of the TestCentric model"));
		PackageTests.Add(new PackageTest(1, ExperimentalRunner,
			MODEL_TESTS,
			ExpectedResult.Success,
			"Run tests of the TestCentric model using the Experimental Runner"));
		PackageTests.Add(new PackageTest(1, StandardRunner,
			V2_MOCK_ASSEMBLY,
			V2_MOCK_ASSEMBLY_RESULT,
			"Run mock-assembly tests using NUnit V2"));
		PackageTests.Add( new PackageTest(1, StandardRunner,
			"engine-tests/net35/testcentric.engine.core.tests.dll engine-tests/net40/testcentric.engine.core.tests.dll",
			ExpectedResult.Success, "Run two builds of the engine core tests together"));
	}

	protected abstract string PackageName { get; }
	protected abstract FilePath PackageUnderTest { get; }
	protected abstract string PackageTestDirectory { get; }
	protected abstract string PackageTestBinDirectory { get; }

	// PackageChecks differ for each package type.
	protected abstract PackageCheck[] PackageChecks { get; }

	// NOTE: Currently, we use the same tests for all packages. There seems to be
	// no reason for the three packages to differ in capability so the only reason
	// to limit tests on some of them would be efficiency... so far not a problem.
	private List<PackageTest> PackageTests { get; }

	protected string StandardRunner => PackageTestBinDirectory + GUI_RUNNER;
	protected string ExperimentalRunner => PackageTestBinDirectory + EXPERIMENTAL_RUNNER;

	public void RunAllTests()
	{
		Console.WriteLine("Testing package " + PackageName);

		CreateTestDirectory();

		RunChecks();

		RunPackageTests();
	}

	private void CreateTestDirectory()
	{
		Console.WriteLine("Unzipping package to directory\n  " + PackageTestDirectory);
		_parameters.Context.CleanDirectory(PackageTestDirectory);
		_parameters.Context.Unzip(PackageUnderTest, PackageTestDirectory);
	}

    public void RunChecks()
    {
		DisplayBanner("Checking Package Content");

        bool allPassed = true;

        if (PackageChecks.Length == 0)
        {
            Console.WriteLine("  Package found but no checks were specified.");
        }
        else
        {
            foreach (var check in PackageChecks)
                allPassed &= check.Apply(PackageTestDirectory);

            if (allPassed)
                Console.WriteLine("  All checks passed!");
        }

        if (!allPassed)
     		ErrorDetail.Add($"Package check failed for {PackageName}");
    }

	// Default implementation does nothing - override as needed.
	public virtual void InstallEngineExtensions() { }

	public void RunPackageTests()
	{
		// Installs whatever extensions are needed for this type of package
		InstallEngineExtensions();

		var label = _parameters.Versions.IsPreRelease ? _parameters.Versions.PreReleaseLabel : "NONE";
		int level;
		switch (label)
		{
			case "NONE":
			case "rc":
			case "alpha":
			case "beta":
				level = 3;
				break;
			case "dev":
			case "pr":
				level = 2;
				break;
			case "ci":
			default:
				level = 1;
				break;
		}
		
		foreach (var packageTest in PackageTests)
		{
			if (packageTest.Level > 0 && packageTest.Level <= level)
			{
				DisplayBanner(packageTest.Description);
				DisplayTestEnvironment(packageTest);

				RunGuiUnattended(packageTest.Runner, packageTest.Arguments);

				var reporter = new ResultReporter(_parameters.OutputDirectory + DEFAULT_RESULT_FILE);
				reporter.Report(packageTest.ExpectedResult);
			}
		}
	}

	protected void DisplayBanner(string message)
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
		Console.WriteLine($"       Runner: {test.Runner}");
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
	protected override string PackageTestBinDirectory => PackageTestDirectory + "bin/";

	public override void InstallEngineExtensions()
	{
		DisplayBanner("Installing V2 Driver Extension");
		_parameters.Context.NuGetInstall("NUnit.Extension.NUnitV2Driver", new NuGetInstallSettings() { OutputDirectory = PackageTestBinDirectory + "addins/"});
	}

	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
		HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.zip.addins"),
		HasDirectory("bin/agents/net20").WithFiles(AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/agents/net40").WithFiles(AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/Images").WithFiles("DebugTests.png", "RunTests.png"),
		HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	};
}

public class NuGetPackageTester : PackageTester
{
	public NuGetPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.NuGetPackageName;
	protected override FilePath PackageUnderTest => _parameters.NuGetPackage;
	protected override string PackageTestDirectory => _parameters.NuGetTestDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	
	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
		HasDirectory("tools/agents/net20").WithFiles(AGENT_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net40").WithFiles(AGENT_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	};

	public override void InstallEngineExtensions()
	{
		DisplayBanner("Installing V2 Driver Extension");
		_parameters.Context.NuGetInstall("NUnit.Extension.NUnitV2Driver", new NuGetInstallSettings() { OutputDirectory = PackageTestDirectory + "../"});
	}
}

public class ChocolateyPackageTester : PackageTester
{
	public ChocolateyPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.ChocolateyPackageName;
	protected override FilePath PackageUnderTest => _parameters.ChocolateyPackage;
	protected override string PackageTestDirectory => _parameters.ChocolateyTestDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	
	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
		HasDirectory("tools/agents/net20").WithFiles(AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net40").WithFiles(AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual%20Studio").WithFiles(TREE_ICONS_PNG)
	};

	public override void InstallEngineExtensions()
	{
		DisplayBanner("Installing V2 Driver Extension");
		// We install using NuGet because Chocolatey requires running as administrator
		_parameters.Context.NuGetInstall("nunit-extension-nunit-v2-driver", new NuGetInstallSettings() { Source = new [] { "https://chocolatey.org/api/v2/" }, OutputDirectory = PackageTestDirectory + "../"});
	}
}

public abstract class PackageCheck
{
    public abstract bool Apply(string dir);

    protected static void RecordError(string msg)
    {
        Console.WriteLine("  ERROR: " + msg);
    }
}

public class FileCheck : PackageCheck
{
    string[] _paths;

    public FileCheck(string[] paths)
    {
        _paths = paths;
    }

    public override bool Apply(string dir)
    {
        var isOK = true;

        foreach (string path in _paths)
        {
            if (!System.IO.File.Exists(dir + path))
            {
                RecordError($"File {path} was not found.");
                isOK = false;
            }
        }

        return isOK;
    }
}

public class DirectoryCheck : PackageCheck
{
    private string _path;
    private List<string> _files = new List<string>();

    public DirectoryCheck(string path)
    {
        _path = path;
    }

    public DirectoryCheck WithFiles(params string[] files)
    {
        _files.AddRange(files);
        return this;
    }

    public DirectoryCheck AndFiles(params string[] files)
    {
        return WithFiles(files);
    }

    public DirectoryCheck WithFile(string file)
    {
        _files.Add(file);
        return this;
    }

	public DirectoryCheck AndFile(string file)
	{
		return AndFiles(file);
	}

    public override bool Apply(string dir)
    {
        if (!System.IO.Directory.Exists(dir + _path))
        {
            RecordError($"Directory {_path} was not found.");
            return false;
        }

        bool isOK = true;

        if (_files != null)
        {
            foreach (var file in _files)
            {
                if (!System.IO.File.Exists(System.IO.Path.Combine(dir + _path, file)))
                {
                    RecordError($"File {file} was not found in directory {_path}.");
                    isOK = false;
                }
            }
        }

        return isOK;
    }
}
