// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestCentric.Gui.Dialogs
{
    using System.Text;
    using Model;
    using NUnit.Engine;
    using Views;

    public partial class TestPropertiesDialog : Form
    {
        private ITestModel _model;
        private ITestTreeView _view;

        private TreeNode _treeNode;
        private TestNode _testNode;
        private ResultNode _resultNode;
        private TestPackage _package;

        private int _clientWidth;

        private Image _pinnedImage;
        private Image _unpinnedImage;

        public TestPropertiesDialog(ITestModel model, ITestTreeView view)
        {
            _model = model;
            _view = view;

            InitializeComponent();

            _pinnedImage = new Bitmap(GetType().Assembly.GetManifestResourceStream("TestCentric.Gui.Images.pinned.gif"));
            _unpinnedImage = new Bitmap(GetType().Assembly.GetManifestResourceStream("TestCentric.Gui.Images.unpinned.gif"));
            pinButton.Image = _unpinnedImage;
        }

        #region Properties

        [Browsable(false)]
        public bool Pinned
        {
            get { return pinButton.Checked; }
            set { pinButton.Checked = value; }
        }

        #endregion

        #region Public Methods

        public void Display(TreeNode treeNode)
        {
            if (treeNode == null)
                throw new ArgumentNullException(nameof(treeNode));

            _treeNode = treeNode;
            _testNode = (TestNode)treeNode.Tag;
            _resultNode = _model.GetResultForTest(_testNode.Id);
            _package = _model.GetPackageForTest(_testNode.Id);

            testResult.Text = _resultNode?.Outcome.ToString() ?? _testNode.RunState.ToString();
            testResult.Font = new Font(this.Font, FontStyle.Bold);
            if (_testNode.Type == "Project" || _testNode.Type == "Assembly")
                testName.Text = Path.GetFileName(_testNode.Name);
            else
                testName.Text = _testNode.Name;

            // Display each groupBox, for which there is data.
            // Boxes are displayed top-down at the vertical
            // offset
            int verticalOffset = packageGroupBox.Top;

            if (_package != null)
                verticalOffset = DisplayPackageGroupBox(verticalOffset) + 4;
            else
                packageGroupBox.Hide();

            // Test details are always shown
            verticalOffset = DisplayTestGroupBox(verticalOffset) + 4;

            if (_resultNode != null)
                verticalOffset = DisplayResultGroupBox(verticalOffset) + 4;
            else
                resultGroupBox.Hide();

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
            packageGroupBox.Location = new Point(
                packageGroupBox.Location.X, verticalOffset);

            FillPackageSettingsList(_package);
            packageGroupBox.Show();

            return packageGroupBox.Bottom;
        }

        private int DisplayTestGroupBox(int verticalOffset)
        {
            testGroupBox.Location = new Point(
                packageGroupBox.Location.X, verticalOffset);

            testType.Text = _testNode.Type;

            fullName.Text = _testNode.FullName;

            description.Text = _testNode.GetProperty("Description");

            categories.Text = _testNode.GetPropertyList("Category");

            testCaseCount.Text = _testNode.TestCount.ToString();
            switch (_testNode.RunState)
            {
                case RunState.Explicit:
                    shouldRun.Text = "Explicit";
                    break;
                case RunState.Runnable:
                    shouldRun.Text = "Yes";
                    break;
                default:
                    shouldRun.Text = "No";
                    break;
            }

            ignoreReason.Text = _testNode.GetProperty("_SKIPREASON");

            FillPropertyList();

            return testGroupBox.Bottom;
        }

        private int DisplayResultGroupBox(int verticalOffset)
        {
            resultGroupBox.Location = new Point(
                resultGroupBox.Location.X, verticalOffset);

            elapsedTime.Text = string.Format("Execution Time: {0}", _resultNode.Duration);
            assertCount.Text = string.Format("Assert Count: {0}", _resultNode.AssertCount);

            // message may have a leading blank line
            // TODO: take care of this in label ?
            var messageText = _resultNode.Message;
            if (messageText != null)
            {
                if (messageText.Length > 64000)
                    message.Text = TrimLeadingBlankLines(messageText.Substring(0, 64000));
                else
                    message.Text = TrimLeadingBlankLines(messageText);
            }

            stackTrace.Text = _resultNode.StackTrace != null
                ? FormatStackTrace(_resultNode.StackTrace)
                : string.Empty;

            resultGroupBox.Show();

            return resultGroupBox.Bottom;
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
            foreach (string item in _testNode.GetAllProperties(hiddenProperties.Checked))
            {
                if (sb.Length > 0)
                    sb.Append(Environment.NewLine);
                sb.Append(item);
            }
            properties.Text = sb.ToString();
        }

        private void FillPackageSettingsList(TestPackage package)
        {
            var sb = new StringBuilder();
            foreach (var key in package.Settings.Keys)
            {
                if (sb.Length > 0)
                    sb.Append(Environment.NewLine);
                sb.Append($"{key} = {package.Settings[key]}");
            }

            packageSettings.Text = sb.ToString();
        }

        #endregion

        #region Event Handlers and Overrides

        private void pinButton_Click(object sender, System.EventArgs e)
        {
            if (pinButton.Checked)
                pinButton.Image = _pinnedImage;
            else
                pinButton.Image = _unpinnedImage;
        }

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
