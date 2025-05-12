// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using System;
using TestCentric.Gui.Views;
using System.Collections.Generic;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    /// <summary>
    /// Helper class to determine the image of a tree node
    /// </summary>
    public class TreeNodeImageHandler
    {
        public static void SetTreeNodeImages(ITestTreeView treeView, IEnumerable<TreeNode> treeNodes, bool recursive)
        {
            foreach (TreeNode treeNode in treeNodes)
            {
                SetTreeNodeImage(treeView, treeNode, recursive);
            }
        }

        private static void SetTreeNodeImage(ITestTreeView treeView, TreeNode treeNode, bool recursive)
        {
            int imageIndex = TestTreeView.InitIndex;
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                if (recursive && childNode.ImageIndex <= TestTreeView.InitIndex)
                    SetTreeNodeImage(treeView, childNode, recursive);

                // Ignore index is set to a TreeNode when created => don't propagate up automatically on loading tree
                if (!recursive || childNode.ImageIndex != TestTreeView.IgnoredIndex)
                    imageIndex = Math.Max(imageIndex, childNode.ImageIndex);
            }

            treeView.SetImageIndex(treeNode, imageIndex);
        }
    }
}
