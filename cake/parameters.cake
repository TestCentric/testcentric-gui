#load "./versioning.cake"
#load "./packaging.cake"
#load "./package-checks.cake"
#load "./package-tests.cake"
#load "./test-results.cake"
#load "./test-reports.cake"
#load "./check-headers.cake"
#load "./console-reporter.cake"
#load "./utilities.cake"
#load "./local-targets.cake"

// URLs for uploading packages
private const string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
private const string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";
private const string CHOCO_PUSH_URL = "https://push.chocolatey.org/";

// Environment Variable names holding API keys
private const string MYGET_API_KEY = "MYGET_API_KEY";
private const string NUGET_API_KEY = "NUGET_API_KEY";
private const string CHOCO_API_KEY = "CHOCO_API_KEY";
private const string GITHUB_ACCESS_TOKEN = "GITHUB_ACCESS_TOKEN";

// Pre-release labels that we publish
private static readonly string[] LABELS_WE_PUBLISH_ON_MYGET = { "dev", "pre" };
private static readonly string[] LABELS_WE_PUBLISH_ON_NUGET = { "alpha", "beta", "rc" };
private static readonly string[] LABELS_WE_PUBLISH_ON_CHOCOLATEY = { "alpha", "beta", "rc" };
private static readonly string[] LABELS_WE_RELEASE_ON_GITHUB = { "alpha", "beta", "rc" };

public class BuildParameters
{
	private ISetupContext _context;
	private BuildSystem _buildSystem;

	public static BuildParameters Create(ISetupContext context)
	{
		var parameters = new BuildParameters(context);
		parameters.Validate();

		return parameters;
	}

