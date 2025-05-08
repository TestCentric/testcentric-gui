// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    public interface INUnitGrouping
    {
        /// <summary>
        /// Creates the complete grouped tree
        /// </summary>
        void CreateTree(TestNode node);

        /// <summary>
        /// Retrieves the list of groups in which a testNode is grouped
        /// </summary>
        IList<string> GetGroupNames(TestNode testNode);

        /// <summary>
        /// Creates (or reuse) all tree nodes along the path from the group to the testNode (representing a test case)
        /// Returns the path of tree nodes from the root tree node (group) to the leaf tree node (test case)
        /// </summary>
        IList<TreeNode> CreateTreeNodes(TestNode testNode, string groupName);
    }
}
