// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using System.IO;
    using Model;
    using Views;

    /// <summary>
    /// NUnitTreeDisplayStrategy is used to display a the tests
    /// in the traditional NUnit tree format.
    /// </summary>
    public class NUnitTreeDisplayStrategy : DisplayStrategy
    {
        public NUnitTreeDisplayStrategy(ITestTreeView view, ITestModel model)
            : base(view, model) { }

        public override string Description
        {
            get { return "NUnit Tree"; }
        }

        public override void OnTestLoaded(TestNode testNode)
        {
            ClearTree();

            foreach (var topLevelNode in testNode.Children)
                _view.Tree.Add(MakeTreeNode(topLevelNode, true));

            if (CanRestoreVisualState())
                RestoreVisualState();
            else
                SetDefaultInitialExpansion();
        }

        public override void OnTestUnloading()
        {
            if (CanSaveVisualState())
            {
                var visualState = new VisualState()
                {
                    ShowCheckBoxes = _view.Tree.CheckBoxes,
                    TopNode = ((TestNode)_view.Tree.TopNode?.Tag).Id,
                    SelectedNode = ((TestNode)_view.Tree.SelectedNode?.Tag).Id,
                };

                foreach (TreeNode node in _view.Tree.Nodes)
                    ProcessTreeNodes(node, visualState);

                visualState.Save(VisualState.GetVisualStateFileName(_model.TestFiles[0]));
            }
        }

        private void ProcessTreeNodes(TreeNode node, VisualState visualState)
        {
            if (node.IsExpanded || node.Checked)
                visualState.Nodes.Add(new VisualTreeNode()
                {
                    Id = ((TestNode)node.Tag).Id,
                    Expanded = node.IsExpanded,
                    Checked = node.Checked
                });
                

            foreach (TreeNode childNode in node.Nodes)
                ProcessTreeNodes(childNode, visualState);
        }

        private bool CanSaveVisualState()
        {
            return 
                _settings.Gui.TestTree.SaveVisualState &&
                _model.TestFiles.Count > 0;
        }

        private bool CanRestoreVisualState()
        {
            return
                CanSaveVisualState() &&
                File.Exists(VisualState.GetVisualStateFileName(_model.TestFiles[0]));
        }

        private void RestoreVisualState()
        {
            var filename = VisualState.GetVisualStateFileName(_model.TestFiles[0]);
            var visualState = VisualState.LoadFrom(filename);

            _view.Tree.CheckBoxes = visualState.ShowCheckBoxes;

            foreach (var visualNode in visualState.Nodes)
                foreach (var treeNode in GetTreeNodesForTest(visualNode.Id))
                {
                    if (treeNode.IsExpanded != visualNode.Expanded)
                        treeNode.Toggle();

                    treeNode.Checked = visualNode.Checked;
                }

            if (visualState.SelectedNode != null)
                _view.Tree.SelectedNode = GetTreeNodeForTest(visualState.SelectedNode);

            if (visualState.TopNode != null)
                _view.Tree.TopNode = GetTreeNodeForTest(visualState.TopNode);
        }

        private TreeNode GetTreeNodeForTest(string id)
        {
            var treeNodes = GetTreeNodesForTest(id);
            return treeNodes.Count > 0 ? treeNodes[0] : null;
        }

        private void SetDefaultInitialExpansion()
        {
            var displayStyle = (InitialTreeExpansion)_settings.Gui.TestTree.InitialTreeDisplay;

            TreeNode firstNode = null;
            foreach (TreeNode node in _view.Tree.Nodes)
            {
                SetInitialExpansion(displayStyle, node);
                if (firstNode == null)
                    firstNode = node;
            }

            firstNode?.EnsureVisible();
        }

        private void SetInitialExpansion(InitialTreeExpansion displayStyle, TreeNode treeNode)
        {
            switch (displayStyle)
            {
                case InitialTreeExpansion.Auto:
                    if (_view.Tree.VisibleCount >= treeNode.GetNodeCount(true))
                        treeNode.ExpandAll();
                    else
                        CollapseToFixtures(treeNode);
                    break;
                case InitialTreeExpansion.Expand:
                    treeNode.ExpandAll();
                    break;
                case InitialTreeExpansion.Collapse:
                    treeNode.Collapse();
                    break;
                case InitialTreeExpansion.HideTests:
                    CollapseToFixtures(treeNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(displayStyle), displayStyle, null);
            }
        }
    }
}
