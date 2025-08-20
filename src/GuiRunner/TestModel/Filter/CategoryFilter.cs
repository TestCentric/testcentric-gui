// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TestCentric.Gui.Model.Filter
{
    /// <summary>
    /// Filters the TestNodes by test categories. Use item 'No category' to filter for tests without any test category.
    /// </summary>
    public class CategoryFilter : ITestFilter
    {
        public const string NoCategory = "No category";

        private List<string> _condition = new List<string>();

        public CategoryFilter(ITestModel model)
        {
            TestModel = model;
        }

        private ITestModel TestModel { get; }

        public string FilterId => "CategoryFilter";

        public IEnumerable<string> Condition
        {
            get { return _condition; }
            set { _condition = value.ToList(); }
        }

        public bool IsActive => AllCategories.Except(_condition).Any();

        public IEnumerable<string> AllCategories { get; private set; }

        public bool IsMatching(TestNode testNode)
        {
            // Consider test-case nodes only: categories from fixtures are applied to test-cases
            if (_condition.Any() == false || testNode.IsProject || testNode.IsSuite)
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
            return _condition.Intersect(categories).Any();
        }

        public void Reset()
        {
            _condition = GetAllCategories();
        }

        public void Init()
        {
            AllCategories = GetAllCategories();
            _condition = AllCategories.ToList();
        }

        private List<string> GetAllCategories()
        {
            var items = TestModel.AvailableCategories;
            var allCategories = items.Concat(new[] { NoCategory });
            return allCategories.ToList();
        }
    }
}
