//////////////////////////////////////////////////////////////////////
// BUILD, VERIFY AND TEST EACH PACKAGE
//////////////////////////////////////////////////////////////////////

Task("PackageEngine")
	.Description("Build and Test the Engine Package")
	.Does(() =>
	{
		BuildSettings.EnginePackage.BuildVerifyAndTest();
	});

Task("PackageEngineCore")
	.Description("Build and Test the Engine Core Package")
	.Does(() =>
	{
		BuildSettings.EngineCorePackage.BuildVerifyAndTest();
	});

Task("PackageEngineApi")
	.Description("Build and Test the Engine Api Package")
	.Does(() =>
	{
		BuildSettings.EngineApiPackage.BuildVerifyAndTest();
	});
