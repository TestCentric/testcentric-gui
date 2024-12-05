// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TestCentric.Gui.Model
{
    public class TestCentricTestFilter : ITestCentricTestFilter
    {
        public const string AllOutcome = "All";
        public const string NotRunOutcome = "Not Run";
        public const string NoCategory = "No category";


        // By default: all outcome filters are enabled
        private List<string> _outcomeFilter = new List<string>() { AllOutcome };
        private string _textFilter = string.Empty;
        private List<string> _categoryFilter = new List<string>();

        public TestCentricTestFilter(ITestModel model, Action filterChangedEvent)
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

        public IEnumerable<string> CategoryFilter
        {
            get => _categoryFilter;

            set
            {
                _categoryFilter = value.ToList();
                FilterNodes(TestModel.LoadedTests);
                FireFilterChangedEvent();
            }
        }

        public string TextFilter
        {
            get { return _textFilter; }
            set
            {
                _textFilter = value;
                FilterNodes(TestModel.LoadedTests);
                FireFilterChangedEvent();
            }
        }

        public IEnumerable<string> AllCategories { get; private set; }

        public void Init()
        {
            AllCategories = GetAllCategories();
            CategoryFilter = AllCategories;
        }

        public void ClearAllFilters()
        {
            _outcomeFilter = new List<string>() { AllOutcome };
            _categoryFilter = GetAllCategories();
            _textFilter = string.Empty;

            FilterNodes(TestModel.LoadedTests);
            FireFilterChangedEvent();
        }


        private bool FilterNodes(TestNode testNode)
        {
            // 1. Check if any child is visible => parent must be visible too
            bool childIsVisible = false;
            foreach (TestNode child in testNode.Children)
                if (FilterNodes(child))
                    childIsVisible = true;

            // 2. Check if node itself is visible
            bool isVisible = IsOutcomeFilterMatching(testNode) && IsTextFilterMatching(testNode) && IsCategoryMatching(testNode);
            testNode.IsVisible = isVisible || childIsVisible;
            return testNode.IsVisible;
        }

        private bool IsTextFilterMatching(TestNode testNode)
        {
            if (string.IsNullOrEmpty(_textFilter))
            {
                return true;
            }

            return testNode.FullName.IndexOf(_textFilter, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        private bool IsCategoryMatching(TestNode testNode)
        {
            if (CategoryFilter.Any() == false)
                return false;

            string xpathExpression = "ancestor-or-self::*/properties/property[@name='Category']";

            // 1. Get list of available categories at TestNode
            IList<string> categories = new List<string>();
            foreach (XmlNode node in testNode.Xml.SelectNodes(xpathExpression))
            {
                var groupName = node.Attributes["value"].Value;
                if (!string.IsNullOrEmpty(groupName))
                    categories.Add(groupName);
            }

            if (categories.Any() == false)
                categories.Add(NoCategory);

            // 2. Check if any filter category matches the available categories
            return CategoryFilter.Intersect(categories).Any();
        }

        private bool IsOutcomeFilterMatching(TestNode testNode)
        {
            // All kind of outcomes should be displayed (no outcome filtering)
            if (OutcomeFilter.Contains(AllOutcome))
                return true;

            string outcome = NotRunOutcome;

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
                        outcome = result.Outcome.Label == "Ignored" ? "Ignored" : "Skipped";
                        break;
                }
            }

            return OutcomeFilter.Contains(outcome);
        }

        private List<string> GetAllCategories()
        {
            var items = TestModel.AvailableCategories;
            var allCategories = items.Concat(new[] { NoCategory });
            return allCategories.ToList();
        }
    }
}
