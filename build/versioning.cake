using System.Text.RegularExpressions;

public class BuildVersion
{
    private BuildParameters _parameters;

    // NOTE: This is complicated because (1) the user may have specified 
    // the package version on the command-line and (2) GitVersion may
    // or may not be available. We'll work on solving (2) by getting
    // GitVersion to run for us on Linux, but (1) will alwas remain.
    //
    // We simplify things a by figuring out the full package version and
    // then parsing it to provide information that is used in the build.
	public BuildVersion(ISetupContext context, BuildParameters parameters)
	{
        _parameters = parameters;

		bool hasVersionArgument = context.HasArgument("packageVersion");
		bool onWindows = context.IsRunningOnWindows();

		// TODO: Get GitVersion to work on Linux
		string packageVersion = hasVersionArgument || !onWindows
			? context.Argument("packageVersion", DEFAULT_VERSION)
			: GetPackageVersion(context.GitVersion());

		int dash = packageVersion.IndexOf('-');
        IsPreRelease = dash > 0;

        string version = packageVersion;
        string suffix = "";
        string label = "";

        if (IsPreRelease)
        {
            version = packageVersion.Substring(0, dash);
            suffix = packageVersion.Substring(dash+1);
            foreach (char c in suffix)
            {
                if (char.IsDigit(c))
                    break;
                label += c;
            } 
        }

        SemVer = new Version(version).ToString(3);
        PreReleaseLabel = label;
        PreReleaseSuffix = suffix;

		PackageVersion = packageVersion;
		AssemblyVersion = SemVer + ".0";
		AssemblyFileVersion =  SemVer;
		AssemblyInformationalVersion = packageVersion;
	}

	public string PackageVersion { get; }
	public string AssemblyVersion { get; }
	public string AssemblyFileVersion { get; }
	public string AssemblyInformationalVersion { get; }

    public string SemVer { get; }
    public bool IsPreRelease { get; }
    public string PreReleaseLabel { get; }
    public string PreReleaseSuffix { get; }

	private static string GetPackageVersion(GitVersion gitVersion)
	{
		string branchName = gitVersion.BranchName;
		// We don't currently use this pattern, but check in case we do later.
		if (branchName.StartsWith ("feature/"))
			branchName = branchName.Substring(8);

		// Default based on GitVersion.yml. This gives us a tag of dev
		// for master, ci for features, pr for pull requests and rc
		// for release branches.
		var packageVersion = gitVersion.LegacySemVerPadded;

		// Full release versions and PRs need no further handling
		int dash = packageVersion.IndexOf('-');
		bool isPreRelease = dash > 0;

		string label = gitVersion.PreReleaseLabel;
		bool isPR = label == "pr"; // Set in our GitVersion.yml

		if (isPreRelease && !isPR)
		{
			// This handles non-standard branch names.
			if (label == branchName)
				label = "ci";

			string suffix = "-" + label + gitVersion.CommitsSinceVersionSourcePadded;

			if (label == "ci")
			{
				branchName = Regex.Replace(branchName, "[^0-9A-Za-z-]+", "-");
				suffix += "-" + branchName;
			}

			// Nuget limits "special version part" to 20 chars. Add one for the hyphen.
			if (suffix.Length > 21)
				suffix = suffix.Substring(0, 21);

			packageVersion = gitVersion.MajorMinorPatch + suffix;
		}

		return packageVersion;
	}

    public void PatchAssemblyInfo(string sourceFile, string assemblyVersion = null)
    {
        ReplaceFileContents(sourceFile, source =>
        {
            source = ReplaceAttributeString(source, "AssemblyVersion", assemblyVersion ?? _parameters.AssemblyVersion);

            source = ReplaceAttributeString(source, "AssemblyFileVersion", _parameters.AssemblyFileVersion);

            source = ReplaceAttributeString(source, "AssemblyInformationalVersion", _parameters.AssemblyInformationalVersion);

            return source;
        });

        string ReplaceAttributeString(string source, string attributeName, string value)
        {
            var matches = Regex.Matches(source, $@"\[assembly: {Regex.Escape(attributeName)}\(""(?<value>[^""]+)""\)\]");
            if (matches.Count != 1) throw new InvalidOperationException($"Expected exactly one line similar to:\r\n[assembly: {attributeName}(\"1.2.3-optional\")]");

            var group = matches[0].Groups["value"];
            return source.Substring(0, group.Index) + value + source.Substring(group.Index + group.Length);
        }

        void ReplaceFileContents(string filePath, Func<string, string> update)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                string source;
                using (var reader = new StreamReader(file, new UTF8Encoding(false), true, 4096, leaveOpen: true))
                    source = reader.ReadToEnd();

                var newSource = update.Invoke(source);
                if (newSource == source) return;

                file.Seek(0, SeekOrigin.Begin);
                using (var writer = new StreamWriter(file, new UTF8Encoding(false), 4096, leaveOpen: true))
                    writer.Write(newSource);
                file.SetLength(file.Position);
            }
        }
    }
}
