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
        throw new Exception("One or more tests failed, breaking the build.\r\n"
                              + copyError.Aggregate((x,y) => x + "\r\n" + y));
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
	protected static readonly string[] ENGINE_FILES = {
		"testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll"};
	protected static readonly string[] ENGINE_CORE_FILES = {
		"testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll" };
	protected static readonly string[] NET_FRAMEWORK_AGENT_FILES = {
		"testcentric-agent.exe", "testcentric-agent.exe.config", "testcentric-agent-x86.exe", "testcentric-agent-x86.exe.config" };
	protected static readonly string[] NET_CORE_AGENT_FILES = {
		"testcentric-agent.dll", "testcentric-agent.dll.config" };
	protected static readonly string[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "nunit.uiexception.dll",
        "TestCentric.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "TestCentric.Common.dll" };
    protected static readonly string[] TREE_ICONS_JPG = {
        "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
    protected static readonly string[] TREE_ICONS_PNG = {
        "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

	protected BuildParameters _parameters;
	private ICakeContext _context;

	public PackageTester(BuildParameters parameters)
		: base(parameters) 
	{
		_parameters = parameters;
		_context = parameters.Context;

		PackageTests = new List<PackageTest>();

		// Level 1 tests are run each time we build the packages
		PackageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll under .NET 4.6.2",
			"engine-tests/net462/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 31,
				Passed = 18,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 7
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
				Skipped = 7
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
				Skipped = 7
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
				Skipped = 7
			}));
		PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll under .NET Core 2.1",
			"engine-tests/netcoreapp2.1/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 31,
				Passed = 18,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 7
			}));
		PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll under .NET Core 3.1",
			"engine-tests/netcoreapp3.1/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 31,
				Passed = 18,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 7
			}));
		PackageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
			"engine-tests/netcoreapp1.1/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 31,
				Passed = 18,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 7
			}));
		PackageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll under .NET 5.0",
			"engine-tests/net5.0/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				// 27 rather than 31 tests due to a bug in nunit.framework 3.11
				Total = 27,
				Passed = 14,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 7
			}));
		PackageTests.Add( new PackageTest(1, "Net35PlusNetCore21Test", "Run different builds of mock-assembly.dll together",
			"engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp2.1/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 62,
				Passed = 36,
				Failed = 10,
				Warnings = 0,
				Inconclusive = 2,
				Skipped = 14
			}));

		// Level 2 tests are run for PRs and when packages will be published

		PackageTests.Add(new PackageTest(2, "TestModelTests", "Re-run tests of the TestCentric model",
			"TestCentric.Gui.Model.Tests.dll",
			new ExpectedResult("Passed")));
		PackageTests.Add(new PackageTest(2, "NUnitV2Test", "Run mock-assembly.dll built for NUnit V2",
			"v2-tests/mock-assembly.dll",
			new ExpectedResult("Failed")
			{
				Total = 28,
				Passed = 18,
				Failed = 5,
				Warnings = 0,
				Inconclusive = 1,
				Skipped = 4
			},
			NUnitV2Driver));
		// TODO: This won't work because the proper config is not used. We need to either
		// create two projects, debug and release, or make the --config option work.
		//PackageTests.Add( new PackageTest(2, "NUnitProjectTest", "Run an NUnit project, specifying Release config",
		//	"../../GuiTests.nunit --config=Release",
		//	new ExpectedResult("Passed"),
		//	NUnitProjectLoader));
	}

	protected abstract string PackageName { get; }
	protected abstract FilePath PackageUnderTest { get; }
	protected abstract string PackageTestDirectory { get; }
	protected abstract string PackageTestBinDirectory { get; }
	protected abstract string PackageResultDirectory { get; }
	protected abstract string ExtensionInstallDirectory { get; }

	protected virtual string NUnitV2Driver => "NUnit.Extension.NUnitV2Driver";
	protected virtual string NUnitProjectLoader => "NUnit.Extension.NUnitProjectLoader";

	// PackageChecks differ for each package type.
	protected abstract PackageCheck[] PackageChecks { get; }

	// NOTE: Currently, we use the same tests for all packages. There seems to be
	// no reason for the three packages to differ in capability so the only reason
	// to limit tests on some of them would be efficiency... so far not a problem.
	private List<PackageTest> PackageTests { get; }

	protected string StandardRunner => PackageTestBinDirectory + GUI_RUNNER;

	public void RunAllTests()
	{
		Console.WriteLine("Testing package " + PackageName);

		CreateTestDirectory();

		RunChecks();

		RunPackageTests(_parameters.PackageTestLevel);

		CheckTestErrors(ref ErrorDetail);
	}

	private void CreateTestDirectory()
	{
		Console.WriteLine("Unzipping package to directory\n  " + PackageTestDirectory);
		_context.CleanDirectory(PackageTestDirectory);
		_context.Unzip(PackageUnderTest, PackageTestDirectory);
	}

    private void RunChecks()
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
     		throw new Exception($"Package check failed for {PackageName}");
    }

	private void CheckExtensionIsInstalled(string extension)
	{
		bool alreadyInstalled = _context.GetDirectories($"{ExtensionInstallDirectory}{extension}.*").Count > 0;

		if (!alreadyInstalled)
		{
			DisplayBanner($"Installing {extension}");
			InstallEngineExtension(extension);
		}
	}

	protected abstract void InstallEngineExtension(string extension);

	private void RunPackageTests(int testLevel)
	{
        var reporter = new ResultReporter(PackageName);

        _context.CleanDirectory(PackageResultDirectory);
        string testToRun = _context.Argument("runTest", "ALL");

		foreach (var packageTest in PackageTests)
		{
            //if (testToRun != "ALL" && testToRun != packageTest.Name)
            //    continue;

			if (packageTest.Level > 0 && packageTest.Level <= testLevel)
			{
				foreach (string extension in packageTest.ExtensionsNeeded)
					CheckExtensionIsInstalled(extension);

				var testResultDir = PackageResultDirectory + packageTest.Name + "/";
				_context.CreateDirectory(testResultDir);

				var resultFile = testResultDir + DEFAULT_TEST_RESULT_FILE;
				
				DisplayBanner(packageTest.Description);
				DisplayTestEnvironment(packageTest);

				RunGuiUnattended(StandardRunner, packageTest.Arguments + $" --work:{testResultDir}");

				try
				{
					var result = new ActualResult(resultFile);
					var report = new TestReport(packageTest, result);
					reporter.AddReport(report);

					if (result.Failed + result.Warnings > 0)
					{
						int index = 0;
						Console.WriteLine();
						Console.WriteLine("Errors, Failures and Warnings");

						foreach (XmlNode childResult in result.Xml.ChildNodes)
							WriteErrorsFailuresAndWarnings(childResult, ref index, 1);
					}

					Console.WriteLine("\nTest Run Summary");
					Console.WriteLine("  Overall Result: " + result.OverallResult);

					Console.WriteLine($"  Test Count: {result.Total}, Passed: {result.Passed}, Failed: {result.Failed}"
						+$" Warnings: {result.Warnings}, Inconclusive: {result.Inconclusive}, Skipped: {result.Skipped}\n");

					Console.WriteLine(report.Errors.Count == 0
						? "\nSUCCESS: Test Result matches expected result!"
						: "\nERROR: Test Result not as expected!");
				}
				catch (Exception ex)
				{
					reporter.AddReport(new TestReport(packageTest, ex));

					Console.WriteLine("\nERROR: No result found!");
				}
			}
		}

        bool hadErrors = reporter.ReportResults();
        Console.WriteLine();

        if (hadErrors)
            throw new Exception("One or more package tests had errors!");
	}

	private void DisplayTestEnvironment(PackageTest test)
	{
		Console.WriteLine("Test Environment");
		Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
		Console.WriteLine($"  CLR Version: {Environment.Version}");
		Console.WriteLine($"    Arguments: {test.Arguments}");
		Console.WriteLine();
	}

	private void WriteErrorsFailuresAndWarnings(XmlNode resultNode, ref int index, int level)
	{
		string resultState = GetAttribute(resultNode, "result");

		switch (resultNode.Name)
		{
			case "test-case":
				if (resultState == "Failed" || resultState == "Warning")
					WriteResultNode(resultNode, ++index);
				return;

			case "test-suite":
				if (resultState == "Failed" || resultState == "Warning")
				{
					var suiteType = GetAttribute(resultNode, "type");
					if (suiteType == "Theory")
					{
						// Report failure of the entire theory and then go on
						// to list the individual cases that failed
						WriteResultNode(resultNode, ++index);
					}
					else
					{
						// Where did this happen? Default is in the current test.
						var site = GetAttribute(resultNode, "site");

						// Correct a problem in some framework versions, whereby warnings and some failures 
						// are promulgated to the containing suite without setting the FailureSite.
						if (site == null)
						{ 
							if (resultNode.SelectSingleNode("reason/message")?.InnerText == "One or more child tests had warnings" ||
								resultNode.SelectSingleNode("failure/message")?.InnerText == "One or more child tests had errors")
							{
								site = "Child";
							}
							else
							{
								site = "Test";
							}
						}

						// Only report errors in the current test method, setup or teardown
						if (site == "SetUp" || site == "TearDown" || site == "Test")
							WriteResultNode(resultNode, ++index);

						// Do not list individual "failed" tests after a one-time setup failure
						if (site == "SetUp") return;
					}
				}

				foreach (XmlNode childResult in resultNode.ChildNodes)
					WriteErrorsFailuresAndWarnings(childResult, ref index, level + 1);
				break;
		}
	}

	private void WriteResultNode(XmlNode resultNode, int index)
	{
		var EOL_CHARS = new char[] { '\r', '\n' };
		string status = GetAttribute(resultNode, "label") ?? GetAttribute(resultNode, "result");
		string fullname = GetAttribute(resultNode, "fullname");
		string message = (resultNode.SelectSingleNode("failure/message") ?? resultNode.SelectSingleNode("reason/message"))?.InnerText.Trim(EOL_CHARS);
		string stackTrace = resultNode.SelectSingleNode("failure/stack-trace")?.InnerText.Trim(EOL_CHARS);

		Console.WriteLine($"\n{index}) {status} : {fullname}");
		if (message != null)
			Console.WriteLine(message);
		if (stackTrace != null)
			Console.WriteLine(stackTrace);
	}

	private string GetAttribute(XmlNode node, string name)
	{
		return node.Attributes[name]?.Value;
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
	protected override string PackageResultDirectory => _parameters.ZipResultDirectory;
	protected override string ExtensionInstallDirectory => PackageTestBinDirectory + "addins/";
	
	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
		HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.zip.addins"),
		HasDirectory("bin/agents/net20").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/agents/netcoreapp2.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.zip.addins"),
		HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	};

	protected override void InstallEngineExtension(string extension)
	{
		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings() { OutputDirectory = ExtensionInstallDirectory});
	}
}

