// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// Utility class for working with TestFilters.
    /// </summary>
    public class TestFilter
    {
        public TestFilter(string xmlText)
        {
            XmlText = xmlText;
            Xml = XmlHelper.CreateXmlNode(xmlText);
            InnerXml = Xml.InnerXml;
            IsEmpty = string.IsNullOrEmpty(InnerXml);
        }

        public static TestFilter Empty { get; } = new TestFilter("<filter/>");

        public string XmlText { get; }

        public XmlNode Xml { get; }

        public string InnerXml { get; }

        public bool IsEmpty { get; }

        public TestCentric.Engine.TestFilter AsNUnitFilter()
        {
            return IsEmpty
                ? TestCentric.Engine.TestFilter.Empty
                : new TestCentric.Engine.TestFilter(XmlText);
        }

        #region Static Methods to Create TestFilters

        public static TestFilter MakeIdFilter(TestNode test)
        {
            return new TestFilter($"<filter><id>{test.Id}</id></filter>");
        }

        public static TestFilter MakeIdFilter(IEnumerable<TestNode> testNodes)
        {
            if (testNodes.Count() == 1)
                return MakeIdFilter(testNodes.First());

            StringBuilder sb = new StringBuilder("<filter><or>");

            foreach (TestNode test in testNodes)
                sb.Append($"<id>{test.Id}</id>");

            sb.Append("</or></filter>");

            return new TestFilter(sb.ToString());
        }

        public static TestFilter MakeCategoryFilter(IList<string> categories)
        {
            var sb = new StringBuilder("<filter>");

            if (categories.Count > 1)
                sb.Append("<or>");

            foreach (string category in categories)
                sb.Append($"<cat>{category}</cat>");

            if (categories.Count > 1)
                sb.Append("</or>");

            sb.Append("</filter>");

            return new TestFilter(sb.ToString());
        }

        /// <summary>
        /// Creates a TestFilter which contains the IDs of all visible child nodes
        /// </summary>
        public static TestFilter MakeVisibleIdFilter(IEnumerable<TestNode> testNodes)
        {
            StringBuilder sb = new StringBuilder("<filter><or>");

            foreach (TestNode test in testNodes)
                MakeVisibleIdFilter(test, sb, t => testNodes.Contains(t));

            sb.Append("</or></filter>");

            return new TestFilter(sb.ToString());
        }

        private static void MakeVisibleIdFilter(TestNode testNode, StringBuilder sb, Func<TestNode, bool> isTestNodeSelected)
        {
            // If testNode is not visible, don't add it or any child to filter
            if (!testNode.IsVisible)
                return;

            // Add only Id for leaf nodes
            if (!testNode.IsProject && !testNode.IsSuite && IsExplicitTestSelected(testNode, isTestNodeSelected) && testNode.Children.Count == 0)
                sb.Append($"<id>{testNode.Id}</id>");

            foreach (TestNode childNode in testNode.Children)
                MakeVisibleIdFilter(childNode, sb, isTestNodeSelected);
        }

        private static bool IsExplicitTestSelected(TestNode testNode, Func<TestNode, bool> isTestNodeSelected)
        {
            if (testNode.RunState != RunState.Explicit)
                return true;

            return isTestNodeSelected(testNode);
        }

        public static TestFilter MakeNotFilter(TestFilter filter)
        {
            return new TestFilter($"<filter><not>{filter.InnerXml}</not></filter>");
        }

        public static TestFilter MakeAndFilter(params TestFilter[] filters)
        {
            StringBuilder sb = new StringBuilder("<filter><and>");

            foreach (var filter in filters)
                sb.Append(filter.InnerXml);

            sb.Append("</and></filter>");

            return new TestFilter(sb.ToString());
        }

        #endregion
    }
}
