// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    /// <summary>
    /// Class is responsible for regrouping treeNodes from one group to another group
    /// It's getting invoked whenever a test case is finished. 
    /// If a regrouping is required, the logic will remove the TestNode from all TestGroups along the path.
    /// If a TestGroup contains no TestNodes anymore, the associated TreeNode can be removed.
    /// </summary>
    internal class ReGrouping
    {
        public ReGrouping(ITestTreeView view, INUnitTreeDisplayStrategy strategy, INUnitGrouping grouping)
        {
            TreeView = view;
            Strategy = strategy;
            Grouping = grouping;
        }

        private INUnitGrouping Grouping { get; }

        private ITestTreeView TreeView { get; }

        private INUnitTreeDisplayStrategy Strategy { get; }

        /// <summary>
        /// Move the TreeNode path of a TestNode to a new group
        /// This method is intended to be called as soon as a Test finished: the TestNode is representing a test case, 
        /// which means the associated TreeNode is a leaf node of the tree.
        /// </summary>
        public void Regroup(TestNode testNode)
        {
            // Currently there's only one single TreeNode associated to a TestNode
            // Only 'category grouping' will have multiple associated TreeNodes, but that grouping doesn't require regrouping
            TreeNode treeNode = Strategy.GetTreeNodesForTest(testNode).FirstOrDefault();
            Regroup(testNode, treeNode);
        }

        private void Regroup(TestNode testNode, TreeNode treeNode)
        {
            // 1. Check if TestNode is assigned to a new group 
            string newGroupName = Grouping.GetGroupNames(testNode).First();
            string oldGroupName = GetOldGroupName(treeNode);
            if (oldGroupName == newGroupName)
                return;

            // 2. Remove TestCase from groups and remove TreeNodes if required
            IList<TreeNode> oldTreeNodes = UpdateOrRemoveTreeNodesInPath(treeNode, testNode);

            // 3. Create new TreeNode path
            IList<TreeNode> newTreeNodes = Grouping.CreateTreeNodes(testNode, newGroupName);
            TreeView.SetImageIndex(newTreeNodes.Last(), treeNode.ImageIndex);

            // 4. Update all tree node names to reflect changed number of containing tests in TestGroups
            TreeNodeNameHandler.UpdateTreeNodeNames(oldTreeNodes.Concat(newTreeNodes));

            // 5. Expand newly created treeNodes
            ExpandTreeNodes(newTreeNodes);
        }

        private string GetOldGroupName(TreeNode treeNode)
        {
            while (treeNode.Parent != null)
                treeNode = treeNode.Parent;

            if (treeNode.Tag is TestGroup testGroup)
                return testGroup.Name;

            return "";
        }

        private IList<TreeNode> UpdateOrRemoveTreeNodesInPath(TreeNode treeNode, TestNode testNode)
        {
            IList<TreeNode> treeNodes = GetTreeNodePath(treeNode);
            foreach (TreeNode node in treeNodes)
                UpdateOrRemoveTreeNode(node, testNode);

            return treeNodes;
        }

        private void UpdateOrRemoveTreeNode(TreeNode treeNode, TestNode testNode)
        {
            // Remove tree node
            if (treeNode.Tag is TestNode treeNodeTestNode && treeNodeTestNode.Id == testNode.Id)
                Strategy.RemoveTreeNode(treeNode);

            // TreeNode is associated with a TestGroup (inner tree node) => Remove testNode from group
            // If TestGroup has no TestNodes anymore, remove the TreeNode
            if (treeNode.Tag is TestGroup testGroup)
            {
                testGroup.RemoveId(testNode.Id);
                if (testGroup.Count() == 0)
                    treeNode.Remove();
            }
        }

        private IList<TreeNode> GetTreeNodePath(TreeNode treeNode)
        {
            IList<TreeNode> treeNodes = new List<TreeNode>();

            while (treeNode != null)
            {
                treeNodes.Add(treeNode);
                treeNode = treeNode.Parent;
            }

            return treeNodes;
        }

        private void ExpandTreeNodes(IList<TreeNode> newTreeNodes)
        {
            foreach (TreeNode node in newTreeNodes)
                node.Expand();
        }
    }
}
