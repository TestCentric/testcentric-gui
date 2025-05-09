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
    /// This class is responsible for the NUNnit tree grouping functionality 
    /// It contains the main logic for performing the grouping and building up the tree
    /// Derived classes need to provide the actual grouping information (duration, outcome, categories)
    /// </summary>
    public class GroupingBase : INUnitGrouping
    {
        public GroupingBase(INUnitTreeDisplayStrategy strategy, ITestModel model, ITestTreeView view)
        {
            Strategy = strategy;
            TreeView = view;
            Model = model;
        }

        protected ITestModel Model { get; }

        protected ITestTreeView TreeView { get; }

        protected INUnitTreeDisplayStrategy Strategy { get; }

        /// <summary>
        /// Derived classes might set this value, if test cases might need to be regrouped after a test run
        /// For example duration or outcome grouping
        /// </summary>
        protected bool SupportsRegrouping { get; set; }

        /// <summary>
        /// Creates the complete grouped tree
        /// </summary>
        public virtual void CreateTree(TestNode rootNode)
        {
            // 1. Get list of all TestNode representing test cases
            IList<TestNode> testNodes = rootNode.Select(n => !n.IsSuite && n.IsVisible).ToList();

            TreeView.TreeView.BeginUpdate();

            // 2. Create all tree nodes for one testNode one-by-one
            foreach (TestNode testNode in testNodes)
            {
                IList<string> groupNames = GetGroupNames(testNode);
                foreach (string groupName in groupNames)
                    CreateTreeNodes(testNode, groupName);
            }

            // 3. Update tree node names
            TreeNodeNameHandler.UpdateTreeNodeNames(TreeView.Nodes);

            TreeView.TreeView.EndUpdate();
        }

        /// <summary>
        /// Creates (or reuse) all tree nodes along the path from the group to the testNode (representing a test case)
        /// Returns the path of tree nodes from the root tree node (group) to the leaf tree node (test case)
        /// </summary>
        public IList<TreeNode> CreateTreeNodes(TestNode testNode, string groupName)
        {
            TreeNode parentNode = GetOrCreateGroupTreeNode(groupName);
            IList<TestNode> path = GetTestNodePath(testNode);
            IList<TreeNode> treeNodes = CreateTreeNodes(parentNode, path);
            AddTestNodeToTestsGroups(treeNodes, testNode);
            return treeNodes;
        }

        /// <summary>
        /// Retrieves the list of groups in which a testNode is grouped
        /// Derived classes need to override this method
        /// </summary>
        public virtual IList<string> GetGroupNames(TestNode testNode)
        {
            return new List<string>();
        }

        /// <summary>
        /// Called when one test case is finished
        /// </summary>
        public void OnTestFinished(ResultNode result)
        {
            if (!result.IsSuite)
            {
                if (SupportsRegrouping)
                    TreeView.InvokeIfRequired(() => ReGroup(result));
            }
        }

        private void ReGroup(ResultNode resultNode)
        {
            TestNode testNode = Model.GetTestById(resultNode.Id);

            ReGrouping r = new ReGrouping(TreeView, Strategy, this);
            r.Regroup(testNode);
        }

        /// <summary>
        /// Check if a root tree node for the groupName already exists
        /// If not, create new TreeNode
        /// </summary>
        private TreeNode GetOrCreateGroupTreeNode(string groupName)
        {
            foreach (TreeNode treeNode in TreeView.Nodes)
                if (treeNode.Tag is TestGroup testGroup && testGroup.Name == groupName)
                    return treeNode;

            // TreeNode doesn't exist => create it
            TreeNode groupTreeNode = Strategy.MakeTreeNode(new TestGroup(groupName));
            TreeView.Add(groupTreeNode);
            return groupTreeNode;
        }

        /// <summary>
        /// Get a path in the NUnit tree (list of TestNodes) from the root node to one TestNode 
        /// </summary>
        private IList<TestNode> GetTestNodePath(TestNode testNode)
        {
            IList<TestNode> path = new List<TestNode>();
            while (testNode != null && testNode.Type != "TestRun")
            {
                path.Add(testNode);
                testNode = testNode.Parent;
            }

            return path.Reverse().ToList();
        }

        /// <summary>
        /// Creates TreeNodes for all TestNodes within the path.
        /// If a TreeNode already exists, it will be reused.
        /// Returns the list of TreeNodes representing the TestNode path.
        /// </summary>
        private IList<TreeNode> CreateTreeNodes(TreeNode parentTreeNode, IList<TestNode> path)
        {
            IList<TreeNode> treeNodes = new List<TreeNode>() { parentTreeNode };

            for (int i = 0; i < path.Count; i++)
            {
                TestNode testNode = path[i];
                if (!Strategy.ShowTreeNodeType(testNode))
                    continue;

                string name = testNode.Name;

                // Try to get child TreeNode => create not exists create TreeNode
                TreeNode childNode = TryGetChildNode(parentTreeNode, name);
                if (childNode == null)
                    childNode = CreateTreeNode(parentTreeNode, testNode, name);

                // Use childNode as new ParentTreeNode and proceed with next node in path
                parentTreeNode = childNode;
                treeNodes.Add(childNode);
            }

            return treeNodes;
        }

        private TreeNode TryGetChildNode(TreeNode treeNode, string name)
        {
            foreach (TreeNode childNode in treeNode.Nodes)
                if (childNode.Tag is TestGroup group && group.Name == name)
                    return childNode;

            return null;
        }

        private TreeNode CreateTreeNode(TreeNode parentTreeNode, TestNode testNode, string name)
        {
            ITestItem testItem = GetTestNodeForTreeItem(testNode, name);
            TreeNode treeNode = Strategy.MakeTreeNode(testItem);
            parentTreeNode?.Nodes.Add(treeNode);
            return treeNode;
        }

        private ITestItem GetTestNodeForTreeItem(TestNode testNode, string name)
        {
            if (!testNode.IsSuite)
                return testNode;

            TestGroup testGroup = new TestGroup(name);
            return testGroup;
        }

        private void AddTestNodeToTestsGroups(IList<TreeNode> treeNodes, TestNode testNode)
        {
            foreach (TreeNode treeNode in treeNodes)
                if (treeNode.Tag is TestGroup testGroup)
                    testGroup.Add(testNode);
        }
    }
}
