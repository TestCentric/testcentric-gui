#tool NuGet.CommandLine&version=6.0.0

const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-adev00049
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

//////////////////////////////////////////////////////////////////////
// INITIALIZE BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
	Context,
	"TestCentric.Engine.Api",
	solutionFile: "testcentric.engine.api.sln",
	githubRepository: "testcentric.engine.api"
);

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGE
//////////////////////////////////////////////////////////////////////

var EngineApiPackage = new NuGetPackage(
	id: "TestCentric.Engine.Api",
	title: "TestCentric Engine Api Assembly",
	description: "This package includes the testcentric.agent.api assembly, containing the interfaces used in creating pluggable agents.",
	basePath: "bin/" + BuildSettings.Configuration,
	packageContent: new PackageContent(
		new FilePath[] { "../../LICENSE.txt", "../../testcentric.png" },
		new DirectoryContent("lib/netstandard2.0").WithFiles(
			"netstandard2.0/testcentric.engine.api.dll", "netstandard2.0/testcentric.engine.api.pdb")),
	checks: new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib/netstandard2.0").WithFiles("testcentric.engine.api.dll")
	});

BuildSettings.Packages.Add(EngineApiPackage);

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package")
	.IsDependentOn("Publish")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
	.Description("Targets to run on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

// We can't use the BuildSettings.Target for this because Setup has
// not yet run and the settings have not been initialized.
RunTarget(Argument("target", Argument("t", "Default")));
