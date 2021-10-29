//////////////////////////////////////////////////////////////////////
// SYNTAX FOR EXPRESSING CHECKS
//////////////////////////////////////////////////////////////////////

private static class Check
{
    public static void That(string testDir, params PackageCheck[] checks)
    {
        bool isOK = true;

        foreach (var check in checks)
            isOK &= check.ApplyTo(testDir);

        if (!isOK)
            throw new System.Exception("Verification failed!");
    }
}

private static FileCheck HasFile(string file) => HasFiles(new[] { file });
private static FileCheck HasFiles(params string[] files) => new FileCheck(files);

private static DirectoryCheck HasDirectory(string dir) => new DirectoryCheck(dir);

//////////////////////////////////////////////////////////////////////
// PACKAGECHECK CLASS
//////////////////////////////////////////////////////////////////////

public abstract class PackageCheck
{
    public abstract bool ApplyTo(string dir);

    protected static void RecordError(string msg)
    {
        Console.WriteLine("  ERROR: " + msg);
    }
}

//////////////////////////////////////////////////////////////////////
// FILECHECK CLASS
//////////////////////////////////////////////////////////////////////

public class FileCheck : PackageCheck
{
    string[] _paths;

    public FileCheck(string[] paths)
    {
        _paths = paths;
    }

    public override bool ApplyTo(string dir)
    {
        var isOK = true;

        foreach (string path in _paths)
        {
            if (!System.IO.File.Exists(dir + path))
            {
                RecordError($"File {path} was not found.");
                isOK = false;
            }
        }

        return isOK;
    }
}

//////////////////////////////////////////////////////////////////////
// DIRECTORYCHECK CLASS
//////////////////////////////////////////////////////////////////////

public class DirectoryCheck : PackageCheck
{
    private string _path;
    private List<string> _files = new List<string>();

    public DirectoryCheck(string path)
    {
        _path = path;
    }

    public DirectoryCheck WithFiles(params string[] files)
    {
        _files.AddRange(files);
        return this;
    }

    public DirectoryCheck AndFiles(params string[] files)
    {
        return WithFiles(files);
    }

    public DirectoryCheck WithFile(string file)
    {
        _files.Add(file);
        return this;
    }

    public DirectoryCheck AndFile(string file)
    {
        return AndFiles(file);
    }

    public override bool ApplyTo(string dir)
    {
        if (!System.IO.Directory.Exists(dir + _path))
        {
            RecordError($"Directory {_path} was not found.");
            return false;
        }

        bool isOK = true;

        if (_files != null)
        {
            foreach (var file in _files)
            {
                if (!System.IO.File.Exists(System.IO.Path.Combine(dir + _path, file)))
                {
                    RecordError($"File {file} was not found in directory {_path}.");
                    isOK = false;
                }
            }
        }

        return isOK;
    }
}
