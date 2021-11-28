// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Xml;

namespace TestCentric.Engine.TestBed
{
    using Internal;

    static class ResultReporter
    {
        public static void ReportResults(XmlNode resultNode)
        {
            int failed = resultNode.GetAttribute("failed", 0);
            int warnings = resultNode.GetAttribute("warnings", 0);
            int skipped = resultNode.GetAttribute("skipped", 0);

            if (skipped > 0)
                DisplayNotRunReport(resultNode);

            if (failed + warnings > 0)
                DisplayErrorsFailuresAndWarnings(resultNode);

            DisplaySummaryReport(resultNode);
        }

        static void DisplayNotRunReport(XmlNode resultNode)
        {

        }

        static void DisplayErrorsFailuresAndWarnings(XmlNode resultNode)
        {
            Console.WriteLine("Errors, Failures and Warnings");
            int index = 0;
            foreach (XmlNode testNode in resultNode.SelectNodes("//test-case"))
            {
                var result = testNode.GetAttribute("result");
                if (result != "Failed" && result != "Skipped")
                    continue;

                var name = testNode.GetAttribute("fullname");
                var label = testNode.GetAttribute("label");
                var status = label == null ? "FAILED" : label.ToUpper();
                var message = 
                    testNode.SelectSingleNode("failure/message")?.InnerText ??
                    testNode.SelectSingleNode("reason/message")?.InnerText;

                Console.WriteLine($"\n{++index}) {status} {name}");
                if (message != null)
                    Console.WriteLine(message);
            }
        }

        static void DisplaySummaryReport(XmlNode resultNode)
        {
            string overall = resultNode.Attributes["result"]?.Value ?? "Unknown";
            int testcasecount = int.Parse(resultNode.Attributes["testcasecount"]?.Value ?? "0");
            int passed = int.Parse(resultNode.Attributes["passed"]?.Value ?? "0");
            int failed = int.Parse(resultNode.Attributes["failed"]?.Value ?? "0");
            int warnings = int.Parse(resultNode.Attributes["warnings"]?.Value ?? "0");
            int inconclusive = int.Parse(resultNode.Attributes["inconclusive"]?.Value ?? "0");
            int skipped = int.Parse(resultNode.Attributes["skipped"]?.Value ?? "0");

            Console.WriteLine("\nTest Run Summary");
            Console.WriteLine($"  Overall Result: {overall}");
            Console.WriteLine($"  Test Count: {testcasecount}, Passed: {passed}, Failed: {failed}," +
                $" Warnings: {warnings}, Inconclusive: {inconclusive}, Skipped: {skipped}");
        }
    }
}
