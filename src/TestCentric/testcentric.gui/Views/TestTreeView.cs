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
    /// TestSuiteTreeView is a tree view control
    /// specialized for displaying the tests
    /// in an assembly. Clients should always
    /// use TestNode rather than TreeNode when
    /// dealing with this class to be sure of
    /// calling the proper methods.
    /// </summary>
    public class TestTreeView : TreeView, ITestTreeView
    {
        //static Logger log = InternalTrace.GetLogger(typeof(TestSuiteTreeView));

        #region Instance Variables

        /// <summary>
        /// Hashtable provides direct access to TestNodes
        /// </summary>
        private Hashtable treeMap = new Hashtable();
        
        /// <summary>
        /// The properties dialog if displayed
        /// </summary>
        private TestPropertiesDialog propertiesDialog;

        public System.Windows.Forms.ImageList treeImages;
        private System.ComponentModel.IContainer components;

		private MenuItem runMenuItem;
		private MenuItem showCheckBoxesMenuItem;
		private MenuItem failedAssumptionsMenuItem;
		private MenuItem propertiesMenuItem;

		private string _alternateImageSet;
		private TestNodeFilter _treeFilter = TestNodeFilter.Empty;
        
		#endregion

        #region Construction and Initialization
        
        public TestTreeView()
        {
            InitializeComponent();

            ContextMenu = new System.Windows.Forms.ContextMenu();
            ContextMenu.Popup += new System.EventHandler(ContextMenu_Popup);
			ContextMenu.Collapse += new EventHandler(ContextMenu_Collapse);

			runMenuItem = new MenuItem("&Run");// new EventHandler(runMenuItem_Click));
			failedAssumptionsMenuItem = new MenuItem("Show Failed Assumptions");
			showCheckBoxesMenuItem = new MenuItem("Show CheckBoxes");
			propertiesMenuItem = new MenuItem("&Properties");
       
			ContextMenu.MenuItems.Add(runMenuItem);
            ContextMenu.MenuItems.Add("-");
			ContextMenu.MenuItems.Add(failedAssumptionsMenuItem);
            ContextMenu.MenuItems.Add(showCheckBoxesMenuItem);
            ContextMenu.MenuItems.Add("-");
            ContextMenu.MenuItems.Add(propertiesMenuItem);

			RunCommand = new MenuCommand(runMenuItem);
			ShowCheckBoxes = new CheckedMenuItem(showCheckBoxesMenuItem);
			ShowFailedAssumptions = new CheckedMenuItem(failedAssumptionsMenuItem);
			PropertiesCommand = new MenuCommand(propertiesMenuItem);
		}

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTreeView));
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeImages
            // 
            this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
            this.treeImages.TransparentColor = System.Drawing.Color.White;
            this.treeImages.Images.SetKeyName(0, "Skipped.png");
            this.treeImages.Images.SetKeyName(1, "Failure.png");
            this.treeImages.Images.SetKeyName(2, "Success.png");
            this.treeImages.Images.SetKeyName(3, "Ignored.png");
            this.treeImages.Images.SetKeyName(4, "Inconclusive.png");
            // 
            // TestSuiteTreeView
            // 
            this.ImageIndex = 0;
            this.ImageList = this.treeImages;
            this.SelectedImageIndex = 0;
            this.DoubleClick += new System.EventHandler(this.TestSuiteTreeView_DoubleClick);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TestSuiteTreeView_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TestSuiteTreeView_DragEnter);
            this.ResumeLayout(false);

        }

		#endregion

		#region ITestTreeView Members

		// The following members are implemented by underlying classes
        //    ContextMenu
        //    Nodes
        //    TopNode
        //    SelectedNode

		public ICommand RunCommand { get; }
		public IChecked ShowCheckBoxes { get; }
		public IChecked ShowFailedAssumptions { get; }
		public ICommand PropertiesCommand { get; }

		public DisplayStyle DisplayStyle { get; set; }
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
        
        [Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether checkboxes are displayed beside test nodes")]
        public new bool CheckBoxes
        {
            get { return base.CheckBoxes; }
            set
            {
                if (base.CheckBoxes != value)
                {
					// When turning off checkboxes with a non-empty tree, the
                    // structure of what is expanded and collapsed is lost.
                    // We save that structure as a VisualState and then restore it.
                    VisualState visualState = !value && TopNode != null
                        ? new VisualState(this)
                        : null;

                    base.CheckBoxes = value;

                    if (visualState != null)
                    {
                        visualState.ShowCheckBoxes = this.CheckBoxes;
                        visualState.Restore(this);
                    }
                }
            }
        }

         /// <summary>
        /// The currently selected test.
        /// </summary>
        [Browsable(false)]
        public TestNode SelectedTest
        {
            get
            {
                TestSuiteTreeNode node = (TestSuiteTreeNode)SelectedNode;
                return node == null ? null : node.Test;
            }
        }

        /// <summary>
        /// The TreeNode that was right clicked to bring up context menu
        /// </summary>
		public TestSuiteTreeNode ContextNode { get; private set; }

        [Browsable(false)]
        public TestNode[] SelectedTests
        {
            get
            {
                TestNode[] result = null;

                if (this.CheckBoxes)
                {
                    CheckedTestFinder finder = new CheckedTestFinder(this);
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
            treeMap.Clear();
            Nodes.Clear();
        }

		/// <summary>
        /// Reload the tree with a changed test hierarchy
        /// while maintaining as much gui state as possible.
        /// </summary>
        /// <param name="test">Test suite to be loaded</param>
        public void Reload(TestNode test)
        {
            VisualState visualState = new VisualState(this);

            Load(test);

            visualState.Restore(this);
        }

		public void ShowPropertiesDialog(TestSuiteTreeNode node)
        {
            TestPropertiesDialog.DisplayProperties(node);
        }

        public void ClosePropertiesDialog()
        {
            if (propertiesDialog != null)
                propertiesDialog.Close();
        }

        public void CheckPropertiesDialog()
        {
            if (propertiesDialog != null && !propertiesDialog.Pinned)
                propertiesDialog.Close();
        }

		/// <summary>
        /// Add the result of a test to the tree
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void SetTestResult(ResultNode result)
        {
            TestSuiteTreeNode node = this[result];
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

		[Browsable(false)]
        public TestNode[] FailedTests
        {
            get
            {
                FailedTestsFilterVisitor visitor = new FailedTestsFilterVisitor();
                Accept(visitor);
                return visitor.Tests;
            }
        }

		[Browsable(false)]
        public TestNodeFilter TreeFilter
        {
            get { return _treeFilter; }
            set
            {
                _treeFilter = value;

                TestFilterVisitor visitor = new TestFilterVisitor(_treeFilter);
                this.Accept(visitor);
            }
        }

		public TestSuiteTreeNode this[string id]
        {
            get { return treeMap[id] as TestSuiteTreeNode; }
        }

		#endregion

		#region Other Public Methods

        /// <summary>
        /// Load the tree with a test hierarchy
        /// </summary>
        /// <param name="topLevelNode">Test-run node for tests to be loaded</param>
        public void Load(TestNode topLevelNode)
        {
            using (new WaitCursor())
            {
                Clear();
                BeginUpdate();

                try
                {
                    AddTreeNodes(Nodes, topLevelNode, false);

                    SetInitialExpansion();
                }
                finally
                {
                    EndUpdate();
                    this.Select();
                }
            }
        }

        public void ClearCheckedNodes()
        {
            Accept(new ClearCheckedNodesVisitor());
        }

        public void CheckFailedNodes()
        {
            Accept(new CheckFailedNodesVisitor());
        }

        public void HideTests()
        {
            this.BeginUpdate();
            foreach (TestSuiteTreeNode node in Nodes)
                HideTestsUnderNode(node);
            this.EndUpdate();
        }

        #endregion

		#region Context Menu
		/// <summary>
		/// Handles right mouse button down by remembering the proper context item
		/// </summary>
		/// <param name="e">MouseEventArgs structure with information about the mouse position and button state</param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ClosePropertiesDialog();
                ContextNode = GetNodeAt(e.X, e.Y) as TestSuiteTreeNode;
            }
			// TODO: Finish this to implement multiple select or remove
            //else if (e.Button == MouseButtons.Left)
            //{
            //    if (Control.ModifierKeys == Keys.Control)
            //    {
            //        TestSuiteTreeNode theNode = GetNodeAt(e.X, e.Y) as TestSuiteTreeNode;
            //        if (theNode != null)
            //            theNode.IsSelected = true;
            //    }
            //    else
            //    {
            //        ClearSelected();
            //    }
            //}

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Build treeview context menu dynamically on popup
        /// </summary>
        private void ContextMenu_Popup(object sender, System.EventArgs e)
        {
			TestSuiteTreeNode targetNode = ContextNode ?? (TestSuiteTreeNode)SelectedNode;
			TestSuiteTreeNode theoryNode = targetNode?.GetTheoryNode();

            runMenuItem.DefaultItem = runMenuItem.Enabled && targetNode != null && targetNode.Included &&
                    (targetNode.Test.RunState == RunState.Runnable || targetNode.Test.RunState == RunState.Explicit);

			//failedAssumptionsMenuItem.Visible = 
			failedAssumptionsMenuItem.Enabled = theoryNode != null;
			failedAssumptionsMenuItem.Checked = theoryNode?.ShowFailedAssumptions ?? false;

			propertiesMenuItem.Enabled = targetNode != null;
        }

        /// <summary>
        /// Clear the ContextNode once the menu is no longer active
        /// </summary>
		private void ContextMenu_Collapse(object sender, EventArgs eventArgs)
		{
			ContextNode = null;
		}

        #endregion

        #region Drag and drop

        /// <summary>
        /// Helper method to determine if an IDataObject is valid
        /// for dropping on the tree view. It must be a the drop
        /// of a single file with a valid assembly file type.
        /// </summary>
        /// <param name="data">IDataObject to be tested</param>
        /// <returns>True if dropping is allowed</returns>
        private bool IsValidFileDrop(IDataObject data)
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
                //	return false;
            }

            return true;
        }

        private void TestSuiteTreeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (IsValidFileDrop(e.Data))
            {
                //string[] fileNames = (string[])e.Data.GetData( DataFormats.FileDrop );
                //if ( fileNames.Length == 1 )
                //	loader.LoadProject( fileNames[0] );
                //else
                //	loader.LoadProject( fileNames );

                //if (loader.IsProjectLoaded && loader.TestProject.IsLoadable)
                //	loader.LoadTest();
            }
        }

        private void TestSuiteTreeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (IsValidFileDrop(e.Data))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        #region UI Event Handlers

        private void TestSuiteTreeView_DoubleClick(object sender, System.EventArgs e)
        {
            //TestSuiteTreeNode node = SelectedNode as TestSuiteTreeNode;
            //if ( runCommandSupported && runCommandEnabled && node.Nodes.Count == 0 && node.Included )
            //{
            //	runCommandEnabled = false;

            //	// TODO: Since this is a terminal node, don't use a category filter
            //	RunTests( new ITest[] { SelectedTest }, true );
            //}
        }

        protected override void OnAfterSelect(System.Windows.Forms.TreeViewEventArgs e)
        {
            if (propertiesDialog != null)
			{
				if (propertiesDialog.Pinned)
                {
                    propertiesDialog.DisplayProperties((TestSuiteTreeNode)e.Node);
                }
                else
                    propertiesDialog.Close();
			}

			base.OnAfterSelect(e);
        }

        #endregion

		#region Private Properties

		/// <summary>
        /// Test node corresponding to a test
        /// </summary>
        private TestSuiteTreeNode this[TestNode test]
        {
            get { return this[test.Id]; }
        }

        /// <summary>
        /// Test node corresponding to a TestResultInfo
        /// </summary>
        private TestSuiteTreeNode this[ResultNode result]
        {
            get { return this[result.Id]; }
        }

		private TestPropertiesDialog TestPropertiesDialog
        {
            get
            {
                if (propertiesDialog == null)
                {
                    Form owner = this.FindForm();
                    propertiesDialog = new TestPropertiesDialog();
                    propertiesDialog.Owner = owner;
                    propertiesDialog.Font = owner.Font;
                    propertiesDialog.StartPosition = FormStartPosition.Manual;
                    propertiesDialog.Left = Math.Max(0, owner.Left + (owner.Width - propertiesDialog.Width) / 2);
                    propertiesDialog.Top = Math.Max(0, owner.Top + (owner.Height - propertiesDialog.Height) / 2);
                    propertiesDialog.Closed += (s, e) => propertiesDialog = null;
                }

                return propertiesDialog;
            }
        }

        #endregion

		#region Private Methods

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
            treeMap.Add(treeNode.Test.Id, treeNode);

            nodes.Add(treeNode);

            if (testNode.IsSuite)
            {
                foreach (TestNode child in testNode.Children)
                    AddTreeNodes(treeNode.Nodes, child, highlight);
            }

            return treeNode;
        }

        //private TestSuiteTreeNode AddTreeNodes( IList nodes, TestResult rootResult, bool highlight )
        //{
        //	TestSuiteTreeNode node = new TestSuiteTreeNode( rootResult );
        //	AddToMap( node );

        //	nodes.Add( node );

        //	if ( rootResult.HasResults )
        //	{
        //		foreach( TestResult result in rootResult.Results )
        //			AddTreeNodes( node.Nodes, result, highlight );
        //	}

        //	node.UpdateImageIndex();

        //	return node;
        //}

        private void RemoveFromMap(TestSuiteTreeNode node)
        {
            foreach (TestSuiteTreeNode child in node.Nodes)
                RemoveFromMap(child);

            treeMap.Remove(node.Test.Id);
        }

        /// <summary>
        /// Remove a node from the tree itself and the hashtable
        /// </summary>
        /// <param name="treeNode">Node to remove</param>
        private void RemoveNode(TestSuiteTreeNode treeNode)
        {
            RemoveFromMap(treeNode);
            treeNode.Remove();
        }

        /// <summary>
        /// Delegate for use in invoking the tree loader
        /// from the watcher thread.
        /// </summary>
        private delegate void LoadHandler( TestNode test );

        private delegate void PropertiesDisplayHandler(TestSuiteTreeNode node);

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
            if ( DisplayStyle != DisplayStyle.Auto )
                return DisplayStyle;

            if ( VisibleCount >= GetNodeCount( true ) )
                return DisplayStyle.Expand;

            return DisplayStyle.HideTests;
        }

        public void SetInitialExpansion()
        {
            CollapseAll();
            
            switch ( GetEffectiveDisplayStyle() )
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

        private TestSuiteTreeNode FindNode(TestNode test)
        {
            TestSuiteTreeNode node = treeMap[test.Id] as TestSuiteTreeNode;

            if (node == null)
                node = FindNodeByName(test.FullName);

            return node;
        }

        private TestSuiteTreeNode FindNodeByName(string fullName)
        {
            foreach (string uname in treeMap.Keys)
            {
                int rbrack = uname.IndexOf(']');
                string name = rbrack >= 0 ? uname.Substring(rbrack + 1) : uname;
                if (name == fullName)
                    return treeMap[uname] as TestSuiteTreeNode;
            }

            return null;
        }
        
		private void Accept(TestSuiteTreeNodeVisitor visitor)
        {
            foreach (TestSuiteTreeNode node in Nodes)
            {
                node.Accept(visitor);
            }
        }

        private void LoadAlternateImages(string imageSet)
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

        #endregion
    }

    #region Helper Classes

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

        public CheckedTestFinder(TestTreeView treeView)
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

    #endregion
}
