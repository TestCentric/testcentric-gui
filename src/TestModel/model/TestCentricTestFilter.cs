// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace TestCentric.Gui.Model
{
    internal class TestCentricTestFilter : ITestCentricTestFilter
    {
        // By default: all outcome filters are enabled
        List<string> _outcomeFilter = new List<string>() { "All" };

        string _textFilter;
        List<string> _categoryFilter = new List<string>();

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
            _outcomeFilter = new List<string>() { "All" };
            _categoryFilter = GetAllCategories();
            _textFilter = null;

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

            IList<string> categories = new List<string>();
            foreach (XmlNode node in testNode.Xml.SelectNodes(xpathExpression))
            {
                var groupName = node.Attributes["value"].Value;
                if (!string.IsNullOrEmpty(groupName))
                    categories.Add(groupName);
            }

            if (categories.Any() == false)
                categories.Add("No category");

            return CategoryFilter.Intersect(categories).Any();
        }

        private bool IsOutcomeFilterMatching(TestNode testNode)
        {
            // All kind of outcomes should be displayed (no outcome filtering)
            if (OutcomeFilter.Contains("All"))
                return true;

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
                        outcome = result.Outcome.Label == "Ignored" ? "Ignored" : "Skipped";
                        break;
                }
            }

            return OutcomeFilter.Contains(outcome);
        }

        private List<string> GetAllCategories()
        {
            var items = TestModel.AvailableCategories;
            var allCategories = items.Concat(new[] { "No category" });
            return allCategories.ToList();
        }
    }
}
