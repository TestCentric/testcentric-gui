// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TestCentric.Gui.Presenters
{
    using System.Linq;
    using Model;
    using Views;

    public class TestPropertiesPresenter
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestPropertiesPresenter));

        private readonly ITestPropertiesView _view;
        private readonly ITestModel _model;

        private ITestItem _selectedItem;

        public TestPropertiesPresenter(TestPropertiesView view, ITestModel model)
        {
            _view = view;
            _model = model;

            _view.Visible = false;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += (ea) => _view.Visible = true;
            _model.Events.TestReloaded += (ea) => _view.Visible = true;
            _model.Events.TestUnloaded += (ea) => _view.Visible = false;
            _model.Events.RunFinished += (ea) => DisplaySelectedItem();
            _model.Events.SelectedItemChanged += (ea) => OnSelectedItemChanged(ea.TestItem);
            _view.DisplayHiddenPropertiesChanged += () => DisplaySelectedItem();
            _view.Resize += (s, e) => DisplaySelectedItem();
        }

        private void OnSelectedItemChanged(ITestItem testItem)
        {
            _selectedItem = testItem;
            DisplaySelectedItem();
        }

        private void DisplaySelectedItem()
        {
            // TODO: Insert checks for errors in the XML
            if (_selectedItem != null)
            {
                _view.Header = _selectedItem.Name;

                var testNode = _selectedItem as TestNode;
                if (testNode != null)
                {
                    //_treeView.SuspendLayout();

                    InitializeTestPackageSubView(testNode);
                    InitializeTestPropertiesSubView(testNode);                    
                    InitializeTestResultSubView(testNode);
                    InitializeTestOutputSubView(testNode);

                    AdjustSubViewHeights();

                    //_treeView.ResumeLayout();
                }
            }

            // HACK: results won't display on Linux otherwise
            //if (Path.DirectorySeparatorChar == '/') // Running on Linux or Unix
            //    _treeView.ResultPanelVisible = true;
            //else
            //    _treeView.ResultPanelVisible = resultNode != null;

            // TODO: We should actually try to set the font for bold items
            // dynamically, since the global application font may be changed.
        }

        private void InitializeTestPackageSubView(TestNode testNode)
        {
            var packageSettings = _model.GetPackageSettingsForTest(testNode.Id);
            var visible = packageSettings != null;

            if (visible)
            {
                var sb = new StringBuilder();
                foreach (var key in packageSettings.Keys)
                {
                    if (sb.Length > 0)
                        sb.Append(Environment.NewLine);
                    sb.Append($"{key} = {packageSettings[key]}");
                }

                _view.TestPackageSubView.PackageSettings = sb.ToString();
            }
                
            _view.TestPackageSubView.Visible = visible;
        }

        private void InitializeTestPropertiesSubView(TestNode testNode)
        {
            // TestPropertiesSubView is always visible
            _view.TestType = GetTestType(testNode);
            _view.FullName = testNode.FullName;
            _view.Description = testNode.GetProperty("Description");
            _view.Categories = testNode.GetPropertyList("Category");
            _view.TestCount = testNode.TestCount.ToString();
            _view.RunState = testNode.RunState.ToString();
            _view.SkipReason = GetSkipReason(testNode);
            _view.Properties = GetTestProperties(testNode);
        }

        private void InitializeTestResultSubView(TestNode testNode)
        {
            var resultNode = _model.GetResultForTest(testNode.Id);
            var visible = resultNode != null;

            if (visible)
            {
                _view.Outcome = resultNode.Outcome.ToString();
                _view.ElapsedTime = resultNode.Duration.ToString("f3");
                _view.AssertCount = resultNode.AssertCount.ToString();
                _view.Assertions = GetAssertionResults(resultNode);
            }

            _view.TestResultSubView.Visible = visible;
        }

        private void InitializeTestOutputSubView(TestNode testNode)
        {
            var resultNode = _model.GetResultForTest(testNode.Id);
            var visible = resultNode != null;

            if (visible)
                _view.Output = resultNode.Xml.SelectSingleNode("output")?.InnerText;

            _view.TestOutputSubView.Visible = visible;
        }

        private void AdjustSubViewHeights()
        {
            var visibleSubViews = _view.SubViews.Where(x => x.Visible);

            // Add up the MinHeights of each visible subviow
            int totalMinHeights = 0;
            foreach (var subView in visibleSubViews)
                totalMinHeights += subView.MinimumSize.Height;

            if (totalMinHeights > _view.ClientHeight)
            {
                // Use MinHeight for all sub-views.
                foreach (var subView in visibleSubViews)
                    subView.Height = subView.MinimumSize.Height;
            }
            else 
            {
                // We know that MinHeight will fit but possibly more. Use
                // the excess space for each sub-view until all is used.
                int excessSpace = _view.ClientHeight - totalMinHeights;
                foreach (var subView in visibleSubViews)
                {
                    int iCanUse = Math.Min(excessSpace, subView.FullHeight - subView.MinimumSize.Height);
                   
                    subView.Height = subView.MinimumSize.Height + iCanUse;
                    excessSpace -= iCanUse;
                }
            }
        }

        #region Helper Methods

        public static string GetTestType(TestNode testNode)
        {
            if (testNode.RunState == RunState.NotRunnable
                && testNode.Type == "Assembly"
                && !string.IsNullOrEmpty(testNode.FullName))
            {
                var fi = new FileInfo(testNode.FullName);
                string extension = fi.Extension.ToLower();
                if (extension != ".exe" && extension != ".dll")
                    return "Unknown";
            }
            return testNode.Type;
        }

        private string GetSkipReason(TestNode testNode)
        {
            StringBuilder reason = new StringBuilder(testNode.GetProperty("_SKIPREASON"));

            string message = testNode.Xml.SelectSingleNode("failure/message")?.InnerText;
            if (!string.IsNullOrEmpty(message))
            {
                if (reason.Length > 0)
                    reason.Append("\r\n");
                reason.Append(message);
                string stackTrace = testNode.Xml.SelectSingleNode("failure/stack-trace")?.InnerText;
                if (!string.IsNullOrEmpty(stackTrace))
                    reason.Append($"\r\n{stackTrace}");
            }

            return reason.ToString();
        }

        private string GetTestProperties(TestNode testNode)
        {
            var sb = new StringBuilder();
            foreach (string item in testNode.GetAllProperties(_view.DisplayHiddenProperties))
            {
                if (sb.Length > 0)
                    sb.Append(Environment.NewLine);
                sb.Append(item);
            }
            return sb.ToString();
        }

        // Sometimes, the message may have leading blank lines and/or
        // may be longer than Windows really wants to display.
        private string TrimMessage(string message)
        {
            if (message != null)
            {
                if (message.Length > 64000)
                    message = message.Substring(0, 64000);

                int start = 0;
                for (int i = 0; i < message.Length; i++)
                {
                    switch (message[i])
                    {
                        case ' ':
                        case '\t':
                            break;
                        case '\r':
                        case '\n':
                            start = i + 1;
                            break;

                        default:
                            return start == 0 ? message : message.Substring(start);
                    }
                }
            }

            return message;
        }

        private string GetAssertionResults(ResultNode resultNode)
        {
            StringBuilder sb;
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

            sb = new StringBuilder();
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

        #endregion
    }
}
