// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Model;
    using NUnit.Engine;

    /// <summary>
    /// Type safe TreeNode for use in the TestSuiteTreeView. 
    /// NOTE: Hides some methods and properties of base class.
    /// </summary>
    public class TestSuiteTreeNode : TreeNode
    {
        #region Instance variables and constant definitions

        /// <summary>
        /// Image indices for various test states - the values 
        /// must match the indices of the image list used
        /// </summary>
        public const int InitIndex = 0;
        public const int SkippedIndex = 0;
        public const int FailureIndex = 1;
        public const int SuccessIndex = 2;
        public const int IgnoredIndex = 3;
        public const int WarningIndex = 3;
        public const int InconclusiveIndex = 4;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a TestNode given a test
        /// </summary>
        public TestSuiteTreeNode(TestNode test, TestPackage package=null) : base(test.Name)
        {
            Test = test;
            TestPackage = package;
            UpdateImageIndex();
        }

        /// <summary>
        /// Construct a TestNode given a TestResult
        /// </summary>
        public TestSuiteTreeNode(ResultNode result, TestPackage package = null) : base(result.Name)
        {
            Test = Result = result;
            TestPackage = package;
            UpdateImageIndex();
        }

        #endregion

        #region Properties	
        /// <summary>
        /// Test represented by this node
        /// </summary>
        public TestNode Test { get; set; }

        /// <summary>
        /// TestPackage this test represents or null if none
        /// </summary>
        public TestPackage TestPackage { get; set; }

        /// <summary>
        /// Test result for this node
        /// </summary>
        public ResultNode Result
        {
            get { return _result; }
            set
            {
                _result = value;
                UpdateImageIndex();
            }
        }
        private ResultNode _result;

        /// <summary>
        /// Return true if the node has a result, otherwise false.
        /// </summary>
        public bool HasResult
        {
            get { return _result != null; }
        }

        public string TestType
        {
            get { return Test.Type; }
        }

        public string StatusText
        {
            get
            {
                return HasResult ? Result.Outcome.ToString() : Test.RunState.ToString();
            }
        }

        public bool Included
        {
            get { return _included; }
            set
            {
                _included = value;
                this.ForeColor = _included ? SystemColors.WindowText : Color.LightBlue;
            }
        }
        private bool _included = true;

        public bool ShowFailedAssumptions
        {
            get { return _showFailedAssumptions; }
            set
            {
                if (value != _showFailedAssumptions)
                {
                    _showFailedAssumptions = value;

                    if (HasInconclusiveResults)
                        RepopulateTheoryNode();
                }
            }
        }
        private bool _showFailedAssumptions = false;

        public bool HasInconclusiveResults
        {
            get
            {
                bool hasInconclusiveResults = false;

                if (HasResult)
                {
                    foreach (ResultNode result in Result.Children)
                    {
                        hasInconclusiveResults |= result.Outcome == ResultState.Inconclusive;
                        if (hasInconclusiveResults)
                            break;
                    }
                }

                return hasInconclusiveResults;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// UPdate the image index based on the result field
        /// </summary>
        public void UpdateImageIndex()
        {
            InvokeIfRequired(() => ImageIndex = SelectedImageIndex = CalcImageIndex());
        }

        /// <summary>
        /// Clear the result of this node and all its children
        /// </summary>
        public void ClearResults()
        {
            Result = null;

            foreach (TestSuiteTreeNode node in Nodes)
                node.ClearResults();
        }

        /// <summary>
        /// Gets the Theory node associated with the current
        /// node. If the current node is a Theory, then the
        /// current node is returned. Otherwise, if the current
        /// node is a test case under a theory node, then that
        /// node is returned. Otherwise, null is returned.
        /// </summary>
        /// <returns></returns>
        public TestSuiteTreeNode GetTheoryNode()
        {
            if (this.Test.Type == "Theory")
                return this;

            TestSuiteTreeNode parent = this.Parent as TestSuiteTreeNode;
            if (parent != null && parent.Test.Type == "Theory")
                return parent;

            return null;
        }

        /// <summary>
        /// Regenerate the test cases under a theory, respecting
        /// the current setting for ShowFailedAssumptions
        /// </summary>
        public void RepopulateTheoryNode()
        {
            // Ignore if it's not a theory or if it has not been run yet
            if (TestType == "Theory" && HasResult)
            {
                Nodes.Clear();

                foreach (ResultNode result in Result.Children)
                    if (ShowFailedAssumptions || result.Outcome != ResultState.Inconclusive)
                        Nodes.Add(new TestSuiteTreeNode(result));
            }
        }

        /// <summary>
        /// Calculate the image index based on the node contents
        /// </summary>
        /// <returns>Image index for this node</returns>
        private int CalcImageIndex()
        {
            if (HasResult)
            {
                switch (Result.Outcome.Status)
                {
                    case TestStatus.Inconclusive:
                        return InconclusiveIndex;
                    case TestStatus.Skipped:
                        return Result.Outcome.Label == "Ignored" ? WarningIndex : SkippedIndex;
                    case TestStatus.Failed:
                        return FailureIndex;
                    case TestStatus.Warning:
                        return WarningIndex;
                    case TestStatus.Passed:
                        int resultIndex = SuccessIndex;
                        foreach (TestSuiteTreeNode node in this.Nodes)
                        {
                            if (node.ImageIndex == FailureIndex)
                                return FailureIndex; // Return FailureIndex if there is any failure
                            if (node.ImageIndex == WarningIndex)
                                resultIndex = WarningIndex; // Remember WarningIndex - we might still find a failure
                        }
                        return resultIndex;
                    default:
                        return InitIndex;
                }
            }
            else
            {
                switch (Test.RunState)
                {
                    case RunState.Ignored:
                        return IgnoredIndex;
                    case RunState.NotRunnable:
                        return FailureIndex;
                    default:
                        return InitIndex;
                }
            }
        }

        internal void Accept(TestSuiteTreeNodeVisitor visitor)
        {
            visitor.Visit(this);
            foreach (TestSuiteTreeNode node in this.Nodes)
            {
                node.Accept(visitor);
            }
        }

        private void InvokeIfRequired(MethodInvoker del)
        {
            if (TreeView != null && TreeView.InvokeRequired)
                TreeView.BeginInvoke(del);
            else
                del();
        }

        #endregion
    }

    public abstract class TestSuiteTreeNodeVisitor
    {
        public abstract void Visit(TestSuiteTreeNode node);
    }
}

