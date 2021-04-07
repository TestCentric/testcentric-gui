// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Xml;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters
{
    using Views;
    using Model;
    using Controls;

    public class ProgressBarPresenterTests
    {
        private IProgressBarView _view;
        private ITestModel _model;
        private ProgressBarPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _view = Substitute.For<IProgressBarView>();
            _model = Substitute.For<ITestModel>();

            _presenter = new ProgressBarPresenter(_view, _model);
        }

        [TearDown]
        public void RemovePresenter()
        {
            _presenter = null;
        }

        [Test]
        public void WhenTestsAreLoaded_ProgressBar_IsInitialized()
        {
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);

            var testNode = new TestNode("<test-suite id='1' testcasecount='1234'/>");
            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestsAreUnloaded_ProgressBar_IsInitialized()
        {
            _model.HasTests.Returns(false);
            _model.IsTestRunning.Returns(false);
            _model.Events.TestUnloaded += Raise.Event<TestEventHandler>(new TestEventArgs());

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestsAreReloaded_ProgressBar_IsInitialized()
        {
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);

            var testNode = new TestNode("<test-suite id='1' testcasecount='1234'/>");
            _model.Events.TestReloaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestRunBegins_ProgressBar_IsInitialized()
        {
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(true);
            _model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(1234));

            _view.Received().Initialize(1234);
        }

        [Test]
        public void WhenTestCaseCompletes_ProgressIsIncremented()
        {
            var result = new ResultNode("<test-case id='1'/>");

            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            _view.Received().Progress++;
        }

        [Test]
        public void WhenTestSuiteCompletes_ProgressIsNotIncremented()
        {
            var result = new ResultNode("<test-suite id='1'/>");

            _model.Events.SuiteFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            _view.DidNotReceive().Progress++;
        }

        //[Test]
        //public void WhenTestSuiteCompletes_ResultIsPostedToView()
        //{
        //    int priorValue = _view.Progress;
        //    var suiteResult = new ResultNode(XmlHelper.CreateTopLevelElement("test-suite"));

        //    _settings.TestFinished += Raise.Event<TestEventHandler>(new TestEventArgs(TestAction.TestFinished, suiteResult));

        //    Assert.That(_view.Progress, Is.EqualTo(priorValue));
        //}

        static object[] statusTestCases = new object[] {
            new object[] { ProgressBarStatus.Success, ResultState.Failure, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Warning, ResultState.Failure, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Failure, ResultState.Failure, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.Error, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Warning, ResultState.Error, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Failure, ResultState.Error, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.NotRunnable, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Warning, ResultState.NotRunnable, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Failure, ResultState.NotRunnable, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.Ignored, ProgressBarStatus.Warning },
            new object[] { ProgressBarStatus.Warning, ResultState.Ignored, ProgressBarStatus.Warning },
            new object[] { ProgressBarStatus.Failure, ResultState.Ignored, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.Inconclusive, ProgressBarStatus.Success },
            new object[] { ProgressBarStatus.Warning, ResultState.Inconclusive, ProgressBarStatus.Warning },
            new object[] { ProgressBarStatus.Failure, ResultState.Inconclusive, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.Skipped, ProgressBarStatus.Success },
            new object[] { ProgressBarStatus.Warning, ResultState.Skipped, ProgressBarStatus.Warning },
            new object[] { ProgressBarStatus.Failure, ResultState.Skipped, ProgressBarStatus.Failure },
            new object[] { ProgressBarStatus.Success, ResultState.Success, ProgressBarStatus.Success },
            new object[] { ProgressBarStatus.Warning, ResultState.Success, ProgressBarStatus.Warning },
            new object[] { ProgressBarStatus.Failure, ResultState.Success, ProgressBarStatus.Failure }
        };

        [TestCaseSource("statusTestCases")]
        public void BarShowsProperStatus(ProgressBarStatus priorStatus, ResultState resultState, ProgressBarStatus expectedStatus)
        {
            _view.Status = priorStatus;

            var doc = new XmlDocument();
            if (resultState.Label == string.Empty)
                doc.LoadXml(string.Format("<test-case id='1' result='{0}'/>", resultState.Status));
            else
                doc.LoadXml(string.Format("<test-case id='1' result='{0}' label='{1}'/>", resultState.Status, resultState.Label));
            var result = new ResultNode(doc.FirstChild);

            _model.HasTests.Returns(true);
            _model.Tests.Returns(result);
            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(result));
            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            Assert.That(_view.Status, Is.EqualTo(expectedStatus));
        }
    }
}
