// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters
{
    /// <summary>
    /// This struct represents the numbers of test cases which passed, failed, ignored... for one TestResult node
    /// </summary>
    public struct TestResultCounts
    {
        public int FailedCount { get; set; }

        public int PassedCount { get; set; }

        public int WarningCount { get; set; }

        public int NotRunCount { get; set; }

        public int InconclusiveCount { get; set; }

        public int IgnoreCount { get; set; }

        public int ExplicitCount { get; set; }

        public int TestCount => FailedCount + PassedCount + WarningCount + NotRunCount + InconclusiveCount + IgnoreCount + ExplicitCount;


        private void Add(TestResultCounts summary)
        {
            FailedCount += summary.FailedCount;
            PassedCount += summary.PassedCount;
            WarningCount += summary.WarningCount;
            NotRunCount += summary.NotRunCount;
            InconclusiveCount += summary.InconclusiveCount;
            IgnoreCount += summary.IgnoreCount;
            ExplicitCount += summary.ExplicitCount;
        }

        public static TestResultCounts GetResultCounts(ITestModel model, TestNode testNode)
        {
            TestResultCounts testCounts = new TestResultCounts();

            ResultNode result = model.GetResultForTest(testNode.Id);

            // Only consider outcome from test cases; and check if testNode is filtered out
            if (!testNode.IsProject && !testNode.IsSuite && !testNode.IsAssembly && testNode.IsVisible)
            {
                if (result != null)
                {
                    if (result.Outcome.Status == TestStatus.Passed)
                        testCounts.PassedCount++;
                    else if (result.Outcome.Status == TestStatus.Failed)
                        testCounts.FailedCount++;
                    else if (result.Outcome.Status == TestStatus.Inconclusive)
                        testCounts.InconclusiveCount++;
                    else if (result.Outcome.Status == TestStatus.Warning)
                        testCounts.WarningCount++;
                    else if (result.Outcome.Status == TestStatus.Skipped && result.Label == "Ignored")
                        testCounts.IgnoreCount++;
                    else if (result.Outcome.Status == TestStatus.Skipped && result.Label == "Explicit")
                        testCounts.ExplicitCount++;
                }
                else
                    testCounts.NotRunCount = 1;
            }

            // Iterate through all child nodes
            foreach (TestNode childNode in testNode.Children)
            {
                testCounts.Add(GetResultCounts(model, childNode));
            }

            return testCounts;
        }

    }
}
