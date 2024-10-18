// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using System.Collections.Generic;
    using Model;
    using Views;

    public abstract class DisplayStrategyTests
    {
        protected ITestTreeView _view;
        protected ITestModel _model;
        protected Model.Settings.UserSettings _settings;
        protected DisplayStrategy _strategy;

        [SetUp]
        public void CreateDisplayStrategy()
        {
            _view = Substitute.For<ITestTreeView>();
            _model = Substitute.For<ITestModel>();
            _settings = new Fakes.UserSettings();
            _model.Settings.Returns(_settings);

            // We can't construct a TreeNodeCollection, so we fake it
            var nodes = new TreeNode().Nodes;
            nodes.Add(new TreeNode("test.dll"));
            _view.Nodes.Returns(nodes);

            var project = new TestCentricProject(_model, "dummy.dll");
            _model.TestCentricProject.Returns(project);

            _strategy = GetDisplayStrategy();
        }

        protected abstract DisplayStrategy GetDisplayStrategy();

        [Test]
        public void WhenTestsAreLoaded_TreeViewIsLoaded()
        {
            _strategy.OnTestLoaded(
                new TestNode("<test-run id='1'><test-suite id='42'/><test-suite id='99'/></test-run>"),
                null);

            _view.Received().Clear();
            _view.Received().Add(Arg.Compat.Is<TreeNode>((tn) => (tn.Tag as TestNode).Id == "42"));
            _view.Received().Add(Arg.Compat.Is<TreeNode>((tn) => (tn.Tag as TestNode).Id == "99"));
        }

        [Test]
        public void WhenTestsAreUnloaded_TreeViewIsCleared()
        {
            _strategy.OnTestUnloaded();

            _view.Received().Clear();
        }
    }

    public class NUnitTreeDisplayStrategyTests : DisplayStrategyTests
    {
        protected override DisplayStrategy GetDisplayStrategy()
        {
            return new NUnitTreeDisplayStrategy(_view, _model);
        }

        [Test]
        public void OnTestRunStarting_ResetAllTreeNodeImages_IsInvoked()
        {
            // Act
            _strategy.OnTestRunStarting();

            // Assert
            _view.Received().ResetAllTreeNodeImages();
        }

        [Test]
        public void OnTestRunStarting_UpdateTreeNodeNames_IsInvoked()
        {
            // Assert
            TestNode testNode = new TestNode("<test-case id='1' name='Test1'/>");
            var treeNode = _strategy.MakeTreeNode(testNode, false);
            _view.InvokeIfRequired(Arg.Do<MethodInvoker>(x => x.Invoke()));
            _view.Nodes.Add(treeNode);

            // Act
            _strategy.OnTestRunStarting();

            // Assert
            Assert.That(treeNode.Text, Does.Match("Test1"));
        }

        [TestCase("Skipped", 0)]
        [TestCase("Inconclusive", 1)]
        [TestCase("Passed", 2)]
        [TestCase("Warning", 3)]
        [TestCase("Failed", 4)]
        public void OnTestFinished_TreeNodeImage_IsUpdated(string testResult, int expectedImageIndex)
        {
            // Arrange
            TestNode testNode = new TestNode("<test-case id='1' />");
            var treeNode = _strategy.MakeTreeNode(testNode, false);
            ResultNode result = new ResultNode($"<test-case id='1' result='{testResult}'/>");

            // Act
            _strategy.OnTestFinished(result);

            // Assert
            _view.Received().SetImageIndex(treeNode, expectedImageIndex);
        }

        [Test]
        public void OnTestFinished_ShowDurationIsInactive_TreeNodeName_IsUpdated()
        {
            // Arrange
            TestNode testNode = new TestNode("<test-case id='1' name='Test1'/>");
            var treeNode = _strategy.MakeTreeNode(testNode, false);
            ResultNode result = new ResultNode($"<test-case id='1' result='Passed'/>");
            _model.GetResultForTest(testNode.Id).Returns(result);
            _view.InvokeIfRequired(Arg.Do<MethodInvoker>(x => x.Invoke()));

            // Act
            _strategy.OnTestFinished(result);

            // Assert
            Assert.That(treeNode.Text, Is.EqualTo("Test1"));
        }

        [Test]
        public void OnTestFinished_ShowDurationIsActive_TreeNodeName_IsUpdated()
        {
            // Arrange
            _settings.Gui.TestTree.ShowTestDuration = true;
            TestNode testNode = new TestNode("<test-case id='1' name='Test1'/>");
            var treeNode = _strategy.MakeTreeNode(testNode, false);
            ResultNode result = new ResultNode($"<test-case id='1' result='Passed' duration='1.5'/>");
            _model.GetResultForTest(testNode.Id).Returns(result);
            _view.InvokeIfRequired(Arg.Do<MethodInvoker>(x => x.Invoke()));

            // Act
            _strategy.OnTestFinished(result);

            // Assert
            Assert.That(treeNode.Text, Does.Match(@"Test1 \[1[,.]500s\]"));
        }

        [Test]
        public void MakeTreeNode_ShowDurationIsActive_TreeNodeName_ContainsDuration()
        {
            // Arrange
            _settings.Gui.TestTree.ShowTestDuration = true;
            TestNode testNode = new TestNode("<test-case id='1' name='Test1'/>");
            ResultNode result = new ResultNode($"<test-case id='1' result='Passed' duration='1.5'/>");
            _model.GetResultForTest(testNode.Id).Returns(result);

            // Act
            var treeNode = _strategy.MakeTreeNode(testNode, false);

            // Assert
            Assert.That(treeNode.Text, Does.Match(@"Test1 \[1[,.]500s\]"));
        }

        [Test]
        public void MakeTreeNode_ShowDurationIsInactive_TreeNodeName_ContainsTestName()
        {
            // Arrange
            _settings.Gui.TestTree.ShowTestDuration = false;
            TestNode testNode = new TestNode("<test-case id='1' name='Test1'/>");
            ResultNode result = new ResultNode($"<test-case id='1' result='Passed' duration='1.5'/>");
            _model.GetResultForTest(testNode.Id).Returns(result);

            // Act
            var treeNode = _strategy.MakeTreeNode(testNode, false);

            // Assert
            Assert.That(treeNode.Text, Does.Match(@"Test1"));
        }
    }


    //public class NUnitTestListStrategyTests : DisplayStrategyTests
    //{
    //    protected override DisplayStrategy GetDisplayStrategy()
    //    {
    //        return new TestListDisplayStrategy(_view, _model);
    //    }
    //}
}
