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
		if (settings.UsingXBuild)
			XBuild(SOLUTION, settings.XBuildSettings
				.WithProperty("Version", settings.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
		else
			MSBuild(SOLUTION, settings.MSBuildSettings
				.WithProperty("Version", settings.PackageVersion));
				//.WithProperty("NUnitApiVersion", "3.16.2"));
	});
