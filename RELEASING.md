# How to Create a new TestCentric GUI Release

This page describes how we create a release, starting at the
point where all issues, which should be included in the release,
have been closed.

## Milestone

There must be a milestone named for the release, e.g.: 1.5.0.
The name of the milestone is required to be in the form of the
version of the release, with three components. All issues to
be included in the release must be part of the milestone. All
issues must be closed - any remaining open issues should be
transferred to the next milestone before begining the release
process.

## Release Branch Creation

1. Create a new local branch named "release-{milestone}",
   like `release-1.5.0`.

2. Make any changes needed for the release, add and commit them.
   Note that there may not be any changes at this point.

3. Push the new branch, for example: `push -u origin release-1.5.0`

4. Use the browser to view the draft release that has been created
   on GitHub. All closed issues should appear and the text should
   be correct. No assets have been added at this point. If necessary,
   repeat steps 2 through 4 on the release branch until everything
   is correct.

5. At a final step before merging the release branch into main, you
   may add additional text, such as a leading summary of the release,
   by editing it in the GUI. You should do this last because the text
   will be lost if you again build the release branch before merging.

## Production Release

6. Create a PR for the release branch and wait for the CI build to
   complete. Merge the PR into main. This will trigger a build on
   main and produce a `dev` release on MyGet. Main now has all the
   changes from the release branch, which may be deleted at this time.

7. Go to the draft release on GitHub and click Publish. This will
   tag the release on main and trigger a final production build.

8. The production build uploads all packages to NuGet and Chocolatey
   and adds them as assets on the GitHub release. The milestone is
   closed and all the issues have comments added indicating the
   release in which they were resolved.

9. The website is automatically rebuilt and any changes made as part
   of the release process are deployed.

## Limitations

1. This process does not yet support milestones with a pre-release
   suffix, like 1.5.0-beta2 This will be a future enhancement.

2. The CHANGES file and the Release Notes on the website must be updated
   manually. Ideally, this should be done on the release branch before
   merging it into main. Automatic update is planned as an enhancement.
