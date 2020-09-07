//////////////////////////////////////////////////////////////////////
// WEBSITE TARGETS
//////////////////////////////////////////////////////////////////////

Task("BuildWebsite")
	.Does<BuildParameters>((parameters) =>
	{
		BuildWebsite(parameters.WebDirectory);
	});

Task("PreviewWebsite")
	.Does<BuildParameters>((parameters) =>
	{
		// NOTE: This builds as well as previewing
		Wyam(new WyamSettings()
		{
			RootPath = parameters.WebDirectory,
			Preview = true,
			PreviewRoot = parameters.WebOutputDirectory,
			PreviewVirtualDirectory = "/testcentric-gui"
		});
	});

Task("DeployWebsite")
	.IsDependentOn("BuildWebsite")
	.Description("Publish the website on GitHub pages")
	.Does<BuildParameters>((parameters) =>
	{
		DeployWebsite(parameters);
	});

Task("UpdateWebsite")
	.Description("Update the website only if this is a production release")
	.Does<BuildParameters>((parameters) =>
	{
		if (parameters.IsProductionRelease)
		{
			DeployWebsite(parameters);
		}
		else
			Information("Skipping website update because this is not a production release");
	});

//////////////////////////////////////////////////////////////////////
// HELPER METHODS
//////////////////////////////////////////////////////////////////////

private void BuildWebsite(string webDirectory)
{
	Wyam(new WyamSettings()
	{
		RootPath = webDirectory
	});
}

private void PreviewWebsite(BuildParameters parameters)
{
	Wyam(new WyamSettings()
	{
		RootPath = parameters.WebDirectory,
		Preview = true,
		PreviewRoot = parameters.WebOutputDirectory,
		PreviewVirtualDirectory = "/testcentric-gui"
	});
}

private void DeployWebsite(BuildParameters parameters)
{
	string deployDir = parameters.WebDeployDirectory;
	string deployBranch = parameters.WebDeployBranch;
	string userId = parameters.GitHubUserId;
	string userEmail = parameters.GitHubUserEmail;
	string userPassword = parameters.GitHubPassword;

	if (FileExists("./CNAME"))
		CopyFile("./CNAME", "output/CNAME");

	if (DirectoryExists(deployDir))
		DeleteDirectory(deployDir, new DeleteDirectorySettings
		{
			Recursive = true,
			Force = true
		});

	Information($"Checking out {deployBranch} branch to {deployDir}");
	GitClone(parameters.ProjectUri, deployDir, new GitCloneSettings()
	{
		Checkout = true,
		BranchName = deployBranch
	});

	Information($"Copying web pages");
	CopyDirectory(parameters.WebOutputDirectory, deployDir);
	GitAddAll(deployDir);

	if (GitHasUncommitedChanges(deployDir))
	{
		Information($"Pushing site to {deployBranch} branch");
		GitCommit(deployDir, userId, userEmail, "Deploy site to GitHub Pages");
		GitPush(deployDir, userId, userPassword, deployBranch);
	}
	else
		Information("Nothing to commit");
}