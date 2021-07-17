// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;
    using Views;

    public class WhenTestCaseCompletes : TreeViewPresenterTestBase
    {
        static object[] resultData = new object[] {
            new object[] { ResultState.Success, TestTreeView.SuccessIndex },
            new object[] { ResultState.Ignored, TestTreeView.WarningIndex },
            new object[] { ResultState.Failure, TestTreeView.FailureIndex },
            new object[] { ResultState.Inconclusive, TestTreeView.InconclusiveIndex },
            new object[] { ResultState.Skipped, TestTreeView.SkippedIndex },
            new object[] { ResultState.NotRunnable, TestTreeView.FailureIndex },
            new object[] { ResultState.Error, TestTreeView.FailureIndex },
            new object[] { ResultState.Cancelled, TestTreeView.FailureIndex }
        };

        [TestCaseSource("resultData")]
        public void TreeShowsProperResult(ResultState resultState, int expectedIndex)
        {
            _model.IsPackageLoaded.Returns(true);
            _model.HasTests.Returns(true);

            var result = resultState.Status.ToString();
            var label = resultState.Label;

            var testNode = new TestNode("<test-run id='1'><test-case id='123'/></test-run>");
            var resultNode = new ResultNode(string.IsNullOrEmpty(label)
                ? string.Format("<test-case id='123' result='{0}'/>", result)
                : string.Format("<test-case id='123' result='{0}' label='{1}'/>", result, label));
            _model.Tests.Returns(testNode);

            //var treeNode = _adapter.MakeTreeNode(result);
            //_adapter.NodeIndex[suiteResult.Id] = treeNode;
            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(testNode));
            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));

            _view.Tree.Received().SetImageIndex(Arg.Compat.Any<TreeNode>(), expectedIndex);
        }
    }
}
