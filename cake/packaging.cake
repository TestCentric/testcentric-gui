////////////////////////////////////////////////////////////////////
// PACKAGING TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
	.IsDependentOn("Build")
	.IsDependentOn("PackageExistingBuild");

Task("PackageExistingBuild")
    .Does(() =>
    {
        foreach(var package in BuildSettings.Packages)
            package.BuildVerifyAndTest();
    });

/*Task("PackageNuGet")
	.Description("Build and Test the NuGet Package")
	.Does(() =>
	{
		BuildSettings.NuGetPackage.BuildVerifyAndTest();
	});

Task("PackageChocolatey")
	.Description("Build and Test the Chocolatey Package")
	.Does(() =>
	{
		BuildSettings.ChocolateyPackage.BuildVerifyAndTest();
	});

Task("CreateZipImage")
	.Description("Create image used for zip package")
	.Does(() => {
		Information("Creating Zip Image Directory");

		CreateDirectory(BuildSettings.PackageDirectory);
		CreateZipImage();
	});

Task("PackageZip")
	.Description("Build and Test the Zip Package")
	.IsDependentOn("CreateZipImage")
	.Does(() =>
	{
		BuildSettings.ZipPackage.BuildVerifyAndTest();
	});*/

//////////////////////////////////////////////////////////////////////
// PACKAGING METHODS AND CLASSES
//////////////////////////////////////////////////////////////////////

static readonly string[] ENGINE_FILES = {
        "testcentric.engine.dll", "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll"};
static readonly string[] ENGINE_CORE_FILES = {
        "testcentric.engine.core.dll", "nunit.engine.api.dll", "testcentric.engine.metadata.dll" };
static readonly string[] NET_FRAMEWORK_AGENT_FILES = {
        "testcentric-agent.exe", "testcentric-agent.exe.config", "testcentric-agent-x86.exe", "testcentric-agent-x86.exe.config" };
static readonly string[] NET_CORE_AGENT_FILES = {
        "testcentric-agent.dll", "testcentric-agent.dll.config" };
static readonly string[] GUI_FILES = {
        "testcentric.exe", "testcentric.exe.config", "nunit.uiexception.dll",
        "TestCentric.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "Mono.Options.dll" };
static readonly string[] TREE_ICONS_JPG = {
        "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
static readonly string[] TREE_ICONS_PNG = {
        "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

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
    "TestCentric.Gui.Runner.dll",
    "nunit.uiexception.dll",
    "TestCentric.Gui.Model.dll",
    "Mono.Options.dll",
    "nunit.engine.api.dll",
    "testcentric.engine.api.dll",
    "testcentric.engine.metadata.dll",
    "testcentric.extensibility.dll",
    "testcentric.engine.core.dll",
    "testcentric.engine.dll",
};

var PdbFiles = new string[]
{
    "testcentric.pdb",
    "TestCentric.Gui.Runner.pdb",
    "nunit.uiexception.pdb",
    "TestCentric.Gui.Model.pdb",
    "Mono.Options.pdb",
    "testcentric.engine.api.pdb",
    "testcentric.engine.core.pdb",
    "testcentric.engine.pdb",
};

private void CreateZipImage()
{
	string zipImageDir = BuildSettings.ZipImageDirectory;
	string zipImageBinDir = zipImageDir + "bin/";

    CreateDirectory(zipImageDir);
    CleanDirectory(zipImageDir);

	CopyFiles(RootFiles, zipImageDir);

	var copyFiles = new List<string>(baseFiles);

	CreateDirectory(zipImageBinDir);

    foreach (string file in copyFiles)
        CopyFileToDirectory(BuildSettings.OutputDirectory + file, zipImageBinDir);

    CopyFileToDirectory(BuildSettings.ZipDirectory + "testcentric.zip.addins", BuildSettings.ZipImageDirectory + "bin/");

    CopyDirectory(BuildSettings.OutputDirectory + "Images", zipImageBinDir + "Images");

    CopyDirectory(BuildSettings.OutputDirectory + "agents", zipImageBinDir + "agents");

    // NOTE: Files specific to a particular package are not copied
    // into the image directory but are added separately.
}
