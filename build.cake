#tool NuGet.CommandLine&version=6.0.0
#tool nuget:?package=GitVersion.CommandLine&version=5.6.3
#tool nuget:?package=GitReleaseManager&version=0.12.1

const string SOLUTION = "testcentric-engine.sln";
const string GITHUB_OWNER = "testcentric";
const string GITHUB_REPO = "testcentric-engine";
const string DEFAULT_VERSION = "2.0.0";
const string DEFAULT_CONFIGURATION = "Release";
static readonly string[] VALID_CONFIGS = { "Release", "Debug" };

const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

static readonly string TEST_BED_EXE = "test-bed.exe";

// Load scripts after defining constants
#load "./cake/parameters.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//
// Arguments taking a value may use  `=` or space to separate the name
// from the value. Examples of each are shown here.
//
// --target=TARGET
// -t Target
//
//    The name of the task to be run, e.g. Test. Defaults to Build.
//
// --configuration=CONFIG
// -c CONFIG
//
//     The name of the configuration to build, test and/or package, e.g. Debug.
//     Defaults to Release.
//
// --packageVersion=VERSION
// --package=VERSION
//     Specifies the full package version, including any pre-release
//     suffix. This version is used directly instead of the default
//     version from the script or that calculated by GitVersion.
//     Note that all other versions (AssemblyVersion, etc.) are
//     derived from the package version.
//
//     NOTE: We can't use "version" since that's an argument to Cake itself.
//
// --testLevel=LEVEL
// --level=LEVEL
//     Specifies the level of package testing, which is normally set
//     automatically for different types of builds like CI, PR, etc.
//     Used by developers to test packages locally without creating
//     a PR or publishing the package. Defined levels are
//       1 = Normal CI tests run every time you build a package
//       2 = Adds more tests for PRs and Dev builds uploaded to MyGet
//       3 = Adds even more tests prior to publishing a release
//
// --nopush
//     Display a message rather than actually pushing when a publish
//     step is run. Used for testing the build script.
//
//////////////////////////////////////////////////////////////////////

using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN
//////////////////////////////////////////////////////////////////////

Setup<BuildParameters>((context) =>
{
	var parameters = BuildParameters.CreateInstance(context);

	if (BuildSystem.IsRunningOnAppVeyor)
			AppVeyor.UpdateBuildVersion(parameters.PackageVersion + "-" + AppVeyor.Environment.Build.Number);

    Information("Building {0} version {1} of TestCentric Engine.", parameters.Configuration, parameters.PackageVersion);

	return parameters;
});

// If we run target Test, we catch errors here in teardown.
// If we run packaging, the CheckTestErrors Task is run instead.
Teardown(context => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// DUMP SETTINGS
//////////////////////////////////////////////////////////////////////

Task("DumpSettings")
	.Does<BuildParameters>((parameters) =>
	{
		parameters.DumpSettings();
	});

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildParameters>((parameters) =>
	{
		Information("Cleaning " + parameters.OutputDirectory);
		CleanDirectory(parameters.OutputDirectory);

        Information("Cleaning Package Directory");
        CleanDirectory(parameters.PackageDirectory);
	});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does<BuildParameters>((parameters) =>
{
	NuGetRestore(SOLUTION, parameters.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("RestorePackages")
	.IsDependentOn("CheckHeaders")
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.UsingXBuild)
			XBuild(SOLUTION, parameters.XBuildSettings
				.WithProperty("Version", parameters.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
		else
			MSBuild(SOLUTION, parameters.MSBuildSettings
				.WithProperty("Version", parameters.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
	});

//////////////////////////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////////////////////////

static var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE
//////////////////////////////////////////////////////////////////////

Task("TestEngine")
	.Description("Tests the TestCentric Engine")
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.EngineRuntimes)
			RunNUnitLite("testcentric.engine.tests", runtime, $"{parameters.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does<BuildParameters>((parameters) =>
	{
		foreach (var runtime in parameters.EngineCoreRuntimes)
		{
			// Only .NET Standard we currently build is 2.0
			var testUnder = runtime == "netstandard2.0" ? "netcoreapp2.1" : runtime;
			RunNUnitLite("testcentric.engine.core.tests", testUnder, $"{parameters.OutputDirectory}engine-tests/{testUnder}/");
		}
	});

//////////////////////////////////////////////////////////////////////
// ENGINE PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		CreateDirectory(parameters.PackageDirectory);

		Information("Creating package " + parameters.EnginePackageName);

        NuGetPack($"{parameters.NuGetDirectory}/{ENGINE_PACKAGE_ID}.nuspec", new NuGetPackSettings()
        {
            Version = parameters.PackageVersion,
            BasePath = parameters.OutputDirectory,
            OutputDirectory = parameters.PackageDirectory,
            NoPackageAnalysis = true
        });
    });

Task("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
        //CleanDirectory(parameters.NuGetTestDirectory);
        //Unzip(parameters.EnginePackage, parameters.NuGetTestDirectory);
		NuGetInstall(ENGINE_PACKAGE_ID,
            new NuGetInstallSettings()
            {
                Source = new [] { parameters.PackageDirectory },
				Version = parameters.PackageVersion,
				ExcludeVersion = true,
                OutputDirectory = parameters.TestDirectory
            });

        Information($"Installed {parameters.EnginePackageName} at {parameters.TestDirectory}");
    });

Task("VerifyEnginePackage")
	.IsDependentOn("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		Check.That(parameters.EngineTestDirectory,
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
				"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric-agent.nuget.addins"),
			HasDirectory("tools/agents/net5.0").WithFiles(
				"testcentric-agent.dll", "testcentric-agent.pdb", "testcentric-agent.dll.config",
				"testcentric.engine.core.dll", "testcentric.engine.core.pdb",
				"nunit.engine.api.dll", "testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric-agent.nuget.addins"));

			Information("Verification was successful!");
	});

