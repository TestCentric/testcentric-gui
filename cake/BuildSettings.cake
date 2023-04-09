//////////////////////////////////////////////////////////////////////
// DUMP SETTINGS
//////////////////////////////////////////////////////////////////////

Task("DumpSettings")
	.Does(() =>
	{
		BuildSettings.DumpSettings();
	});

//////////////////////////////////////////////////////////////////////
// BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

public static class BuildSettings
{
	private static BuildSystem _buildSystem;

	public static void Initialize(
		// Required parameters
		ICakeContext context,
		string title,
		// Optional named parameters
		string solutionFile = null,
        string unitTests = GUI_TESTS,
		TestRunner unitTestRunner = null,
		string[] exemptFiles = null)
	{
		if (context == null)
			throw new ArgumentNullException(nameof(context));

		Context = context;
		_buildSystem = context.BuildSystem();

		Title = title;
		SolutionFile = solutionFile;
		UnitTests= unitTests;
		UnitTestRunner = unitTestRunner;

		StandardHeader = DEFAULT_STANDARD_HEADER;
		ExemptFiles = exemptFiles ?? new string[0];

		Configuration = context.Argument("configuration", context.Argument("c", DEFAULT_CONFIGURATION));
		ProjectDirectory = context.Environment.WorkingDirectory.FullPath + "/";

        MyGetApiKey = GetApiKey(TESTCENTRIC_MYGET_API_KEY, MYGET_API_KEY);
        NuGetApiKey = GetApiKey(TESTCENTRIC_NUGET_API_KEY, NUGET_API_KEY);
		ChocolateyApiKey = GetApiKey(TESTCENTRIC_CHOCO_API_KEY, CHOCO_API_KEY);
		GitHubAccessToken = context.EnvironmentVariable(GITHUB_ACCESS_TOKEN);

		BuildVersion = new BuildVersion(context);

		if (HasArgument("testLevel|level"))
			PackageTestLevel = GetArgument("testLevel|level", 1);
		else if (!BuildVersion.IsPreRelease)
			PackageTestLevel = 3;
		else switch (BuildVersion.PreReleaseLabel)
		{
			case "pre":
			case "rc":
			case "alpha":
			case "beta":
				PackageTestLevel = 3;
				break;
			case "dev":
			case "pr":
				PackageTestLevel = 2;
				break;
			case "ci":
			default:
				PackageTestLevel = 1;
				break;
		}

		MSBuildSettings = new MSBuildSettings {
			Verbosity = Verbosity.Minimal,
			//ToolVersion = MSBuildToolVersion.Default,//The highest available MSBuild tool version//VS2017
			Configuration = Configuration,
			PlatformTarget = PlatformTarget.MSIL,
			//MSBuildPlatform = MSBuildPlatform.Automatic,
			DetailedSummary = true,
		};

		RestoreSettings = new NuGetRestoreSettings();
	}

	// Cake Context
	public static ICakeContext Context { get; private set; }

	// Targets - not set until Setup runs
	public static string Target { get; }
	public static IEnumerable<string> TasksToExecute { get; set; }

	// Arguments
	public static string Configuration { get; private set; }
	public static bool NoPush => Context.HasArgument("nopush");

	// Versioning
	public static BuildVersion BuildVersion { get; private set; }
    public static string BranchName => BuildVersion.BranchName;
	public static bool IsReleaseBranch => BuildVersion.IsReleaseBranch;
	public static string PackageVersion => BuildVersion.PackageVersion;
	public static string AssemblyVersion => BuildVersion.AssemblyVersion;
	public static string AssemblyFileVersion => BuildVersion.AssemblyFileVersion;
	public static string AssemblyInformationalVersion => BuildVersion.AssemblyInformationalVersion;
	public static bool IsPreRelease => BuildVersion.IsPreRelease;

	public static string SolutionFile { get; private set; }
	public static string Title { get; private set; }

	// Checking 
	public static string[] StandardHeader { get; private set; }
	public static string[] ExemptFiles { get; private set; }

	//Testing
	public static string UnitTests { get; private set; }
	public static TestRunner UnitTestRunner {get; private set; }

	//public static List<PackageTest> PackageTests { get; } = new List<PackageTest>();
	public static int PackageTestLevel { get; private set; }

	public static ExtensionSpecifier NUnitV2Driver =>
		new ExtensionSpecifier("NUnit.Extension.NUnitV2Driver", "nunit-extension-nunit-v2-driver", "3.9.0");
	public static ExtensionSpecifier NUnitProjectLoader => 
		new ExtensionSpecifier("NUnit.Extension.NUnitProjectLoader", "nunit-extension-nunit-project-loader", "3.7.1");
	public static ExtensionSpecifier Net20PluggableAgent => 
		new ExtensionSpecifier("NUnit.Extension.Net20PluggableAgent", "nunit-extension-net20-pluggable-agent", "2.0.0");
	public static ExtensionSpecifier NetCore21PluggableAgent => 
		new ExtensionSpecifier("NUnit.Extension.NetCore21PluggableAgent", "nunit-extension-netcore21-pluggable-agent", "2.1.0");
	public static ExtensionSpecifier Net80PluggableAgent => 
		new ExtensionSpecifier("NUnit.Extension.Net80PluggableAgent", "nunit-extension-net80-pluggable-agent", "2.1.0");

