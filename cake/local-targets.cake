////////////////////////////////////////////////////////////////////
// TARGETS FOR DEVELOPER USE ON LOCAL MACHINE
//////////////////////////////////////////////////////////////////////

using System;

// Dependent task for all local targets
Task("MustBeLocalBuild")
	.Does<BuildSettings>((settings) =>
	{
		if (!settings.IsLocalBuild)
			throw new Exception($"{settings.Target} may only be run locally!");
	});

Task("CleanAll")
	.Description("Clean both configs and all obj directories")
	.IsDependentOn("MustBeLocalBuild")
	.Does<BuildSettings>((settings) =>
	{
		Information("Cleaning all output directories");
		CleanDirectory(settings.ProjectDirectory + "bin/");

		Information("Deleting object directories");
		DeleteObjectDirectories(settings);
	});

// Download existing draft release for modification or for use in
// updating the CHANGES.md file.
Task("DownloadDraftRelease")
	.Description("Download draft release for local use")
	.IsDependentOn("MustBeLocalBuild")
	.Does<BuildSettings>((settings) =>
	{
		if (!settings.IsReleaseBranch)
			throw new Exception("DownloadDraftRelease requires a release branch!");

		string milestone = settings.BranchName.Substring(8);

		GitReleaseManagerExport(settings.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, "DraftRelease.md",
			new GitReleaseManagerExportSettings() { TagName = milestone });
	});

