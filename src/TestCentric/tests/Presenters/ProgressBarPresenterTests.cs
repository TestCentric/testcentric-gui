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

using System.Xml;
using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Controls;

namespace TestCentric.Gui.Presenters
{
    using Views;
    using Model;

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
