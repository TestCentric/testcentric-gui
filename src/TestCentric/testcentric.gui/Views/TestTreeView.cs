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
        /// Hashtable provides direct access to TestNodes
        /// </summary>
		private Hashtable _treeMap = new Hashtable();

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
                TestSuiteTreeNode targetNode = ContextNode ?? (TestSuiteTreeNode)SelectedNode;
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
            get { return tree.CheckBoxes; }
            set
            {
                if (tree.CheckBoxes != value)
                {
                    // When turning off checkboxes with a non-empty tree, the
                    // structure of what is expanded and collapsed is lost.
                    // We save that structure as a VisualState and then restore it.
                    VisualState visualState = !value && TopNode != null
                        ? GetVisualState()
                        : null;

                    EnableCheckBoxes(value);

                    if (visualState != null)
                    {
                        visualState.ShowCheckBoxes = this.CheckBoxes;
                        RestoreVisualState(visualState);
                    }
                }
            }
        }

        private void EnableCheckBoxes(bool enable)
        {
            tree.CheckBoxes = enable;
            buttonPanel.Visible = enable;
            clearAllButton.Visible = enable;
            checkFailedButton.Visible = enable;
        }

        [Browsable(false)]
        public DisplayStyle DisplayStyle { get; set; }

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
        public TreeNodeCollection Nodes => tree.Nodes;

        [Browsable(false)]
        public TreeNode TopNode
        {
            get => tree.TopNode;
            set => tree.TopNode = value;
        }

        [Browsable(false)]
        public TestSuiteTreeNode ContextNode { get; private set; }

        [Browsable(false)]
        public TreeNode SelectedNode
        {
            get => tree.SelectedNode;
            set => tree.SelectedNode = value;
        }

        /// <summary>
        /// The currently selected test.
        /// </summary>
        [Browsable(false)]
        public TestNode SelectedTest => ((TestSuiteTreeNode)tree.SelectedNode)?.Test;


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
                    if (SelectedTest != null)
                        result = new TestNode[] { SelectedTest };

                return result;
            }
        }

        /// <summary>
        /// Clear all the info in the tree.
        /// </summary>
        public void Clear()
        {
            _treeMap.Clear();
            Nodes.Clear();
        }

        /// <summary>
        /// Reload the tree with a changed test hierarchy
        /// while maintaining as much gui state as possible.
        /// </summary>
        /// <param name="test">Test suite to be loaded</param>
        public void Reload(TestNode test)
        {
            InvokeIfRequired(() =>
            {
                VisualState visualState = GetVisualState();
                LoadTests(test);
                RestoreVisualState(visualState);
            });
        }

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

            foreach (TestSuiteTreeNode node in Nodes)
                HideTestsUnderNode(node);

            tree.EndUpdate();
        }

        public VisualState GetVisualState()
        {
            var visualState = new VisualState()
            {
                ShowCheckBoxes = tree.CheckBoxes,
                TopNode = ((TestSuiteTreeNode)tree.TopNode).Test.Id,
                SelectedNode = ((TestSuiteTreeNode)tree.SelectedNode).Test.Id,
            };

            foreach (TestSuiteTreeNode node in tree.Nodes)
                visualState.ProcessTreeNodes(node);

            return visualState;
        }

        public void RestoreVisualState(VisualState visualState)
        {
            EnableCheckBoxes(visualState.ShowCheckBoxes);

            foreach (VisualTreeNode visualNode in visualState.Nodes)
            {
                TestSuiteTreeNode treeNode = this[visualNode.Id];
                if (treeNode != null)
                {
                    if (treeNode.IsExpanded != visualNode.Expanded)
                        treeNode.Toggle();

                    treeNode.Checked = visualNode.Checked;
                }
            }

            if (visualState.SelectedNode != null)
            {
                TestSuiteTreeNode treeNode = this[visualState.SelectedNode];
                if (treeNode != null)
                    this.SelectedNode = treeNode;
            }

            if (visualState.TopNode != null)
            {
                TestSuiteTreeNode treeNode = this[visualState.TopNode];
                if (treeNode != null)
                    this.TopNode = treeNode;
            }

            this.Select();
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

        /// <summary>
        /// Add the result of a test to the tree
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void SetTestResult(ResultNode result)
        {
            TestSuiteTreeNode node = this[result.Id];
            if (node == null)
            {
                Debug.WriteLine("Test not found in tree: " + result.FullName);
            }
            else
            {
                node.Result = result;

                if (result.Type == "Theory")
                    node.RepopulateTheoryNode();

                Invalidate(node.Bounds);
                Update();
            }
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

        public TestSuiteTreeNode this[string id] => _treeMap[id] as TestSuiteTreeNode;

        #endregion

        #region Other Public Methods

        /// <summary>
        /// Load the tree with a test hierarchy
        /// </summary>
        /// <param name="topLevelNode">Test-run node for tests to be loaded</param>
        public void LoadTests(TestNode topLevelNode)
        {
            using (new WaitCursor())
            {
                Clear();
                tree.BeginUpdate();

                try
                {
                    AddTreeNodes(Nodes, topLevelNode, false);
                    SetInitialExpansion();
                }
                finally
                {
                    tree.EndUpdate();
                    tree.Select();
                }
            }
        }

        public void ClearCheckedNodes() => Accept(new ClearCheckedNodesVisitor());

        public void CheckFailedNodes() => Accept(new CheckFailedNodesVisitor());

        #endregion

        #region Private Properties

        public TestPropertiesDialog TestPropertiesDialog
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
        /// Add nodes to the tree constructed from a test
        /// </summary>
        /// <param name="nodes">The TreeNodeCollection to which the new node should  be added</param>
        /// <param name="testNode">The test for which a node is to be built</param>
        /// <param name="highlight">If true, highlight the text for this node in the tree</param>
        /// <returns>A newly constructed TestSuiteTreeNode, possibly with descendant nodes</returns>
        private TestSuiteTreeNode AddTreeNodes(IList nodes, TestNode testNode, bool highlight)
        {
            TestSuiteTreeNode treeNode = new TestSuiteTreeNode(testNode);
            if (highlight) treeNode.ForeColor = Color.Blue;
            _treeMap.Add(treeNode.Test.Id, treeNode);

            nodes.Add(treeNode);

            if (testNode.IsSuite)
            {
                foreach (TestNode child in testNode.Children)
                    AddTreeNodes(treeNode.Nodes, child, highlight);
            }

            return treeNode;
        }

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

        /// <summary>
        /// Helper used to figure out the display style
        /// to use when the setting is Auto
        /// </summary>
        /// <returns>DisplayStyle to be used</returns>
        private DisplayStyle GetEffectiveDisplayStyle()
        {
            if (DisplayStyle != DisplayStyle.Auto)
                return DisplayStyle;

            if (tree.VisibleCount >= tree.GetNodeCount(true))
                return DisplayStyle.Expand;

            return DisplayStyle.HideTests;
        }
        
        private void SetInitialExpansion()
        {
            CollapseAll();

            switch (GetEffectiveDisplayStyle())
            {
                case DisplayStyle.Expand:
                    ExpandAll();
                    break;
                case DisplayStyle.HideTests:
                    HideTests();
                    break;
                case DisplayStyle.Collapse:
                default:
                    break;
            }

            if (Nodes.Count > 0)
            {
                SelectedNode = Nodes[0];
                SelectedNode.EnsureVisible();
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

        private void clearAllButton_Click(object sender, System.EventArgs e)
        {
            ClearCheckedNodes();
        }

        private void checkFailedButton_Click(object sender, System.EventArgs e)
        {
            CheckFailedNodes();
        }

        #endregion

        #region ClearCheckedNodesVisitor

        internal class ClearCheckedNodesVisitor : TestSuiteTreeNodeVisitor
        {
            public override void Visit(TestSuiteTreeNode node)
            {
                node.Checked = false;
            }
        }

        #endregion

        #region CheckFailedNodesVisitor

        internal class CheckFailedNodesVisitor : TestSuiteTreeNodeVisitor
        {
            public override void Visit(TestSuiteTreeNode node)
            {
                if (!node.Test.IsSuite &&
                    node.HasResult &&
                    node.Result.Outcome.Status == TestStatus.Failed)
                {
                    node.Checked = true;
                    node.EnsureVisible();
                }
                else
                    node.Checked = false;
            }
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