	private static List<ExtensionSpecifier> InstalledExtensions { get; } = new List<ExtensionSpecifier>();
	public static bool IsLocalBuild => _buildSystem.IsLocalBuild;
	public static bool IsRunningOnUnix => Context.IsRunningOnUnix();
	public static bool IsRunningOnWindows => Context.IsRunningOnWindows();
	public static bool IsRunningOnAppVeyor => _buildSystem.AppVeyor.IsRunningOnAppVeyor;

	// Standard Directory Structure - not changeable by user
	public static string ProjectDirectory { get; private set; }
	public static string SourceDirectory => ProjectDirectory + "src/";
	public static string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public static string ZipDirectory => ProjectDirectory + "zip/";
	public static string NuGetDirectory => ProjectDirectory + "nuget/";
	public static string ChocolateyDirectory => ProjectDirectory + "choco/";
	public static string PackageDirectory => ProjectDirectory + "package/";
	public static string PackageTestDirectory => PackageDirectory + "tests/";
	public static string NuGetTestDirectory => PackageTestDirectory + "nuget/";
	public static string NuGetTestRunnerDirectory => NuGetTestDirectory + "runners/";
	public static string ChocolateyTestDirectory => PackageTestDirectory + "choco/";
	public static string ChocolateyTestRunnerDirectory => ChocolateyTestDirectory + "runners/";
	public static string ZipTestDirectory => PackageTestDirectory + "zip/";
	public static string PackageResultDirectory => PackageDirectory + "results/";
	public static string NuGetResultDirectory => PackageResultDirectory + "nuget/";
	public static string ChocolateyResultDirectory => PackageResultDirectory + "choco/";
	public static string ZipResultDirectory => PackageResultDirectory + "zip/";
	public static string ZipImageDirectory => PackageDirectory + "zipimage/";

	public static List<PackageDefinition> Packages { get; } = new List<PackageDefinition>();

	public static string GitHubRepository => "testcentric-gui";
	public static string GitHubOwner => "TestCentric";
	//public static string GitHubReleaseAssets => Context.IsRunningOnWindows()
	//    ? $"\"{ZipPackage},{NuGetPackage},{ChocolateyPackage}\""
    //    : $"\"{ZipPackage},{NuGetPackage}\"";

	public static string MyGetPushUrl => MYGET_PUSH_URL;
	public static string NuGetPushUrl => NUGET_PUSH_URL;
	public static string ChocolateyPushUrl => CHOCO_PUSH_URL;
	
	public static string MyGetApiKey { get; private set; }
	public static string NuGetApiKey { get; private set; }
	public static string ChocolateyApiKey { get; private set; }
	public static string GitHubAccessToken { get; private set; }

	public static bool ShouldPublishToMyGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_MYGET.Contains(BuildVersion.PreReleaseLabel);
	public static bool ShouldPublishToNuGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_NUGET.Contains(BuildVersion.PreReleaseLabel);
	public static bool ShouldPublishToChocolatey =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_CHOCOLATEY.Contains(BuildVersion.PreReleaseLabel);
	public static bool IsProductionRelease =>
		!IsPreRelease || LABELS_WE_RELEASE_ON_GITHUB.Contains(BuildVersion.PreReleaseLabel);
	
	public static MSBuildSettings MSBuildSettings { get; private set; }
	public static NuGetRestoreSettings RestoreSettings { get; private set; }

	public static string[] SupportedEngineRuntimes => new string[] {"net40"};

	public static bool HasArgument(string altNames)
	{
		foreach (string name in altNames.Split('|'))
			if (Context.HasArgument(name))
				return true;

		return false;
	}

	public static T GetArgument<T>(string altNames, T defaultValue)
	{
		foreach (string name in altNames.Split('|'))
			if (Context.HasArgument(name))
				return Context.Argument(name, defaultValue);

		return defaultValue;
	}

