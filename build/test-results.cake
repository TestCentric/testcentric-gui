// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

// This file contains classes used to interpret the result XML that is
// produced by test runs of the GUI.

public abstract class ResultSummary
{
	public string OverallResult { get; set; }
	public int Total  { get; set; }
	public int Passed  { get; set; }
	public int Failed  { get; set; }
	public int Warnings  { get; set; }
	public int Inconclusive  { get; set; }
	public int Skipped { get; set; }
}

public class ExpectedResult : ResultSummary
{
	public ExpectedResult(string overallResult)
	{
		if (string.IsNullOrEmpty(overallResult))
			throw new ArgumentNullException(nameof(overallResult));

		OverallResult = overallResult;
		
		// Initialize counters to -1, indicating no expected value.
		// Set properties of those items to be checked.
		Total = Passed = Failed = Warnings = Inconclusive = Skipped = -1;
	}

	public ExpectedAssemblyResult[] Assemblies { get; set; } = new ExpectedAssemblyResult[0];
}

public class ExpectedAssemblyResult
{
	public ExpectedAssemblyResult(string name, string expectedRuntime)
	{
		Name = name;
		Runtime = expectedRuntime;
	}

	public string Name { get; }
	public string Runtime { get; }
}

public class ActualResult : ResultSummary
{
	public ActualResult(string resultFile)
	{
		var doc = new XmlDocument();
		doc.Load(resultFile);

		Xml = doc.DocumentElement;
		if (Xml.Name != "test-run")
			throw new Exception("The test-run element was not found.");

		OverallResult = GetAttribute(Xml, "result");
		Total = IntAttribute(Xml, "total");
		Passed = IntAttribute(Xml, "passed");
		Failed = IntAttribute(Xml, "failed");
		Warnings = IntAttribute(Xml, "warnings");
		Inconclusive = IntAttribute(Xml, "inconclusive");
		Skipped = IntAttribute(Xml, "skipped");

		var assemblies = new List<ActualAssemblyResult>();

		foreach (XmlNode node in Xml.SelectNodes("//test-suite[@type='Assembly']"))
			assemblies.Add(new ActualAssemblyResult(node));

		Assemblies = assemblies.ToArray();
	}

	public XmlNode Xml { get; }

	public ActualAssemblyResult[] Assemblies { get; }

	private string GetAttribute(XmlNode node, string name)
	{
		return node.Attributes[name]?.Value;
	}

	private int IntAttribute(XmlNode node, string name)
	{
		string s = GetAttribute(node, name);
		// TODO: We should replace 0 with -1, representing a missing counter
		// attribute, after issue #707 is fixed.
		return s == null ? 0 : int.Parse(s);
	}
}

public class ActualAssemblyResult
{
	public ActualAssemblyResult(XmlNode xml)
    {
		Name = xml.Attributes["name"]?.Value;

		var env = xml.SelectSingleNode("environment");
		var settings = xml.SelectSingleNode("settings");

		// If TargetRuntimeFramework setting is not present, the GUI will have crashed anyway
		var runtimeSetting = settings.SelectSingleNode("setting[@name='TargetRuntimeFramework']");
		TargetRuntime = runtimeSetting?.Attributes["value"]?.Value;
		Runtime = DeduceActualRuntime(xml);
	}

	public string Name { get; }
	public string Runtime { get; }

	public string TargetRuntime { get; }

	// Code to determine the runtime actually used is adhoc
	// and works only for the set of runtimes we use in our
	// package  tests. We have to go through all this because
	// sufficient information for a cleaner approach is not
	// present in the result file.
	// TODO: Modify result file schema so this can be cleaner
	private static string DeduceActualRuntime(XmlNode assembly)
	{
		var env = assembly.SelectSingleNode("environment");
		// If TargetRuntimeFramework setting is not present, the GUI will have crashed anyway
		var runtimeSetting = assembly.SelectSingleNode("settings/setting[@name='TargetRuntimeFramework']");

		var clrVersion = env.Attributes["clr-version"]?.Value;
		var targetRuntime = runtimeSetting.Attributes["value"]?.Value;
		var runtime = "UNKNOWN";

		if (clrVersion.StartsWith(".NET Core 4.6") && targetRuntime == "netcore-1.1")
			runtime = targetRuntime;
		else if (clrVersion.StartsWith("3.1") && targetRuntime == "netcore-3.1")
			runtime = targetRuntime;
		else if (clrVersion.StartsWith("4.0") && targetRuntime.StartsWith("netcore-"))
			runtime = targetRuntime;
		else if (clrVersion.StartsWith("5.0") && targetRuntime == "netcore-5.0")
			runtime = targetRuntime;
		else if (clrVersion.StartsWith("2.0") && targetRuntime == "net-2.0")
			runtime = targetRuntime;
		else if (clrVersion.StartsWith("4.0") && targetRuntime.StartsWith("net-"))
			runtime = targetRuntime;

		return runtime;
	}
}
