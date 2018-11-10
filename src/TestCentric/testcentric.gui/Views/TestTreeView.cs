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
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace TestCentric.Gui.Views
{
    using Model;
    using Elements;

    /// <summary>
    /// TestTreeView contains the tree control that displays tests.
    /// </summary>
    public partial class TestTreeView : UserControlView, ITestTreeView
    {
        #region Instance Variables

        /// <summary>
        /// The properties dialog if displayed
        /// </summary>
        private TestPropertiesDialog _propertiesDialog;

        private string _alternateImageSet;
        private TestNodeFilter _treeFilter = TestNodeFilter.Empty;

        #endregion

        #region Constructor

        public TestTreeView()
        {
            InitializeComponent();

            RunCommand = new MenuCommand(runMenuItem);
            ShowCheckBoxes = new CheckedMenuItem(showCheckBoxesMenuItem);
            ShowFailedAssumptions = new CheckedMenuItem(failedAssumptionsMenuItem);
            PropertiesCommand = new MenuCommand(propertiesMenuItem);
            ClearAllCheckBoxes = new ButtonElement(clearAllButton);
            CheckFailedTests = new ButtonElement(checkFailedButton);
            Tree = new TreeViewElement(tree);

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            tree.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextNode = tree.GetNodeAt(e.X, e.Y) as TestSuiteTreeNode;
                }
            };

            tree.AfterSelect += (s, e) =>
            {
                if (_propertiesDialog != null)
                {
                    if (_propertiesDialog.Pinned)
                    {
                        _propertiesDialog.DisplayProperties((TestSuiteTreeNode)e.Node);
                    }
                    else
                        _propertiesDialog.Close();
                }
            };

            tree.DragDrop += (s, e) =>
            {
                if (IsValidFileDrop(e.Data))
                    FileDrop?.Invoke((string[])e.Data.GetData(DataFormats.FileDrop));
            };

            tree.DragEnter += (s, e) =>
            {
                e.Effect = IsValidFileDrop(e.Data)
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
            };

            treeMenu.Popup += (s, e) =>
            {
                TestSuiteTreeNode targetNode = ContextNode ?? (TestSuiteTreeNode)tree.SelectedNode;
                TestSuiteTreeNode theoryNode = targetNode?.GetTheoryNode();


                runMenuItem.DefaultItem = runMenuItem.Enabled && targetNode != null && targetNode.Included &&
                        (targetNode.Test.RunState == RunState.Runnable || targetNode.Test.RunState == RunState.Explicit);

                showCheckBoxesMenuItem.Checked = tree.CheckBoxes;

                //failedAssumptionsMenuItem.Visible = 
                failedAssumptionsMenuItem.Enabled = theoryNode != null;
                failedAssumptionsMenuItem.Checked = theoryNode?.ShowFailedAssumptions ?? false;

                propertiesMenuItem.Enabled = targetNode != null;
            };

            treeMenu.Collapse += (s, e) => ContextNode = null;
        }

        #endregion

        #region ITestTreeView Implementation

        public event FileDropEventHandler FileDrop;

        public ICommand RunCommand { get; private set; }
        public IChecked ShowFailedAssumptions { get; private set; }
        public IChecked ShowCheckBoxes { get; private set; }
        public ICommand PropertiesCommand { get; private set; }

        [Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether checkboxes are displayed beside test nodes")]
        public bool CheckBoxes
        {
            get => tree.CheckBoxes;
            set
            {
                tree.CheckBoxes = value;
                buttonPanel.Visible = value;
                clearAllButton.Visible = value;
                checkFailedButton.Visible = value;
            }
        }

        public ICommand ClearAllCheckBoxes { get; private set; }
        public ICommand CheckFailedTests { get; private set; }

        [Browsable(false)]
        public string AlternateImageSet
        {
            get { return _alternateImageSet; }
            set
            {
                _alternateImageSet = value;
                if (!string.IsNullOrEmpty(value))
                    LoadAlternateImages(value);
            }
        }

        [Browsable(false)]
        public ITreeView Tree { get; private set; }

        [Browsable(false)]
        public TestSuiteTreeNode ContextNode { get; private set; }

        [Browsable(false)]
        public TestNode[] SelectedTests
        {
            get
            {
                TestNode[] result = null;

                if (tree.CheckBoxes)
                {
                    var finder = new CheckedTestFinder(tree);
                    result = finder.GetCheckedTests(
                        CheckedTestFinder.SelectionFlags.Top | CheckedTestFinder.SelectionFlags.Explicit);
                }

                if (result == null || result.Length == 0)
                    if (tree.SelectedNode != null)
                    result = new TestNode[] { ((TestSuiteTreeNode)tree.SelectedNode).Test };

                return result;
            }
        }

        public void Clear()
        {
            tree.Nodes.Clear();
        }

        public void ShowPropertiesDialog(TestSuiteTreeNode node)
        {
            TestPropertiesDialog.DisplayProperties(node);
        }

        public void ClosePropertiesDialog()
        {
            if (_propertiesDialog != null)
                _propertiesDialog.Close();
        }

        public void CheckPropertiesDialog()
        {
            if (_propertiesDialog != null && !_propertiesDialog.Pinned)
                _propertiesDialog.Close();
        }

        #endregion

        #region Other Public Properties

        public TestNode[] FailedTests
        {
            get
            {
                var visitor = new FailedTestsFilterVisitor();
                Accept(visitor);
                return visitor.Tests;
            }
        }

        public TestNodeFilter TreeFilter
        {
            get => _treeFilter;
            set => Accept(new TestFilterVisitor(_treeFilter = value));
        }

        #endregion

        #region Other Public Methods

        public void ExpandAll()
        {
            tree.BeginUpdate();
            tree.ExpandAll();
            tree.EndUpdate();
        }

        public void CollapseAll()
        {
            tree.BeginUpdate();
            tree.CollapseAll();
            tree.EndUpdate();
        }

        public void HideTests()
        {
            tree.BeginUpdate();

            foreach (TestSuiteTreeNode node in tree.Nodes)
                HideTestsUnderNode(node);

            tree.EndUpdate();
        }

        #endregion

        #region Private Properties

        private TestPropertiesDialog TestPropertiesDialog
        {
            get
            {
                if (_propertiesDialog == null)
                {
                    Form owner = FindForm();
                    _propertiesDialog = new TestPropertiesDialog()
                    {
                        Owner = owner,
                        Font = owner.Font,
                        StartPosition = FormStartPosition.Manual
                    };

                    _propertiesDialog.Left = Math.Max(0, owner.Left + (owner.Width - _propertiesDialog.Width) / 2);
                    _propertiesDialog.Top = Math.Max(0, owner.Top + (owner.Height - _propertiesDialog.Height) / 2);
                    _propertiesDialog.Closed += (s, e) => _propertiesDialog = null;
                }

                return _propertiesDialog;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper collapses all fixtures under a node
        /// </summary>
        /// <param name="node">Node under which to collapse fixtures</param>
        private void HideTestsUnderNode(TestSuiteTreeNode node)
        {
            if (node.Test.IsSuite)
            {
                if (node.Test.Type == "TestFixture")
                    node.Collapse();
                else
                {
                    node.Expand();

                    foreach (TestSuiteTreeNode child in node.Nodes)
                        HideTestsUnderNode(child);
                }
            }
        }

		private void Accept(TestSuiteTreeNodeVisitor visitor)
        {
            foreach (TestSuiteTreeNode node in tree.Nodes)
            {
                node.Accept(visitor);
            }
        }

		public void LoadAlternateImages(string imageSet)
        {
            string[] imageNames = { "Skipped", "Failure", "Success", "Ignored", "Inconclusive" };

            string imageDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("Images", Path.Combine("Tree", imageSet)));

            for (int index = 0; index < imageNames.Length; index++)
                LoadAlternateImage(index, imageNames[index], imageDir);
            this.Invalidate();
            this.Refresh();
        }

        private void LoadAlternateImage(int index, string name, string imageDir)
        {
            string[] extensions = { ".png", ".jpg" };

            foreach (string ext in extensions)
            {
                string filePath = Path.Combine(imageDir, name + ext);
                if (File.Exists(filePath))
                {
                    treeImages.Images[index] = Image.FromFile(filePath);
                    break;
                }
            }
        }

		/// <summary>
        /// Helper method to determine if an IDataObject is valid
        /// for dropping on the tree view. It must be a the drop
        /// of a single file with a valid assembly file type.
        /// </summary>
        /// <param name="data">IDataObject to be tested</param>
        /// <returns>True if dropping is allowed</returns>
        private static bool IsValidFileDrop(IDataObject data)
        {
            if (!data.GetDataPresent(DataFormats.FileDrop))
                return false;

            string[] fileNames = data.GetData(DataFormats.FileDrop) as string[];

            if (fileNames == null || fileNames.Length == 0)
                return false;

            // Multiple assemblies are allowed - we
            // assume they are all in the same directory
            // since they are being dragged together.
            foreach (string fileName in fileNames)
            {
                //if ( !PathUtils.IsAssemblyFileType( fileName ) )
                //  return false;
            }

            return true;
        }

        #endregion

        #region FailedTestsFilterVisitor

        internal class FailedTestsFilterVisitor : TestSuiteTreeNodeVisitor
        {
            List<TestNode> tests = new List<TestNode>();

            public TestNode[] Tests
            {
                get { return tests.ToArray(); }
            }

            public override void Visit(TestSuiteTreeNode node)
            {
                if (!node.Test.IsSuite && node.HasResult && node.Result.Outcome.Status == TestStatus.Failed)
                {
                    tests.Add(node.Test);
                }
            }
        }

        #endregion

        #region TestFilterVisitor

        public class TestFilterVisitor : TestSuiteTreeNodeVisitor
        {
            private TestNodeFilter filter;

            public TestFilterVisitor(TestNodeFilter filter)
            {
                this.filter = filter;
            }

            public override void Visit(TestSuiteTreeNode node)
            {
                node.Included = filter.Pass(node.Test);
            }
        }

        #endregion

        #region CheckedTestFinder

        internal class CheckedTestFinder
        {
            [Flags]
            public enum SelectionFlags
            {
                Top = 1,
                Sub = 2,
                Explicit = 4,
                All = Top + Sub
            }

            private List<CheckedTestInfo> checkedTests = new List<CheckedTestInfo>();
            private struct CheckedTestInfo
            {
                public TestNode Test;
                public bool TopLevel;

                public CheckedTestInfo(TestNode test, bool topLevel)
                {
                    this.Test = test;
                    this.TopLevel = topLevel;
                }
            }

            public TestNode[] GetCheckedTests(SelectionFlags flags)
            {
                int count = 0;
                foreach (CheckedTestInfo info in checkedTests)
                    if (isSelected(info, flags)) count++;

                TestNode[] result = new TestNode[count];

                int index = 0;
                foreach (CheckedTestInfo info in checkedTests)
                    if (isSelected(info, flags))
                        result[index++] = info.Test;

                return result;
            }

            private bool isSelected(CheckedTestInfo info, SelectionFlags flags)
            {
                if (info.TopLevel && (flags & SelectionFlags.Top) != 0)
                    return true;
                else if (!info.TopLevel && (flags & SelectionFlags.Sub) != 0)
                    return true;
                else if (info.Test.RunState == RunState.Explicit && (flags & SelectionFlags.Explicit) != 0)
                    return true;
                else
                    return false;
            }

            public CheckedTestFinder(TreeView treeView)
            {
                FindCheckedNodes(treeView.Nodes, true);
            }

            private void FindCheckedNodes(TestSuiteTreeNode node, bool topLevel)
            {
                if (node.Checked)
                {
                    checkedTests.Add(new CheckedTestInfo(node.Test, topLevel));
                    topLevel = false;
                }

                FindCheckedNodes(node.Nodes, topLevel);
            }

            private void FindCheckedNodes(TreeNodeCollection nodes, bool topLevel)
            {
                foreach (TestSuiteTreeNode node in nodes)
                    FindCheckedNodes(node, topLevel);
            }
        }

        #endregion
    }
}