	private static void Validate()
	{
		var validationErrors = new List<string>();

		bool validConfig = false;
		foreach (string config in VALID_CONFIGS)
		{
			if (string.Equals(config, Configuration, StringComparison.OrdinalIgnoreCase))
			{
				Configuration = config;
				validConfig = true;
			}
		}

		if (!validConfig)
			validationErrors.Add($"Invalid configuration: {Configuration}");
			
		if (TasksToExecute.Contains("PublishPackages"))
		{
			if (ShouldPublishToMyGet && string.IsNullOrEmpty(MyGetApiKey))
				validationErrors.Add("MyGet ApiKey was not set.");
			if (ShouldPublishToNuGet && string.IsNullOrEmpty(NuGetApiKey))
				validationErrors.Add("NuGet ApiKey was not set.");
			if (ShouldPublishToChocolatey && string.IsNullOrEmpty(ChocolateyApiKey))
				validationErrors.Add("Chocolatey ApiKey was not set.");
		}

		if (TasksToExecute.Contains("CreateDraftRelease") && (IsReleaseBranch || IsProductionRelease))
		{
			if (string.IsNullOrEmpty(GitHubAccessToken))
				validationErrors.Add("GitHub Access Token was not set.");		
		}

		if (validationErrors.Count > 0)
		{
			DumpSettings();

			var msg = new StringBuilder("Parameter validation failed! See settings above.\r\n\nErrors found:\r\n");
			foreach (var error in validationErrors)
				msg.AppendLine("  " + error);

			throw new InvalidOperationException(msg.ToString());
		}
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

	public static void DumpSettings()
	{
		//Console.WriteLine("\nTASKS");
		//Console.WriteLine("Target:                       " + Target);
		//Console.WriteLine("TasksToExecute:               " + string.Join(", ", TasksToExecute));

		Console.WriteLine("\nENVIRONMENT");
		Console.WriteLine("IsLocalBuild:                 " + IsLocalBuild);
		Console.WriteLine("IsRunningOnWindows:           " + IsRunningOnWindows);
		Console.WriteLine("IsRunningOnUnix:              " + IsRunningOnUnix);
		Console.WriteLine("IsRunningOnAppVeyor:          " + IsRunningOnAppVeyor);

		Console.WriteLine("\nVERSIONING");
		Console.WriteLine("PackageVersion:               " + PackageVersion);
		Console.WriteLine("AssemblyVersion:              " + AssemblyVersion);
		Console.WriteLine("AssemblyFileVersion:          " + AssemblyFileVersion);
		Console.WriteLine("AssemblyInformationalVersion: " + AssemblyInformationalVersion);
		Console.WriteLine("SemVer:                       " + BuildVersion.SemVer);
		Console.WriteLine("IsPreRelease:                 " + BuildVersion.IsPreRelease);
		Console.WriteLine("PreReleaseLabel:              " + BuildVersion.PreReleaseLabel);
		Console.WriteLine("PreReleaseSuffix:             " + BuildVersion.PreReleaseSuffix);

		Console.WriteLine("\nDIRECTORIES");
		Console.WriteLine("Project:    " + ProjectDirectory);
		Console.WriteLine("Output:     " + OutputDirectory);
		Console.WriteLine("Source:     " + SourceDirectory);
		Console.WriteLine("NuGet:      " + NuGetDirectory);
		Console.WriteLine("Chocolatey: " + ChocolateyDirectory);
		Console.WriteLine("Package:    " + PackageDirectory);
		Console.WriteLine("ZipImage:   " + ZipImageDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", SupportedEngineRuntimes));

		Console.WriteLine("\nPACKAGES");
		foreach (PackageDefinition package in Packages)
		{
			Console.WriteLine($"{package.PackageFileName}");
			Console.WriteLine($"  PackageId:               {package.PackageId}");
			Console.WriteLine($"  PackageVersion:          {package.PackageVersion}");
			Console.WriteLine($"  PackageSource:           {package.PackageSource}");
			Console.WriteLine($"  BasePath:                {package.BasePath}");
			Console.WriteLine($"  PackageInstallDirectory: {package.PackageInstallDirectory}");
			Console.WriteLine($"  ResultDirectory:         {package.PackageResultDirectory}");
		}

		Console.WriteLine("\nPUBLISHING");
		Console.WriteLine("ShouldPublishToMyGet:      " + ShouldPublishToMyGet);
		Console.WriteLine("  MyGetPushUrl:            " + MyGetPushUrl);
		Console.WriteLine("  MyGetApiKey:             " + KeyAvailable(MYGET_API_KEY));
		Console.WriteLine("ShouldPublishToNuGet:      " + ShouldPublishToNuGet);
		Console.WriteLine("  NuGetPushUrl:            " + NuGetPushUrl);
		Console.WriteLine("  NuGetApiKey:             " + KeyAvailable(NUGET_API_KEY));
		Console.WriteLine("ShouldPublishToChocolatey: " + ShouldPublishToChocolatey);
		Console.WriteLine("  ChocolateyPushUrl:       " + ChocolateyPushUrl);
		Console.WriteLine("  ChocolateyApiKey:        " + KeyAvailable(CHOCO_API_KEY));

		Console.WriteLine("\nRELEASING");
		Console.WriteLine("BranchName:                 " + BranchName);
		Console.WriteLine("IsReleaseBranch:            " + IsReleaseBranch);
		Console.WriteLine("IsProductionRelease:        " + IsProductionRelease);
		Console.WriteLine("GitHubAccessToken:          " + KeyAvailable(GITHUB_ACCESS_TOKEN));
	}

    private static string GetApiKey(string name, string fallback=null)
    {
        var apikey = Context.EnvironmentVariable(name);

        if (string.IsNullOrEmpty(apikey) && fallback != null)
            apikey = Context.EnvironmentVariable(fallback);

        return apikey;
    }

	private static string KeyAvailable(string name)
	{
		return !string.IsNullOrEmpty(GetApiKey(name)) ? "AVAILABLE" : "NOT AVAILABLE";
	}
}
