//////////////////////////////////////////////////////////////////////
// PACKAGE TYPE ENUMERATION
//////////////////////////////////////////////////////////////////////

public enum PackageType
{
    NuGet,
    Chocolatey,
    Msi,
    Zip
}

//////////////////////////////////////////////////////////////////////
// PACKAGE DEFINITION ABSTRACT CLASS
//////////////////////////////////////////////////////////////////////

/// <summary>
/// The abstract base of all packages
/// </summary>
public abstract class PackageDefinition
{
    protected BuildSettings _settings;
    protected ICakeContext _context;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings">An instance of BuildSettings</param>
    /// <param name="packageType">A PackageType value specifying one of the four known package types</param>
    /// <param name="id">A string containing the package ID, used as the root of the PackageName</param>
    /// <param name="source">A string representing the source used to create the package, e.g. a nuspec file</param>
    /// <param name="basePath">Path used in locating binaries for the package</param>
    /// <param name="executable">A string containing the path to the executable used in running tests. If relative, the path is contained within the package itself.</param>
    /// <param name="checks">An array of PackageChecks be made on the content of the package. Optional.</param>
    /// <param name="symbols">An array of PackageChecks to be made on the symbol package, if one is created. Optional. Only supported for nuget packages.</param>
    /// <param name="tests">An array of PackageTests to be run against the package. Optional.</param>
	protected PackageDefinition(
        BuildSettings settings,
        PackageType packageType,
        string id,
        string source,
        string basePath,
        string executable = null,
        PackageCheck[] checks = null,
        PackageCheck[] symbols = null,
        IEnumerable<PackageTest> tests = null)
    {
        if (executable == null && tests != null)
            throw new System.ArgumentException($"Unable to create {packageType} package {id}: Executable must be provided if there are tests", nameof(executable));

        _settings = settings;
        _context = settings.SetupContext;

        PackageType = packageType;
        PackageId = id;
        PackageVersion = settings.PackageVersion;
        PackageSource = source;
        BasePath = basePath;
        TestExecutable = executable;
        PackageChecks = checks;
        PackageTests = tests;
        SymbolChecks = symbols;
    }

    public PackageType PackageType { get; }
	public string PackageId { get; protected set; }
    public string PackageVersion { get; protected set; }
	public string PackageSource { get; protected set; }
    public string BasePath { get; protected set; }
    public string TestExecutable { get; protected set; }
    public PackageCheck[] PackageChecks { get; protected set; }
    public PackageCheck[] SymbolChecks { get; protected set; }
    public IEnumerable<PackageTest> PackageTests { get; protected set; }
    public bool HasTests => PackageTests != null;
    public bool HasChecks => PackageChecks != null;
    public bool HasSymbols => SymbolChecks != null;
    public virtual string SymbolPackageName => throw new System.NotImplementedException($"Symbols are not available for this type of package.");

    public abstract string PackageFileName { get; }
    public abstract string PackageInstallDirectory { get; }
    public abstract string PackageResultDirectory { get; }
    public abstract string ExtensionInstallDirectory { get; }

    public string PackageFilePath => _settings.PackageDirectory + PackageFileName;

    protected abstract void doBuildPackage();
    protected abstract void doInstallPackage();

    public void BuildVerifyAndTest()
    {
        _context.EnsureDirectoryExists(_settings.PackageDirectory);

        BuildPackage();
        InstallPackage();

        if (HasChecks)
            VerifyPackage();

        if (HasSymbols)
            VerifySymbolPackage();

        if (HasTests)
            RunPackageTests();
    }

    public void BuildPackage()
    {
        DisplayAction("Building");
        doBuildPackage();
    }

    public void InstallPackage()
    {
        DisplayAction("Installing");
        Console.WriteLine($"Installing package to {PackageInstallDirectory}");
        _context.CleanDirectory(PackageInstallDirectory);
        doInstallPackage();
    }

    public void VerifyPackage()
    {
        DisplayAction("Verifying");

        bool allOK = true;
        foreach (var check in PackageChecks)
            allOK &= check.ApplyTo(PackageInstallDirectory);

        if (allOK)
            Console.WriteLine("All checks passed!");
        else 
            throw new Exception("Verification failed!");
    }

