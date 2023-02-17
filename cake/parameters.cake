#load "./versioning.cake"
#load "./package-checks.cake"
#load "./package-tests.cake"
#load "./package-tester.cake"
#load "./test-results.cake"
#load "./test-reports.cake"
#load "./check-headers.cake"
#load "./utilities.cake"
#load "./local-targets.cake"

// URLs for uploading packages
private const string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
private const string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";

// Environment Variable names holding API keys
private const string MYGET_API_KEY = "TESTCENTRIC_MYGET_API_KEY";
private const string FALLBACK_MYGET_API_KEY = "MYGET_API_KEY";
private const string NUGET_API_KEY = "TESTCENTRIC_NUGET_API_KEY";
private const string FALLBACK_NUGET_API_KEY = "NUGET_API_KEY";
private const string GITHUB_ACCESS_TOKEN = "GITHUB_ACCESS_TOKEN";

// Pre-release labels that we publish
private static readonly string[] LABELS_WE_PUBLISH_ON_MYGET = { "dev" };
private static readonly string[] LABELS_WE_PUBLISH_ON_NUGET = { "alpha", "beta", "rc" };
private static readonly string[] LABELS_WE_RELEASE_ON_GITHUB = { "alpha", "beta", "rc" };

public class BuildParameters
{
	private BuildSystem _buildSystem;

	// BuildParameters is effectively a singleton because it is only created in the Setup method.
	private static BuildParameters _instance;

	public static BuildParameters CreateInstance(ISetupContext context)
	{
		if (_instance != null)
			throw new Exception("BuildParameters instance may only be created once.");

		_instance = new BuildParameters(context);
		_instance.Validate();

		return _instance;
	}

	private BuildParameters(ISetupContext context)
	{
		SetupContext = context;
		_buildSystem = SetupContext.BuildSystem();

		Target = SetupContext.TargetTask.Name;
		TasksToExecute = SetupContext.TasksToExecute.Select(t => t.Name);

		Configuration = GetArgument("configuration|c", DEFAULT_CONFIGURATION);
		ProjectDirectory = context.Environment.WorkingDirectory.FullPath + "/";

        MyGetApiKey = GetApiKey(MYGET_API_KEY, FALLBACK_MYGET_API_KEY);
        NuGetApiKey = GetApiKey(NUGET_API_KEY, FALLBACK_NUGET_API_KEY);
		GitHubAccessToken = SetupContext.EnvironmentVariable(GITHUB_ACCESS_TOKEN);

		UsingXBuild = context.EnvironmentVariable("USE_XBUILD") != null;
		
		BuildVersion = new BuildVersion(context, this);

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

		XBuildSettings = new XBuildSettings {
			Verbosity = Verbosity.Minimal,
			ToolVersion = XBuildToolVersion.Default,//The highest available XBuild tool version//NET40
			Configuration = Configuration,
		};

		RestoreSettings = new NuGetRestoreSettings();
		// Older Mono version was not picking up the testcentric source
		// TODO: Check if this is still needed
		if (UsingXBuild)
			RestoreSettings.Source = new string [] {
				"https://www.myget.org/F/testcentric/api/v2/",
				"https://www.myget.org/F/testcentric/api/v3/index.json",
				"https://www.nuget.org/api/v2/",
				"https://api.nuget.org/v3/index.json",
				"https://www.myget.org/F/nunit/api/v2/",
				"https://www.myget.org/F/nunit/api/v3/index.json"
			};
	}

	public ISetupContext SetupContext { get; }

	public string Target { get; }
	public IEnumerable<string> TasksToExecute { get; }

	public string Configuration { get; private set; }

	public BuildVersion BuildVersion { get; }
	public string PackageVersion => BuildVersion.PackageVersion;
	public string AssemblyVersion => BuildVersion.AssemblyVersion;
	public string AssemblyFileVersion => BuildVersion.AssemblyFileVersion;
	public string AssemblyInformationalVersion => BuildVersion.AssemblyInformationalVersion;

	public int PackageTestLevel { get; }

	public bool IsLocalBuild => _buildSystem.IsLocalBuild;
	public bool IsRunningOnUnix => SetupContext.IsRunningOnUnix();
	public bool IsRunningOnWindows => SetupContext.IsRunningOnWindows();
	public bool IsRunningOnAppVeyor => _buildSystem.AppVeyor.IsRunningOnAppVeyor;

	public string ProjectDirectory { get; }
	public string SourceDirectory => ProjectDirectory + "src/";
	public string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public string NuGetDirectory => ProjectDirectory + "nuget/";
	public string PackageDirectory => ProjectDirectory + "package/";
	public string TestDirectory => PackageDirectory + "test/";
	public string EngineTestDirectory => TestDirectory + ENGINE_PACKAGE_ID + "/";
	public string EngineCoreTestDirectory => TestDirectory + ENGINE_CORE_PACKAGE_ID + "/";
	public string EngineApiTestDirectory => TestDirectory + ENGINE_API_PACKAGE_ID + "/";
	public string ResultDirectory => PackageDirectory + "results/";
	public string EngineResultDirectory => ResultDirectory + ENGINE_PACKAGE_ID + "/";

	public string EnginePackageName => ENGINE_PACKAGE_ID + "." + PackageVersion + ".nupkg";
	public string EngineCorePackageName => ENGINE_CORE_PACKAGE_ID + "." + PackageVersion + ".nupkg";
	public string EngineApiPackageName => ENGINE_API_PACKAGE_ID + "." + PackageVersion + ".nupkg";

