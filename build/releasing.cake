public class ReleaseManager
{
    const string GRM = "dotnet-gitreleasemanager";
    const string OWNER = "TestCentric";
    const string REPO = "testcentric-gui";
    
    private ISetupContext _context;
    private BuildParameters _parameters;

	private string _token;
    private string _releaseName;
	private string _milestone;
	private string _assets;

    public ReleaseManager(ISetupContext context, BuildParameters parameters)
    {
        _context = context;
        _parameters = parameters;

        _token = _parameters.GitHubAccessToken;
        _releaseName = $"\"TestCentric {_parameters.BuildVersion.SemVer}\"";
        _milestone = _parameters.IsReleaseBuild
            ? GetMilestoneFromBranchName(_parameters.BranchName)
            : string.Empty;
    	_assets = _context.IsRunningOnWindows()
            ? $"\"{_parameters.ZipPackage},{_parameters.NuGetPackage},{_parameters.ChocolateyPackage},{_parameters.MetadataPackage}\""
            : $"\"{_parameters.ZipPackage},{_parameters.NuGetPackage}\"";
    }

    public string ReleaseMilestone => _milestone;

    public void CreateDraftRelease()
    {
        _context.Information($"Creating draft release {_releaseName} from milestone {_milestone}");

		if (_context.StartProcess(GRM, $"create --token {_token} -o {OWNER} -r {REPO} -a {_assets} -m {_milestone} -n {_releaseName}") != 0)
			throw new InvalidOperationException("Failed to create draft release.");
    }

    private string GetMilestoneFromBranchName(string branchName)
    {
        Version versionFromBranch;
        if (!Version.TryParse(branchName.Substring(8), out versionFromBranch))
            throw new InvalidOperationException($"Branch name {branchName} incorporates an invalid version format.");

        if (versionFromBranch.Build < 0)
            throw new InvalidOperationException("Release branch must specify three version components.");

        string milestone = versionFromBranch.Build <= 0
            ? versionFromBranch.ToString(2)
            : versionFromBranch.ToString(3);
        if (_parameters.IsPreRelease)
            milestone += $"-{_parameters.BuildVersion.PreReleaseSuffix}";

        return milestone;
    }
}