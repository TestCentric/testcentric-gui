// ***********************************************************************
// Copyright (c) 2016-2018 Charlie Poole
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

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters
{
    using Views;
    using Model;

    public class StatusBarPresenterTests
    {
        private IStatusBarView _view;
        private ITestModel _model;
        private StatusBarPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _view = Substitute.For<IStatusBarView>();
            _model = Substitute.For<ITestModel>();

            _presenter = new StatusBarPresenter(_view, _model);
        }

        [TearDown]
        public void RemovePresenter()
        {
            _presenter = null;
        }

        [Test]
        public void WhenTestsAreLoaded_StatusBar_IsInitialized()
        {
            var testNode = new TestNode("<test-run id='2' testcasecount='123' />");
            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));

            _view.Received().Initialize(123);
            _view.Received().DisplayText("Ready");
        }

        [Test]
        public void WhenTestsArReloaded_StatusBar_IsInitialized()
        {
            var testNode = new TestNode("<test-run id='2' testcasecount='123' />");
            _model.Events.TestReloaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));

            _view.Received().Initialize(123);
            _view.Received().DisplayText("Reloaded");
        }

        [Test]
        public void WhenTestsAreUnloaded_StatusBar_IsInitialized()
        {
            _model.Events.TestUnloaded += Raise.Event<TestEventHandler>(new TestEventArgs());

            _view.Received().Initialize(0);
            _view.Received().DisplayText("Unloaded");
        }

        [Test]
        public void WhenTestRunBegins_StatusBar_IsInitialized()
        {
            _model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(1234));

            _view.Received().Initialize(1234);
            _view.Received().DisplayTestsRun(0);
            _view.Received().DisplayPassed(0);
            _view.Received().DisplayFailed(0);
            _view.Received().DisplayWarnings(0);
            _view.Received().DisplayInconclusive(0);
            _view.Received().DisplayTime(0.0);
        }

        [Test]
        public void WhenTestBegins_NameIsDisplayed()
        {
            var testNode = new TestNode("<test-case id='1' name='NAME' fullname='FULLNAME' />");
            _model.Events.TestStarting += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));

            _view.Received().DisplayText("FULLNAME");
        }

        [Test]
        public void WhenTestCasePasses_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Passed' />");

            for (int i = 1; i <= 3; i++)
            {
                _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

                _view.Received().DisplayTestsRun(i);
                _view.Received().DisplayPassed(i);
            }
        }

        [Test]
        public void WhenTestCaseHasWarning_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Warning' />");

            for (int i = 1; i <= 3; i++)
            {
                _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

                _view.Received().DisplayTestsRun(i);
                _view.Received().DisplayWarnings(i);
            }
        }

        [Test]
        public void WhenTestCaseIsInconclusive_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Inconclusive' />");

            for (int i = 1; i <= 3; i++)
            {
                _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

                _view.Received().DisplayTestsRun(i);
                _view.Received().DisplayInconclusive(i);
            }
        }

        [Test]
        public void WhenTestCaseFails_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Failed' />");

            for (int i = 1; i <= 3; i++)
            {
                _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

                _view.Received().DisplayTestsRun(i);
                _view.Received().DisplayFailed(i);
            }
        }

        [Test]
        public void WhenTestsFinish_DurationIsDisplayed()
        {
            var result = new ResultNode("<test-run duration='1.234' />");
            _model.Events.RunFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            _view.Received().DisplayText("Completed");
            _view.Received().DisplayTime(1.234);
        }
    }
}
