//////////////////////////////////////////////////////////////////////
// HELPER METHODS - GENERAL
//////////////////////////////////////////////////////////////////////

T GetArgument<T>(string pattern, T defaultValue)
{
    foreach (string name in pattern.Split('|'))
        if (HasArgument(name))
            return Argument<T>(name);

    return defaultValue;
}

public void DeleteObjectDirectories(BuildParameters parameters)
{
    string pattern = parameters.SourceDirectory + "**/obj/";

    Information("Deleting object directories");

    foreach (var dir in GetDirectories(pattern))
        DeleteDirectory(dir, new DeleteDirectorySettings() { Recursive = true });
}

public static void DisplayBanner(string message)
{
    var bar = new string('-', Math.Max(message.Length, 40));
    Console.WriteLine();
    Console.WriteLine(bar);
    Console.WriteLine(message);
    Console.WriteLine(bar);
}
