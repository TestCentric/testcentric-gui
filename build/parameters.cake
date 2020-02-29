//////////////////////////////////////////////////////////////////////
// BUILD PARAMETERS
//////////////////////////////////////////////////////////////////////

public class BuildParameters
{
	public BuildParameters(ICakeContext context)
	{
		Target = context.Argument("target", "Default");
		Configuration = context.Argument("configuration", "Release");
		PackageVersion = context.Argument("packageVersion", "1.3.0");

		var baseDir = context.Environment.WorkingDirectory.FullPath + "/";
		OutputDirectory = $"{baseDir}bin/{Configuration}/";
		NuGetDirectory = $"{baseDir}nuget/";
		ChocoDirectory = $"{baseDir}choco/";
		PackageDirectory = $"{baseDir}package/";
		ImageDirectory = PackageDirectory + "image/";
		ZipTestDirectory = PackageDirectory + "test/zip/";
		NuGetTestDirectory = PackageDirectory + "test/nuget/";
		ChocolateyTestDirectory = PackageDirectory + "test/choco/";

		//ZipPackage = $"{PackageDirectory}{PACKAGE_NAME}-{PackageVersion}.zip";
		//NuGetPackage = $"{PackageDirectory}{NUGET_PACKAGE_NAME}.{PackageVersion}.nupkg";
		//ChocolateyPackage = $"{PackageDirectory}{PACKAGE_NAME}.{PackageVersion}.nupkg";

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

	public string Target { get; }
	public string Configuration { get; }
	public string PackageVersion { get; set; }

	public string OutputDirectory { get; }
	public string NuGetDirectory { get; }
	public string ChocoDirectory { get; }
	public string PackageDirectory { get; }
	public string ImageDirectory { get; }
	public string ZipTestDirectory { get; } 
	public string NuGetTestDirectory { get; } 
	public string ChocolateyTestDirectory { get; } 

	// Values need to track changing PackageVersion
	public string ZipPackage => $"{PackageDirectory}{PACKAGE_NAME}-{PackageVersion}.zip";
	public string NuGetPackage => $"{PackageDirectory}{NUGET_PACKAGE_NAME}.{PackageVersion}.nupkg";
	public string ChocolateyPackage => $"{PackageDirectory}{PACKAGE_NAME}.{PackageVersion}.nupkg";

	public bool UsingXBuild { get; }
	public MSBuildSettings MSBuildSettings { get; }
	public XBuildSettings XBuildSettings { get; }
	public NuGetRestoreSettings RestoreSettings { get; }

	public string[] SupportedEngineRuntimes { get; }
	public string[] SupportedCoreRuntimes { get; }
	public string[] SupportedAgentRuntimes { get; }

}

//////////////////////////////////////////////////////////////////////
// BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

