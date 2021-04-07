// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    /// <summary>
    /// TestGrouping is the base class of all groupings. It represents a
    /// set of TestGroups containing tests. In the base class, the contents
    /// of each group is stable once created.
    /// </summary>
    public abstract class TestGrouping
    {
        protected GroupDisplayStrategy _displayStrategy;

        #region Constructor

        /// <summary>
        /// Construct a TestGrouping for a given GroupDisplayStrategy
        /// </summary>
        /// <param name="displayStrategy"></param>
        public TestGrouping(GroupDisplayStrategy displayStrategy)
        {
            _displayStrategy = displayStrategy;
            Groups = new List<TestGroup>();
        }

        #endregion

        #region Public Members

        public List<TestGroup> Groups { get; private set; }

        public virtual void Load(IEnumerable<TestNode> tests)
        {
            foreach (TestNode testNode in tests)
                foreach (var group in SelectGroups(testNode))
                    group.Add(testNode);
        }

        public void ChangeGroupsBasedOnTestResult(ResultNode result, bool updateImages)
        {
            var treeNodes = _displayStrategy.GetTreeNodesForTest(result);

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

            var newGroup = SelectGroups(result)[0];

            // If the group didn't change, we can get out of here
            if (oldGroup == newGroup)
                return;

            var newParent = newGroup.TreeNode;

            _displayStrategy.Tree.InvokeIfRequired(() =>
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
                    oldParent.Text = _displayStrategy.GroupDisplayName(oldGroup);
                    if (updateImages)
                        oldParent.ImageIndex = oldParent.SelectedImageIndex = oldGroup.ImageIndex =
                            _displayStrategy.CalcImageIndexForGroup(oldGroup);
                }

                newParent.Nodes.Add(treeNode);
                newParent.Text = _displayStrategy.GroupDisplayName(newGroup);
                newParent.Expand();

                if (updateImages)
                {
                    var imageIndex = DisplayStrategy.CalcImageIndex(result.Outcome);
                    if (imageIndex >= TestTreeView.SuccessIndex && imageIndex > newGroup.ImageIndex)
                        newParent.ImageIndex = newParent.SelectedImageIndex = newGroup.ImageIndex = imageIndex;
                }

                if (newGroup.Count == 1)
                {
                    _displayStrategy.Tree.Clear();
                    TreeNode topNode = null;
                    foreach (var group in Groups)
                        if (group.Count > 0)
                        {
                            _displayStrategy.Tree.Add(group.TreeNode);
                            if (topNode == null)
                                topNode = group.TreeNode;
                        }

                    if (topNode != null)
                        topNode.EnsureVisible();
                }
            });
        }

        /// <summary>
        /// Post a test result to the tree, changing the treeNode
        /// color to reflect success or failure.
        /// </summary>
        public virtual void OnTestFinished(ResultNode result)
        {
            // Override to take any necessary action
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Returns an array of groups in which a TestNode is categorized.
        /// </summary>
        protected abstract TestGroup[] SelectGroups(TestNode testNode);

        #endregion
    }
}
