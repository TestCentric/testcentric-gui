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

using System.Drawing;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class ErrorsAndFailuresPresenterTests : PresenterTestBase<IErrorsAndFailuresView>
    {
        private static readonly TestNode FAKE_TEST_RUN = new TestNode("<test-suite id='1' testcasecount='1234' />");

        [SetUp]
        public void CreatePresenter()
        {
            new ErrorsAndFailuresPresenter(_view, _model);
        }

        [Test]
        public void WhenTestIsLoaded_DisplayIsCleared()
        {
            FireTestLoadedEvent(FAKE_TEST_RUN);

            _view.Received().Clear();
        }

        [Test]
        public void WhenTestIsReloaded_IfClearResultsIsFalse_DisplayIsNotCleared()
        {
            _settings.Gui.ClearResultsOnReload = false;

            FireTestReloadedEvent(FAKE_TEST_RUN);

            _view.DidNotReceive().Clear();
        }

        [Test]
        public void WhenTestIsReloaded_IfClearResultsIsTrue_DisplayIsCleared()
        {
            _settings.Gui.ClearResultsOnReload = true;

            FireTestReloadedEvent(FAKE_TEST_RUN);

            _view.Received().Clear();
        }

        [Test]
        public void WhenTestIsUnloaded_DisplayIsCleared()
        {
            FireTestUnloadedEvent();

            _view.Received().Clear();
        }

        [Test]
        public void WhenTestRunStarts_DisplayIsCleared()
        {
            FireRunStartingEvent(1234);

            _view.Received().Clear();
        }

        [TestCase("Passed", FailureSite.Test, false)]
        [TestCase("Failed", FailureSite.Test, true)]
        [TestCase("Failed", FailureSite.SetUp, true)]
        [TestCase("Failed", FailureSite.TearDown, true)]
        [TestCase("Failed", FailureSite.Parent, false)]
        [TestCase("Failed:Error", FailureSite.Test, true)]
        [TestCase("Failed:Invalid", FailureSite.Test, true)]
        [TestCase("Failed:Cancelled", FailureSite.Test, true)]
        [TestCase("Warning", FailureSite.Test, true)]
        [TestCase("Warning", FailureSite.SetUp, true)]
        [TestCase("Warning", FailureSite.TearDown, true)]
        [TestCase("Warning", FailureSite.Parent, false)]
        [TestCase("Skipped", FailureSite.Test, false)]
        [TestCase("Skipped:Ignored", FailureSite.Test, false)]
        [TestCase("Inconclusive", FailureSite.Test, false)]
        public void WhenTestCaseFinishes_FailuresAndErrorsAreDisplayed(string resultState, FailureSite site, bool shouldDisplay)
        {
            FireTestFinishedEvent("MyTest", resultState, site);

            VerifyDisplay(shouldDisplay);
        }

        [TestCase("Passed", FailureSite.Test, false)]
        [TestCase("Failed", FailureSite.Parent, false)]
        [TestCase("Failed", FailureSite.SetUp, true)]
        [TestCase("Failed", FailureSite.TearDown, true)]
        [TestCase("Failed", FailureSite.Child, false)]
        [TestCase("Failed", FailureSite.Test, true)]
        [TestCase("Failed:Error", FailureSite.Test, true)]
        [TestCase("Failed:Invalid", FailureSite.Test, true)]
        [TestCase("Failed:Cancelled", FailureSite.Test, true)]
        [TestCase("Warning", FailureSite.Test, true)]
        [TestCase("Warning", FailureSite.SetUp, true)]
        [TestCase("Warning", FailureSite.TearDown, true)]
        [TestCase("Warning", FailureSite.Parent, false)]
        [TestCase("Warning", FailureSite.Child, false)]
        [TestCase("Skipped", FailureSite.Test, false)]
        [TestCase("Skipped:Ignored", FailureSite.Test, false)]
        [TestCase("Inconclusive", FailureSite.Test, false)]
        public void WhenTestSuiteFinishes_FailuresAndErrorsAreDisplayed(string resultState, FailureSite site, bool shouldDisplay)
        {
            FireSuiteFinishedEvent("MyTest", resultState, site);

            VerifyDisplay(shouldDisplay);
        }

        [Test]
        public void WhenPresenterIsCreated_WordWrapIsSetToDefault()
        {
            bool wordwrap = _settings.Gui.ErrorDisplay.WordWrapEnabled;
            _view.Received().WordWrap = wordwrap;
        }

        [Test]
        public void WhenPresenterIsCreated_FontIsSetToDefault()
        {
            var font = _settings.Gui.FixedFont;
            _view.Received().Font = font;
        }

        [Test]
        public void WhenPresenterIsCreated_SplitterPositionIsSet()
        {
            int split = _settings.Gui.ErrorDisplay.SplitterPosition;
            _view.Received().SplitterPosition = split;
        }

        [Test]
        public void WhenPresenterIsCreated_SourceCodeSplitterDistanceIsSet()
        {
            var orientation = _settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation;
            var distance = orientation == Orientation.Vertical
                ? _settings.Gui.ErrorDisplay.SourceCodeVerticalSplitterPosition
                : _settings.Gui.ErrorDisplay.SourceCodeHorizontalSplitterPosition;
            _view.Received().SourceCodeSplitterDistance = distance;
        }

        [Test]
        public void WhenPresenterIsCreated_SourceCodeSplitOrientationIsSet()
        {
            var orientation = _settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation;
            _view.Received().SourceCodeSplitOrientation = orientation;
        }

        [Test]
        public void WhenPresenterIsCreated_SourceCodeDisplayIsSet()
        {
            bool enabled = _settings.Gui.ErrorDisplay.SourceCodeDisplay;
            _view.Received().SourceCodeDisplay = enabled;
        }

        [Test]
        public void WhenPresenterIsCreated_EnableToolTipsIsSet()
        {
            bool enabled = _settings.Gui.ErrorDisplay.ToolTipsEnabled;
            _view.Received().EnableToolTips = enabled;
        }

        [Test]
        public void WhenWordWrapSettingChanges_ViewIsUpdated()
        {
            _view.ClearReceivedCalls();
            _settings.Gui.ErrorDisplay.WordWrapEnabled = true;
            _view.Received().WordWrap = true;
        }

        [Test]
        public void WhenFixedFontSettingChanges_ViewIsUpdated()
        {
            _view.ClearReceivedCalls();
            var newFont = new Font(FontFamily.GenericMonospace, 12.0f);
            _settings.Gui.FixedFont = newFont;
            _view.Received().Font = newFont;
        }

        [Test]
        public void WhenUserChangesSplitterPosition_SettingIsUpdated()
        {
            _view.SplitterPosition = 1234;
            _view.SplitterPositionChanged += Raise.Event<System.EventHandler>(this, new System.EventArgs());

            Assert.That(_settings.Gui.ErrorDisplay.SplitterPosition, Is.EqualTo(1234));
        }

        [Test]
        public void WhenUserChangesSourceCodeSplitOrientation_SettingIsUpdated()
        {
            var orientation = _settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation == Orientation.Vertical
                ? Orientation.Horizontal
                : Orientation.Vertical;

            _view.SourceCodeSplitOrientation = orientation;
            _view.SourceCodeSplitOrientationChanged += Raise.Event<System.EventHandler>(this, new System.EventArgs());

            Assert.That(_settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation, Is.EqualTo(orientation));
        }

        [Test]
        public void WhenUserChangesSourceCodeHorizontalSplitterDistance_SettingIsUpdated()
        {
            _view.SourceCodeSplitOrientation.Returns(Orientation.Horizontal);
            _view.SourceCodeSplitterDistance.Returns(0.35f);
            _view.SourceCodeSplitterDistanceChanged += Raise.Event<System.EventHandler>(this, new System.EventArgs());

            Assert.That(_settings.Gui.ErrorDisplay.SourceCodeHorizontalSplitterPosition, Is.EqualTo(0.35f));
        }

        [Test]
        public void WhenUserChangesSourceCodeVerticalSplitterDistance_SettingIsUpdated()
        {
            _view.SourceCodeSplitOrientation.Returns(Orientation.Vertical);
            _view.SourceCodeSplitterDistance.Returns(0.5f);
            _view.SourceCodeSplitterDistanceChanged += Raise.Event<System.EventHandler>(this, new System.EventArgs());

            Assert.That(_settings.Gui.ErrorDisplay.SourceCodeVerticalSplitterPosition, Is.EqualTo(0.5f));
        }

        [Test]
        public void WhenUserChangesSourceCodeDisplay_SettingIsUpdated()
        {
            bool originalSetting = _settings.Gui.ErrorDisplay.SourceCodeDisplay;
            bool newSetting = !originalSetting;

            _view.SourceCodeDisplay.Returns(newSetting);
            _view.SourceCodeDisplayChanged += Raise.Event<System.EventHandler>(this, new System.EventArgs());

            Assert.That(_settings.Gui.ErrorDisplay.SourceCodeDisplay, Is.EqualTo(newSetting));
        }

        private void VerifyDisplay(bool shouldDisplay)
        {
            // NOTE: We only verify that something was sent, not the content
            if (shouldDisplay)
                _view.Received().AddResult(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            else
                _view.DidNotReceiveWithAnyArgs().AddResult(null, null, null);
        }
    }
}
