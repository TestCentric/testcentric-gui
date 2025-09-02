// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    public class CategoryGrouping : GroupingBase
    {
        public CategoryGrouping(INUnitTreeDisplayStrategy support, ITestModel model, ITestTreeView view) :
            base(support, model, view)
        {
        }

        public override IList<string> GetGroupNames(TestNode testNode)
        {
            return GetAllCategories(testNode);
        }


        private List<string> GetAllCategories(TestNode testNode)
        {
            string xpathExpression = "ancestor-or-self::*/properties/property[@name='Category']";

            // Get list of available categories of the TestNode
            List<string> categories = new List<string>();
            foreach (XmlNode node in testNode.Xml.SelectNodes(xpathExpression))
            {
                var groupName = node.Attributes["value"].Value;
                if (!string.IsNullOrEmpty(groupName) && !categories.Contains(groupName))
                    categories.Add(groupName);
            }

            if (categories.Any() == false)
                categories.Add("None");

            return categories;
        }

        /// <inheritdoc />
        protected override TestGroup CreateTestGroup(string name, TestNode testNode)
        {
            return new CategoryGroupingTestGroup(testNode, CurrentRootGroupName, name);
        }
    }
}
