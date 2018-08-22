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

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Views;
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
            ResultNode resultNode = CreateResultNode("test-case", testName, result, output);

            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));
        }

        protected void FireSuiteFinishedEvent(string testName, string result, string output = null)
        {
            ResultNode resultNode = CreateResultNode("test-suite", testName, result, output);

            _model.Events.SuiteFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));
        }

        private static ResultNode CreateResultNode(string element, string testName, string result, string output = null)
        {
            int colon = result.IndexOf(':');
            ResultNode resultNode;

            if (colon > 0)
            {
                string status = result.Substring(0, colon);
                string label = result.Substring(colon + 1);
                resultNode = new ResultNode($"<{element} fullname='{testName}' result='{status}' label='{label}'/>");
            }
            else
            {
                resultNode = new ResultNode($"<{element} fullname='{testName}' result='{result}'/>");
            }

            if (output != null)
                resultNode.Xml.AddElementWithCDataSection("output", output);

            return resultNode;
        }

        protected void FireTestOutputEvent(string testName, string stream, string text)
        {
            _model.Events.TestOutput += Raise.Event<TestOutputEventHandler>(new TestOutputEventArgs(testName, stream, text));
        }
    }
}
