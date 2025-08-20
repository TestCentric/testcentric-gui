// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
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

            _view.Received().Initialize();
            _view.Received().Text = "Ready";
        }

        [Test]
        public void WhenTestsArReloaded_StatusBar_IsInitialized()
        {
            FireTestReloadedEvent(new TestNode("<test-run id='2' testcasecount='123' />"));

            _view.Received().Initialize();
            _view.Received().Text = "Reloaded";
        }

        [Test]
        public void WhenTestsAreUnloaded_StatusBar_IsInitialized()
        {
            FireTestUnloadedEvent();

            _view.Received().Initialize();
            _view.Received().Text = "Unloaded";
        }

        [Test]
        public void WhenTestRunBegins_StatusBar_IsInitialized()
        {
            FireRunStartingEvent(1234);

            _view.Received().Initialize();
        }

        [Test]
        public void WhenTestBegins_NameIsDisplayed()
        {
            FireTestStartingEvent("TestName");

            _view.Received().Text = "TestName";
        }

        [Test]
        public void WhenTestCasePasses_CountIsIncremented()
        {
            var result = new ResultNode("<test-case id='1' result='Passed' />");

            for (int i = 1; i <= 3; i++)
                FireTestFinishedEvent(result);

            Received.InOrder(() =>
            {
                _view.Passed = 1;
                _view.Passed = 2;
                _view.Passed = 3;
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
                _view.Warnings = 1;
                _view.Warnings = 2;
                _view.Warnings = 3;
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
                _view.Inconclusive = 1;
                _view.Inconclusive = 2;
                _view.Inconclusive = 3;
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
                _view.Failed = 1;
                _view.Failed = 2;
                _view.Failed = 3;
            });
        }

        [Test]
        public void WhenTestsFinish_DurationIsDisplayed()
        {
            var result = new ResultNode("<test-run duration='1.234' />");
            FireRunFinishedEvent(result);

            _view.Received().Text = "Completed";
            _view.Received().Duration = 1.234;
        }
    }
}
