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
    /// GroupDisplayStrategy is the abstract base class for 
    /// DisplayStrategies that list tests in various groupings.
    /// </summary>
    public abstract class GroupDisplayStrategy : DisplayStrategy
    {
        protected TestGrouping _grouping;

        #region Construction and Initialization

        public GroupDisplayStrategy(ITestTreeView view, ITestModel model)
            : base(view, model)
        {
            _view.GroupBy.Enabled = true;
            _view.GroupBy.SelectionChanged += OnGroupByChanged;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Post a test result to the tree, changing the treeNode
        /// color to reflect success or failure. Overridden here
        /// to allow for moving nodes from one group to another
        /// based on the result of running the test.
        /// </summary>
        public override void OnTestFinished(ResultNode result)
        {
            base.OnTestFinished(result);

            if (_grouping != null)
                _grouping.OnTestFinished(result);
        }

        // TODO: Move this to TestGroup? Would need access to results.
        public int CalcImageIndexForGroup(TestGroup group)
        {
            var groupIndex = -1;

            foreach (var testNode in group)
            {
                var result = GetResultForTest(testNode);
                if (result != null)
                {
                    var imageIndex = CalcImageIndex(result.Outcome);

                    if (imageIndex == TestTreeView.FailureIndex)
                        return TestTreeView.FailureIndex; // Early return - can't get any worse!

                    if (imageIndex >= TestTreeView.SuccessIndex) // Only those values propagate
                        groupIndex = Math.Max(groupIndex, imageIndex);
                }
            }

            return groupIndex;
        }

        #endregion

        #region Protected Members

        protected void OnGroupByChanged()
        {
            SetTestGrouping(_view.GroupBy.SelectedItem);

            Reload();
        }

        protected void SetDefaultTestGrouping()
        {
            _grouping = CreateTestGrouping(DefaultGroupSetting);
        }

        protected void SetTestGrouping(string groupBy)
        {
            _grouping = CreateTestGrouping(groupBy);
            DefaultGroupSetting = groupBy;
        }

        protected abstract string DefaultGroupSetting { get; set; }

        protected TestGrouping CreateTestGrouping(string groupBy)
        {
            _view.GroupBy.SelectedItem = groupBy;

            switch (groupBy)
            {
                case "OUTCOME":
                    return new OutcomeGrouping(this);
                case "DURATION":
                    return new DurationGrouping(this);
                case "CATEGORY":
                    return new CategoryGrouping(this, false);
                case "CATEGORY_EXTENDED":
                    return new CategoryGrouping(this, true);
            }

            return null;
        }

        protected void UpdateDisplay()
        {
            if (_grouping != null)
            {
                this.ClearTree();
                TreeNode topNode = null;
                foreach (var group in _grouping.Groups)
                {
                    var treeNode = MakeTreeNode(group, true);
                    group.TreeNode = treeNode;
                    treeNode.Expand();
                    if (group.Count > 0)
                    {
                        _view.Tree.Add(treeNode);
                        if (topNode == null)
                            topNode = treeNode;
                    }
                }
                if (topNode != null)
                    topNode.EnsureVisible();
            }
        }

        #endregion
    }
}
