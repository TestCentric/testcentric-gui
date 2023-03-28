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
// RESTORE NUGET PACKAGES
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
    .Does(() =>
{
	NuGetRestore(BuildSettings.SolutionFile, BuildSettings.RestoreSettings);
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
		MSBuild(BuildSettings.SolutionFile, BuildSettings.MSBuildSettings
			.WithProperty("Version", BuildSettings.PackageVersion));
	});
