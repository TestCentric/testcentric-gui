// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Elements;
    using TestCentric.Gui.Model;
    using Views;

    public class TestSelection_WithCheckboxes : TreeViewPresenterTestBase
    {
        private static TestNode TEST_CASE_NODE = new TestNode(XmlHelper.CreateXmlNode("<test-case/>"));
        private static TreeNode TEST_CASE_TREE_NODE = new TreeNode()
        {
            Tag = TEST_CASE_NODE
        };

        private static TestNode TEST_SUITE_NODE = new TestNode(XmlHelper.CreateXmlNode("<test-suite/>"));
        private static TreeNode TEST_SUITE_TREE_NODE = new TreeNode()
        {
            Tag = TEST_SUITE_NODE
        };

        private static TestCaseData[] SingleTestSelectionTests = new TestCaseData[]
        {
            new TestCaseData(TEST_CASE_TREE_NODE, TEST_CASE_NODE),
            new TestCaseData(TEST_SUITE_TREE_NODE, TEST_SUITE_NODE)
        };

        [SetUp]
        public void CheckBoxesOn()
        {
            _view.CheckBoxes.Returns(true);
            _view.ShowCheckBoxes.Checked.Returns(true);
        }

        [TestCaseSource(nameof(SingleTestSelectionTests))]
        public void WhenSelectedNodeChanges_ActiveItemIsSet(TreeNode treeNode, TestNode testNode)
        {
            _view.SelectedNodeChanged += Raise.Event<TreeNodeActionHandler>(treeNode);
            _model.Received().ActiveTestItem = testNode;
        }

        [TestCaseSource(nameof(SingleTestSelectionTests))]
        public void WhenSelectedNodeChanges_SelectedTestsAreNotSet(TreeNode treeNode, TestNode testNode)
        {
            _view.SelectedNodeChanged += Raise.Event<TreeNodeActionHandler>(treeNode);
            _model.DidNotReceiveWithAnyArgs().SelectedTests = null;
        }

        public void WhenCheckedNodesChange_SelectedTestsAreSet()
        {
            _view.CheckedNodes.Returns(new List<TreeNode>( new[] { TEST_CASE_TREE_NODE }));

            _view.AfterCheck += Raise.Event<TreeNodeActionHandler>(TEST_CASE_TREE_NODE);
            _model.Received().SelectedTests = Arg.Is<TestSelection>(s => s.Count == 1 && s.Contains(TEST_CASE_NODE));
        }

        [Test]
        public void WhenCheckBoxesAreTurnedOff_ActiveTestBecomesSelected()
        {
            _view.SelectedNode.Returns(TEST_CASE_TREE_NODE);
            _view.ShowCheckBoxes.CheckedChanged += Raise.Event<CommandHandler>();
        }
    }
}
