// This file contains all tasks related to building the project

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
	{
		Information("Cleaning " + BuildSettings.OutputDirectory);
		CleanDirectory(BuildSettings.OutputDirectory);

		Information("Cleaning Package Directory");
		CleanDirectory(BuildSettings.PackageDirectory);
	});

//////////////////////////////////////////////////////////////////////
// CLEAN AND DELETE ALL OBJ DIRECTORIES
//////////////////////////////////////////////////////////////////////

Task("CleanAll")
	.Description("Clean both configs and all obj directories")
	.Does(() =>
	{
		Information("Cleaning all output directories");
		CleanDirectory(BuildSettings.ProjectDirectory + "bin/");

		Information("Cleaning Package Directory");
		CleanDirectory(BuildSettings.PackageDirectory);

		Information("Deleting object directories");
		foreach (var dir in GetDirectories("src/**/obj/"))
			DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
	});

//////////////////////////////////////////////////////////////////////
// DELETE ALL OBJ DIRECTORIES
//////////////////////////////////////////////////////////////////////

Task("DeleteObjectDirectories")
	.Does(() =>
	{
		Information("Deleting object directories");
		foreach (var dir in GetDirectories("src/**/obj/"))
			DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
	});

//////////////////////////////////////////////////////////////////////
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does(() =>
{
    NuGetRestore(SOLUTION, BuildSettings.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
	.IsDependentOn("CheckHeaders")
    .Does(() =>
{
    MSBuild(SOLUTION, BuildSettings.MSBuildSettings.WithProperty("Version", BuildSettings.PackageVersion));

	// The package does not restore correctly. As a temporary
	// fix, we install a local copy and then copy agents and
	// content to the output directory.

	string tempEngineInstall = BuildSettings.ProjectDirectory + "tempEngineInstall/";

	CleanDirectory(tempEngineInstall);

	NuGetInstall("TestCentric.Engine", new NuGetInstallSettings()
	{
		Version = REF_ENGINE_VERSION,
		OutputDirectory = tempEngineInstall,
		ExcludeVersion = true
	});

	CopyFileToDirectory(
		tempEngineInstall + "TestCentric.Engine/content/testcentric.nuget.addins",
		BuildSettings.OutputDirectory);
	Information("Copied testcentric.nuget.addins");
	CopyDirectory(
		tempEngineInstall + "TestCentric.Engine/tools",
		BuildSettings.OutputDirectory);
	Information("Copied engine files");

});
