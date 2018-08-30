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

using System;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Model.Settings;
    using Views;

    public class ErrorsAndFailuresPresenter
    {
        private IErrorsAndFailuresView _view;
        private ITestModel _model;
        private UserSettings _settings;

        public ErrorsAndFailuresPresenter(IErrorsAndFailuresView view, ITestModel model)
        {
            _view = view;
            _model = model;
            _settings = model.Services.UserSettings;

            _view.WordWrap = _settings.Gui.ErrorDisplay.WordWrapEnabled;
            _view.Font = _settings.Gui.FixedFont;
            _view.SplitterPosition = _settings.Gui.ErrorDisplay.SplitterPosition;

            var orientation = _settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation;
            _view.SourceCodeSplitOrientation = orientation;
            _view.SourceCodeSplitterDistance = orientation == Orientation.Vertical
                ? _settings.Gui.ErrorDisplay.SourceCodeVerticalSplitterPosition
                : _settings.Gui.ErrorDisplay.SourceCodeHorizontalSplitterPosition;

            _view.SourceCodeDisplay = _settings.Gui.ErrorDisplay.SourceCodeDisplay;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            // Events that arise in the model

            _model.Events.TestLoaded += ((TestNodeEventArgs e) =>
            {
                _view.Clear();
            });

            _model.Events.TestUnloaded += ((TestEventArgs e) =>
            {
                _view.Clear();
            });

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (_settings.Gui.ClearResultsOnReload)
                    _view.Clear();
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                _view.Clear();
            };

            _model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Status == TestStatus.Failed || e.Result.Status == TestStatus.Warning)
                    if (e.Result.Site != FailureSite.Parent)
                        AddResult(e.Result);
            };

            _model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Status == TestStatus.Failed || e.Result.Status == TestStatus.Warning)
                    if (e.Result.Site != FailureSite.Parent && e.Result.Site != FailureSite.Child)
                        AddResult(e.Result);
            };

            _model.Services.UserSettings.Changed += (object sender, SettingsEventArgs e) =>
            {
                _view.WordWrap = _settings.Gui.ErrorDisplay.WordWrapEnabled;
                _view.Font = _settings.Gui.FixedFont;
            };

            // Events that arise in the view

            _view.SplitterPositionChanged += (object sender, EventArgs e) =>
            {
                _model.Services.UserSettings.Gui.ErrorDisplay.SplitterPosition = _view.SplitterPosition;
            };

            _view.SourceCodeSplitOrientationChanged += (object sender, EventArgs e) =>
            {
                _settings.Gui.ErrorDisplay.SourceCodeSplitterOrientation = _view.SourceCodeSplitOrientation;
            };

            _view.SourceCodeSplitterDistanceChanged += (object sender, EventArgs e) =>
            {
                if (_view.SourceCodeSplitOrientation == Orientation.Vertical)
                    _settings.Gui.ErrorDisplay.SourceCodeVerticalSplitterPosition = _view.SourceCodeSplitterDistance;
                else
                    _settings.Gui.ErrorDisplay.SourceCodeHorizontalSplitterPosition = _view.SourceCodeSplitterDistance;
            };

            _view.SourceCodeDisplayChanged += (object sender, EventArgs e) =>
            {
                _settings.Gui.ErrorDisplay.SourceCodeDisplay = _view.SourceCodeDisplay;
            };
        }

        private void AddResult(ResultNode result)
        {
            var testName = result.FullName;
            var message = result.Message;
            var stackTrace = result.StackTrace;

            if (result.IsSuite && result.Site == FailureSite.SetUp)
                testName += " (TestFixtureSetUp)";

            _view.AddResult(testName, message, stackTrace);
        }
    }
}