	private BuildParameters(ISetupContext context)
	{
		_context = context;
		_buildSystem = _context.BuildSystem();

		Target = _context.TargetTask.Name;
		TasksToExecute = _context.TasksToExecute.Select(t => t.Name);

		Configuration = context.Argument("configuration", context.Argument("c", DEFAULT_CONFIGURATION));
		ProjectDirectory = context.Environment.WorkingDirectory.FullPath + "/";

		MyGetApiKey = _context.EnvironmentVariable(MYGET_API_KEY);
		NuGetApiKey = _context.EnvironmentVariable(NUGET_API_KEY);
		ChocolateyApiKey = _context.EnvironmentVariable(CHOCO_API_KEY);
		GitHubAccessToken = _context.EnvironmentVariable(GITHUB_ACCESS_TOKEN);

		BuildVersion = new BuildVersion(context, this);

		if (context.HasArgument("testLevel"))
			PackageTestLevel = context.Argument("testLevel", 1);
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

	public string Target { get; }
	public IEnumerable<string> TasksToExecute { get; }

	public ICakeContext Context => _context;

	public string Configuration { get; private set; }

	public BuildVersion BuildVersion { get; }
	public string PackageVersion => BuildVersion.PackageVersion;
	public string AssemblyVersion => BuildVersion.AssemblyVersion;
	public string AssemblyFileVersion => BuildVersion.AssemblyFileVersion;
	public string AssemblyInformationalVersion => BuildVersion.AssemblyInformationalVersion;

	public int PackageTestLevel { get; }

	public bool IsLocalBuild => _buildSystem.IsLocalBuild;
	public bool IsRunningOnUnix => _context.IsRunningOnUnix();
	public bool IsRunningOnWindows => _context.IsRunningOnWindows();
	public bool IsRunningOnAppVeyor => _buildSystem.AppVeyor.IsRunningOnAppVeyor;

	public string ProjectDirectory { get; }
	public string SourceDirectory => ProjectDirectory + "src/";
	public string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public string ZipDirectory => ProjectDirectory + "zip/";
	public string NuGetDirectory => ProjectDirectory + "nuget/";
	public string ChocoDirectory => ProjectDirectory + "choco/";
	public string PackageDirectory => ProjectDirectory + "package/";
	public string ZipImageDirectory => PackageDirectory + "zipimage/";
	public string TestDirectory => PackageDirectory + "tests/";
	public string ZipTestDirectory => TestDirectory + "zip/";
	public string NuGetTestDirectory => TestDirectory + "nuget/";
	public string ChocolateyTestDirectory => TestDirectory + "choco/";
	public string ResultDirectory => PackageDirectory + "results/";
	public string ZipResultDirectory => ResultDirectory + "zip/";
	public string NuGetResultDirectory => ResultDirectory + "nuget/";
	public string ChocolateyResultDirectory => ResultDirectory + "choco/";
	public string WebDirectory => ProjectDirectory + "web/";
	public string WebOutputDirectory => WebDirectory + "output/";
	public string WebDeployDirectory => ProjectDirectory + "../testcentric-gui.deploy/";

	public string ZipPackageName => PACKAGE_NAME + "-" + PackageVersion + ".zip";
	public string NuGetPackageName => NUGET_PACKAGE_NAME + "." + PackageVersion + ".nupkg";
	public string ChocolateyPackageName => PACKAGE_NAME + "." + PackageVersion + ".nupkg";

	public FilePath ZipPackage => new FilePath(PackageDirectory + ZipPackageName);
	public FilePath NuGetPackage => new FilePath(PackageDirectory + NuGetPackageName);
	public FilePath ChocolateyPackage => new FilePath(PackageDirectory + ChocolateyPackageName);

	public string GitHubReleaseAssets => _context.IsRunningOnWindows()
		? $"\"{ZipPackage},{NuGetPackage},{ChocolateyPackage}\""
        : $"\"{ZipPackage},{NuGetPackage}\"";

	public string MyGetPushUrl => MYGET_PUSH_URL;
	public string NuGetPushUrl => NUGET_PUSH_URL;
	public string ChocolateyPushUrl => CHOCO_PUSH_URL;
	
	public string MyGetApiKey { get; }
	public string NuGetApiKey { get; }
	public string ChocolateyApiKey { get; }

    public string BranchName => BuildVersion.BranchName;
	public bool IsReleaseBranch => BuildVersion.IsReleaseBranch;

	public bool IsPreRelease => BuildVersion.IsPreRelease;
	public bool ShouldPublishToMyGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_MYGET.Contains(BuildVersion.PreReleaseLabel);
	public bool ShouldPublishToNuGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_NUGET.Contains(BuildVersion.PreReleaseLabel);
	public bool ShouldPublishToChocolatey =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_CHOCOLATEY.Contains(BuildVersion.PreReleaseLabel);
	public bool IsProductionRelease =>
		!IsPreRelease || LABELS_WE_RELEASE_ON_GITHUB.Contains(BuildVersion.PreReleaseLabel);
	
	public MSBuildSettings MSBuildSettings { get; }
	public NuGetRestoreSettings RestoreSettings { get; }

	public string[] SupportedEngineRuntimes => new string[] {"net40"};

	public string ProjectUri => "https://github.com/TestCentric/testcentric-gui";
	public string GitHubAccessToken { get; }

	private void Validate()
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

	public void DumpSettings()
	{
		Console.WriteLine("\nTASKS");
		Console.WriteLine("Target:                       " + Target);
		Console.WriteLine("TasksToExecute:               " + string.Join(", ", TasksToExecute));

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
		Console.WriteLine("Project:   " + ProjectDirectory);
		Console.WriteLine("Output:    " + OutputDirectory);
		Console.WriteLine("Source:    " + SourceDirectory);
		Console.WriteLine("NuGet:     " + NuGetDirectory);
		Console.WriteLine("Choco:     " + ChocoDirectory);
		Console.WriteLine("Package:   " + PackageDirectory);
		Console.WriteLine("ZipImage:  " + ZipImageDirectory);
		Console.WriteLine("ZipTest:   " + ZipTestDirectory);
		Console.WriteLine("NuGetTest: " + NuGetTestDirectory);
		Console.WriteLine("ChocoTest: " + ChocolateyTestDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", SupportedEngineRuntimes));

		Console.WriteLine("\nPACKAGING");
		Console.WriteLine("MyGetPushUrl:              " + MyGetPushUrl);
		Console.WriteLine("NuGetPushUrl:              " + NuGetPushUrl);
		Console.WriteLine("ChocolateyPushUrl:         " + ChocolateyPushUrl);
		Console.WriteLine("MyGetApiKey:               " + (!string.IsNullOrEmpty(MyGetApiKey) ? "AVAILABLE" : "NOT AVAILABLE"));
		Console.WriteLine("NuGetApiKey:               " + (!string.IsNullOrEmpty(NuGetApiKey) ? "AVAILABLE" : "NOT AVAILABLE"));
		Console.WriteLine("ChocolateyApiKey:          " + (!string.IsNullOrEmpty(ChocolateyApiKey) ? "AVAILABLE" : "NOT AVAILABLE"));

		Console.WriteLine("\nPUBLISHING");
		Console.WriteLine("ShouldPublishToMyGet:      " + ShouldPublishToMyGet);
		Console.WriteLine("ShouldPublishToNuGet:      " + ShouldPublishToNuGet);
		Console.WriteLine("ShouldPublishToChocolatey: " + ShouldPublishToChocolatey);

		Console.WriteLine("\nRELEASING");
		Console.WriteLine("BranchName:                   " + BranchName);
		Console.WriteLine("IsReleaseBranch:              " + IsReleaseBranch);
		Console.WriteLine("IsProductionRelease:          " + IsProductionRelease);
	}
}
