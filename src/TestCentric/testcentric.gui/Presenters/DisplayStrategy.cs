// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Model.Settings;
    using Views;
	using Elements;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// DisplayStrategy is the abstract base for the various
    /// strategies used to display tests in the tree control.
    /// It works primarily as a traditional strategy, with methods
    /// called by the TreeViewPresenter, but may also function
    /// as a presenter in it's own right, since it is created 
    /// with references to the view and mode.
    /// 
    /// We currently support three different strategies:
    /// NunitTreeDisplay, TestListDisplay and FixtureListDisplay.
    /// </summary>
    public abstract class DisplayStrategy : ITreeDisplayStrategy
    {
        // TODO: This class is temporarily using image index values
        // from TestSuiteTreeNode rather than TestTreeView.

        protected ITestTreeView _view;
        protected ITestModel _model;
        protected UserSettings _settings;

        protected Dictionary<string, List<TreeNode>> _nodeIndex = new Dictionary<string, List<TreeNode>>();

        #region Construction and Initialization

        public DisplayStrategy(ITestTreeView view, ITestModel model)
        {
            _view = view;
            _model = model;
            _settings = _model.Settings;
        }

        #endregion

        #region Properties

        public bool HasResults
        {
            get { return _model.HasResults; }
        }

        public abstract string StrategyID { get; }

        public abstract string Description { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load all tests into the tree, starting from a root TestNode.
        /// </summary>
        public abstract void OnTestLoaded(TestNode testNode, VisualState visualState);

        public void SaveVisualState() => CreateVisualState().Save(VisualState.GetVisualStateFileName(_model.TestCentricProject.TestFiles[0]));

        protected abstract VisualState CreateVisualState();

        public void OnTestUnloaded()
        {
            ClearTree();
        }

        public virtual void OnTestFinished(ResultNode result)
        {
            int imageIndex = CalcImageIndex(result.Outcome);
            foreach (TreeNode treeNode in GetTreeNodesForTest(result))
            {
                _view.SetImageIndex(treeNode, imageIndex);
                UpdateTreeNodeName(treeNode);
            }
        }

        public virtual void OnTestRunStarting()
        {
            _view.ResetAllTreeNodeImages();
            UpdateTreeNodeNames();
        }

        public virtual void OnTestRunFinished()
        {
        }

        // Called when either the display strategy or the grouping
        // changes. May need to distinguish these cases.
        public void Reload()
        {
            TestNode testNode = _model.LoadedTests;
            if (testNode != null)
            {
                OnTestLoaded(testNode, null);

                if (_view.Nodes != null) // TODO: Null when mocked
                    foreach (TreeNode treeNode in _view.Nodes)
                        ApplyResultsToTree(treeNode);
            }
        }

        #endregion

        #region Helper Methods

        protected void ClearTree()
        {
            _view.Clear();
            _nodeIndex.Clear();
        }

        protected TreeNode MakeTreeNode(TestGroup group, bool recursive)
        {
            TreeNode treeNode = new TreeNode(GroupDisplayName(group), group.ImageIndex, group.ImageIndex);
            treeNode.Tag = group;

            if (recursive)
                foreach (TestNode test in group)
                    treeNode.Nodes.Add(MakeTreeNode(test, true));

            return treeNode;
        }

        public TreeNode MakeTreeNode(TestNode testNode, bool recursive)
        {
            string treeNodeName = GetTreeNodeDisplayName(testNode);
            TreeNode treeNode = new TreeNode(treeNodeName);
            treeNode.Tag = testNode;

            int imageIndex = TestTreeView.SkippedIndex;

            switch (testNode.RunState)
            {
                case RunState.Ignored:
                    imageIndex = TestTreeView.WarningIndex;
                    break;
                case RunState.NotRunnable:
                    imageIndex = TestTreeView.FailureIndex;
                    break;
            }

            treeNode.ImageIndex = treeNode.SelectedImageIndex = imageIndex;

            string id = testNode.Id;
            if (_nodeIndex.ContainsKey(id))
                _nodeIndex[id].Add(treeNode);
            else
            {
                var list = new List<TreeNode>();
                list.Add(treeNode);
                _nodeIndex.Add(id, list);
            }

            if (recursive)
                foreach (TestNode childNode in testNode.Children)
                    treeNode.Nodes.Add(MakeTreeNode(childNode, true));

            return treeNode;
        }

        public string GroupDisplayName(TestGroup group)
        {
            return string.Format("{0} ({1})", group.Name, group.Count());
        }

        protected virtual string GetTreeNodeName(TestNode testNode)
        {
            return testNode.Name;
        }

        private string GetTreeNodeDisplayName(TestNode testNode)
        {
            string treeNodeName = GetTreeNodeName(testNode);

            // Check if test result is available for this node
            ResultNode result = _model.GetResultForTest(testNode.Id);
            if (_settings.Gui.TestTree.ShowTestDuration && result != null)
                treeNodeName += $" [{result.Duration:0.000}s]";

            return treeNodeName;
        }

        /// <summary>
        /// Update all tree node names
        /// If setting 'ShowDuration' is active and test results are available, show test duration in tree node.
        /// </summary>
        public void UpdateTreeNodeNames()
        {
            UpdateTreeNodeNames(_view.Nodes);
        }

        private void UpdateTreeNodeNames(TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                UpdateTreeNodeName(treeNode);
                UpdateTreeNodeNames(treeNode.Nodes);
            }
        }

        private void UpdateTreeNodeName(TreeNode treeNode)
        {
            TestNode testNode = treeNode.Tag as TestNode;           // Update for TestFixture or Testcase nodes only
            if (testNode == null)
                return;

            string treeNodeName = GetTreeNodeDisplayName(testNode);
            _view.InvokeIfRequired(() => treeNode.Text = treeNodeName);
        }

        public static int CalcImageIndex(ResultState outcome)
        {
            switch (outcome.Status)
            {
                case TestStatus.Inconclusive:
                    return TestTreeView.InconclusiveIndex;
                case TestStatus.Passed:
                    return TestTreeView.SuccessIndex;
                case TestStatus.Failed:
                    return TestTreeView.FailureIndex;
                case TestStatus.Warning:
                    return TestTreeView.WarningIndex;
                case TestStatus.Skipped:
                default:
                    return outcome.Label == "Ignored"
                        ? TestTreeView.WarningIndex
                        : TestTreeView.SkippedIndex;
            }
        }

        private void ApplyResultsToTree(TreeNode treeNode)
        {
            TestNode testNode = treeNode.Tag as TestNode;

            if (testNode != null)
            {
                ResultNode resultNode = GetResultForTest(testNode);
                if (resultNode != null)
                    treeNode.ImageIndex = treeNode.SelectedImageIndex = CalcImageIndex(resultNode.Outcome);
            }

            foreach (TreeNode childNode in treeNode.Nodes)
                ApplyResultsToTree(childNode);
        }

        public void CollapseToFixtures()
        {
            if (_view.Nodes != null) // TODO: Null when mocked
                foreach (TreeNode treeNode in _view.Nodes)
                    CollapseToFixtures(treeNode);
        }

        protected void CollapseToFixtures(TreeNode treeNode)
        {
            var testNode = treeNode.Tag as TestNode;
            if (testNode != null && testNode.Type == "TestFixture")
                treeNode.Collapse();
            else if (testNode == null || testNode.IsSuite)
            {
                treeNode.Expand();
                foreach (TreeNode child in treeNode.Nodes)
                    CollapseToFixtures(child);
            }
        }

        public List<TreeNode> GetTreeNodesForTest(TestNode testNode)
        {
            return GetTreeNodesForTest(testNode.Id);
        }

        public List<TreeNode> GetTreeNodesForTest(string id)
        {
            List<TreeNode> treeNodes;
            if (!_nodeIndex.TryGetValue(id, out treeNodes))
                treeNodes = new List<TreeNode>();

            return treeNodes;
        }

        public ResultNode GetResultForTest(TestNode testNode)
        {
            return _model.GetResultForTest(testNode.Id);
        }

        #endregion
    }
}
