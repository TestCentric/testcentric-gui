// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

public class PackageTestReport
{
	public PackageTest Test;
	public ActualResult Result;
	public List<string> Errors;

	public PackageTestReport(PackageTest test, ActualResult actualResult)
	{
		Test = test;
		Result = actualResult;
		Errors = new List<string>();

		var expectedResult = test.ExpectedResult;

		ReportMissingFiles();

		if (actualResult.OverallResult == null)
			Errors.Add("   The test-run element has no result attribute.");
		else if (expectedResult.OverallResult != actualResult.OverallResult)
			Errors.Add($"   Expected: Overall Result = {expectedResult.OverallResult}\n   But was: {actualResult.OverallResult}");
		CheckCounter("Test Count", expectedResult.Total, actualResult.Total);
		CheckCounter("Passed", expectedResult.Passed, actualResult.Passed);
		CheckCounter("Failed", expectedResult.Failed, actualResult.Failed);
		CheckCounter("Warnings", expectedResult.Warnings, actualResult.Warnings);
		CheckCounter("Inconclusive", expectedResult.Inconclusive, actualResult.Inconclusive);
		CheckCounter("Skipped", expectedResult.Skipped, actualResult.Skipped);

		var expectedAssemblies = expectedResult.Assemblies;
		var actualAssemblies = actualResult.Assemblies;

		for (int i = 0; i < expectedAssemblies.Length && i < actualAssemblies.Length; i++)
        {
			var expected = expectedAssemblies[i];
			var actual = actualAssemblies[i];

			if (expected.Name != actual.Name)
				Errors.Add($"   Expected: {expected.Name}\n    But was: { actual.Name}");
			else if (!actual.Runtime.StartsWith(expected.Runtime))
				Errors.Add($"   Assembly {actual.Name}\n     Expected: {expected.Runtime}\n      But was: {actual.Runtime}");
        }

		for (int i = actualAssemblies.Length; i < expectedAssemblies.Length; i++)
			Errors.Add($"   Assembly {expectedAssemblies[i].Name} was not found");

		for (int i = expectedAssemblies.Length; i < actualAssemblies.Length; i++)
			Errors.Add($"   Found unexpected assembly {actualAssemblies[i].Name}");
	}

	public PackageTestReport(PackageTest test, Exception ex)
	{
		Test = test;
		Result = null;
		Errors = new List<string>();
		Errors.Add($"     {ex.Message}");
	}

	public void Display(int index)
	{
		Console.WriteLine($"\n{index}. {Test.Description}");
		Console.WriteLine($"   Args: {Test.Arguments}\n");

		foreach (var error in Errors)
			Console.WriteLine(error);

		Console.WriteLine(Errors.Count == 0
			? "   SUCCESS: Test Result matches expected result!"
			: "\n   ERROR: Test Result not as expected!");
	}

	// File level errors, like missing or mal-formatted files, need to be highlighted
	// because otherwise it's hard to detect the cause of the problem without debugging.
	// This method finds and reports that type of error.
	private void ReportMissingFiles()
	{
		// Start with all the top-level test suites. Note that files that
		// cannot be found show up as Unknown as do unsupported file types.
		var suites = Result.Xml.SelectNodes(
			"//test-suite[@type='Unknown'] | //test-suite[@type='Project'] | //test-suite[@type='Assembly']");

		// If there is no top-level suite, it generally means the file format could not be interpreted
		if (suites.Count == 0)
			Errors.Add("   No top-level suites! Possible empty command-line or misformed project.");

		foreach (XmlNode suite in suites)
		{
			// Narrow down to the specific failures we want
			string runState = GetAttribute(suite, "runstate");
			string suiteResult = GetAttribute(suite, "result");
			string label = GetAttribute(suite, "label");
			string site = suite.Attributes["site"]?.Value ?? "Test";
			if (runState == "NotRunnable" || suiteResult == "Failed" && site == "Test" && (label == "Invalid" || label == "Error"))
			{
				string message = suite.SelectSingleNode("reason/message")?.InnerText;
				Errors.Add($"   {message}");
			}
		}
	}

	private void CheckCounter(string label, int expected, int actual)
	{
		// If expected value of counter is negative, it means no check is needed
		if (expected >= 0 && expected != actual)
			Errors.Add($"   Expected: {label} = {expected}\n    But was: {actual}");
	}

	private string GetAttribute(XmlNode node, string name)
	{
		return node.Attributes[name]?.Value;
	}
}

public class ResultReporter
{
	private string _packageName;
	private List<PackageTestReport> _reports = new List<PackageTestReport>();

	public ResultReporter(string packageName)
	{
		_packageName = packageName;
	}

	public void AddReport(PackageTestReport report)
	{
		_reports.Add(report);
	}

	public bool ReportResults()
	{
		Console.WriteLine("\n=================================================="); ;
		Console.WriteLine($"Test Results for {_packageName}");
		Console.WriteLine("=================================================="); ;

		Console.WriteLine("\nTest Environment");
		Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
		Console.WriteLine($"  CLR Version: {Environment.Version}\n");

		int index = 0;
		bool hasErrors = false;

		foreach (var report in _reports)
		{
			hasErrors |= report.Errors.Count > 0;
			report.Display(++index);
		}

		return hasErrors;
	}
}
