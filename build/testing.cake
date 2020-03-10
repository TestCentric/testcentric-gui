//////////////////////////////////////////////////////////////////////
// TESTING HELPER METHODS
//////////////////////////////////////////////////////////////////////

const string DEFAULT_RESULT_FILE = "TestResult.xml";

void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

private void RunNUnitLite(string testName, string framework, string directory)
{
	bool isDotNetCore = framework.StartsWith("netcoreapp");
	string ext = isDotNetCore ? ".dll" : ".exe";
	string testPath = directory + testName + ext;

	Information("========================================");
	Information("Running tests under " + framework);
	Information("========================================");

	int rc = isDotNetCore
		? StartProcess("dotnet", testPath)
		: StartProcess(testPath);

	if (rc > 0)
		ErrorDetail.Add($"{testName}: {rc} tests failed running under {framework}");
	else if (rc < 0)
		ErrorDetail.Add($"{testName} returned rc = {rc} running under {framework}");
}

public void RunGuiAndReportResults(string runnerPath, string arguments, string workingDirectory)
{
	if (FileExists(workingDirectory + DEFAULT_RESULT_FILE))
		DeleteFile(workingDirectory + DEFAULT_RESULT_FILE);

	RunGuiUnattended(runnerPath, arguments, workingDirectory);
	ReportTestResult(workingDirectory + DEFAULT_RESULT_FILE);
}

private void RunGuiUnattended(string runnerPath, string arguments, string workingDirectory)
{
	StartProcess(runnerPath, new ProcessSettings()
	{
		Arguments = arguments + " --run --unattended",
		WorkingDirectory = workingDirectory
	});
}

// Examine the result file to make sure a test run passed
private void ReportTestResult(string resultFile)
{
	var doc = new XmlDocument();
	doc.Load(resultFile);

	XmlNode testRun = doc.DocumentElement;
	if (testRun.Name != "test-run")
		throw new Exception("The test-run element was not found.");

	string result = GetAttribute(testRun, "result");
	if (result == null)
		throw new Exception("The test-run element has no result attribute.");

    Console.WriteLine("\nTest Run Summary");
	Console.WriteLine("  Overall Result: " + result);

	var total = GetAttribute(testRun, "total", "0");
	var passed = GetAttribute(testRun, "passed", "0");
	var failed = GetAttribute(testRun, "failed", "0");
	var warnings = GetAttribute(testRun, "warnings", "0");
	var inconclusive = GetAttribute(testRun, "inconclusive", "0");
	var skipped = GetAttribute(testRun, "skipped", "0");

	Console.WriteLine($"  Test Count: {total}, Passed: {passed}, Failed: {failed}, Warnings: {warnings}, Inconclusive: {inconclusive}, Skipped: {skipped}");

	if (failed != "0" || warnings != "0")
	{
		int index = 0;
		Console.WriteLine();
		Console.WriteLine("Errors, Failures and Warnings");

		foreach (XmlNode childResult in testRun.ChildNodes)
			WriteErrorsFailuresAndWarnings(childResult, ref index, 1);
	}

	if (result == "Failed")
		throw new Exception("The test run failed.");
}

public void WriteErrorsFailuresAndWarnings(XmlNode resultNode, ref int index, int level)
{
	string resultState = GetAttribute(resultNode, "result");

	switch (resultNode.Name)
	{
		case "test-case":
			if (resultState == "Failed" || resultState == "Warning")
				WriteResultNode(resultNode, ++index);
			return;

		case "test-suite":
			if (resultState == "Failed" || resultState == "Warning")
			{
				var suiteType = GetAttribute(resultNode, "type");
				if (suiteType == "Theory")
				{
					// Report failure of the entire theory and then go on
					// to list the individual cases that failed
					WriteResultNode(resultNode, ++index);
				}
				else
				{
					// Where did this happen? Default is in the current test.
					var site = GetAttribute(resultNode, "site");

					// Correct a problem in some framework versions, whereby warnings and some failures 
					// are promulgated to the containing suite without setting the FailureSite.
					if (site == null)
					{ 
						if (resultNode.SelectSingleNode("reason/message")?.InnerText == "One or more child tests had warnings" ||
							resultNode.SelectSingleNode("failure/message")?.InnerText == "One or more child tests had errors")
						{
							site = "Child";
						}
						else
						{
							site = "Test";
						}
					}

					// Only report errors in the current test method, setup or teardown
					if (site == "SetUp" || site == "TearDown" || site == "Test")
						WriteResultNode(resultNode, ++index);

					// Do not list individual "failed" tests after a one-time setup failure
					if (site == "SetUp") return;
				}
			}

			foreach (XmlNode childResult in resultNode.ChildNodes)
				WriteErrorsFailuresAndWarnings(childResult, ref index, level + 1);
			break;
	}
}

private void WriteResultNode(XmlNode resultNode, int index)
{
	var EOL_CHARS = new char[] { '\r', '\n' };
	string status = GetAttribute(resultNode, "label") ?? GetAttribute(resultNode, "result");
	string fullname = GetAttribute(resultNode, "fullname");
	string message = (resultNode.SelectSingleNode("failure/message") ?? resultNode.SelectSingleNode("reason/message"))?.InnerText.Trim(EOL_CHARS);
	string stackTrace = resultNode.SelectSingleNode("failure/stack-trace")?.InnerText.Trim(EOL_CHARS);

	Console.WriteLine($"\n{index}) {status} : {fullname}");
	if (message != null)
		Console.WriteLine(message);
	if (stackTrace != null)
		Console.WriteLine(stackTrace);
}

private string GetAttribute(XmlNode node, string name, string defaultValue = null)
{
	return node.Attributes[name]?.Value ?? defaultValue;
}
