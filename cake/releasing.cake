//////////////////////////////////////////////////////////////////////
// CREATE A DRAFT RELEASE
//////////////////////////////////////////////////////////////////////

Task("CreateDraftRelease")
    .Does<BuildSettings>((settings) =>
    {
        if (settings.IsReleaseBranch)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

            // NOTE: Since this is a release branch, the pre-release label
            // is "pre", which we don't want to use for the draft release.
            // The branch name contains the full information to be used
            // for both the name of the draft release and the milestone,
            // i.e. release-2.0.0, release-2.0.0-beta2, etc.
            string milestone = settings.BranchName.Substring(8);
            string releaseName = $"TestCentric Engine {milestone}";

            Information($"Creating draft release for {releaseName}");

		    if (settings.NoPush)
			    Information("NoPush option suppressed creation of draft release");
			else
				try
				{
					GitReleaseManagerCreate(settings.GitHubAccessToken, GITHUB_OWNER, GITHUB_REPO, new GitReleaseManagerCreateSettings()
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

////////////////////////////////////////////////////////////////////////
//// CREATE A PRODUCTION RELEASE
////////////////////////////////////////////////////////////////////////

Task("CreateProductionRelease")
    .Does<BuildSettings>((settings) =>
    {
        if (settings.IsProductionRelease)
        {
            // Exit if any PackageTests failed
            CheckTestErrors(ref ErrorDetail);

			string tagName = settings.PackageVersion;
            Information($"Publishing release {tagName} to GitHub");

            if (settings.NoPush)
            {
                Information("NoPush option suppressed publishing of assets:");
                foreach (var asset in settings.GitHubReleaseAssets)
                    Information("  " + asset);
            }
			else
			{
				string token = settings.GitHubAccessToken;
				string assets = $"\"{string.Join(',', settings.GitHubReleaseAssets)}\"";

				GitReleaseManagerAddAssets(token, GITHUB_OWNER, GITHUB_REPO, tagName, assets);
				GitReleaseManagerClose(token, GITHUB_OWNER, GITHUB_REPO, tagName);
			}
        }
        else
        {
            Information("Skipping CreateProductionRelease because this is not a production release");
        }
    });
