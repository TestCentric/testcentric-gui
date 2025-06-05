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
    /// A bulk processing approach is used to improve the treeview performance.
    /// This class is getting invoked whenever the bulk processing is triggered, and receives a list of test nodes to be regrouped. 
    /// 1. The regroup logic will remove the TestNode from all TestGroups along the current path.
    /// 2. If a test group no longer contains any test nodes, the associated tree node can be removed.
    /// 3. The test node is inserted into the new group building up a new tree path.
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
        /// This method gets a list of TestNodes either representing test cases or test suites. 
        /// </summary>
        public void Regroup(IList<TestNode> testNodes)
        {
            TreeView.TreeView.BeginUpdate();

            var treeNodes = new List<TreeNode>();
            foreach (TestNode testNode in testNodes)
            {
                // Finish of test suites (Fixture/Namespace...) requires that all pending regroup operations are executed first, finally the image icon of the test suite can be updated
                if (testNode.IsSuite)
                {
                    TreeNodeImageHandler.SetTreeNodeImages(TreeView, Strategy.GetTreeNodesForTest(testNode), false);
                    continue;
                }

                TreeNode treeNode = GetTreeNode(testNode);

                // 1. Determine new group
                string newGroupName = Grouping.GetGroupNames(testNode).First();

                // 2. Remove TestCase from groups and remove TreeNodes if required
                IList<TreeNode> oldTreeNodes = UpdateOrRemoveTreeNodesInPath(treeNode, testNode);
                treeNodes.AddRange(oldTreeNodes);

                // 3. Create new TreeNode path
                IList <TreeNode> newTreeNodes = Grouping.CreateTreeNodes(testNode, newGroupName);
                TreeView.SetImageIndex(newTreeNodes.Last(), treeNode.ImageIndex);
                treeNodes.AddRange(newTreeNodes);

                // 4. Expand newly created treeNodes
                ExpandTreeNodes(newTreeNodes);
            }

            // 5. Update all tree node names to reflect changed number of containing tests in TestGroups
            Strategy.UpdateTreeNodeNames(treeNodes.Distinct());

            TreeView.TreeView.EndUpdate();
        }

        /// <summary>
        /// Checks if a TestNode will be grouped into a different group than the current one
        /// </summary>
        public bool IsRegroupRequired(TestNode testNode)
        {
            TreeNode treeNode = GetTreeNode(testNode);
            string newGroupName = Grouping.GetGroupNames(testNode).First();
            string oldGroupName = GetOldGroupName(treeNode);
            return oldGroupName != newGroupName;
        }

        private TreeNode GetTreeNode(TestNode testNode)
        {
            // Currently there's only one single TreeNode associated to a TestNode
            // Only 'category grouping' will have multiple associated TreeNodes, but that grouping doesn't require regrouping
            return Strategy.GetTreeNodesForTest(testNode).FirstOrDefault();
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
                if (!testGroup.Any())
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
            newTreeNodes.FirstOrDefault()?.Expand();
        }
    }
}
