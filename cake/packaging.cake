//////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST EACH PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EnginePackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does<BuildSettings>(settings =>
	{
		settings.EngineApiPackage.BuildVerifyAndTest();
	});
