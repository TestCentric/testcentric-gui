string[] ENGINE_FILES = { 
    "testcentric.engine.dll", "testcentric.engine.core.dll", "testcentric.engine.api.dll", "testcentric.engine.metadata.dll", "Mono.Cecil.dll"};
string[] AGENT_FILES = { 
    "testcentric-agent.exe", "testcentric-agent.exe.config", "testcentric-agent-x86.exe", "testcentric-agent-x86.exe.config",
    "testcentric.engine.core.dll", "testcentric.engine.api.dll", "testcentric.engine.metadata.dll" };
string[] GUI_FILES = {
	"testcentric.exe", "testcentric.exe.config", "tc-next.exe", "tc-next.exe.config", "nunit.uiexception.dll",
	"TestCentric.Gui.Runner.dll", "Experimental.Gui.Runner.dll", "TestCentric.Gui.Model.dll", "TestCentric.Common.dll" };
string[] TREE_ICONS_JPG = {
    "Success.jpg", "Failure.jpg", "Ignored.jpg", "Inconclusive.jpg", "Skipped.jpg" };
string[] TREE_ICONS_PNG = {
    "Success.png", "Failure.png", "Ignored.png", "Inconclusive.png", "Skipped.png" };

private class PackageChecker
{
    protected string _packageName;
    protected string _packageDir;

    public PackageChecker(string packageName, string packageDir)
    {
        _packageName = packageName;
        _packageDir = packageDir;
    }

    public bool RunChecks(params Check[] checks)
    {
        bool allPassed = true;

        if (checks.Length == 0)
        {
            Console.WriteLine("  Package found but no checks were specified.");
        }
        else
        {
            foreach (var check in checks)
                allPassed &= check.Apply(_packageDir);

            if (allPassed)
                Console.WriteLine("  All checks passed!");
        }

        return allPassed;
    }
}

private abstract class Check
{
    public abstract bool Apply(string dir);

    protected static void RecordError(string msg)
    {
        Console.WriteLine("  ERROR: " + msg);
    }
}

private class FileCheck : Check
{
    string[] _paths;

    public FileCheck(string[] paths)
    {
        _paths = paths;
    }

    public override bool Apply(string dir)
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

private class DirectoryCheck : Check
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

    public override bool Apply(string dir)
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

private FileCheck HasFile(string file) => HasFiles(new [] { file });
private FileCheck HasFiles(params string[] files) => new FileCheck(files);  

private DirectoryCheck HasDirectory(string dir) => new DirectoryCheck(dir);
