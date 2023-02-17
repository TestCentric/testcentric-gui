//////////////////////////////////////////////////////////////////////
// GLOBALLY ACCESSIBLE UTILITY METHODS CALLED BY CAKE TASKS
//////////////////////////////////////////////////////////////////////

public void DeleteObjectDirectories(BuildParameters parameters)
{
    string pattern = parameters.SourceDirectory + "**/obj/";

    foreach (var dir in GetDirectories(pattern))
        DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
}

static void CheckTestErrors(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more tests failed, breaking the build.\r\n"
                              + copyError.Aggregate((x,y) => x + "\r\n" + y));
    }
}

private void RunNUnitLite(string testName, string runtime, string directory)
{
    bool isDotNetCore = runtime.StartsWith("netcoreapp");
    string ext = isDotNetCore ? ".dll" : ".exe";
    string testPath = directory + testName + ext;

    Information("==================================================");
    Information("Running tests under " + runtime);
    Information("==================================================");

    int rc = isDotNetCore
        ? StartProcess("dotnet", testPath)
        : StartProcess(testPath);

    if (rc > 0)
        ErrorDetail.Add($"{testName}: {rc} tests failed running under {runtime}");
    else if (rc < 0)
        ErrorDetail.Add($"{testName} returned rc = {rc} running under {runtime}");
}

private void PushNuGetPackage(FilePath package, string apiKey, string url)
{
	CheckPackageExists(package);
	NuGetPush(package, new NuGetPushSettings() { ApiKey = apiKey, Source = url });
}

private void CheckPackageExists(FilePath package)
{
	if (!FileExists(package))
		throw new InvalidOperationException(
			$"Package not found: {package.GetFilename()}.\nCode may have changed since package was last built.");
}
