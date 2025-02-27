// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using System.IO;
    using System.Text;
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

            _view.SetFixedFont(_settings.Gui.FixedFont);
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
                if (e.SettingName == "TestCentric.Gui.FixedFont")
                    _view.SetFixedFont(_settings.Gui.FixedFont);
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
                else if (_selectedResult.TestCount == 1)
                    // Add result to detail list only for single test cases: avoid displaying general message "One or more child tests had errors"
                    AddResult(_selectedResult);
            }

            InitializeTestResultSubView();
            InitializeTestOutputSubView();
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
            return message == null ? null : message.StartsWith("  ") ? message : "  " + message;
        }

        private void InitializeTestResultSubView()
        {
            _view.TestResultSubView.Outcome = (_selectedResult != null) ? _selectedResult.Outcome.ToString() : "";
            _view.TestResultSubView.ElapsedTime = (_selectedResult != null) ? _selectedResult.Duration.ToString("f3") : "";
            _view.TestResultSubView.AssertCount = (_selectedResult != null) ? _selectedResult.AssertCount.ToString() : "";
            _view.TestResultSubView.Assertions = (_selectedResult != null) ? GetAssertionResults(_selectedResult) : "";
        }

        private void InitializeTestOutputSubView()
        {
            string testOutput = (_selectedResult != null) ? _selectedResult.Xml.SelectSingleNode("output")?.InnerText : "";
            _view.TestOutputSubView.Output = testOutput;
            _view.TestOutputSubView.SetVisibility(!string.IsNullOrEmpty(testOutput));
        }

        private string GetAssertionResults(ResultNode resultNode)
        {
            var assertionResults = resultNode.Assertions;

            // If there were no actual assertionresult entries, we fake
            // one if there is a message to display
            if (assertionResults.Count == 0)
            {
                if (resultNode.Outcome.Status == TestStatus.Failed)
                {
                    string status = resultNode.Outcome.Label ?? "Failed";
                    XmlNode failure = resultNode.Xml.SelectSingleNode("failure");
                    if (failure != null)
                        assertionResults.Add(new AssertionResult(failure, status));
                }
                else
                {
                    string status = resultNode.Outcome.Label ?? "Skipped";
                    XmlNode reason = resultNode.Xml.SelectSingleNode("reason");
                    if (reason != null)
                        assertionResults.Add(new AssertionResult(reason, status));
                }
            }

            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (var assertion in assertionResults)
            {
                sb.AppendLine($"{++index}) {assertion.Status.ToUpper()} {assertion.Message}");
                if (assertion.StackTrace != null)
                    sb.AppendLine(AdjustStackTrace(assertion.StackTrace));

            }

            return sb.ToString();
        }

        // Some versions of the framework return the stacktrace
        // without leading spaces, so we add them if needed.
        // TODO: Make sure this is valid across various cultures.
        private const string LEADING_SPACES = "   ";

        private static string AdjustStackTrace(string stackTrace)
        {
            // Check if no adjustment needed. We assume that all
            // lines start the same - either with or without spaces.
            if (stackTrace.StartsWith(LEADING_SPACES))
                return stackTrace;

            var sr = new StringReader(stackTrace);
            var sb = new StringBuilder();
            string line = sr.ReadLine();
            while (line != null)
            {
                sb.Append(LEADING_SPACES);
                sb.AppendLine(line);
                line = sr.ReadLine();
            }

            return sb.ToString();
        }
    }
}