    public void RunPackageTests()
    {
        DisplayAction("Testing");

        var reporter = new ResultReporter(PackageFileName);

        _context.CleanDirectory(PackageResultDirectory);

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

        foreach (var packageTest in PackageTests)
        {
            foreach (ExtensionSpecifier extension in packageTest.ExtensionsNeeded)
                CheckExtensionIsInstalled(extension);

            var testResultDir = $"{PackageResultDirectory}{PackageType}/{packageTest.Name}/";
            var resultFile = testResultDir + "TestResult.xml";

            DisplayBanner(packageTest.Description);
			DisplayTestEnvironment(packageTest);

            _context.CreateDirectory(testResultDir);
            string arguments = packageTest.Arguments + $" --work={testResultDir} --run --unattended --full-gui";

            int rc = TestExecutable.EndsWith(".dll")
                ? _context.StartProcess(
                    "dotnet",
                    new ProcessSettings()
                    {
                        Arguments = $"\"{PackageInstallDirectory}{TestExecutable}\" {arguments}",
                        WorkingDirectory = _settings.OutputDirectory
                    })
                : _context.StartProcess(
                    PackageInstallDirectory + TestExecutable,
                    new ProcessSettings()
                    {
                        Arguments = arguments,
                        WorkingDirectory = _settings.OutputDirectory
                    });

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

        bool hadErrors = reporter.ReportResults();
        Console.WriteLine();

        if (hadErrors)
            throw new Exception("One or more package tests had errors!");
    }

    public void DisplayAction(string action)
    {
        DisplayBanner($"{action} package {PackageFileName}");
    }

	private void DisplayTestEnvironment(PackageTest test)
	{
		Console.WriteLine("Test Environment");
		Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
		Console.WriteLine($"  CLR Version: {Environment.Version}");
		//Console.WriteLine($"       Runner: {GuiRunner}");
		Console.WriteLine($"    Arguments: {test.Arguments}");
		Console.WriteLine();
	}

    public virtual void VerifySymbolPackage() { } // Does nothing. Overridden for NuGet packages.

    private void CheckExtensionIsInstalled(ExtensionSpecifier extension)
    {
        string extensionId = PackageType == PackageType.Chocolatey ? extension.ChocoId : extension.NuGetId;

        bool alreadyInstalled = _context.GetDirectories($"{ExtensionInstallDirectory}{extensionId}.*").Count > 0;

        if (!alreadyInstalled)
        {
            DisplayBanner($"Installing {extensionId} version {extension.Version}");

            _context.NuGetInstall(extensionId,
                new NuGetInstallSettings()
                {
                    OutputDirectory = ExtensionInstallDirectory,
                    Version = extension.Version
                });
        }
    }
}

//////////////////////////////////////////////////////////////////////
// NUGET PACKAGE DEFINITION
//////////////////////////////////////////////////////////////////////

// Users may only instantiate the derived classes, which avoids
// exposing PackageType and makes it impossible to create a
// PackageDefinition with an unknown package type.
public class NuGetPackageDefinition : PackageDefinition
{
    /// <summary>
    /// Construct passing all required arguments
    /// </summary>
    /// <param name="packageType">A PackageType value specifying one of the four known package types</param>
    /// <param name="id">A string containing the package ID, used as the root of the PackageName</param>
    /// <param name="source">A string representing the source used to create the package, e.g. a nuspec file</param>
    /// <param name="basePath">Path used in locating binaries for the package</param>
    /// <param name="executable">A string containing the path to the executable used in running tests. If relative, the path is contained within the package itself.</param>
    /// <param name="checks">An array of PackageChecks be made on the content of the package. Optional.</param>
    /// <param name="symbols">An array of PackageChecks to be made on the symbol package, if one is created. Optional. Only supported for nuget packages.</param>
    /// <param name="tests">An array of PackageTests to be run against the package. Optional.</param>
	public NuGetPackageDefinition(
        BuildSettings settings, string id, string source, string basePath, string executable = null,
        PackageCheck[] checks = null, PackageCheck[] symbols = null, IEnumerable<PackageTest> tests = null)
      : base (settings, PackageType.NuGet, id, source, basePath, executable: executable, checks: checks, symbols: symbols, tests: tests)
    {
    }

    public override string PackageFileName => $"{PackageId}.{PackageVersion}.nupkg";
    public override string SymbolPackageName => System.IO.Path.ChangeExtension(PackageFileName, ".snupkg");
    public override string PackageInstallDirectory => $"{_settings.NuGetTestDirectory}{PackageId}/";
    public override string PackageResultDirectory => $"{_settings.NuGetResultDirectory}{PackageId}/";
    public override string ExtensionInstallDirectory => _settings.NuGetTestDirectory;

    protected override void doBuildPackage()
    {
        var nugetPackSettings = new NuGetPackSettings()
        {
            Version = PackageVersion,
            OutputDirectory = _settings.PackageDirectory,
            BasePath = BasePath,
            NoPackageAnalysis = true,
            Symbols = HasSymbols
        };

        if (HasSymbols)
            nugetPackSettings.SymbolPackageFormat = "snupkg";

        _context.NuGetPack(PackageSource, nugetPackSettings);
    }

