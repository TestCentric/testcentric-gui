// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;
using TestCentric.Engine;

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

            _view.Font = _settings.Gui.FixedFont;
            _view.SplitterPosition = _settings.Gui.ErrorDisplay.SplitterPosition;
            _view.EnableToolTips = _settings.Gui.ErrorDisplay.ToolTipsEnabled;

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

            _model.Events.TestLoaded += (e) => _view.Clear();

            _model.Events.TestUnloaded += (e) => _view.Clear();

            _model.Events.TestReloaded += (e) =>
            {
                if (_settings.Gui.ClearResultsOnReload)
                    _view.Clear();
            };

            _model.Events.RunStarting += (e) => _view.Clear();

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
