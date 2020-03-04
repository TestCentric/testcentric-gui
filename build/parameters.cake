//////////////////////////////////////////////////////////////////////
// BUILD PARAMETERS
//////////////////////////////////////////////////////////////////////

public class BuildParameters
{
	public BuildParameters(ICakeContext context)
	{
		Configuration = context.Argument("configuration", "Release");
		PackageVersion = GetPackageVersion(context);

		var baseDir = context.Environment.WorkingDirectory.FullPath + "/";
		OutputDirectory = $"{baseDir}bin/{Configuration}/";
		NuGetDirectory = $"{baseDir}nuget/";
		ChocoDirectory = $"{baseDir}choco/";
		PackageDirectory = $"{baseDir}package/";
		ImageDirectory = PackageDirectory + "image/";
		ZipTestDirectory = PackageDirectory + "test/zip/";
		NuGetTestDirectory = PackageDirectory + "test/nuget/";
		ChocolateyTestDirectory = PackageDirectory + "test/choco/";

		ZipPackage = new FilePath($"{PackageDirectory}{PACKAGE_NAME}-{PackageVersion}.zip");
		NuGetPackage = new FilePath($"{PackageDirectory}{NUGET_PACKAGE_NAME}.{PackageVersion}.nupkg");
		ChocolateyPackage = new FilePath($"{PackageDirectory}{PACKAGE_NAME}.{PackageVersion}.nupkg");

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
		SupportedAgentRuntimes = new string[] { "net20" };
	}

	public string Configuration { get; }
	public string PackageVersion { get; }

	public string OutputDirectory { get; }
	public string NuGetDirectory { get; }
	public string ChocoDirectory { get; }
	public string PackageDirectory { get; }
	public string ImageDirectory { get; }
	public string ZipTestDirectory { get; } 
	public string NuGetTestDirectory { get; } 
	public string ChocolateyTestDirectory { get; } 

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

	private string GetPackageVersion(ICakeContext context)
	{
		var packageVersion = context.Argument("packageVersion", "1.3.0");

		// TODO: Make GitVersion work on Linux
		if (context.IsRunningOnWindows())
		{
			var gitVersion = context.GitVersion();

			string branchName = gitVersion.BranchName;
			// We don't currently use this pattern, but check in case we do later.
			if (branchName.StartsWith ("feature/"))
				branchName = branchName.Substring(8);

			// Default based on GitVersion.yml. This gives us a tag of dev
			// for master, ci for features, pr for pull requests and rc
			// for release branches.
			packageVersion = gitVersion.LegacySemVerPadded;

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
		}

		return packageVersion;
	}
}
