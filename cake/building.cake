// This file contains all tasks related to building the project

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildSettings>((settings) =>
	{
		Information("Cleaning " + settings.OutputDirectory);
		CleanDirectory(settings.OutputDirectory);

		Information("Cleaning Package Directory");
		CleanDirectory(settings.PackageDirectory);
	});

//////////////////////////////////////////////////////////////////////
// CLEAN AND DELETE ALL OBJ DIRECTORIES
//////////////////////////////////////////////////////////////////////

Task("CleanAll")
	.Description("Clean both configs and all obj directories")
	.Does<BuildSettings>((settings) =>
	{
		Information("Cleaning all output directories");
		CleanDirectory(settings.ProjectDirectory + "bin/");

		Information("Cleaning Package Directory");
		CleanDirectory(settings.PackageDirectory);

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
    .Does<BuildSettings>((settings) =>
{
    NuGetRestore(SOLUTION, settings.RestoreSettings);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
	.IsDependentOn("CheckHeaders")
    .Does<BuildSettings>((settings) =>
{
    MSBuild(SOLUTION, settings.MSBuildSettings.WithProperty("Version", settings.PackageVersion));

	// The package does not restore correctly. As a temporary
	// fix, we install a local copy and then copy agents and
	// content to the output directory.

	string tempEngineInstall = settings.ProjectDirectory + "tempEngineInstall/";

	CleanDirectory(tempEngineInstall);

	NuGetInstall("TestCentric.Engine", new NuGetInstallSettings()
	{
		Version = REF_ENGINE_VERSION,
		OutputDirectory = tempEngineInstall,
		ExcludeVersion = true
	});

	CopyFileToDirectory(
		tempEngineInstall + "TestCentric.Engine/content/testcentric.nuget.addins",
		settings.OutputDirectory);
	Information("Copied testcentric.nuget.addins");
	CopyDirectory(
		tempEngineInstall + "TestCentric.Engine/tools",
		settings.OutputDirectory);
	Information("Copied engine files");

});
