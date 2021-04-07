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

    public class ProgressBarPresenterTests : PresenterTestBase<IProgressBarView>
    {
        [SetUp]
        public void CreatePresenter()
        {
            new ProgressBarPresenter(_view, _model);
        }

        [Test]
        public void WhenTestsAreLoaded_ProgressBar_IsInitialized()
        {
            var testNode = new TestNode("<test-suite id='1' testcasecount='1234'/>");
            FireTestLoadedEvent(testNode);

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestsAreUnloaded_ProgressBar_IsInitialized()
        {
            FireTestUnloadedEvent();

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestsAreReloaded_ProgressBar_IsInitialized()
        {
            var testNode = new TestNode("<test-suite id='1' testcasecount='1234'/>");
            FireTestReloadedEvent(testNode);

            _view.Received().Initialize(100);
        }

        [Test]
        public void WhenTestRunBegins_ProgressBar_IsInitialized()
        {
            FireRunStartingEvent(1234);

            _view.Received().Initialize(1234);
        }

        [Test]
        public void WhenTestCaseCompletes_ProgressIsIncremented()
        {
            var result = new ResultNode("<test-case id='1'/>");

            FireTestFinishedEvent(result);

            _view.Received().Progress++;
        }

        [Test]
        public void WhenTestSuiteCompletes_ProgressIsNotIncremented()
        {
            var result = new ResultNode("<test-suite id='1'/>");

            FireSuiteFinishedEvent(result);

            _view.DidNotReceive().Progress++;
        }

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

            FireTestFinishedEvent("SomeTest", resultState.ToString());

            Assert.That(_view.Status, Is.EqualTo(expectedStatus));
        }
    }
}
