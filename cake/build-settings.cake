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
		ICakeContext context)
	{
		if (context == null)
			throw new ArgumentNullException(nameof(context));

		Context = context;
		_buildSystem = context.BuildSystem();

		Configuration = context.Argument("configuration", context.Argument("c", DEFAULT_CONFIGURATION));
		NoPush = context.HasArgument("nopush");
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

		// Define Package Tests
        //   Level 1 tests are run each time we build the packages
        //   Level 2 tests are run for PRs and when packages will be published
        //   Level 3 tests are run only when publishing a release

		// Tests of single assemblies targeting each runtime we support

		PackageTests.Add(new PackageTest(1, "Net462Test", "Run net462 mock-assembly.dll under .NET 4.6.2",
			"net462/mock-assembly.dll",
			MockAssemblyExpectedResult("Net462AgentLauncher")));

		PackageTests.Add(new PackageTest(1, "Net35Test", "Run net35 mock-assembly.dll under .NET 4.6.2",
		"net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

		PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run .NET Core 2.1 mock-assembly.dll under .NET Core 3.1",
            "netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll under .NET Core 3.1",
            "netcoreapp3.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        //    PackageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
        //        "netcoreapp1.1/mock-assembly.dll",
        //        new ExpectedResult("Failed")
        //        {
        //Total = 41,
        //Passed = 22,
        //Failed = 7,
        //Warnings = 0,
        //Inconclusive = 5,
        //Skipped = 7,
        //Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "netcore-1.1") }
        //        }));

        PackageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll under .NET 5.0",
            "net5.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net50AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll under .NET 6.0",
            "net6.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net60AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll under .NET 7.0",
            "net7.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net70AgentLauncher")));

        // AspNetCore tests

		PackageTests.Add(new PackageTest(1, "AspNetCore31Test", "Run test using AspNetCore under .NET Core 3.1",
            "netcoreapp3.1/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "NetCore31AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
            "net5.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net50AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
            "net6.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
                "net7.0/aspnetcore-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
                }));

		// Windows Forms Tests

		// TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
                "net5.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net50AgentLauncher") }
                }));

        PackageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
            "net6.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
            }));

        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
                "net7.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
                }));

		// TODO: Temporarily suppress test of .NET 2.0 pluggable agent
		// This test installs the .NET 2.0 pluggable agent. All subsequent
		// tests will use that agent for .NET 2.0 through 3.5 tests.
		//PackageTests.Add(new PackageTest(1, "Net20PluggableAgentTest", "Run net35 mock-assembly.dll under .NET 2.0 pluggable agent",
		//    "net35/mock-assembly.dll",
        //    MockAssemblyExpectedResult("Net20AgentLauncher"),
		//    Net20PluggableAgent));

		// This test installs the .NET Core 2.1 pluggable agent. All subsequent
		// tests will use that agent for .NET Core tests up to version 2.1.
		PackageTests.Add(new PackageTest(1, "NetCore21PluggableAgentTest", "Run .NET Core 2.1 mock-assembly.dll under .NET Core 2.1 pluggable agent",
			"netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore21AgentLauncher"),
			NetCore21PluggableAgent));

		// Multiple assembly tests

        PackageTests.Add(new PackageTest(1, "Net462PlusNet35Test", "Run net462 and net35 builds of mock-assembly.dll together",
            "net462/mock-assembly.dll net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "Net462AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net462PlusNet60Test", "Run different builds of mock-assembly.dll together",
            "net462/mock-assembly.dll net6.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "Net60AgentLauncher")));

        // Level 2 tests are run for PRs and when packages will be published

		// TODO: Suppress V2 tests until driver is working
        //PackageTests.Add(new PackageTest(2, "NUnitV2Test", "Run mock-assembly.dll built for NUnit V2",
        //	"v2-tests/mock-assembly.dll",
        //	new ExpectedResult("Failed")
        //	{
        //		Total = 28,
        //		Passed = 18,
        //		Failed = 5,
        //		Warnings = 0,
        //		Inconclusive = 1,
        //		Skipped = 4
        //	},
        //	NUnitV2Driver));

        // TODO: Use --config option when it's supported by the extension.
        // Current test relies on the fact that the Release config appears
        // first in the project file.
        //if (_parameters.Configuration == "Release")
        //{
        //    PackageTests.Add(new PackageTest(2, "NUnitProjectTest", "Run an NUnit project",
        //        "../../GuiTests.nunit",
        //        new ExpectedResult("Passed")
        //        {
        //            Assemblies = new[] {
        //                    new ExpectedAssemblyResult("TestCentric.Gui.Tests.dll", "net-4.5"),
        //                    new ExpectedAssemblyResult("TestCentric.Gui.Model.Tests.dll", "net-4.5") }
        //        },
        //        NUnitProjectLoader));
        //}

		// TODO: Make this work on AppVeyor
		const string NET80_MOCK_ASSEMBLY = "../../../net80-pluggable-agent/bin/Release/tests/net8.0/mock-assembly.dll";
		if (IsLocalBuild && Context.FileExists(OutputDirectory + NET80_MOCK_ASSEMBLY))
			PackageTests.Add(new PackageTest(1, "NetCore80PluggableAgentTest", "Run mock-assembly.dll targeting Net 8.0 using NetCore80PluggableAgent",
				NET80_MOCK_ASSEMBLY,
				new ExpectedResult("Failed")
				{
					Total = 36,
					Passed = 23,
					Failed = 5,
					Warnings = 1,
					Inconclusive = 1,
					Skipped = 7,
					Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "Net80AgentLauncher") }
				},
				Net80PluggableAgent));

		NuGetPackage = new NuGetPackageDefinition(
			id: "TestCentric.GuiRunner",
			source: NuGetDirectory + "TestCentric.GuiRunner.nuspec",
			basePath: OutputDirectory,
			executable: "tools/testcentric.exe",
			checks: new PackageCheck[] {
				HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "testcentric.png"),
				HasDirectory("tools").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.nuget.addins"),
				HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFiles(ENGINE_CORE_FILES).AndFile("testcentric-agent.nuget.addins"),
				HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
				HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
				HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
				HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
				HasDirectory("tools/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
			},
			tests: PackageTests);

		ChocolateyPackage = new ChocolateyPackageDefinition(
			id: "testcentric-gui",
			source: "choco/testcentric-gui.nuspec",
			basePath: OutputDirectory,
			executable: "tools/testcentric.exe",
			checks: new PackageCheck[] {
				HasDirectory("tools").WithFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt", "VERIFICATION.txt", "testcentric.choco.addins").AndFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.choco.addins"),
				HasDirectory("tools/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
				HasDirectory("tools/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
				HasDirectory("tools/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
				HasDirectory("tools/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
				HasDirectory("tools/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES).AndFile("testcentric-agent.choco.addins"),
				HasDirectory("tools/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
				HasDirectory("tools/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
				HasDirectory("tools/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
				HasDirectory("tools/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
				HasDirectory("tools/Images/Tree/Visual%20Studio").WithFiles(TREE_ICONS_PNG)
			},
			tests: PackageTests);

		ZipPackage = new ZipPackageDefinition(
			id: "TestCentric.Gui.Runner",
			source: NuGetDirectory + "TestCentric.Gui.Runner.nuspec",
			basePath: OutputDirectory,
			executable: "bin/testcentric.exe",
			checks: new PackageCheck[] {
				HasFiles("CHANGES.txt", "LICENSE.txt", "NOTICES.txt"),
				HasDirectory("bin").WithFiles(GUI_FILES).AndFiles(ENGINE_FILES).AndFile("testcentric.zip.addins"),
				HasDirectory("bin/agents/net462").WithFiles(NET_FRAMEWORK_AGENT_FILES),
				HasDirectory("bin/agents/netcoreapp3.1").WithFiles(NET_CORE_AGENT_FILES),
				HasDirectory("bin/agents/net5.0").WithFiles(NET_CORE_AGENT_FILES),
				HasDirectory("bin/agents/net6.0").WithFiles(NET_CORE_AGENT_FILES),
				HasDirectory("bin/agents/net7.0").WithFiles(NET_CORE_AGENT_FILES),
				HasDirectory("bin/Images").WithFiles("DebugTests.png", "RunTests.png", "StopRun.png", "GroupBy_16x.png", "SummaryReport.png"),
				HasDirectory("bin/Images/Tree/Circles").WithFiles(TREE_ICONS_JPG),
				HasDirectory("bin/Images/Tree/Classic").WithFiles(TREE_ICONS_JPG),
				HasDirectory("bin/Images/Tree/Default").WithFiles(TREE_ICONS_PNG),
				HasDirectory("bin/Images/Tree/Visual Studio").WithFiles(TREE_ICONS_PNG)
			},
			tests: PackageTests);

		AllPackages = new List<PackageDefinition>(new [] {
			NuGetPackage,
			ChocolateyPackage,
			ZipPackage
		});
	}

	public static ICakeContext Context { get; private set; }

	public static string Target { get; }
	public static IEnumerable<string> TasksToExecute { get; }

	public static string Configuration { get; private set; }
	public static bool NoPush { get; private set; }

	public static BuildVersion BuildVersion { get; private set; }
	public static string PackageVersion => BuildVersion.PackageVersion;
	public static string AssemblyVersion => BuildVersion.AssemblyVersion;
	public static string AssemblyFileVersion => BuildVersion.AssemblyFileVersion;
	public static string AssemblyInformationalVersion => BuildVersion.AssemblyInformationalVersion;

	public static List<PackageTest> PackageTests { get; } = new List<PackageTest>();
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

	public static string ProjectDirectory { get; private set; }
	public static string SourceDirectory => ProjectDirectory + "src/";
	public static string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public static string ZipDirectory => ProjectDirectory + "zip/";
	public static string NuGetDirectory => ProjectDirectory + "nuget/";
	public static string ChocoDirectory => ProjectDirectory + "choco/";
	public static string PackageDirectory => ProjectDirectory + "package/";
	public static string PackageTestDirectory => PackageDirectory + "tests/";
	public static string NuGetTestDirectory => PackageTestDirectory + "nuget/";
	public static string ChocoTestDirectory => PackageTestDirectory + "choco/";
	public static string ZipTestDirectory => PackageTestDirectory + "zip/";
	public static string PackageResultDirectory => PackageDirectory + "results/";
	public static string NuGetResultDirectory => PackageResultDirectory + "nuget/";
	public static string ChocoResultDirectory => PackageResultDirectory + "choco/";
	public static string ZipResultDirectory => PackageResultDirectory + "zip/";
	public static string ZipImageDirectory => PackageDirectory + "zipimage/";

	public static PackageDefinition ZipPackage { get; private set; }
	public static PackageDefinition NuGetPackage { get; private set; }
	public static PackageDefinition ChocolateyPackage { get; private set; }
	public static List<PackageDefinition> AllPackages { get; private set; }

	public static string GitHubReleaseAssets => Context.IsRunningOnWindows()
		? $"\"{ZipPackage},{NuGetPackage},{ChocolateyPackage}\""
        : $"\"{ZipPackage},{NuGetPackage}\"";

	public static string MyGetPushUrl => MYGET_PUSH_URL;
	public static string NuGetPushUrl => NUGET_PUSH_URL;
	public static string ChocolateyPushUrl => CHOCO_PUSH_URL;
	
	public static string MyGetApiKey { get; private set; }
	public static string NuGetApiKey { get; private set; }
	public static string ChocolateyApiKey { get; private set; }
	public static string GitHubAccessToken { get; private set; }

    public static string BranchName => BuildVersion.BranchName;
	public static bool IsReleaseBranch => BuildVersion.IsReleaseBranch;

	public static bool IsPreRelease => BuildVersion.IsPreRelease;
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
		Console.WriteLine("Project:   " + ProjectDirectory);
		Console.WriteLine("Output:    " + OutputDirectory);
		Console.WriteLine("Source:    " + SourceDirectory);
		Console.WriteLine("NuGet:     " + NuGetDirectory);
		Console.WriteLine("Choco:     " + ChocoDirectory);
		Console.WriteLine("Package:   " + PackageDirectory);
		Console.WriteLine("ZipImage:  " + ZipImageDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", SupportedEngineRuntimes));

		Console.WriteLine("\nPACKAGES");
		foreach (PackageDefinition package in AllPackages)
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