	public FilePath EnginePackage => new FilePath(PackageDirectory + EnginePackageName);
	public FilePath EngineCorePackage => new FilePath(PackageDirectory + EngineCorePackageName);
	public FilePath EngineApiPackage => new FilePath(PackageDirectory + EngineApiPackageName);

	public string GitHubReleaseAssets => SetupContext.IsRunningOnWindows()
		? $"\"{EnginePackage},{EngineCorePackage},{EngineApiPackage}\""
        : $"\"{EnginePackage}\"";

	public string MyGetPushUrl => MYGET_PUSH_URL;
	public string NuGetPushUrl => NUGET_PUSH_URL;
	
	public string MyGetApiKey { get; }
	public string NuGetApiKey { get; }

    public string BranchName => BuildVersion.BranchName;
	public bool IsReleaseBranch => BuildVersion.IsReleaseBranch;

	public bool IsPreRelease => BuildVersion.IsPreRelease;
	public bool ShouldPublishToMyGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_MYGET.Contains(BuildVersion.PreReleaseLabel);
	public bool ShouldPublishToNuGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_NUGET.Contains(BuildVersion.PreReleaseLabel);
	public bool IsProductionRelease =>
		!IsPreRelease || LABELS_WE_RELEASE_ON_GITHUB.Contains(BuildVersion.PreReleaseLabel);
	
	public bool UsingXBuild { get; }
	public MSBuildSettings MSBuildSettings { get; }
	public XBuildSettings XBuildSettings { get; }
	public NuGetRestoreSettings RestoreSettings { get; }

	public string[] EngineRuntimes => new string[] {"net462"};
	public string[] EngineCoreRuntimes => new string[] {"net462", "net35", "netstandard2.0", "netcoreapp3.1"};
	// Unused at present
	public string[] AgentRuntimes => new string[] {
		"net20", "net462", "netcoreapp2.1", "netcoreapp3.1", "net5.0", "net6.0", "net7.0" };

	public string ProjectUri => "https://github.com/TestCentric/testcentric-engine";
	public string GitHubUserId => "charliepoole";
	public string GitHubUserEmail => "charliepoole@gmail.com";
	public string GitHubPassword { get; }
	public string GitHubAccessToken { get; }

	public bool HasArgument(string altNames)
	{
		foreach (string name in altNames.Split('|'))
			if (SetupContext.HasArgument(name))
				return true;

		return false;
	}

	public T GetArgument<T>(string altNames, T defaultValue)
	{
		foreach (string name in altNames.Split('|'))
			if (SetupContext.HasArgument(name))
				return SetupContext.Argument(name, defaultValue);

		return defaultValue;
	}

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
		}

		if (TasksToExecute.Contains("CreateDraftRelease") && (IsReleaseBranch || IsProductionRelease))
		{
			if (string.IsNullOrEmpty(GitHubAccessToken))
				validationErrors.Add("GitHub Access Token was not set.");		
		}

		if (TasksToExecute.Contains("DeployWebsite"))
        {
			// We use a password rather than an access token for the website
			if (string.IsNullOrEmpty(GitHubPassword))
				validationErrors.Add("GitHub password was not set");
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
		Console.WriteLine("Project:        " + ProjectDirectory);
		Console.WriteLine("Output:         " + OutputDirectory);
		Console.WriteLine("Source:         " + SourceDirectory);
		Console.WriteLine("NuGet:          " + NuGetDirectory);
		Console.WriteLine("Package:        " + PackageDirectory);
		Console.WriteLine("EngineTest:     " + EngineTestDirectory);
		Console.WriteLine("EngineCoreTest: " + EngineCoreTestDirectory);
		Console.WriteLine("EngineApiTest:  " + EngineApiTestDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Build With:      " + (UsingXBuild ? "XBuild" : "MSBuild"));
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", EngineRuntimes));
		Console.WriteLine("Core Runtimes:   " + string.Join(", ", EngineCoreRuntimes));
		Console.WriteLine("Agent Runtimes:  " + string.Join(", ", AgentRuntimes));

		Console.WriteLine("\nPACKAGING");
		Console.WriteLine("MyGetPushUrl:              " + MyGetPushUrl);
		Console.WriteLine("NuGetPushUrl:              " + NuGetPushUrl);
		Console.WriteLine("MyGetApiKey:               " + (!string.IsNullOrEmpty(MyGetApiKey) ? "AVAILABLE" : "NOT AVAILABLE"));
		Console.WriteLine("NuGetApiKey:               " + (!string.IsNullOrEmpty(NuGetApiKey) ? "AVAILABLE" : "NOT AVAILABLE"));

		Console.WriteLine("\nPUBLISHING");
		Console.WriteLine("ShouldPublishToMyGet:      " + ShouldPublishToMyGet);
		Console.WriteLine("ShouldPublishToNuGet:      " + ShouldPublishToNuGet);

		Console.WriteLine("\nRELEASING");
		Console.WriteLine("BranchName:                   " + BranchName);
		Console.WriteLine("IsReleaseBranch:              " + IsReleaseBranch);
		Console.WriteLine("IsProductionRelease:          " + IsProductionRelease);
	}

    private string GetApiKey(string name, string fallback=null)
    {
        var apikey = SetupContext.EnvironmentVariable(name);

        if (string.IsNullOrEmpty(apikey) && fallback != null)
            apikey = SetupContext.EnvironmentVariable(fallback);

        return apikey;
    }
}
