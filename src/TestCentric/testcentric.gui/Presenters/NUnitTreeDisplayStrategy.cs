// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    /// <summary>
    /// NUnitTreeDisplayStrategy is used to display a the tests
    /// in the traditional NUnit tree format.
    /// </summary>
    public class NUnitTreeDisplayStrategy : DisplayStrategy
    {
        #region Construction and Initialization

        public NUnitTreeDisplayStrategy(ITestTreeView view, ITestModel model) : base(view, model)
        {
            //_view.GroupBy.Enabled = false;
            //_view.CollapseToFixturesCommand.Enabled = true;
        }

        #endregion

        public override string Description
        {
            get { return "NUnit Tree"; }
        }

        public override void OnTestLoaded(TestNode testNode)
        {
            var displayStyle = _settings.Gui.TestTree.InitialTreeDisplay;
            ClearTree();

            TreeNode topNode = null;
            foreach (var topLevelNode in testNode.Children)
            {
                var treeNode = MakeTreeNode(topLevelNode, true);

                if (topNode == null)
                    topNode = treeNode;

                _view.Tree.Add(treeNode);

                SetInitialExpansion((InitialTreeExpansion)displayStyle, treeNode);
            }

            topNode?.EnsureVisible();
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
