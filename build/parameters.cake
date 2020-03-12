//////////////////////////////////////////////////////////////////////
// BUILD PARAMETERS
//////////////////////////////////////////////////////////////////////

public class BuildParameters
{
	public BuildParameters(ICakeContext context)
	{
		Configuration = context.Argument("configuration", DEFAULT_CONFIGURATION);
		PackageVersion = context.Argument("packageVersion", DEFAULT_VERSION);
		bool versionProvided = context.HasArgument("packageVersion");

		var dash = PackageVersion.IndexOf('-');
		var version = dash > 0
			? PackageVersion.Substring(0, dash)
			: PackageVersion;
		
		AssemblyVersion = version + ".0";
		AssemblyFileVersion =  version;
		AssemblyInformationalVersion = PackageVersion;

		// TODO: Make GitVersion work on Linux
		if (!versionProvided && context.IsRunningOnWindows())
			UpdateVersionInfo(context.GitVersion());

		var baseDir = context.Environment.WorkingDirectory.FullPath + "/";
		OutputDirectory = $"{baseDir}bin/{Configuration}/";
		NuGetDirectory = $"{baseDir}nuget/";
		ChocoDirectory = $"{baseDir}choco/";
		PackageDirectory = $"{baseDir}package/";
		ImageDirectory = PackageDirectory + "image/";
		ZipTestDirectory = PackageDirectory + "test/zip/";
		NuGetTestDirectory = PackageDirectory + "test/nuget/";
		ChocolateyTestDirectory = PackageDirectory + "test/choco/";

		ZipPackageName = $"{PACKAGE_NAME}-{PackageVersion}.zip";
		NuGetPackageName = $"{NUGET_PACKAGE_NAME}.{PackageVersion}.nupkg";
		ChocolateyPackageName = $"{PACKAGE_NAME}.{PackageVersion}.nupkg";

		ZipPackage = new FilePath(PackageDirectory + ZipPackageName);
		NuGetPackage = new FilePath(PackageDirectory + NuGetPackageName);
		ChocolateyPackage = new FilePath(PackageDirectory + ChocolateyPackageName);

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
		if (UsingXBuild)
			RestoreSettings.Source = new string [] {
				"https://www.myget.org/F/testcentric/api/v2/",
				"https://www.myget.org/F/testcentric/api/v3/index.json",
				"https://www.nuget.org/api/v2/",
				"https://api.nuget.org/v3/index.json",
				"https://www.myget.org/F/nunit/api/v2/",
				"https://www.myget.org/F/nunit/api/v3/index.json"
			};

		SupportedEngineRuntimes = new string[] {"net40", "netcoreapp2.1"};
		SupportedCoreRuntimes = context.IsRunningOnWindows()
			? new string[] {"net40", "net35", "netcoreapp2.1", "netcoreapp1.1"}
			: new string[] {"net40", "net35", "netcoreapp2.1"};
		SupportedAgentRuntimes = new string[] { "net20", "net40" };
	}

	public string Configuration { get; }
	public string PackageVersion { get; private set; }
	public string AssemblyVersion { get; private set; }
	public string AssemblyFileVersion { get; private set; }
	public string AssemblyInformationalVersion { get; private set; }

	public string OutputDirectory { get; }
	public string NuGetDirectory { get; }
	public string ChocoDirectory { get; }
	public string PackageDirectory { get; }
	public string ImageDirectory { get; }
	public string ZipTestDirectory { get; } 
	public string NuGetTestDirectory { get; } 
	public string ChocolateyTestDirectory { get; } 

	public string ZipPackageName { get; }
	public string NuGetPackageName { get; }
	public string ChocolateyPackageName { get; }

	public FilePath ZipPackage { get; }
	public FilePath NuGetPackage { get; }
	public FilePath ChocolateyPackage { get; }

	public bool UsingXBuild { get; }
	public MSBuildSettings MSBuildSettings { get; }
	public XBuildSettings XBuildSettings { get; }
	public NuGetRestoreSettings RestoreSettings { get; }

	public string[] SupportedEngineRuntimes { get; }
	public string[] SupportedCoreRuntimes { get; }
	public string[] SupportedAgentRuntimes { get; }

	private void UpdateVersionInfo(GitVersion gitVersion)
	{
		string branchName = gitVersion.BranchName;
		// We don't currently use this pattern, but check in case we do later.
		if (branchName.StartsWith ("feature/"))
			branchName = branchName.Substring(8);

		// Default based on GitVersion.yml. This gives us a tag of dev
		// for master, ci for features, pr for pull requests and rc
		// for release branches.
		var packageVersion = gitVersion.LegacySemVerPadded;

		// Full release versions and PRs need no further handling
		int dash = packageVersion.IndexOf('-');
		bool isPreRelease = dash > 0;

		string label = gitVersion.PreReleaseLabel;
		bool isPR = label == "pr"; // Set in our GitVersion.yml

		if (isPreRelease && !isPR)
		{
			// This handles non-standard branch names.
			if (label == branchName)
				label = "ci";

			string suffix = "-" + label + gitVersion.CommitsSinceVersionSourcePadded;

			if (label == "ci")
			{
				branchName = Regex.Replace(branchName, "[^0-9A-Za-z-]+", "-");
				suffix += "-" + branchName;
			}

			// Nuget limits "special version part" to 20 chars. Add one for the hyphen.
			if (suffix.Length > 21)
				suffix = suffix.Substring(0, 21);

			packageVersion = gitVersion.MajorMinorPatch + suffix;
		}

		PackageVersion = packageVersion;
		AssemblyVersion = gitVersion.AssemblySemVer;
		AssemblyFileVersion = gitVersion.MajorMinorPatch;
		AssemblyInformationalVersion = packageVersion;
	}
}
