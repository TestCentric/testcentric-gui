//////////////////////////////////////////////////////////////////////
// DUMP SETTINGS
//////////////////////////////////////////////////////////////////////

Task("DumpSettings")
	.Does(() => BuildSettings.DumpSettings());

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
		// Optional parameters
		string solutionFile = null,
		string unitTests = null,
		string githubOwner = null,
		string githubRepository = null)
	{
		if (context == null)
			throw new ArgumentNullException(nameof(context));
		if (title == null)
			throw new ArgumentNullException(nameof(title));

		Context = context;
		_buildSystem = Context.BuildSystem();

		Title = title;
		SolutionFile = solutionFile;
		if (solutionFile == null && title != null)
		{
			var sln = title + ".sln";
			if (System.IO.File.Exists(sln))
				SolutionFile = sln;
		}
		UnitTests = unitTests;

		//Target = SetupContext.TargetTask.Name;
		//TasksToExecute = SetupContext.TasksToExecute.Select(t => t.Name);

		BuildVersion = new BuildVersion(context);

		if (context.HasArgument("testLevel"))
			PackageTestLevel = context.Argument<int>("testLevel");
		else if (context.HasArgument("level"))
			PackageTestLevel = context.Argument<int>("level");
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

		GitHubOwner = githubOwner;
		GitHubRepository = githubRepository;

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

        PackageTests.Add(new PackageTest(1, "Net462Test", "Run mock-assembly.dll targeting .NET 4.6.2",
            "engine-tests/net462/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net35Test", "Run mock-assembly.dll targeting .NET 3.5",
            "engine-tests/net35/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore21Test", "Run mock-assembly.dll targeting .NET Core 2.1",
            "engine-tests/netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore31Test", "Run mock-assembly.dll targeting .NET Core 3.1",
            "engine-tests/netcoreapp3.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "NetCore11Test", "Run mock-assembly.dll targeting .NET Core 1.1",
            "engine-tests/netcoreapp1.1/mock-assembly.dll",
            MockAssemblyExpectedResult("NetCore31AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net50Test", "Run mock-assembly.dll targeting .NET 5.0",
            "engine-tests/net5.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net50AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net60Test", "Run mock-assembly.dll targeting .NET 6.0",
            "engine-tests/net6.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net60AgentLauncher")));

        PackageTests.Add(new PackageTest(1, "Net70Test", "Run mock-assembly.dll targeting .NET 7.0",
            "engine-tests/net7.0/mock-assembly.dll",
            MockAssemblyExpectedResult("Net70AgentLauncher")));

        // AspNetCore Tests

        PackageTests.Add(new PackageTest(1, "AspNetCore31Test", "Run test using AspNetCore under .NET Core 3.1",
            "engine-tests/netcoreapp3.1/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "NetCore31AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore50Test", "Run test using AspNetCore under .NET 5.0",
            "engine-tests/net5.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net50AgentLauncher") }
            }));

        PackageTests.Add(new PackageTest(1, "AspNetCore60Test", "Run test using AspNetCore under .NET 6.0",
            "engine-tests/net6.0/aspnetcore-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: AspNetCore test won't run on AppVeyor under .NET 7.0 - we don't yet know why
        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "AspNetCore70Test", "Run test using AspNetCore under .NET 7.0",
                "engine-tests/net7.0/aspnetcore-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("aspnetcore-test.dll", "Net70AgentLauncher") }
                }));

        // Windows Forms Tests

        // TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net50WindowsFormsTest", "Run test using windows forms under .NET 5.0",
                "engine-tests/net5.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net50AgentLauncher") }
                }));

        PackageTests.Add(new PackageTest(1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
            "engine-tests/net6.0-windows/windows-forms-test.dll",
            new ExpectedResult("Passed")
            {
                Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net60AgentLauncher") }
            }));

        // TODO: Windows Forms tests won't run on AppVeyor under .NET 5.0 or 7.0, we don't yet know why
        if (!IsRunningOnAppVeyor)
            PackageTests.Add(new PackageTest(1, "Net70WindowsFormsTest", "Run test using windows forms under .NET 7.0",
                "engine-tests/net7.0-windows/windows-forms-test.dll",
                new ExpectedResult("Passed")
                {
                    Assemblies = new [] { new ExpectedAssemblyResult("windows-forms-test.dll", "Net70AgentLauncher") }
                }));

        // Multiple Assembly Tests

        PackageTests.Add(new PackageTest(1, "Net35PlusNetCore21Test", "Run different builds of mock-assembly.dll together",
            "engine-tests/net35/mock-assembly.dll engine-tests/netcoreapp2.1/mock-assembly.dll",
            MockAssemblyExpectedResult("Net462AgentLauncher", "NetCore31AgentLauncher")));

        // TODO: Use --config option when it's supported by the extension.
        // Current test relies on the fact that the Release config appears
        // first in the project file.
        if (Configuration == "Release")
        {
            PackageTests.Add(new PackageTest(1, "NUnitProjectTest", "Run an NUnit project",
                "TestProject.nunit",
                new ExpectedResult("Failed")
                {
                    Assemblies = new[] {
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net462AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "NetCore31AgentLauncher"),
                                    new ExpectedAssemblyResult("mock-assembly.dll", "Net50AgentLauncher") }
                },
                NUnitProjectLoader));
        }

        // NOTE: Package tests using a pluggable agent must be run after all tests
        // that assume no pluggable agents are installed!

        // TODO: Disabling Net20PluggableAgentTest until the agent is updated
		//PackageTests.Add(new PackageTest(1, "Net20PluggableAgentTest", "Run mock-assembly.dll targeting net35 using Net20PluggableAgent",
        //    "engine-tests/net35/mock-assembly.dll",
        //    new ExpectedResult("Failed")
        //    {
        //        Total = 36,
        //        Passed = 23,
        //        Failed = 5,
        //        Warnings = 1,
        //        Inconclusive = 1,
        //        Skipped = 7,
        //        Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "Net20AgentLauncher") }
        //    },
        //    Net20PluggableAgent));

        PackageTests.Add(new PackageTest(1, "NetCore21PluggableAgentTest", "Run mock-assembly.dll targeting Net Core 2.1 using NetCore21PluggableAgent",
            "engine-tests/netcoreapp2.1/mock-assembly.dll",
            new ExpectedResult("Failed")
            {
                Total = 36,
                Passed = 23,
                Failed = 5,
                Warnings = 1,
                Inconclusive = 1,
                Skipped = 7,
                Assemblies = new[] { new ExpectedAssemblyResult("mock-assembly.dll", "NetCore21AgentLauncher") }
            },
            NetCore21PluggableAgent));

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

        // TODO: Disabling NUnitV2Test until the driver works
		//PackageTests.Add(new PackageTest(1, "NUnitV2Test", "Run tests using the V2 framework driver",
        //	"v2-tests/net35/v2-test-assembly.dll",
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

		// Define packages
		EnginePackage = new NuGetPackageDefinition(
			id: "TestCentric.Engine",
			source: NuGetDirectory + "TestCentric.Engine.nuspec",
			basePath: OutputDirectory,
			executable: "tools/test-bed.exe",
			checks: new PackageCheck[] {
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("tools").WithFiles(
					"testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"testcentric.engine.pdb", "testcentric.engine.core.pdb", "test-bed.exe", "test-bed.addins"),
				HasDirectory("content").WithFile("testcentric.nuget.addins"),
				HasDirectory("tools/agents/net462").WithFiles(
					"testcentric-agent.exe", "testcentric-agent.pdb", "testcentric-agent.exe.config",
					"testcentric-agent-x86.exe", "testcentric-agent-x86.pdb", "testcentric-agent-x86.exe.config",
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
					"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/netcoreapp3.1").WithFiles(
					"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
					"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net5.0").WithFiles(
					"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
					"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net6.0").WithFiles(
					"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
					"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins"),
				HasDirectory("tools/agents/net7.0").WithFiles(
					"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
					"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"Microsoft.Extensions.DependencyModel.dll", "testcentric-agent.nuget.addins")
			},
			tests: PackageTests);

		EngineCorePackage = new NuGetPackageDefinition(
			id: "TestCentric.Engine.Core",
			source: NuGetDirectory + "TestCentric.Engine.Core.nuspec",
			basePath: ProjectDirectory,
			checks:new PackageCheck[] {
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/net20").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
				HasDirectory("lib/net462").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
				HasDirectory("lib/netstandard2.0").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
				HasDirectory("lib/netcoreapp3.1").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll",
					"Microsoft.Extensions.DependencyModel.dll")
			});

		EngineApiPackage = new NuGetPackageDefinition(
			id: "TestCentric.Engine.Api",
			source: NuGetDirectory + "TestCentric.Engine.Api.nuspec",
			basePath: ProjectDirectory,
			checks: new PackageCheck[] {
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb")
			});

		AllPackages = new List<PackageDefinition>(new [] {
			EnginePackage,
			EngineCorePackage,
			EngineApiPackage
		});
	}

	// Cake Context
	public static ICakeContext Context { get; private set; }

	// Targets
	//public static string Target => Context.TargetTask.Name;
	//public static IEnumerable<string> TasksToExecute => Context.TasksToExecute.Select(t => t.Name);
	
	// Arguments
	public static string Configuration => Context.Argument("configuration", DEFAULT_CONFIGURATION);
	public static bool NoPush => Context.HasArgument("nopush");

	// Versioning
	public static BuildVersion BuildVersion { get; private set; }
	public static string BranchName => BuildVersion.BranchName;
	public static bool IsReleaseBranch => BuildVersion.IsReleaseBranch;
	public static string PackageVersion => BuildVersion.PackageVersion;
	public static string AssemblyVersion => BuildVersion.AssemblyVersion;
	public static string AssemblyFileVersion => BuildVersion.AssemblyFileVersion;
	public static string AssemblyInformationalVersion => BuildVersion.AssemblyInformationalVersion;
	public static bool IsDevelopmentRelease => PackageVersion.Contains("-dev");

    // NOTE: Currently, we use the same tests for all packages. There seems to be
    // no reason for the three packages to differ in capability so the only reason
    // to limit tests on some of them would be efficiency... so far not a problem.
	public static List<PackageTest> PackageTests { get; } = new List<PackageTest>();
	public static int PackageTestLevel { get; private set; }

    public static ExtensionSpecifier NUnitV2Driver => new ExtensionSpecifier("NUnit.Extension.NUnitV2Driver", "3.9.0");
    public static ExtensionSpecifier NUnitProjectLoader => new ExtensionSpecifier("NUnit.Extension.NUnitProjectLoader", "3.7.1");
    public static ExtensionSpecifier Net20PluggableAgent => new ExtensionSpecifier("NUnit.Extension.Net20PluggableAgent", "2.0.0");
    public static ExtensionSpecifier NetCore21PluggableAgent => new ExtensionSpecifier("NUnit.Extension.NetCore21PluggableAgent", "2.1.0");
    public static ExtensionSpecifier Net80PluggableAgent => new ExtensionSpecifier("NUnit.Extension.Net80PluggableAgent", "2.1.0");

	// Build System
	public static bool IsLocalBuild => _buildSystem.IsLocalBuild;
	public static bool IsRunningOnUnix => Context.IsRunningOnUnix();
	public static bool IsRunningOnWindows => Context.IsRunningOnWindows();
	public static bool IsRunningOnAppVeyor => _buildSystem.AppVeyor.IsRunningOnAppVeyor;

	// Standard Directory Structure - not changeable by user
	public static string ProjectDirectory => Context.Environment.WorkingDirectory.FullPath + "/";
	public static string SourceDirectory => ProjectDirectory + "src/";
	public static string OutputDirectory => ProjectDirectory + "bin/" + Configuration + "/";
	public static string NuGetDirectory => ProjectDirectory + "nuget/";
	public static string PackageDirectory => ProjectDirectory + "package/";
	public static string PackageTestDirectory => PackageDirectory + "tests/";
	public static string NuGetTestDirectory => PackageTestDirectory + "nuget/";
	public static string PackageResultDirectory => PackageDirectory + "results/";
	public static string NuGetResultDirectory => PackageResultDirectory + "nuget/";

	// Files
	public static string Title { get; private set; }
	public static string SolutionFile { get; private set; }

	//Testing
	public static string UnitTests { get; set; }

	public static PackageDefinition EnginePackage { get; private set; }
	public static PackageDefinition EngineCorePackage { get; private set; }
	public static PackageDefinition EngineApiPackage { get; private set; }
	public static List<PackageDefinition> AllPackages { get; private set; }

	public static string[] GitHubReleaseAssets => Context.IsRunningOnWindows()
		? new [] { EnginePackage.PackageFilePath, EngineCorePackage.PackageFilePath, EngineApiPackage.PackageFilePath }
        : new [] { EnginePackage.PackageFilePath };

	// Publishing - MyGet
	public static string MyGetPushUrl => MYGET_PUSH_URL;
	public static string MyGetApiKey => Context.EnvironmentVariable(MYGET_API_KEY);
	
	// Publishing - NuGet
	public static string NuGetPushUrl => NUGET_PUSH_URL;	
	public static string NuGetApiKey => Context.EnvironmentVariable(NUGET_API_KEY);

	// Publishing - GetHub
	public static string GitHubOwner { get; set; }
	public static string GitHubRepository { get; set; }
	public static string GitHubAccessToken => Context.EnvironmentVariable(GITHUB_ACCESS_TOKEN);

	public static bool IsPreRelease => BuildVersion.IsPreRelease;
	public static bool ShouldPublishToMyGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_MYGET.Contains(BuildVersion.PreReleaseLabel);
	public static bool ShouldPublishToNuGet =>
		!IsPreRelease || LABELS_WE_PUBLISH_ON_NUGET.Contains(BuildVersion.PreReleaseLabel);
	public static bool IsProductionRelease =>
		!IsPreRelease || LABELS_WE_RELEASE_ON_GITHUB.Contains(BuildVersion.PreReleaseLabel);
	
	public static MSBuildSettings MSBuildSettings { get; private set; }
	public static NuGetRestoreSettings RestoreSettings { get; private set; }

	public static string[] EngineRuntimes => new string[] {"net462"};
	public static string[] EngineCoreRuntimes => new string[] {"net462", "net35", "netstandard2.0", "netcoreapp3.1"};

	private static void Validate()
	{
		var validationErrors = new List<string>();
		
		bool validConfig = false;
		foreach (string config in VALID_CONFIGS)
		{
			if (string.Equals(config, Configuration, StringComparison.OrdinalIgnoreCase))
			{
				//Configuration = config;
				validConfig = true;
			}
		}

		if (!validConfig)
			validationErrors.Add($"Invalid configuration: {Configuration}");

		/*if (TasksToExecute.Contains("PublishPackages"))
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
		}*/

		if (validationErrors.Count > 0)
		{
			DumpSettings();

			var msg = new StringBuilder("Parameter validation failed! See settings above.\r\n\nErrors found:\r\n");
			foreach (var error in validationErrors)
				msg.AppendLine("  " + error);

			throw new InvalidOperationException(msg.ToString());
		}
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
		Console.WriteLine("Project:        " + ProjectDirectory);
		Console.WriteLine("Output:         " + OutputDirectory);
		Console.WriteLine("Source:         " + SourceDirectory);
		Console.WriteLine("NuGet:          " + NuGetDirectory);
		Console.WriteLine("Package:        " + PackageDirectory);

		Console.WriteLine("\nBUILD");
		Console.WriteLine("Title:           " + Title);
		Console.WriteLine("SolutionFile:    " + SolutionFile ?? "NULL");
		Console.WriteLine("Configuration:   " + Configuration);
		Console.WriteLine("Engine Runtimes: " + string.Join(", ", EngineRuntimes));
		Console.WriteLine("Core Runtimes:   " + string.Join(", ", EngineCoreRuntimes));
		//Console.WriteLine("Agent Runtimes:  " + string.Join(", ", AgentRuntimes));

		Console.WriteLine("\nPACKAGES");
		foreach (PackageDefinition package in AllPackages)
		{
			Console.WriteLine($"{package.PackageId}");
			Console.WriteLine($"  FileName: {package.PackageFileName}");
			Console.WriteLine($"  FilePath: {package.PackageFilePath}");
		}

		Console.WriteLine("\nPUBLISHING");
		Console.WriteLine("ShouldPublishToMyGet:   " + ShouldPublishToMyGet);
		Console.WriteLine("  MyGetPushUrl:         " + MyGetPushUrl);
		Console.WriteLine("  MyGetApiKey:          " + KeyAvailable(MYGET_API_KEY));
		Console.WriteLine("ShouldPublishToNuGet:   " + ShouldPublishToNuGet);
		Console.WriteLine("  NuGetPushUrl:         " + NuGetPushUrl);
		Console.WriteLine("  NuGetApiKey:          " + KeyAvailable(NUGET_API_KEY));
		Console.WriteLine("NoPush:                 " + NoPush);

		Console.WriteLine("\nRELEASING");
		Console.WriteLine("BranchName:             " + BranchName);
		Console.WriteLine("IsReleaseBranch:        " + IsReleaseBranch);
		Console.WriteLine("IsProductionRelease:    " + IsProductionRelease);
		Console.WriteLine("GitHubAccessToken:      " + KeyAvailable(GITHUB_ACCESS_TOKEN));
		Console.WriteLine("GitHubReleaseAssets:");
		foreach (string asset in GitHubReleaseAssets)
			Console.WriteLine("  " + asset);
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

    static ExpectedResult MockAssemblyExpectedResult(params string[] agentNames)
    {
        int ncopies = agentNames.Length;

        var assemblies = new ExpectedAssemblyResult[ncopies];
        for (int i = 0; i < ncopies; i++)
            assemblies[i] = new ExpectedAssemblyResult("mock-assembly.dll", agentNames[i]);

        return new ExpectedResult("Failed")
        {
            Total = 36 * ncopies,
            Passed = 23 * ncopies,
            Failed = 5 * ncopies,
            Warnings = 1 * ncopies,
            Inconclusive = 1 * ncopies,
            Skipped = 7 * ncopies,
            Assemblies = assemblies
        };
    }
}
