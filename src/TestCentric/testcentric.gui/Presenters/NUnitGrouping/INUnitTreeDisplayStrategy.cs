// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    public interface INUnitTreeDisplayStrategy
    {
        /// <summary>
        /// Creates a new tree node for one TestNode or TestGroup
        /// </summary>
        TreeNode MakeTreeNode(ITestItem item);

        /// <summary>
        /// Check if a tree node type should be shown or omitted
        /// Currently we support only omitting the namespace nodes
        /// </summary>
        bool ShowTreeNodeType(TestNode testNode);
    }
}
