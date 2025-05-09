// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    internal class TreeNodeNameHandler
    {
        internal static void UpdateTreeNodeNames(TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                UpdateTreeNodeName(treeNode);
                UpdateTreeNodeNames(treeNode.Nodes);
            }
        }

        internal static void UpdateTreeNodeNames(IEnumerable<TreeNode> nodes)
        {
            foreach (TreeNode treeNode in nodes)
                UpdateTreeNodeName(treeNode);
        }

        internal static void UpdateTreeNodeName(TreeNode treeNode)
        {
            if (treeNode.Tag is TestGroup testGroup)
            {
                string groupName = $"{testGroup.Name} ({testGroup.Count()})";
                treeNode.Text = groupName;
            }
        }
    }
}
