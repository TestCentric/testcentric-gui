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
                _view.Add(MakeTreeNode(topLevelNode, true));

            if (!TryRestoreVisualState())
                SetDefaultInitialExpansion();
        }

        private void SetDefaultInitialExpansion()
        {
            var displayStyle = (InitialTreeExpansion)_settings.Gui.TestTree.InitialTreeDisplay;

            TreeNode firstNode = null;
            foreach (TreeNode node in _view.Nodes)
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
                    if (_view.VisibleNodeCount >= treeNode.GetNodeCount(true))
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
