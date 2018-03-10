// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

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

        public static TestFilter MakeIdFilter(params TestNode[] testNodes)
        {
            StringBuilder sb = new StringBuilder("<filter>");

            if (testNodes.Length > 1)
                sb.Append("<or>");

            foreach (TestNode test in testNodes)
                if (test.RunState != RunState.Explicit)
                    sb.AppendFormat("<id>{0}</id>", test.Id);

            if (testNodes.Length > 1)
                sb.Append("</or>");

            sb.Append("</filter>");

            return new TestFilter(sb.ToString());
        }

        public static TestFilter MakeCategoryFilter(params string[] categories)
        {
            var sb = new StringBuilder("<filter>");

            if (categories.Length > 1)
                sb.Append("<or>");

            foreach (string category in categories)
                sb.Append($"<cat>{category}</cat>");

            if (categories.Length > 1)
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