Task("TestEnginePackage")
	.IsDependentOn("InstallEnginePackage")
	.Does<BuildParameters>((parameters) =>
	{
		new EnginePackageTester(parameters).RunAllTests();
	});

//////////////////////////////////////////////////////////////////////
// ENGINE CORE PACKAGE
//////////////////////////////////////////////////////////////////////

// NOTE: The testcentric.engine.core assembly and its dependencies are
// included in all the main packages. It is also published separately 
// as a nuget package for use in creating pluggable agents and for any
// other projects, which may want to make use of it.

Task("BuildEngineCorePackage")
	.Does<BuildParameters>((parameters) =>
	{
		NuGetPack($"{parameters.NuGetDirectory}/{ENGINE_CORE_PACKAGE_ID}.nuspec", new NuGetPackSettings()
		{
			Version = parameters.PackageVersion,
			OutputDirectory = parameters.PackageDirectory,
			NoPackageAnalysis = true
		});
	});

Task("VerifyEngineCorePackage")
	.Does<BuildParameters>((parameters) =>
	{
		string dirName = $"{System.Guid.NewGuid()}/";

		try
		{
			Unzip(parameters.EngineCorePackage, dirName);

			Check.That(dirName,
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/net20").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
				HasDirectory("lib/net462").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"),
				HasDirectory("lib/netstandard2.0").WithFiles(
					"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "nunit.engine.api.dll",
					"testcentric.engine.metadata.dll", "testcentric.extensibility.dll"));
		}
		finally
		{
			DeleteDirectory(dirName, new DeleteDirectorySettings()
			{
				Recursive = true
			});
		}

		Information("Verification was successful!");
	});

//////////////////////////////////////////////////////////////////////
// ENGINE API PACKAGE
//////////////////////////////////////////////////////////////////////

Task("BuildEngineApiPackage")
	.Does<BuildParameters>((parameters) =>
	{
		NuGetPack($"{parameters.NuGetDirectory}/{ENGINE_API_PACKAGE_ID}.nuspec", new NuGetPackSettings()
		{
			Version = parameters.PackageVersion,
			OutputDirectory = parameters.PackageDirectory,
			NoPackageAnalysis = true
		});
	});

