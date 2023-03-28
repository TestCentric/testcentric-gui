////////////////////////////////////////////////////////////////////
// TARGETS FOR DEVELOPER USE ON LOCAL MACHINE
//////////////////////////////////////////////////////////////////////

using System;

Task("CleanAll")
	.Description("Clean both configs and all obj directories")
	.Does(() =>
	{
		Information("Cleaning all output directories");
		CleanDirectory(BuildSettings.ProjectDirectory + "bin/");

		Information("Deleting object directories");
		DeleteObjectDirectories();
	});

// Download existing draft release for modification or for use in
// updating the CHANGES.md file.
Task("DownloadDraftRelease")
	.Description("Download draft release for local use")
	.Does(() =>
	{
		if (!BuildSettings.IsReleaseBranch)
			throw new Exception("DownloadDraftRelease requires a release branch!");

		string milestone = BuildSettings.BranchName.Substring(8);

		GitReleaseManagerExport(BuildSettings.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, "DraftRelease.md",
			new GitReleaseManagerExportSettings() { TagName = milestone });
	});

