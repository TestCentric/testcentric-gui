// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using System;
using TestCentric.Gui.Views;
using System.Collections.Generic;
using System.Linq;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    /// <summary>
    /// Helper class to determine the image of a tree node
    /// </summary>
    public class TreeNodeImageHandler
    {
        public static void SetTreeNodeImages(ITestTreeView treeView, IEnumerable<TestGroup> groups)
        {
            foreach (TestGroup group in groups)
                treeView.SetImageIndex(group.TreeNode, group.ImageIndex);
        }

        public static void OnTestFinished(ResultNode result, IList<TestGroup> groups, ITestTreeView treeView)
        {
            if (!result.IsSuite)
            {
                int imageIndex = DisplayStrategy.CalcImageIndex(result);
                foreach (TestGroup group in groups)
                    group.ImageIndex = Math.Max(imageIndex, group.ImageIndex);
            } 
            else
                foreach (TestGroup group in groups)
                    treeView.SetImageIndex(group.TreeNode, group.ImageIndex);
        }
    }
}
