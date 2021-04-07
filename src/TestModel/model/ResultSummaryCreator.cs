// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Xml;

namespace TestCentric.Gui.Model
{
    public class ResultSummaryCreator
    {
        public static ResultSummary FromResultNode(ResultNode resultNode)
        {
            var result = resultNode.Xml;

            if (result.Name != "test-run")
                throw new InvalidOperationException("Expected <test-run> as top-level element but was <" + result.Name + ">");

            var summary = new ResultSummary();
            summary.OverallResult = resultNode.Outcome.Status.ToString();
            summary.Duration = result.GetAttribute("duration", 0.0);
            summary.StartTime = result.GetAttribute("start-time", DateTime.MinValue);
            summary.EndTime = result.GetAttribute("end-time", DateTime.MaxValue);
            Summarize(result, summary);
            return summary;
        }

        private static void Summarize(XmlNode node, ResultSummary summary)
        {
            string type = node.GetAttribute("type");
            string status = node.GetAttribute("result");
            string label = node.GetAttribute("label");

            switch (node.Name)
            {
                case "test-case":
                    summary.TestCount++;

                    SummarizeTestCase(summary, status, label);
                    break;

                case "test-suite":
                    SummarizeTestSuite(node.ChildNodes, summary, type, status, label);
                    break;

                case "test-run":
                    Summarize(node.ChildNodes, summary);
                    break;
            }
        }

        private static void SummarizeTestCase(ResultSummary summary, string status, string label)
        {
            switch (status)
            {
                case "Passed":
                    summary.PassCount++;
                    break;
                case "Failed":
                    if (label == null)
                        summary.FailureCount++;
                    else if (label == "Invalid")
                        summary.InvalidCount++;
                    else
                        summary.ErrorCount++;
                    break;
                case "Warning":
                    summary.WarningCount++;
                    break;
                case "Inconclusive":
                    summary.InconclusiveCount++;
                    break;
                case "Skipped":
                    if (label == "Ignored")
                        summary.IgnoreCount++;
                    else if (label == "Explicit")
                        summary.ExplicitCount++;
                    else
                        summary.SkipCount++;
                    break;
                default:
                    summary.SkipCount++;
                    break;
            }
        }

        private static void SummarizeTestSuite(XmlNodeList childNodes, ResultSummary summary, string type, string status, string label)
        {
            if (status == "Failed" && label == "Invalid")
            {
                if (type == "Assembly") summary.InvalidAssemblies++;
                else summary.InvalidTestFixtures++;
            }
            if (type == "Assembly" && status == "Failed" && label == "Error")
            {
                summary.InvalidAssemblies++;
                summary.UnexpectedError = true;
            }

            Summarize(childNodes, summary);
        }

        private static void Summarize(XmlNodeList nodes, ResultSummary summary)
        {
            foreach (XmlNode childResult in nodes)
                Summarize(childResult, summary);
        }
    }
}
