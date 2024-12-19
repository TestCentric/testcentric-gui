// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using System.Collections.Generic;
    using Elements;

    public delegate void TreeNodeActionHandler(TreeNode treeNode);
    public delegate void MultipleTreeNodeActionHandler(IList<TreeNode> treeNodes);

    // Interface used for testing
    public interface ITestTreeView : IView
    {
        // Events
        event TreeNodeActionHandler SelectedNodeChanged;
        event TreeNodeActionHandler AfterCheck;
        event TreeNodeActionHandler TreeNodeDoubleClick;
        event EventHandler ContextMenuOpening;

        // Commands
        ICommand RunContextCommand { get; }
        ICommand DebugContextCommand { get; }
        IToolStripMenu ActiveConfiguration { get; }
        IChecked ShowCheckBoxes { get; }
        IChecked ShowTestDuration { get; }
        ICommand ExpandAllCommand { get; }
        ICommand CollapseAllCommand { get; }
        ICommand CollapseToFixturesCommand { get; }
        ICommand TestPropertiesCommand { get; }
        ICommand ViewAsXmlCommand { get; }

        // Tree Properties
        ContextMenuStrip TreeContextMenu { get; }

        bool CheckBoxes { get; set; }

        string AlternateImageSet { get; set; }
        int VisibleNodeCount { get; }
        TreeNode TopNode { get; set; }

        TreeView TreeView { get; }
        TreeNodeCollection Nodes { get; }
        TreeNode ContextNode { get; }
        TreeNode SelectedNode { get; set; }
        IList<TreeNode> CheckedNodes { get; }

        // Test Filter related properties / methods
        IMultiSelection OutcomeFilter { get; }

        ISelection TextFilter { get; }

        void SetTestFilterVisibility(bool visible);

        // Tree-related Methods
        void Clear();
        void Add(TreeNode treeNode);
        void ExpandAll();
        void CollapseAll();
        void SetImageIndex(TreeNode treeNode, int imageIndex);

        /// <summary>
        /// Reset the image of all tree nodes to the initial state
        /// </summary>
        void ResetAllTreeNodeImages();

        /// <summary>
        /// Invoke a delegate if necessary, otherwise just call it
        /// </summary>
        void InvokeIfRequired(MethodInvoker _delegate);
    }
}
