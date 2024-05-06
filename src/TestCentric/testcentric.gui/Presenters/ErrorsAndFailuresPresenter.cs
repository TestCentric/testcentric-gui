// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using System.Drawing.Text;
    using System.Xml;
    using Model;
    using Model.Settings;
    using Views;

    public class ErrorsAndFailuresPresenter
    {
        private IErrorsAndFailuresView _view;
        private ITestModel _model;
        private UserSettings _settings;

        private ITestItem _selectedItem;
        private ResultNode _selectedResult;

        public ErrorsAndFailuresPresenter(IErrorsAndFailuresView view, ITestModel model)
        {
            _view = view;
            _model = model;
            _settings = model.Settings;

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

            _model.Events.TestFinished += (TestResultEventArgs e) => OnNewResult(e.Result);
            _model.Events.SuiteFinished += (TestResultEventArgs e) => OnNewResult(e.Result);

            _model.Events.SelectedItemChanged += (TestItemEventArgs e) =>
            {
                _selectedItem = e.TestItem;
                var testNode = e.TestItem as TestNode;
                _selectedResult = testNode == null ? null : _model.GetResultForTest(testNode.Id);

                _view.Header = e.TestItem.Name;
                UpdateDisplay();
            };

            _model.Settings.Changed += (object sender, SettingsEventArgs e) =>
            {
                _view.Font = _settings.Gui.FixedFont;
            };

            // Events that arise in the view

            _view.SplitterPositionChanged += (object sender, EventArgs e) =>
            {
                _model.Settings.Gui.ErrorDisplay.SplitterPosition = _view.SplitterPosition;
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

        private void OnNewResult(ResultNode resultNode)
        {
            if (resultNode.Name == _selectedItem?.Name)
            {
                _selectedResult = resultNode;
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            _view.Clear();

            if (_selectedResult != null &&
                (_selectedResult.Status == TestStatus.Failed || _selectedResult.Status == TestStatus.Warning) &&
                _selectedResult.Site != FailureSite.Parent)
            {
                string status = _selectedResult.Outcome.Label;
                if (string.IsNullOrEmpty(status))
                    status = _selectedResult.Status.ToString();
                string testName = _selectedResult.FullName;

                if (_selectedResult.IsSuite && _selectedResult.Site == FailureSite.SetUp)
                    testName += " (TestFixtureSetUp)";

                if (_selectedResult.Assertions.Count > 0)
                    foreach (var assertion in _selectedResult.Assertions)
                        AddResult(testName, assertion);
                else
                    AddResult(_selectedResult);
            }
        }

        private void AddResult(string testName, AssertionResult assertion)
        {
            string status = assertion.Status;
            if (string.IsNullOrEmpty(status))
                status = GetStatusDisplay(_selectedResult);
            _view.AddResult(status, testName, IndentMessage(assertion.Message), assertion.StackTrace);
        }

        private void AddResult(ResultNode resultNode)
        {
            _view.AddResult(GetStatusDisplay(resultNode), resultNode.FullName, IndentMessage(resultNode.Message), resultNode.StackTrace);
        }

        private string GetStatusDisplay(ResultNode resultNode)
        {
            string status = _selectedResult.Outcome.Label;
            if (string.IsNullOrEmpty(status))
                status = _selectedResult.Status.ToString();
            return status;
        }

        private string IndentMessage(string message)
        {
            return message.StartsWith("  ") ? message : "  " + message;
        }
    }
}