    protected override void doInstallPackage()
    {
        _context.NuGetInstall(PackageId, new NuGetInstallSettings
        {
            Source = new[] { _settings.PackageDirectory },
            Prerelease = true,
            OutputDirectory = _settings.NuGetTestDirectory,
            ExcludeVersion = true
        });
    }
}

//////////////////////////////////////////////////////////////////////
// CHOCOLATEY PACKAGE
//////////////////////////////////////////////////////////////////////

public class ChocolateyPackageDefinition : PackageDefinition
{
    /// <summary>
    /// Construct passing all required arguments
    /// </summary>
    /// <param name="packageType">A PackageType value specifying one of the four known package types</param>
    /// <param name="id">A string containing the package ID, used as the root of the PackageName</param>
    /// <param name="source">A string representing the source used to create the package, e.g. a nuspec file</param>
    /// <param name="basePath">Path used in locating binaries for the package</param>
    /// <param name="executable">A string containing the path to the executable used in running tests. If relative, the path is contained within the package itself.</param>
    /// <param name="checks">An array of PackageChecks be made on the content of the package. Optional.</param>
    /// <param name="symbols">An array of PackageChecks to be made on the symbol package, if one is created. Optional. Only supported for nuget packages.</param>
    /// <param name="tests">An array of PackageTests to be run against the package. Optional.</param>
	public ChocolateyPackageDefinition(
        BuildSettings settings, string id, string source, string basePath, string executable = null,
        PackageCheck[] checks = null, IEnumerable<PackageTest> tests = null)
      : base (settings, PackageType.Chocolatey, id, source, basePath, executable: executable, checks: checks, tests: tests)
    {
    }

    public override string PackageFileName => $"{PackageId}.{PackageVersion}.nupkg";
    public override string PackageInstallDirectory => $"{_settings.ChocoTestDirectory}{PackageId}/";
    public override string PackageResultDirectory => $"{_settings.ChocoResultDirectory}{PackageId}/";
    public override string ExtensionInstallDirectory => _settings.ChocoTestDirectory;
    
    protected override void doBuildPackage()
    {
        _context.ChocolateyPack(PackageSource,
            new ChocolateyPackSettings()
            {
                Version = PackageVersion,
                OutputDirectory = _settings.PackageDirectory,
                ArgumentCustomization = args => args.Append($"BIN={BasePath}")
            });
    }

    protected override void doInstallPackage()
    {
        // TODO: We can't run chocolatey install effectively
        // so for now we just unzip the package.
        _context.Unzip(_settings.PackageDirectory + PackageFileName, PackageInstallDirectory);
    }
}

///////////////////////////////////////////////////////// /////////////
// ZIP PACKAGE
//////////////////////////////////////////////////////////////////////

public class ZipPackageDefinition : PackageDefinition
{
    /// <summary>
    /// Construct passing all required arguments
    /// </summary>
    /// <param name="packageType">A PackageType value specifying one of the four known package types</param>
    /// <param name="id">A string containing the package ID, used as the root of the PackageName</param>
    /// <param name="source">A string representing the source used to create the package, e.g. a nuspec file</param>
    /// <param name="basePath">Path used in locating binaries for the package</param>
    /// <param name="executable">A string containing the path to the executable used in running tests. If relative, the path is contained within the package itself.</param>
    /// <param name="checks">An array of PackageChecks be made on the content of the package. Optional.</param>
    /// <param name="symbols">An array of PackageChecks to be made on the symbol package, if one is created. Optional. Only supported for nuget packages.</param>
    /// <param name="tests">An array of PackageTests to be run against the package. Optional.</param>
	public ZipPackageDefinition(
        BuildSettings settings, string id, string source, string basePath, string executable = null,
        PackageCheck[] checks = null, IEnumerable<PackageTest> tests = null)
      : base (settings, PackageType.Zip, id, source, basePath, executable: executable, checks: checks, tests: tests)
    {
    }

    public override string PackageFileName => $"{PackageId}-{PackageVersion}.zip";
    public override string PackageInstallDirectory => $"{_settings.ZipTestDirectory}{PackageId}/";
    public override string PackageResultDirectory => $"{_settings.ZipResultDirectory}{PackageId}/";
    public override string ExtensionInstallDirectory => $"{_settings.ZipTestDirectory}{PackageId}/bin/addins/";
  
    protected override void doBuildPackage()
    {
        _context.Zip(_settings.ZipImageDirectory, _settings.PackageDirectory + PackageFileName);
    }

    protected override void doInstallPackage()
    {
        _context.Unzip(_settings.PackageDirectory + PackageFileName, PackageInstallDirectory);
    }
}
