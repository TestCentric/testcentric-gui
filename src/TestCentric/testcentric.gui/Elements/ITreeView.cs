// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    public delegate void TreeNodeActionHandler(TreeNode treeNode);

    /// <summary>
    /// The ITreeViewElement interface provides additional methods
    /// used when wrapping a TreeView.
    /// </summary>
    public interface ITreeView : IControlElement
    {
        event TreeNodeActionHandler SelectedNodeChanged;

        bool CheckBoxes { get; set; }
        int VisibleCount { get; }

        TreeNode TopNode { get; set; }

        TreeNode SelectedNode { get; set; }
        TreeNodeCollection Nodes { get; }
        IList<TreeNode> CheckedNodes { get; }

        void Clear();
        void ExpandAll();
        void CollapseAll();
        void Add(TreeNode treeNode);
        void Load(TreeNode treeNode);
        void SetImageIndex(TreeNode treeNode, int imageIndex);
    }
}
