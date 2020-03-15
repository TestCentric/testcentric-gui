#load "./versioning.cake"
#load "./testing.cake"
#load "./packaging.cake"

public class BuildParameters
{
	private ISetupContext _context;
	private BuildSystem _buildSystem;

	public BuildParameters(ISetupContext context)
	{
		_context = context;
		_buildSystem = _context.BuildSystem();

		Configuration = context.Argument("configuration", DEFAULT_CONFIGURATION);
		ProjectDirectory = context.Environment.WorkingDirectory.FullPath + "/";

		Versions = new BuildVersion(context, this);

		bool publishAllowed = IsRunningOnAppVeyor || IsLocalBuild && Versions.HasPublishArgument;
		ShouldPublishToMyGet = publishAllowed && Versions.IsPreRelease && (Versions.PreReleaseLabel == "dev" || Versions.PreReleaseLabel == "rc");
		ShouldPublishToNuGet = ShouldPublishToChocolatey = publishAllowed && !Versions.IsPreRelease;

		MyGetApiKey = _context.EnvironmentVariable("MYGET_API_KEY");
		NuGetApiKey = _context.EnvironmentVariable("NUGET_API_KEY");
		ChocolateyApiKey = _context.EnvironmentVariable("CHOCO_API_KEY");
		
		Publisher = new Publisher(context, this);

		UsingXBuild = context.EnvironmentVariable("USE_XBUILD") != null;

		MSBuildSettings = new MSBuildSettings {
			Verbosity = Verbosity.Minimal,
			ToolVersion = MSBuildToolVersion.Default,//The highest available MSBuild tool version//VS2017
			Configuration = Configuration,
			PlatformTarget = PlatformTarget.MSIL,
			MSBuildPlatform = MSBuildPlatform.Automatic,
			DetailedSummary = true,
		};

		XBuildSettings = new XBuildSettings {
			Verbosity = Verbosity.Minimal,
			ToolVersion = XBuildToolVersion.Default,//The highest available XBuild tool version//NET40
			Configuration = Configuration,
		};

		RestoreSettings = new NuGetRestoreSettings();
		// Older Mono version was not picking up the testcentric source
		// TOOD: Check if this is still needed
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

	public string Configuration { get; }
	public BuildVersion Versions { get; }
	public string PackageVersion => Versions.PackageVersion;
	public string AssemblyVersion => Versions.AssemblyVersion;
	public string AssemblyFileVersion => Versions.AssemblyFileVersion;
	public string AssemblyInformationalVersion => Versions.AssemblyInformationalVersion;

	public bool IsLocalBuild => _buildSystem.IsLocalBuild;
	public bool IsRunningOnUnix => _context.IsRunningOnUnix();
	public bool IsRunningOnWindows => _context.IsRunningOnWindows();

	public bool IsRunningOnAppVeyor => _buildSystem.AppVeyor.IsRunningOnAppVeyor;
	public bool IsPullRequest => _buildSystem.IsPullRequest;

	public string ProjectDirectory { get; }
	public string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public string NuGetDirectory => ProjectDirectory + "nuget/";
	public string ChocoDirectory => ProjectDirectory + "choco/";
	public string PackageDirectory => ProjectDirectory + "package/";
	public string ImageDirectory => PackageDirectory + "image/";
	public string ZipTestDirectory => PackageDirectory + "test/zip/";
	public string NuGetTestDirectory => PackageDirectory + "test/nuget/";
	public string ChocolateyTestDirectory => PackageDirectory + "test/choco/";

	public string ZipPackageName => PACKAGE_NAME + "-" + PackageVersion + ".zip";
	public string NuGetPackageName => NUGET_PACKAGE_NAME + "." + PackageVersion + ".nupkg";
	public string ChocolateyPackageName => PACKAGE_NAME + "." + PackageVersion + ".nupkg";

	public FilePath ZipPackage => new FilePath(PackageDirectory + ZipPackageName);
	public FilePath NuGetPackage => new FilePath(PackageDirectory + NuGetPackageName);
	public FilePath ChocolateyPackage => new FilePath(PackageDirectory + ChocolateyPackageName);

	public Publisher Publisher { get; }

	public string MyGetApiKey { get; }
	public string NuGetApiKey { get; }
	public string ChocolateyApiKey { get; }

	public bool ShouldPublishToMyGet { get; }
	public bool ShouldPublishToNuGet { get; }
	public bool ShouldPublishToChocolatey { get; }

	public bool UsingXBuild { get; }
	public MSBuildSettings MSBuildSettings { get; }
	public XBuildSettings XBuildSettings { get; }
	public NuGetRestoreSettings RestoreSettings { get; }

	public string[] SupportedEngineRuntimes => new string[] {"net40", "netcoreapp2.1"};
	public string[] SupportedCoreRuntimes => IsRunningOnWindows
		? new string[] {"net40", "net35", "netcoreapp2.1", "netcoreapp1.1"}
		: new string[] {"net40", "net35", "netcoreapp2.1"};
	public string[] SupportedAgentRuntimes => new string[] { "net20", "net40" };
}
