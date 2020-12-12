public void DeleteObjectDirectories(BuildParameters parameters)
{
    string pattern = parameters.SourceDirectory + "**/obj/";

    Context.Information("Deleting object directories");

    foreach (var dir in Context.GetDirectories(pattern))
        Context.DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
}