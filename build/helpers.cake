//////////////////////////////////////////////////////////////////////
// PACKAGING HELPER METHODS
//////////////////////////////////////////////////////////////////////

var RootFiles = new string[]
{
    "LICENSE.txt",
    "NOTICES.txt",
    "CHANGES.txt"
};

var baseFiles = new string[]
{
    "testcentric.exe",
    "testcentric.exe.config",
    "tc-next.exe",
    "tc-next.exe.config",
    "TestCentric.Common.dll",
    "TestCentric.Gui.Components.dll",
    "TestCentric.Gui.Runner.dll",
    "Experimental.Gui.Runner.dll",
    "nunit.uiexception.dll",
    "TestCentric.Gui.Model.dll",
    "testcentric.engine.api.dll",
    "testcentric.engine.metadata.dll",
    "testcentric.engine.core.dll",
    "testcentric.engine.dll",
    "Mono.Cecil.dll"
};

var PdbFiles = new string[]
{
    "testcentric.pdb",
    "tc-next.pdb",
    "TestCentric.Common.pdb",
    "TestCentric.Gui.Components.pdb",
    "TestCentric.Gui.Runner.pdb",
    "Experimental.Gui.Runner.pdb",
    "nunit.uiexception.pdb",
    "TestCentric.Gui.Model.pdb",
    "testcentric.engine.api.pdb",
    "testcentric.engine.metadata.pdb",
    "testcentric.engine.core.pdb",
    "testcentric.engine.pdb",
};

private void CreateImage(BuildParameters parameters)
{
	string imageDir = parameters.ImageDirectory;
	string imageBinDir = imageDir + "bin/";

    CreateDirectory(imageDir);
    CleanDirectory(imageDir);

	CopyFiles(RootFiles, imageDir);

	var copyFiles = new List<string>(baseFiles);
	if (!parameters.UsingXBuild)
		copyFiles.AddRange(PdbFiles);

	CreateDirectory(imageBinDir);

	foreach (string file in copyFiles)
		CopyFileToDirectory(parameters.OutputDirectory + file, imageBinDir);

	CopyDirectory(parameters.OutputDirectory + "Images", imageBinDir + "Images");

	foreach (var runtime in parameters.SupportedAgentRuntimes)
    {
        var targetDir = imageBinDir + "agents/" + Directory(runtime);
        var sourceDir = parameters.OutputDirectory + "agents/" + Directory(runtime);
        CopyDirectory(sourceDir, targetDir);
	}

	// NOTE: Files specific to a particular package are not copied
	// into the image directory but are added separately.
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
