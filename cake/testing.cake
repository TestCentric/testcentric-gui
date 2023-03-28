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
	.Does(() =>
	{
		foreach (var runtime in BuildSettings.EngineRuntimes)
			RunNUnitLite("testcentric.engine.tests", runtime, $"{BuildSettings.OutputDirectory}engine-tests/{runtime}/");
	});

//////////////////////////////////////////////////////////////////////
// TESTS OF TESTCENTRIC.ENGINE.CORE
//////////////////////////////////////////////////////////////////////

Task("TestEngineCore")
	.Description("Tests the TestCentric Engine Core")
	.IsDependentOn("Build")
	.Does(() =>
	{
		foreach (var runtime in BuildSettings.EngineCoreRuntimes)
		{
			// Only .NET Standard we currently build is 2.0
			var testUnder = runtime == "netstandard2.0" ? "netcoreapp2.1" : runtime;
			RunNUnitLite("testcentric.engine.core.tests", testUnder, $"{BuildSettings.OutputDirectory}engine-tests/{testUnder}/");
		}
	});
