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

using System.Globalization;
using System.Xml;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// TestNode represents a single NUnit test result in the test model.
    /// </summary>
    public class ResultNode : TestNode
    {
        #region Constructors

        public ResultNode(XmlNode xmlNode) : base(xmlNode)
        {
            Status = GetStatus();
            Label = GetAttribute("label");
            Site = GetSite();
            Outcome = new ResultState(Status, Label);
            AssertCount = GetAttribute("asserts", 0);
            var duration = GetAttribute("duration");
            Duration = duration != null
                ? double.Parse(duration, CultureInfo.InvariantCulture)
                : 0.0;
        }

        public ResultNode(string xmlText) : this(XmlHelper.CreateXmlNode(xmlText)) { }

        #endregion

        #region Public Properties

        public TestStatus Status { get; }
        public string Label { get; }
        public FailureSite Site { get; }
        public ResultState Outcome { get; }
        public int AssertCount { get; }
        public double Duration { get; }

        public string Message
        {
            get
            {
                return GetTrimmedInnerText(Xml.SelectSingleNode("failure/message")) ??
                       GetTrimmedInnerText(Xml.SelectSingleNode("reason/message"));
            }
        }

        public string StackTrace
        {
            get { return GetTrimmedInnerText(Xml.SelectSingleNode("failure/stack-trace")); }
        }

        public string Output
        {
            get { return GetTrimmedInnerText(Xml.SelectSingleNode("output")); }
        }

        #endregion

        #region Helper Methods

        private TestStatus GetStatus()
        {
            string status = GetAttribute("result");
            switch (status)
            {
                case "Passed":
                default:
                    return TestStatus.Passed;
                case "Inconclusive":
                    return TestStatus.Inconclusive;
                case "Failed":
                    return TestStatus.Failed;
                case "Warning":
                    return TestStatus.Warning;
                case "Skipped":
                    return TestStatus.Skipped;
            }
        }

        private FailureSite GetSite()
        {
            string site = GetAttribute("site");
            switch (site)
            {
                case "Test":
                default:
                    return FailureSite.Test;
                case "SetUp":
                    return FailureSite.SetUp;
                case "TearDown":
                    return FailureSite.TearDown;
                case "Parent":
                    return FailureSite.Parent;
                case "Child":
                    return FailureSite.Child;
            }
        }

        private static readonly char[] EOL_CHARS = new char[] { '\r', '\n' };

        private static string GetTrimmedInnerText(XmlNode node)
        {
            // In order to control the format, we trim any line-end chars
            // from end of the strings we write and supply them via calls
            // to WriteLine(). Newlines within the strings are retained.
            return node != null
                ? node.InnerText.TrimEnd(EOL_CHARS)
                : null;
        }

        #endregion
    }
}
