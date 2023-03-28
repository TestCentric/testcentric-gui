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
	.Does(() =>
	{
        if (!BuildSettings.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
		else if (BuildSettings.NoPush)
			Information("NoPush option suppressing publication to MyGet");
        else
            try
			{
				PushNuGetPackage(BuildSettings.EnginePackage.PackageFilePath, BuildSettings.MyGetApiKey, BuildSettings.MyGetPushUrl);
				PushNuGetPackage(BuildSettings.EngineCorePackage.PackageFilePath, BuildSettings.MyGetApiKey, BuildSettings.MyGetPushUrl);
				PushNuGetPackage(BuildSettings.EngineApiPackage.PackageFilePath, BuildSettings.MyGetApiKey, BuildSettings.MyGetPushUrl);
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
	.Does(() =>
	{
		if (!BuildSettings.ShouldPublishToNuGet)
			Information("Nothing to publish to NuGet from this run.");
		else if (BuildSettings.NoPush)
			Information("NoPush option suppressing publication to NuGet");
		else
			try
			{
				PushNuGetPackage(BuildSettings.EnginePackage.PackageFilePath, BuildSettings.NuGetApiKey, BuildSettings.NuGetPushUrl);
				PushNuGetPackage(BuildSettings.EngineCorePackage.PackageFilePath, BuildSettings.NuGetApiKey, BuildSettings.NuGetPushUrl);
				PushNuGetPackage(BuildSettings.EngineApiPackage.PackageFilePath, BuildSettings.NuGetApiKey, BuildSettings.NuGetPushUrl);
			}
			catch(Exception)
            {
				hadPublishingErrors = true;
			}
	});
