// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    public interface INUnitTreeDisplayStrategy
    {
        /// <summary>
        /// Creates a new tree node for a TestNode
        /// </summary>
        TreeNode MakeTreeNode(TestNode testNode);

        /// <summary>
        /// Creates a new tree node for a TestGroup which is associated to a TestNode
        /// </summary>
        TreeNode MakeTreeNode(TestGroup testGroup, TestNode testNode);

        /// <summary>
        /// Removes one tree node from the tree
        /// </summary>
        void RemoveTreeNode(TreeNode treeNode);

        /// <summary>
        /// Check if a tree node type should be shown or omitted
        /// Currently we support only omitting the namespace nodes
        /// </summary>
        bool ShowTreeNodeType(TestNode testNode);

        /// <summary>
        /// Get the associated tree nodes for one test node
        /// In common there'll be only one single tree node, only for category grouping multiple tree nodes might exists
        /// </summary>
        List<TreeNode> GetTreeNodesForTest(TestNode testNode);

        void UpdateTreeNodeNames();

        void UpdateTreeNodeNames(IEnumerable<TreeNode> treeNodes);

    }
}