Task("VerifyEngineApiPackage")
	.Does<BuildParameters>((parameters) =>
	{
		string dirName = $"{System.Guid.NewGuid()}/";

		try
		{
			Unzip(parameters.EngineApiPackage, dirName);

			Check.That(dirName,
				HasFiles("LICENSE.txt", "testcentric.png"),
				HasDirectory("lib/net20").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"),
				HasDirectory("lib/net462").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"),
				HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll", "testcentric.engine.api.pdb"));
		}
		finally
		{
			DeleteDirectory(dirName, new DeleteDirectorySettings()
			{
				Recursive = true
			});
		}

		Information("Verification was successful!");
	});

//////////////////////////////////////////////////////////////////////
// PUBLISH PACKAGES
//////////////////////////////////////////////////////////////////////

static bool hadPublishingErrors = false;

Task("PublishPackages")
	.Description("Publish packages according to the current settings")
	.IsDependentOn("PublishToMyGet")
    .IsDependentOn("PublishToNuGet")
    .Does(() =>
	{
		if (hadPublishingErrors)
			throw new Exception("One of the publishing steps failed.");
	});

// This task may either be run by the PublishPackages task,
// which depends on it, or directly when recovering from errors.
Task("PublishToMyGet")
	.Description("Publish packages to MyGet")
	.Does<BuildParameters>((parameters) =>
	{
        if (!parameters.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
        else
            try
			{
				PushNuGetPackage(parameters.EnginePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.MyGetApiKey, parameters.MyGetPushUrl);
			}
			catch(Exception)
			{
				hadPublishingErrors = true;
			}
	});

// This task may either be run by the PublishPackages task,
// which depends on it, or directly when recovering from errors.
Task("PublishToNuGet")
	.Description("Publish packages to NuGet")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.ShouldPublishToNuGet)
			Information("Nothing to publish to NuGet from this run.");
		else
			try
			{
				PushNuGetPackage(parameters.EnginePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineCorePackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
				PushNuGetPackage(parameters.EngineApiPackage, parameters.NuGetApiKey, parameters.NuGetPushUrl);
			}
			catch(Exception)
            {
				hadPublishingErrors = true;
			}
	});

//////////////////////////////////////////////////////////////////////
// CREATE A DRAFT RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateDraftRelease")
    .Does<BuildParameters>((parameters) =>
    {
        if (parameters.IsReleaseBranch)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

            // NOTE: Since this is a release branch, the pre-release label
            // is "pre", which we don't want to use for the draft release.
            // The branch name contains the full information to be used
            // for both the name of the draft release and the milestone,
            // i.e. release-2.0.0, release-2.0.0-beta2, etc.
            string milestone = parameters.BranchName.Substring(8);
            string releaseName = $"TestCentric Engine {milestone}";

            Information($"Creating draft release for {releaseName}");

            try
            {
                GitReleaseManagerCreate(parameters.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, new GitReleaseManagerCreateSettings()
                {
                    Name = releaseName,
                    Milestone = milestone
                });
            }
            catch
            {
                Error($"Unable to create draft release for {releaseName}.");
                Error($"Check that there is a {milestone} milestone with at least one closed issue.");
                Error("");
                throw;
            }
        }
        else
        {
            Information("Skipping Release creation because this is not a release branch");
        }
    });

////////////////////////////////////////////////////////////////////////
//// CREATE A PRODUCTION RELEASE
////////////////////////////////////////////////////////////////////////

Task("CreateProductionRelease")
    .Does<BuildParameters>((parameters) =>
    {
        if (parameters.IsProductionRelease)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

            string token = parameters.GitHubAccessToken;
            string tagName = parameters.PackageVersion;
            string assets = parameters.GitHubReleaseAssets;

            Information($"Publishing release {tagName} to GitHub");

            GitReleaseManagerAddAssets(token, GITHUB_OWNER, GITHUB_REPO, tagName, assets);
            GitReleaseManagerClose(token, GITHUB_OWNER, GITHUB_REPO, tagName);
        }
        else
        {
            Information("Skipping CreateProductionRelease because this is not a production release");
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
	.Description("Build and package all components")
	.IsDependentOn("Build")
	.IsDependentOn("PackageExistingBuild");

Task("PackageExistingBuild")
	.Description("Package all components using existing build")
	.IsDependentOn("PackageEngine")
	.IsDependentOn("PackageEngineCore")
	.IsDependentOn("PackageEngineApi");

Task("PackageEngine")
	.Description("Package the engine")
	.IsDependentOn("BuildEnginePackage")
	.IsDependentOn("VerifyEnginePackage")
	.IsDependentOn("TestEnginePackage");

Task("PackageEngineCore")
	.Description("Package the engine core separately")
	.IsDependentOn("BuildEngineCorePackage")
	.IsDependentOn("VerifyEngineCorePackage");

Task("PackageEngineApi")
	.Description("Package the engine api separately")
	.IsDependentOn("BuildEngineApiPackage")
	.IsDependentOn("VerifyEngineApiPackage");

Task("Test")
	.Description("Builds and tests engine core and  engine")
	.IsDependentOn("TestEngineCore")
	.IsDependentOn("TestEngine");

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("CheckTestErrors")
	.IsDependentOn("Package")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
	.Description("Targets to run on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Full")
	.Description("Build, Test and Package")
	.IsDependentOn("DumpSettings")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
	.IsDependentOn("CheckTestErrors")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

// We can't use the BuildParameters.Target for this because Setup has
// not yet run and the parameters have not been initialized.
RunTarget(Argument("target", Argument("t", "Default")));
