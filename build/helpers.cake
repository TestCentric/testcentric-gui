//////////////////////////////////////////////////////////////////////
// TESTING
//////////////////////////////////////////////////////////////////////

// Copy all files needed to run tests from one directory to another
private void CopyTestFiles(string fromDir, string toDir)
{
	CopyFiles(fromDir + "*.Tests.*", toDir);
	CopyFiles(fromDir + "nunit.framework.*", toDir);
	CopyFiles(fromDir + "mock-assembly.*", toDir);
	CopyFiles(fromDir + "test-utilities.*", toDir);
	CopyFiles(fromDir + "System.Threading.Tasks.*", toDir);
	CopyFiles(fromDir + "NSubstitute.*", toDir);
	CopyFiles(fromDir + "Castle.Core.*", toDir);
}

// Examine the result file to make sure a test run passed
private void CheckTestResult(string resultFile)
{
	var doc = new XmlDocument();
	doc.Load(resultFile);

	XmlNode testRun = doc.DocumentElement;
	if (testRun.Name != "test-run")
		throw new Exception("The test-run element was not found.");

	string result = testRun.Attributes["result"]?.Value;
	if (result == null)
		throw new Exception("The test-run element has no result attribute.");

	if (result == "Failed")
	{
		string msg = "The test run failed.";
		string failed = testRun.Attributes["failed"]?.Value;
		if (failed != null)
			msg += $" {int.Parse(failed)} tests failed";
		throw new Exception(msg);
	}
}

void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more unit tests failed, breaking the build.\n"
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
		ErrorDetail.Add(string.Format($"{testName}: {rc} tests failed running under {framework}"));
	else if (rc < 0)
		ErrorDetail.Add(string.Format($"{testName} returned rc = {rc} running under {framework}"));
}

//////////////////////////////////////////////////////////////////////
// PACKAGING
//////////////////////////////////////////////////////////////////////

private void CheckNuGetContent(string nugetDir)
{
	if (!DirectoryExists(nugetDir))
		throw new Exception($"Directory {nugetDir} not found!");

	if (!FileExists(nugetDir + "testcentric.png"))
		throw new Exception($"File 'testcentric.png' not found in the package");
		
	string addinsFile = nugetDir + "tools/testcentric-gui.addins";
	if (!FileExists(addinsFile))
		throw new Exception($"File {addinsFile} not found in the package.");
}

	string MYGET_API_KEY = EnvironmentVariable("MYGET_API_KEY");
	string MYGET_PUSH_URL = "https://www.myget.org/F/testcentric/api/v2";
	string NUGET_API_KEY = EnvironmentVariable("NUGET_API_KEY");
	string NUGET_PUSH_URL = "https://api.nuget.org/v3/index.json";
	string CHOCO_API_KEY = EnvironmentVariable("CHOCO_API_KEY");
	string CHOCO_PUSH_URL = "https://push.chocolatey.org/";

	private void PublishToMyGet(FilePath packageName)
	{
		EnsurePackageExists(packageName);

		Information($"Publishing {packageName} to myget.org.");
		NuGetPush(packageName, new NuGetPushSettings() { ApiKey=MYGET_API_KEY, Source=MYGET_PUSH_URL });
	}

	private void PublishToNuGet(FilePath packageName)
	{
		EnsurePackageExists(packageName);

		Information($"Publishing {packageName} to nuget.org.");
		NuGetPush(packageName, new NuGetPushSettings() { ApiKey=NUGET_API_KEY, Source=NUGET_PUSH_URL });
	}

	private void PublishToChocolatey(FilePath packageName)
	{
		EnsurePackageExists(packageName);
		EnsureKeyIsSet(CHOCO_API_KEY);

		Information($"Publishing {packageName} to chocolatey.");
		ChocolateyPush(packageName, new ChocolateyPushSettings() { ApiKey=CHOCO_API_KEY, Source=CHOCO_PUSH_URL });
	}

	private void EnsurePackageExists(FilePath path)
	{
		if (!FileExists(path))
		{
			var packageName = path.GetFilename();
			throw new InvalidOperationException(
			  $"Package not found: {packageName}.\nCode may have changed since package was last built.");
		}
	}

	private void EnsureKeyIsSet(string apiKey)
	{
		if (string.IsNullOrEmpty(apiKey))
			throw new InvalidOperationException("The Api Key has not been set.");
	}
