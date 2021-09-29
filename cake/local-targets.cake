////////////////////////////////////////////////////////////////////
// TARGETS FOR DEVELOPER USE ON LOCAL MACHINE
//////////////////////////////////////////////////////////////////////

using System;

// Dependent task for all local targets
Task("MustBeLocalBuild")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.IsLocalBuild)
			throw new Exception($"{parameters.Target} may only be run locally!");
	});

Task("CleanAll")
	.Description("Clean both configs and all obj directories")
	.IsDependentOn("MustBeLocalBuild")
	.Does<BuildParameters>((parameters) =>
	{
		Information("Cleaning all output directories");
		CleanDirectory(parameters.ProjectDirectory + "bin/");

		Information("Deleting object directories");
		DeleteObjectDirectories(parameters);
	});

// Download existing draft release for modification or for use in
// updating the CHANGES.md file.
Task("DownloadDraftRelease")
	.Description("Download draft release for local use")
	.IsDependentOn("MustBeLocalBuild")
	.Does<BuildParameters>((parameters) =>
	{
		if (!parameters.IsReleaseBranch)
			throw new Exception("DownloadDraftRelease requires a release branch!");

		string milestone = parameters.BranchName.Substring(8);

		GitReleaseManagerExport(parameters.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, "DraftRelease.md",
			new GitReleaseManagerExportSettings() { TagName = milestone });
	});

