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
	.Does<BuildSettings>((settings) =>
	{
        if (!settings.ShouldPublishToMyGet)
            Information("Nothing to publish to MyGet from this run.");
		else if (settings.NoPush)
			Information("NoPush option suppressing publication to MyGet");
        else
            try
			{
				PushNuGetPackage(settings.EnginePackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
				PushNuGetPackage(settings.EngineCorePackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
				PushNuGetPackage(settings.EngineApiPackage.PackageFilePath, settings.MyGetApiKey, settings.MyGetPushUrl);
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
	.Does<BuildSettings>((settings) =>
	{
		if (!settings.ShouldPublishToNuGet)
			Information("Nothing to publish to NuGet from this run.");
		else if (settings.NoPush)
			Information("NoPush option suppressing publication to NuGet");
		else
			try
			{
				PushNuGetPackage(settings.EnginePackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
				PushNuGetPackage(settings.EngineCorePackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
				PushNuGetPackage(settings.EngineApiPackage.PackageFilePath, settings.NuGetApiKey, settings.NuGetPushUrl);
			}
			catch(Exception)
            {
				hadPublishingErrors = true;
			}
	});
