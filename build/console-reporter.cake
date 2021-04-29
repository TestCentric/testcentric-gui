// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

// Reports on the output of a GUI test run using a format
// simiar to that of the NUnit console runner.
public class ConsoleReporter
{
    ActualResult _result;
    int ReportIndex;

    public ConsoleReporter(ActualResult result)
    {
        _result = result;
    }

    public void Display()
    {
        if (_result.Skipped > 0)
            DisplayNotRunReport();

        if (_result.Failed > 0 || _result.Warnings > 0)
            DisplayErrorsFailuresAndWarningsReport();

        DisplaySummaryReport();
    }

    public void DisplayNotRunReport()
    {
        ReportIndex = 0;
        Console.WriteLine("Tests Not Run");
        Console.WriteLine();

        DisplayTestsNotRun(_result.Xml);
    }

    private void DisplayTestsNotRun(XmlNode node)
    {
        switch (node.Name)
        {
            case "test-case":
                string status = node.Attributes["result"]?.Value;

                if (status == "Skipped")
                    DisplayTestResultItem(node);

                break;

            case "test-suite":
            case "test-run":
                foreach (XmlNode child in node.ChildNodes)
                    DisplayTestsNotRun(child);

                break;
        }
    }

    public void DisplayErrorsFailuresAndWarningsReport()
    {
        ReportIndex = 0;
        Console.WriteLine("Errors, Failures and Warnings");
        Console.WriteLine();

        DisplayErrorsFailuresAndWarnings(_result.Xml);
    }

    private void DisplayErrorsFailuresAndWarnings(XmlNode node)
    {
        string resultState = node.Attributes["result"]?.Value;

        switch (node.Name)
        {
            case "test-case":
                if (resultState == "Failed" || resultState == "Warning")
                    DisplayTestResultItem(node);
                return;

            case "test-run":
                foreach (XmlNode child in node.ChildNodes)
                    DisplayErrorsFailuresAndWarnings(child);
                break;

            case "test-suite":
                if (resultState == "Failed" || resultState == "Warning")
                {
                    var suiteType = node.Attributes["type"]?.Value;
                    if (suiteType == "Theory")
                    {
                        // Report failure of the entire theory and then go on
                        // to list the individual cases that failed
                        DisplayTestResultItem(node);
                    }
                    else
                    {
                        // Where did this happen? Default is in the current test.
                        var site = node.Attributes["site"]?.Value;

                        // Correct a problem in some framework versions, whereby warnings and some failures 
                        // are promulgated to the containing suite without setting the FailureSite.
                        if (site == null)
                        {
                            if (node.SelectSingleNode("reason/message")?.InnerText == "One or more child tests had warnings" ||
                                node.SelectSingleNode("failure/message")?.InnerText == "One or more child tests had errors")
                            {
                                site = "Child";
                            }
                            else
                                site = "Test";
                        }

                        // Only report errors in the current test method, setup or teardown
                        if (site == "SetUp" || site == "TearDown" || site == "Test")
                            DisplayTestResultItem(node);

                        // Do not list individual "failed" tests after a one-time setup failure
                        if (site == "SetUp") return;
                    }
                }

                foreach (XmlNode child in node.ChildNodes)
                    DisplayErrorsFailuresAndWarnings(child);

                break;
        }
    }

    private static readonly char[] EOL_CHARS = new char[] { '\r', '\n' };

    private void DisplayTestResultItem(XmlNode node)
    {
        string result = node.Attributes["result"]?.Value;
        string label = node.Attributes["label"]?.Value;
        string site = node.Attributes["site"]?.Value;

        string status = label ?? result;
        if (status == "Failed" || status == "Error")
            if (site == "SetUp" || site == "TearDown")
                status = site + " " + status;

        string fullname = node.Attributes["fullname"]?.Value;

        XmlNode messageNode = 
            node?.SelectSingleNode("failure/message") ?? 
            node?.SelectSingleNode("reason/message");
        string message = messageNode?.InnerText.TrimEnd(EOL_CHARS);

        string stacktrace = node?.SelectSingleNode("failure/stack-trace")?.InnerText.TrimEnd(EOL_CHARS);

        Console.WriteLine($"{++ReportIndex}) {status} : { fullname}");
        if (message != null)
            Console.WriteLine(message);
        if (stacktrace != null)
            Console.WriteLine(stacktrace);

        Console.WriteLine();
    }

    private void DisplaySummaryReport()
    {
        Console.WriteLine("Test Run Summary");
        Console.WriteLine($"  Overall result: {_result.OverallResult}");

        Console.Write($"  Test Count: {_result.Total}");
        Console.Write($", Passed: {_result.Passed}");
        Console.Write($", Failed: {_result.Failed}");
        Console.Write($", Warnings: {_result.Warnings}");
        Console.Write($", Inconclusive: {_result.Inconclusive}");
        Console.Write($", Skipped: {_result.Skipped}");
        Console.WriteLine();

        Console.WriteLine();
    }
}