#load "./versioning.cake"
#load "./packaging.cake"
#load "./package-checks.cake"
#load "./package-tests.cake"
#load "./test-results.cake"
#load "./test-reports.cake"
#load "./check-headers.cake"
#load "./utilities.cake"
#load "./local-targets.cake"

// URLs for uploading packages
private const string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
private const string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";

// Environment Variable names holding API keys
private const string MYGET_API_KEY = "MYGET_API_KEY";
private const string NUGET_API_KEY = "NUGET_API_KEY";
private const string GITHUB_ACCESS_TOKEN = "GITHUB_ACCESS_TOKEN";

// Pre-release labels that we publish
private static readonly string[] LABELS_WE_PUBLISH_ON_MYGET = { "dev", "pre" };
private static readonly string[] LABELS_WE_PUBLISH_ON_NUGET = { "alpha", "beta", "rc" };
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

		Configuration = context.Argument("configuration", DEFAULT_CONFIGURATION);
		ProjectDirectory = context.Environment.WorkingDirectory.FullPath + "/";

		MyGetApiKey = _context.EnvironmentVariable(MYGET_API_KEY);
		NuGetApiKey = _context.EnvironmentVariable(NUGET_API_KEY);
		GitHubAccessToken = _context.EnvironmentVariable(GITHUB_ACCESS_TOKEN);

		UsingXBuild = context.EnvironmentVariable("USE_XBUILD") != null;
		
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

	public string Target { get; }
	public IEnumerable<string> TasksToExecute { get; }

	public ICakeContext Context => _context;

	public string Configuration { get; }

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
	public string NuGetDirectory => ProjectDirectory + "nuget/";
	public string PackageDirectory => ProjectDirectory + "package/";
	public string TestDirectory => PackageDirectory + "test/";
	public string NuGetTestDirectory => TestDirectory + "nuget/";

	public string EnginePackageName => ENGINE_PACKAGE_NAME + "." + PackageVersion + ".nupkg";
	public string EngineCorePackageName => ENGINE_CORE_PACKAGE_NAME + "." + PackageVersion + ".nupkg";
	public string EngineApiPackageName => ENGINE_API_PACKAGE_NAME + "." + PackageVersion + ".nupkg";

	public FilePath EnginePackage => new FilePath(PackageDirectory + EnginePackageName);
	public FilePath EngineCorePackage => new FilePath(PackageDirectory + EngineCorePackageName);
	public FilePath EngineApiPackage => new FilePath(PackageDirectory + EngineApiPackageName);

	public string GitHubReleaseAssets => _context.IsRunningOnWindows()
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

	public string[] SupportedEngineRuntimes => new string[] {"net40"};
	public string[] SupportedCoreRuntimes => new string[] {"net40", "net35", "netcoreapp2.1"};
	public string[] SupportedAgentRuntimes => new string[] { "net20", "net40", "netcoreapp2.1", "netcoreapp3.1", "net5.0" };

	public string ProjectUri => "https://github.com/TestCentric/testcentric-engine";
	public string GitHubUserId => "charliepoole";
	public string GitHubUserEmail => "charliepoole@gmail.com";
	public string GitHubPassword { get; }
	public string GitHubAccessToken { get; }

	private void Validate()
	{
		var validationErrors = new List<string>();

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

			var msg = new StringBuilder("Parameter validation failed! See settings above.\n\nErrors found:\n");
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
		Console.WriteLine("Package:   " + PackageDirectory);
		Console.WriteLine("NuGetTest: " + NuGetTestDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Build With:      " + (UsingXBuild ? "XBuild" : "MSBuild"));
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", SupportedEngineRuntimes));
		Console.WriteLine("Core Runtimes:   " + string.Join(", ", SupportedCoreRuntimes));
		Console.WriteLine("Agent Runtimes:  " + string.Join(", ", SupportedAgentRuntimes));

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
}
