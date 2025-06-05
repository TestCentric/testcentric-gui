// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    /// <summary>
    /// This class is responsible to set/reset the group's duration property before/after a test run
    /// The current approach sums up all individual durations of the test cases included in the group.
    /// Please note that this approach doesn't consider the TestFixture Setup time.
    /// </summary>
    public class TreeNodeDurationHandler
    {
        public static void ClearGroupDurations(IEnumerable<TreeNode> treeNodes)
        {
            foreach (TreeNode treeNode in treeNodes)
            {
                ClearGroupDuration(treeNode);
                ClearGroupDurations(treeNode.Nodes.OfType<TreeNode>());
            }
        }

        private static void ClearGroupDuration(TreeNode treeNode)
        {
            if (treeNode.Tag is TestGroup testGroup)
                testGroup.Duration = null;

        }

        public static void SetGroupDurations(ITestModel model, IEnumerable<TreeNode> treeNodes)
        {
            foreach (TreeNode treeNode in treeNodes)
            {
                SetGroupDurations(model, treeNode.Nodes.OfType<TreeNode>());
                SetGroupDuration(model, treeNode);
            }
        }

        private static void SetGroupDuration(ITestModel model, TreeNode treeNode)
        {
            TestGroup testGroup = treeNode.Tag as TestGroup;
            if (testGroup == null)
                return;

            double duration = 0;
            bool childHasResult = false;
            foreach (TestNode testNode in testGroup)
            {
                ResultNode childResult = model.GetResultForTest(testNode.Id);
                if (childResult != null)
                {
                    duration += childResult.Duration;
                    childHasResult = true;
                }
            }

            if (childHasResult)
                testGroup.Duration = duration;
        }
    }
}