public class NuGetPackageTester : PackageTester
{
	public NuGetPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.NuGetPackageName;
	protected override FilePath PackageUnderTest => _parameters.NuGetPackage;
	protected override string PackageTestDirectory => _parameters.NuGetTestDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	protected override string PackageResultDirectory => _parameters.NuGetResultDirectory;
	protected override string ExtensionInstallDirectory => _parameters.TestDirectory;
	
	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
		HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
		HasDirectory("tools/agents/net20").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/netcoreapp2.1").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
	};

	protected override void InstallEngineExtension(string extension)
	{
		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings() { OutputDirectory = ExtensionInstallDirectory});
	}
}

public class ChocolateyPackageTester : PackageTester
{
	public ChocolateyPackageTester(BuildParameters parameters) : base(parameters) { }

	protected override string PackageName => _parameters.ChocolateyPackageName;
	protected override FilePath PackageUnderTest => _parameters.ChocolateyPackage;
	protected override string PackageTestDirectory => _parameters.ChocolateyTestDirectory;
	protected override string PackageTestBinDirectory => PackageTestDirectory + "tools/";
	protected override string PackageResultDirectory => _parameters.ChocolateyResultDirectory;
	protected override string ExtensionInstallDirectory => _parameters.TestDirectory;
	
	// Chocolatey packages have a different naming convention from NuGet
	protected override string NUnitV2Driver => "nunit-extension-nunit-v2-driver";
	protected override string NUnitProjectLoader => "nunit-extension-nunit-project-loader";

	protected override PackageCheck[] PackageChecks => new PackageCheck[]
	{
		HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
		HasDirectory("tools/agents/net20").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/netcoreapp2.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
		HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
		HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
		HasDirectory("tools/Images/Tree/Visual%20Studio").WithFiles(TREE_ICONS_PNG)
	};

	protected override void InstallEngineExtension(string extension)
	{
		// Install with NuGet because choco requires administrator access
		_parameters.Context.NuGetInstall(extension,
			new NuGetInstallSettings()
			{
				Source = new [] { "https://chocolatey.org/api/v2/" },
				OutputDirectory = ExtensionInstallDirectory
			});
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
