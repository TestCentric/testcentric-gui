// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

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

    public partial class TestPropertiesDisplay : PinnableDisplay
    {
        private ITestModel _model;
        private ITestTreeView _view;

        private TreeNode _treeNode;
        private TestNode _testNode;
        private ResultNode _resultNode;
        private IDictionary<string, object> _packageSettings;

        private int _clientWidth;

        public TestPropertiesDisplay(ITestModel model, ITestTreeView view)
        {
            _model = model;
            _view = view;

            InitializeComponent();
        }

        #region Public Methods

        public void Display(TreeNode treeNode)
        {
            if (treeNode == null)
                throw new ArgumentNullException(nameof(treeNode));

            _treeNode = treeNode;
            _testNode = treeNode.Tag as TestNode;
            _resultNode = _model.GetResultForTest(_testNode.Id);
            _packageSettings = _model.GetPackageSettingsForTest(_testNode.Id);

            outcome.Text = _resultNode?.Outcome.ToString() ?? _testNode.RunState.ToString();
            outcome.Font = new Font(this.Font, FontStyle.Bold);
            if (_testNode.Type == "Project" || _testNode.Type == "Assembly")
                TestName = Path.GetFileName(_testNode.Name);
            else
                TestName = _testNode.Name;

            // Display each groupBox, for which there is data.
            // Boxes are displayed top-down at the vertical
            // offset
            int verticalOffset = packagePanel.Top;

            if (_packageSettings != null)
                verticalOffset = DisplayPackageGroupBox(verticalOffset) + 4;
            else
                packagePanel.Hide();

            // Test details are always shown
            verticalOffset = DisplayTestGroupBox(verticalOffset) + 4;

            if (_resultNode != null)
                verticalOffset = DisplayResultGroupBox(verticalOffset) + 4;
            else
                resultPanel.Hide();

            ClientSize = new Size(
                ClientSize.Width, verticalOffset);

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

        private int DisplayPackageGroupBox(int verticalOffset)
        {
            packagePanel.Location = new Point(
                packagePanel.Location.X, verticalOffset);

            FillPackageSettingsList(_packageSettings);
            packagePanel.Show();

            return packagePanel.Bottom;
        }

        private int DisplayTestGroupBox(int verticalOffset)
        {
            testPanel.Location = new Point(
                packagePanel.Location.X, verticalOffset);

            testType.Text = _testNode.Type;

            fullName.Text = _testNode.FullName;

            description.Text = _testNode.GetProperty("Description");

            categories.Text = _testNode.GetPropertyList("Category");

            testCaseCount.Text = _testNode.TestCount.ToString();
            runState.Text = _testNode.RunState.ToString();
            //switch (_testNode.RunState)
            //{
            //    case RunState.Explicit:
            //        runState.Text = "Explicit";
            //        break;
            //    case RunState.Runnable:
            //        runState.Text = "Yes";
            //        break;
            //    default:
            //        runState.Text = "No";
            //        break;
            //}

            skipReason.Text = _testNode.GetProperty("_SKIPREASON");

            FillPropertyList();

            return testPanel.Bottom;
        }

        private int DisplayResultGroupBox(int verticalOffset)
        {
            resultPanel.Location = new Point(
                resultPanel.Location.X, verticalOffset);

            elapsedTime.Text = _resultNode.Duration.ToString("f3");
            assertCount.Text = _resultNode.AssertCount.ToString();

            var messageText = _resultNode.Message ?? string.Empty;
            // message may have a leading blank line
            // TODO: take care of this in label ?
            if (messageText.Length > 64000)
                message.Text = TrimLeadingBlankLines(messageText.Substring(0, 64000));
            else
                message.Text = TrimLeadingBlankLines(messageText);

            stackTrace.Text = _resultNode.StackTrace != null
                ? FormatStackTrace(_resultNode.StackTrace)
                : string.Empty;

            resultPanel.Show();

            return resultPanel.Bottom;
        }

        private static string FormatStackTrace(string stackTrace)
        {
            const char SP = ' ';
            const char NBSP = (char)160;

            var rdr = new StringReader(stackTrace);
            var line = rdr.ReadLine();
            var sb = new StringBuilder();

            while (line != null)
            {
                sb.AppendLine(line.Trim().Replace(SP, NBSP));
                line = rdr.ReadLine();
            }

            return sb.ToString();
        }

        private void FillPropertyList()
        {
            var sb = new StringBuilder();
            foreach (string item in _testNode.GetAllProperties(displayHiddenProperties.Checked))
            {
                if (sb.Length > 0)
                    sb.Append(Environment.NewLine);
                sb.Append(item);
            }
            properties.Text = sb.ToString();
        }

        private void FillPackageSettingsList(IDictionary<string, object> settings)
        {
            var sb = new StringBuilder();
            foreach (var key in settings.Keys)
            {
                if (sb.Length > 0)
                    sb.Append(Environment.NewLine);
                sb.Append($"{key} = {settings[key]}");
            }

            packageSettings.Text = sb.ToString();
        }

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
            FillPropertyList();
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
