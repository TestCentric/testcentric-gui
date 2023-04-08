//////////////////////////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////////////////////////

static var ErrorDetail = new List<string>();

Task("CheckTestErrors")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckTestErrors(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TESTS OF THE GUI
//////////////////////////////////////////////////////////////////////

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var guiTests = GetFiles(BuildSettings.OutputDirectory + GUI_TESTS);
		var args = new StringBuilder();
		foreach (var test in guiTests)
			args.Append($"\"{test}\" ");

		var guiTester = new GuiTester(Context);
		Information ($"Running {BuildSettings.OutputDirectory + GUI_RUNNER} with arguments {args}");
		guiTester.RunGuiUnattended(BuildSettings.OutputDirectory + GUI_RUNNER, args.ToString());
		var result = new ActualResult(BuildSettings.OutputDirectory + "TestResult.xml");

		new ConsoleReporter(result).Display();

		if (result.OverallResult == "Failed")
			throw new System.Exception("There were test failures or errors. See listing.");
	});

