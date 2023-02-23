// Class that knows how to run tests against either GUI
public class GuiTester
{
	private BuildSettings _settings;
	private ICakeContext _context;

	public GuiTester(BuildSettings settings)
	{
		_settings = settings;
		_context = settings.Context;
	}

	public int RunGuiUnattended(string runnerPath, string arguments)
	{
		if (!arguments.Contains(" --run"))
			arguments += " --run";
		if (!arguments.Contains(" --unattended"))
			arguments += " --unattended";
		if (!arguments.Contains(" --full-gui"))
			arguments += " --full-gui";

		return RunGui(runnerPath, arguments);
	}

	public int RunGui(string runnerPath, string arguments)
	{
		return _context.StartProcess(runnerPath, new ProcessSettings()
		{
			Arguments = arguments,
			WorkingDirectory = _settings.OutputDirectory
		});
	}
}
