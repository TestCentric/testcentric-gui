// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCentric.Gui.Model
{
    internal class TestCentricTestFilter : ITestCentricTestFilter
    {
        // By default: all outcome filters are enabled
        List<string> _outcomeFilter = new List<string>()
        {
            "Passed",
            "Failed",
            "Ignored",
            "Skipped",
            "Inconclusive",
            "Not Run",
        };

        internal TestCentricTestFilter(TestModel model, Action filterChangedEvent)
        {
            TestModel = model;
            FireFilterChangedEvent = filterChangedEvent;
        }

        private ITestModel TestModel { get; }

        private Action FireFilterChangedEvent;

        public IEnumerable<string> OutcomeFilter
        {
            get => _outcomeFilter;

            set
            {
                _outcomeFilter = value.ToList();
                FilterNodes(TestModel.LoadedTests);
                FireFilterChangedEvent();
            }
        }

        private bool FilterNodes(TestNode testNode)
        {
            // 1. Check if any child is visible => parent must be visible too
            bool childIsVisible = false;
            foreach (TestNode child in testNode.Children)
                if (FilterNodes(child))
                    childIsVisible = true;

            // 2. Check if node itself is visible
            bool isVisible = IsOutcomeFilterMatching(testNode);
            testNode.IsVisible = isVisible || childIsVisible;
            return testNode.IsVisible;
        }

        private bool IsOutcomeFilterMatching(TestNode testNode)
        {
            string outcome = "Not Run";

            var result = TestModel.GetResultForTest(testNode.Id);
            if (result != null)
            {
                switch (result.Outcome.Status)
                {
                    case TestStatus.Failed:
                    case TestStatus.Passed:
                    case TestStatus.Inconclusive:
                        outcome = result.Outcome.Status.ToString();
                        break;
                    case TestStatus.Skipped:
                        outcome = result.Outcome.Label == "Ignored" ? "Ignored" : "Skippeed";
                        break;
                }
            }

            return OutcomeFilter.Contains(outcome);
        }
    }
}
