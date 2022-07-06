// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    /// <summary>
    /// GroupDisplayStrategy is the abstract base class for 
    /// DisplayStrategies that list tests in various groupings.
    /// </summary>
    public abstract class GroupDisplayStrategy : DisplayStrategy
    {
        protected TestGrouping _grouping;

        #region Construction and Initialization

        public GroupDisplayStrategy(ITestTreeView view, ITestModel model)
            : base(view, model)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Post a test result to the tree, changing the treeNode
        /// color to reflect success or failure. Overridden here
        /// to allow for moving nodes from one group to another
        /// based on the result of running the test.
        /// </summary>
        public override void OnTestFinished(ResultNode result)
        {
            base.OnTestFinished(result);

            ApplyResultToGroup(result, false);
        }

        // TODO: Move this to TestGroup? Would need access to results.
        public int CalcImageIndexForGroup(TestGroup group)
        {
            var groupIndex = -1;

            foreach (var testNode in group)
            {
                var result = GetResultForTest(testNode);
                if (result != null)
                {
                    var imageIndex = CalcImageIndex(result.Outcome);

                    if (imageIndex == TestTreeView.FailureIndex)
                        return TestTreeView.FailureIndex; // Early return - can't get any worse!

                    if (imageIndex >= TestTreeView.SuccessIndex) // Only those values propagate
                        groupIndex = Math.Max(groupIndex, imageIndex);
                }
            }

            return groupIndex;
        }

        public void Add(TreeNode treeNode)
        {
            _view.Add(treeNode);
        }

        public void ApplyResultToGroup(ResultNode result, bool updateImages)
        {
            var treeNodes = GetTreeNodesForTest(result);

            // Result may be for a TestNode not shown in the tree
            if (treeNodes.Count == 0)
                return;

            // This implementation ignores any but the first node
            // since changing of groups is currently only needed
            // for groupings that display each node once.
            var treeNode = treeNodes[0];
            var oldParent = treeNode.Parent;
            var oldGroup = oldParent.Tag as TestGroup;

            // We only have to proceed for tests that are direct
            // descendants of a group node.
            if (oldGroup == null)
                return;

            var newGroup = _grouping.SelectGroups(result)[0];

            // If the group didn't change, we can get out of here
            if (oldGroup == newGroup)
                return;

            var newParent = newGroup.TreeNode;

            _view.InvokeIfRequired(() =>
            {
                oldGroup.RemoveId(result.Id);
                // TODO: Insert in order
                newGroup.Add(result);

                // Remove test from tree
                treeNode.Remove();

                // If it was last test in group, remove group
                if (oldGroup.Count == 0)
                    oldParent.Remove();
                else // update old group
                {
                    oldParent.Text = GroupDisplayName(oldGroup);
                    if (updateImages)
                        oldParent.ImageIndex = oldParent.SelectedImageIndex = oldGroup.ImageIndex =
                            CalcImageIndexForGroup(oldGroup);
                }

                newParent.Nodes.Add(treeNode);
                newParent.Text = GroupDisplayName(newGroup);
                newParent.Expand();

                if (updateImages)
                {
                    var imageIndex = DisplayStrategy.CalcImageIndex(result.Outcome);
                    if (imageIndex >= TestTreeView.SuccessIndex && imageIndex > newGroup.ImageIndex)
                        newParent.ImageIndex = newParent.SelectedImageIndex = newGroup.ImageIndex = imageIndex;
                }

                if (newGroup.Count == 1)
                {
                    _view.Clear();
                    TreeNode topNode = null;
                    foreach (var group in _grouping.Groups)
                        if (group.Count > 0)
                        {
                            Add(group.TreeNode);
                            if (topNode == null)
                                topNode = group.TreeNode;
                        }

                    if (topNode != null)
                        topNode.EnsureVisible();
                }
            });
        }

        #endregion

        #region Protected Members

        protected void SetDefaultTestGrouping()
        {
            _grouping = CreateTestGrouping(DefaultGroupSetting);
        }

        protected void SetTestGrouping(string groupBy)
        {
            _grouping = CreateTestGrouping(groupBy);
            DefaultGroupSetting = groupBy;
        }

        protected abstract string DefaultGroupSetting { get; set; }

        protected TestGrouping CreateTestGrouping(string groupBy)
        {
            switch (groupBy)
            {
                case "OUTCOME":
                    return new OutcomeGrouping(this);
                case "DURATION":
                    return new DurationGrouping(this);
                case "CATEGORY":
                    return new CategoryGrouping(this, false);
                case "CATEGORY_EXTENDED":
                    return new CategoryGrouping(this, true);
            }

            return null;
        }

        protected void UpdateDisplay()
        {
            if (_grouping != null)
            {
                this.ClearTree();
                TreeNode topNode = null;
                foreach (var group in _grouping.Groups)
                {
                    var treeNode = MakeTreeNode(group, true);
                    group.TreeNode = treeNode;
                    treeNode.Expand();
                    if (group.Count > 0)
                    {
                        _view.Add(treeNode);
                        if (topNode == null)
                            topNode = treeNode;
                    }
                }
                if (topNode != null)
                    topNode.EnsureVisible();
            }
        }

        protected override void TrySaveVisualState()
        {
            if (_model.TestFiles.Count > 0)
            {
                var visualState = new VisualState(StrategyID, _grouping?.ID ?? "ASSEMBLY").LoadFrom(_view.TreeView);
                visualState.Save(VisualState.GetVisualStateFileName(_model.TestFiles[0]));
            }
        }

        #endregion
    }
}
