// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

// TODO: Code using TestPropertiesPresenter currently fails. It would be nice
// to make it work as it saves about 100 lines of partially duplicated code.
// Presenter fails when opening the dialog for the second time.
//#define USING_PRESENTER

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestCentric.Gui.Dialogs
{
    using System.Collections.Generic;
    using System.Text;
    using Model;
    using Views;
    using Presenters;
    using System.Linq;
    using System.Xml;

    public partial class TestPropertiesDialog : PinnableDisplay
    {
        private ITestModel _model;
        private ITestTreeView _treeView;

        private TreeNode _treeNode;
        private TestNode _testNode;

        private TestPropertiesView _view;
#if USING_PRESENTER
        private TestPropertiesPresenter _presenter;
#else
        private List<TestPropertiesView.SubView> _visibleSubViews = new List<TestPropertiesView.SubView>();
#endif
        private int _clientWidth;

        public TestPropertiesDialog(ITestModel model, ITestTreeView treeView)
        {
            _model = model;
            _treeView = treeView;

            InitializeComponent();

            _view = testPropertiesView;
#if USING_PRESENTER
            _presenter = new TestPropertiesPresenter(_view, _model);
#else
            _view.Resize += (s, e) => { if (_treeNode != null) Display(_treeNode); };
#endif
        }

        #region Public Methods

        public void Display(TreeNode treeNode)
        {
            _view.Visible = true;

            if (treeNode == null)
                throw new ArgumentNullException(nameof(treeNode));

            _treeNode = treeNode;
            _testNode = treeNode.Tag as TestNode;

            if (_testNode.Type == "Project" || _testNode.Type == "Assembly")
                TestName = Path.GetFileName(_testNode.Name);
            else
                TestName = _testNode.Name;
#if USING_PRESENTER
            // Trigger update of the new display by repeating the selection
            // of the currently selected item.
            _model.ActiveTestItem = _testNode;
#else
            _visibleSubViews.Clear();

            InitializeTestPackageSubView(_testNode);
            InitializeTestPropertiesSubView(_testNode);
            AdjustSubViewHeights();
#endif

            Rectangle screenArea = Screen.GetWorkingArea(this);
            Location = new Point(
                Location.X,
                Math.Max(0, Math.Min(Location.Y, screenArea.Bottom - Height)));

            Show();
        }

        public void OnTestFinished(ResultNode result)
        {
            if (result.Id == _testNode.Id)
                Invoke(new Action(() => Display(_treeNode)));
        }

#if !USING_PRESENTER
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

                _view.PackageSettings = sb.ToString();

                _visibleSubViews.Add(_view.TestPackageSubView);
            }

            _view.TestPackageSubView.Visible = visible;
        }

        private void InitializeTestPropertiesSubView(TestNode testNode)
        {
            _view.TestType = GetTestType(testNode);
            _view.FullName = testNode.FullName;
            _view.Description = testNode.GetProperty("Description");
            _view.Categories = testNode.GetPropertyList("Category");
            _view.TestCount = testNode.TestCount.ToString();
            _view.RunState = testNode.RunState.ToString();
            _view.SkipReason = GetSkipReason(testNode);
            _view.Properties = GetTestProperties(testNode);

            // Always visible
            _visibleSubViews.Add(_view.TestPropertiesSubView);
            _view.TestPropertiesSubView.Visible = true;
        }


        private void AdjustSubViewHeights()
        {
            int endOfSubViews = 0;

            foreach (var subView in _visibleSubViews)
            {
                subView.Height = subView.FullHeight;
                endOfSubViews = subView.Bottom + 8;
            }

            _view.ClientSize = new Size(_view.ClientSize.Width, endOfSubViews + 20);
            this.ClientSize = new Size(this.ClientSize.Width, endOfSubViews + 28);
        }

        private static string GetTestType(TestNode testNode)
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
#endif

        #endregion

        #region Event Handlers and Overrides

        private void TestPropertiesDialog_ResizeEnd(object sender, EventArgs e)
        {
            if (_clientWidth != ClientSize.Width && _treeNode != null)
                Display(_treeNode);

            _clientWidth = ClientSize.Width;
        }

        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            const int ESCAPE = 27;
            const int WM_CHAR = 258;

            if (m.Msg == WM_CHAR && m.WParam.ToInt32() == ESCAPE)
            {
                Close();
                return true;
            }

            return base.ProcessKeyEventArgs(ref m);
        }

        private void hiddenProperties_CheckedChanged(object sender, EventArgs e)
        {
            Display(_treeNode);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Helper Methods

        private static string TrimLeadingBlankLines(string s)
        {
            if (s == null) return s;

            int start = 0;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case ' ':
                    case '\t':
                        break;
                    case '\r':
                    case '\n':
                        start = i + 1;
                        break;

                    default:
                        goto getout;
                }
            }

        getout:
            return start == 0 ? s : s.Substring(start);
        }

        #endregion

    }
}
