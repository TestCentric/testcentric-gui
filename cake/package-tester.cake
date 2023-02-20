const string DEFAULT_TEST_RESULT_FILE = "TestResult.xml";

// Abstract base for all package testers.
public abstract class PackageTester
{
    protected BuildSettings _settings;
    private ICakeContext _context;

    public PackageTester(BuildSettings settings)
    {
        _settings = settings;
        _context = settings.SetupContext;
    }

    protected abstract string PackageName { get; }
    protected abstract FilePath PackageUnderTest { get; }
    protected abstract string PackageTestDirectory { get; }
	protected abstract string PackageResultDirectory { get; }
    protected abstract string PackageTestBinDirectory { get; }
    protected abstract string ExtensionInstallDirectory { get; }

    public void RunAllTests()
    {
        Console.WriteLine("Testing package " + PackageName);

        RunPackageTests(_settings.PackageTestLevel);

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
            _settings.OutputDirectory + TEST_BED_EXE, 
            PackageTestBinDirectory);

        var reporter = new ResultReporter(PackageName);

        foreach (var packageTest in _settings.PackageTests)
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
                        WorkingDirectory = _settings.OutputDirectory
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
}

public class EnginePackageTester : PackageTester
{
    public EnginePackageTester(BuildSettings settings) : base(settings) { }

    protected override string PackageName => _settings.EnginePackageName;
    protected override FilePath PackageUnderTest => _settings.EnginePackage;
    protected override string PackageTestDirectory => _settings.EngineTestDirectory;
    protected override string PackageResultDirectory => _settings.EngineResultDirectory;
    protected override string PackageTestBinDirectory => _settings.EngineTestDirectory + "tools/";
    protected override string ExtensionInstallDirectory => _settings.NuGetTestDirectory;

    protected override void InstallEngineExtension(ExtensionSpecifier extension)
    {
        _settings.SetupContext.NuGetInstall(extension.Id,
            new NuGetInstallSettings()
            {
                OutputDirectory = ExtensionInstallDirectory,
                Version = extension.Version
            });
    }
}
