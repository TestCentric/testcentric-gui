public void DeleteObjectDirectories(BuildParameters parameters)
{
    string pattern = parameters.SourceDirectory + "**/obj/";

    Information("Deleting object directories");

    foreach (var dir in GetDirectories(pattern))
        DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
}