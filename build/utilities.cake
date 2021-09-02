public void DeleteObjectDirectories(BuildParameters parameters)
{
    string pattern = parameters.SourceDirectory + "**/obj/";

    foreach (var dir in GetDirectories(pattern))
        DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
}