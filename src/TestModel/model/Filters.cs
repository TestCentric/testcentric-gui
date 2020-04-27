// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// Utility class for working with TestFilters.
    /// </summary>
    public static class Filters
    {
        #region TestFilter Extensions

        public static bool IsEmpty(this TestFilter filter)
        {
            return filter == TestFilter.Empty;
        }

        private static XmlNode ToXml(this TestFilter filter)
        {
            return XmlHelper.CreateXmlNode(filter.Text);
        }

        //public static TestFilter FromXml(this XmlNode node)
        //{
        //    return new TestFilter(node.OuterXml);
        //}

        #endregion

        #region Methods to Create TestFilters

        public static TestFilter MakeIdFilter(TestNode test)
        {
            return new TestFilter($"<filter><id>{test.Id}</id></filter>");
        }

        public static TestFilter MakeIdFilter(IList<TestNode> testNodes)
        {
            if (testNodes.Count == 1)
                return MakeIdFilter(testNodes[0]);

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

        public static TestFilter MakeNotFilter(TestFilter filter)
        {
            return new TestFilter($"<filter><not>{filter.ToXml().InnerXml}</not></filter>");
        }

        public static TestFilter MakeAndFilter(params TestFilter[] filters)
        {
            StringBuilder sb = new StringBuilder("<filter><and>");

            foreach (var filter in filters)
                sb.Append(filter.ToXml().InnerXml);

            sb.Append("</and></filter>");

            return new TestFilter(sb.ToString());
        }

        #endregion
    }
}
