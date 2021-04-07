// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class StatusBarPresenterTests : PresenterTestBase<IStatusBarView>
    {
        [SetUp]
        public void CreatePresenter()
        {
            new StatusBarPresenter(_view, _model);
        }

        [Test]
        public void WhenTestsAreLoaded_StatusBar_IsInitialized()
        {
            FireTestLoadedEvent(new TestNode("<test-run id='2' testcasecount='123' />"));

            _view.Received().Initialize(123);
            _view.Received().DisplayText("Ready");
        }

        [Test]
        public void WhenTestsArReloaded_StatusBar_IsInitialized()
        {
            FireTestReloadedEvent(new TestNode("<test-run id='2' testcasecount='123' />"));

            _view.Received().Initialize(123);
            _view.Received().DisplayText("Reloaded");
        }

        [Test]
        public void WhenTestsAreUnloaded_StatusBar_IsInitialized()
        {
            FireTestUnloadedEvent();

            _view.Received().Initialize(0);
            _view.Received().DisplayText("Unloaded");
        }

        [Test]
        public void WhenTestRunBegins_StatusBar_IsInitialized()
        {
            FireRunStartingEvent(1234);

            _view.Received().Initialize(1234);
            _view.Received().DisplayTestsRun(0);
            _view.Received().DisplayPassed(0);
            _view.Received().DisplayFailed(0);
            _view.Received().DisplayWarnings(0);
            _view.Received().DisplayInconclusive(0);
            _view.Received().DisplayDuration(0.0);
        }

        [Test]
        public void WhenTestBegins_NameIsDisplayed()
        {
            FireTestStartingEvent("TestName");

            _view.Received().DisplayText("TestName");
        }

        [Test]
        public void WhenTestCasePasses_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Passed' />");

            for (int i = 1; i <= 3; i++)
                FireTestFinishedEvent(result);

            Received.InOrder(() =>
            {
                _view.DisplayTestsRun(1);
                _view.DisplayPassed(1);
                _view.DisplayTestsRun(2);
                _view.DisplayPassed(2);
                _view.DisplayTestsRun(3);
                _view.DisplayPassed(3);
            });
        }

        [Test]
        public void WhenTestCaseHasWarning_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Warning' />");

            for (int i = 1; i <= 3; i++)
                FireTestFinishedEvent(result);

            Received.InOrder(() =>
            {
                _view.DisplayTestsRun(1);
                _view.DisplayWarnings(1);
                _view.DisplayTestsRun(2);
                _view.DisplayWarnings(2);
                _view.DisplayTestsRun(3);
                _view.DisplayWarnings(3);
            });
        }

        [Test]
        public void WhenTestCaseIsInconclusive_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Inconclusive' />");

            for (int i = 1; i <= 3; i++)
                FireTestFinishedEvent(result);

            Received.InOrder(() =>
            {
                _view.DisplayTestsRun(1);
                _view.DisplayInconclusive(1);
                _view.DisplayTestsRun(2);
                _view.DisplayInconclusive(2);
                _view.DisplayTestsRun(3);
                _view.DisplayInconclusive(3);
            });
        }

        [Test]
        public void WhenTestCaseFails_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Failed' />");

            for (int i = 1; i <= 3; i++)
                FireTestFinishedEvent(result);

            Received.InOrder(() =>
            {
                _view.DisplayTestsRun(1);
                _view.DisplayFailed(1);
                _view.DisplayTestsRun(2);
                _view.DisplayFailed(2);
                _view.DisplayTestsRun(3);
                _view.DisplayFailed(3);
            });
        }

        [Test]
        public void WhenTestsFinish_DurationIsDisplayed()
        {
            var result = new ResultNode("<test-run duration='1.234' />");
            FireRunFinishedEvent(result);

            _view.Received().DisplayText("Completed");
            _view.Received().DisplayDuration(1.234);
        }
    }
}
