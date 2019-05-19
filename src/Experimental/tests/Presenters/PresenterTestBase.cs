// ***********************************************************************
// Copyright (c) 2018 Charlie Poole, Rob Prouse
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

using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NSubstitute;
using NUnit.Framework;

using FakeUserSettings = NUnit.TestUtilities.Fakes.UserSettings;

namespace TestCentric.Gui.Presenters
{
    using Elements;
    using Model;

    /// <summary>
    /// Base class for presenter tests, providing utiity methods
    /// </summary>
    public class PresenterTestBase<TView> where TView : class
    {
        protected TView _view;
        protected ITestModel _model;
        protected FakeUserSettings _settings;

        [SetUp]
        public void Initialize()
        {
            _view = Substitute.For<TView>();
            _model = Substitute.For<ITestModel>();
            _settings = new FakeUserSettings();
            _model.Services.UserSettings.Returns(_settings);
            _model.TestFiles.Returns(new List<string>());
        }

        #region Helper Methods

        protected void ClearAllReceivedCalls()
        {
            _view.ClearReceivedCalls();

            foreach (PropertyInfo prop in _view.GetType().GetProperties())
            {
                if (typeof(IViewElement).IsAssignableFrom(prop.PropertyType))
                    prop.GetValue(_view).ClearReceivedCalls();
            }
        }

        protected void FireTestsLoadingEvent(IList<string> testFiles)
        {
            _model.Events.TestsLoading += Raise.Event<TestFilesLoadingEventHandler>(new TestFilesLoadingEventArgs(testFiles));
        }

        protected void FireTestLoadedEvent(TestNode testNode)
        {
            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));
        }

        protected void FireTestsReloadingEvent()
        {
            _model.Events.TestsReloading += Raise.Event<TestEventHandler>(new TestEventArgs());
        }

        protected void FireTestReloadedEvent(TestNode testNode)
        {
            _model.Events.TestReloaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));
        }

        protected void FireTestUnloadedEvent()
        {
            _model.Events.TestUnloaded += Raise.Event<TestEventHandler>(new TestEventArgs());
        }

        protected void FireTestsUnloadingEvent()
        {
            _model.Events.TestsUnloading += Raise.Event<TestEventHandler>(new TestEventArgs());
        }

        protected void FireRunStartingEvent(int testCount)
        {
            _model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(testCount));
        }

        protected void FireRunFinishedEvent(ResultNode result)
        {
            _model.Events.RunFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));
        }

        protected void FireTestStartingEvent(string testName)
        {
            var start = new TestNode($"<test-start fullname='{testName}'/>");
            _model.Events.TestStarting += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(start));
        }

        protected void FireSuiteStartingEvent(string testName)
        {
            var start = new TestNode($"<suite-start fullname='{testName}'/>");
            _model.Events.SuiteStarting += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(start));
        }

        protected void FireTestFinishedEvent(string testName, string result, string output = null)
        {
            FireTestFinishedEvent(CreateResultNode("test-case", testName, result, output));

        }

        protected void FireTestFinishedEvent(string testName, string result, FailureSite site)
        {
            FireTestFinishedEvent(CreateResultNode("test-case", testName, result, site));
        }

        protected void FireTestFinishedEvent(ResultNode result)
        {
            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));
        }

        protected void FireSuiteFinishedEvent(string testName, string result, string output = null)
        {
            FireSuiteFinishedEvent(CreateResultNode("test-suite", testName, result, output));
        }

        protected void FireSuiteFinishedEvent(string testName, string result, FailureSite site)
        {
            FireSuiteFinishedEvent(CreateResultNode("test-suite", testName, result, site));
        }

        protected void FireSuiteFinishedEvent(ResultNode result)
        {
            _model.Events.SuiteFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));
        }

        private static ResultNode CreateResultNode(string element, string testName, string result, string output = null)
        {
            return new ResultNode(CreateResultXml(element, testName, result, output));
        }

        private static ResultNode CreateResultNode(string element, string testName, string result, FailureSite site)
        {
            return new ResultNode(CreateResultXml(element, testName, result, site));
        }

        private static XmlNode CreateResultXml(string element, string testName, string result, FailureSite site)
        {
            var xmlNode = CreateResultXml(element, testName, result);
            xmlNode.AddAttribute("site", site.ToString());
            return xmlNode;
        }

        private static XmlNode CreateResultXml(string element, string testName, string result, string output = null)
        {
            int colon = result.IndexOf(':');
            string xml;

            if (colon > 0)
            {
                string status = result.Substring(0, colon);
                string label = result.Substring(colon + 1);
                xml = $"<{element} fullname='{testName}' result='{status}' label='{label}'/>";
            }
            else
            {
                xml = $"<{element} fullname='{testName}' result='{result}'/>";
            }

            var xmlNode = XmlHelper.CreateXmlNode(xml);
            if (output != null)
                xmlNode.AddElementWithCDataSection("output", output);

            return xmlNode;
        }

        protected void FireTestOutputEvent(string testName, string stream, string text)
        {
            _model.Events.TestOutput += Raise.Event<TestOutputEventHandler>(new TestOutputEventArgs(testName, stream, text));
        }

        #endregion
    }
}
