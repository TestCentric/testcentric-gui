//////////////////////////////////////////////////////////////////////
// CREATE A DRAFT RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateDraftRelease")
	.Does(() =>
	{
		if (BuildSettings.IsReleaseBranch)
		{
			// Exit if any PackageTests failed
			CheckTestErrors(ref ErrorDetail);

			// NOTE: Since this is a release branch, the pre-release label
			// is "pre", which we don't want to use for the draft release.
			// The branch name contains the full information to be used
			// for both the name of the draft release and the milestone,
			// i.e. release-2.0.0, release-2.0.0-beta2, etc.
			string milestone = BuildSettings.BranchName.Substring(8);
			string releaseName = $"TestCentric {milestone}";

			Information($"Creating draft release for {releaseName}");

			try
			{
				GitReleaseManagerCreate(BuildSettings.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, new GitReleaseManagerCreateSettings()
				{
					Name = releaseName,
					Milestone = milestone
				});
			}
			catch
            {
				Error($"Unable to create draft release for {releaseName}.");
				Error($"Check that there is a {milestone} milestone with at least one closed issue.");
				Error("");
				throw;
            }
		}
		else
		{
			Information("Skipping Release creation because this is not a release branch");
		}
	});

//////////////////////////////////////////////////////////////////////
// DOWNLOAD THE DRAFT RELEASE
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// CREATE A PRODUCTION RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateProductionRelease")
	.Does(() =>
	{
		if (BuildSettings.IsProductionRelease)
		{
			// Exit if any PackageTests failed
			CheckTestErrors(ref ErrorDetail);

			string token = BuildSettings.GitHubAccessToken;
			string tagName = BuildSettings.PackageVersion;
			string assets = BuildSettings.GitHubReleaseAssets;

			Information($"Publishing release {tagName} to GitHub");

			GitReleaseManagerAddAssets(token, GITHUB_OWNER, GITHUB_REPO, tagName, assets);
			GitReleaseManagerClose(token, GITHUB_OWNER, GITHUB_REPO, tagName);
		}
		else
		{
			Information("Skipping CreateProductionRelease because this is not a production release");
		}
	});
