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
        public void OnStrategyCreated_OutcomeFilter_IsVisible()
        {
            // Assert
            _view.Received().SetTestFilterVisibility(true);
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

        [Test]
        public void OnTestLoaded_Namespaces_AreShown_NamespaceNodes_AreCreated()
        {
            // Arrange
            _settings.Gui.TestTree.ShowNamespace = true;
            string xml = 
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='TestSuite' id='1-1031' name='Library'>" +
                    "</test-suite>" +
                "</test-suite>";

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            _view.Received().Add(Arg.Compat.Is<TreeNode>(tn => (tn.Tag as TestNode).Id == "1-1031"));
        }

        [Test]
        public void OnTestLoaded_Namespaces_AreNotShown_NamespaceNodes_AreNotCreated()
        {
            // Arrange
            _settings.Gui.TestTree.ShowNamespace = false;
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='TestSuite' id='1-1031' name='Library'>" +
                    "</test-suite>" +
                "</test-suite>";

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            _view.DidNotReceive().Add(Arg.Compat.Is<TreeNode>(tn => (tn.Tag as TestNode).Id == "1-1031"));
        }

        [Test]
        public void OnTestLoaded_NamespacesNodes_ContainingOneNamespace_AreFolded()
        {
            // Arrange
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='TestSuite' id='1-1031' name='Library'>" +
                        "<test-suite type='TestSuite' id='1-1032' name='Test'>" +
                            "<test-suite type='TestSuite' id='1-1033' name='Folder'>" +
                            "</test-suite>" +
                        "</test-suite>" +
                    "</test-suite>" +
                "</test-suite>";

            TreeNode treeNode = null;
            _view.Add(Arg.Do<TreeNode>(tn => treeNode = tn));

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            Assert.That((treeNode.Tag as TestNode).Id, Is.EqualTo("1-1031"));
            Assert.That(treeNode.Text, Is.EqualTo("Library.Test.Folder"));
            Assert.That(treeNode.Nodes.Count, Is.EqualTo(0));
        }

        [Test]
        public void OnTestLoaded_NamespacesNode_ContainingTwoNamespaces_AreNotFolded()
        {
            // Arrange
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='TestSuite' id='1-1031' name='Library'>" +
                        @"<test-suite type='TestSuite' id='1-1032' name='Test' />" +
                        @"<test-suite type='TestSuite' id='1-1033' name='Folder' />" +
                    "</test-suite>" +
                "</test-suite>";

            TreeNode treeNode = null;
            _view.Add(Arg.Do<TreeNode>(tn => treeNode = tn));

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            Assert.That((treeNode.Tag as TestNode).Id, Is.EqualTo("1-1031"));
            Assert.That(treeNode.Text, Is.EqualTo("Library"));

            var child1 = treeNode.Nodes[0];
            Assert.That((child1.Tag as TestNode).Id, Is.EqualTo("1-1032"));
            Assert.That(child1.Text, Is.EqualTo("Test"));

            var child2 = treeNode.Nodes[1];
            Assert.That((child2.Tag as TestNode).Id, Is.EqualTo("1-1033"));
            Assert.That(child2.Text, Is.EqualTo("Folder"));
        }

        [Test]
        public void OnTestLoaded_SetupFixtureNode_ContainingOneNamespace_AreFolded()
        {
            // Arrange
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='SetUpFixture' id='1-1031' name='Library'>" +
                        "<test-suite type='TestSuite' id='1-1032' name='Test'>" +
                            "<test-suite type='SetUpFixture' id='1-1033' name='Folder'>" +
                            "</test-suite>" +
                        "</test-suite>" +
                    "</test-suite>" +
                "</test-suite>";

            TreeNode treeNode = null;
            _view.Add(Arg.Do<TreeNode>(tn => treeNode = tn));

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            Assert.That((treeNode.Tag as TestNode).Id, Is.EqualTo("1-1031"));
            Assert.That(treeNode.Text, Is.EqualTo("Library.Test.Folder"));
            Assert.That(treeNode.Nodes.Count, Is.EqualTo(0));
        }

        [Test]
        public void OnTestLoaded_SetupFixtureNode_ContainingTwoNamespaces_AreNotFolded()
        {
            // Arrange
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                    "<test-suite type='SetUpFixture' id='1-1031' name='Library'>" +
                        @"<test-suite type='TestSuite' id='1-1032' name='Test' />" +
                        @"<test-suite type='TestSuite' id='1-1033' name='Folder' />" +
                    "</test-suite>" +
                "</test-suite>";

            TreeNode treeNode = null;
            _view.Add(Arg.Do<TreeNode>(tn => treeNode = tn));

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            Assert.That((treeNode.Tag as TestNode).Id, Is.EqualTo("1-1031"));
            Assert.That(treeNode.Text, Is.EqualTo("Library"));

            var child1 = treeNode.Nodes[0];
            Assert.That((child1.Tag as TestNode).Id, Is.EqualTo("1-1032"));
            Assert.That(child1.Text, Is.EqualTo("Test"));

            var child2 = treeNode.Nodes[1];
            Assert.That((child2.Tag as TestNode).Id, Is.EqualTo("1-1033"));
            Assert.That(child2.Text, Is.EqualTo("Folder"));
        }

        [Test]
        public void OnTestLoaded_OutcomeFilter_IsEnabled()
        {
            // Arrange
            string xml =
                "<test-suite type='Assembly' id='1-1030' name='Library.Test.dll'>" +
                "</test-suite>";

            // Act
            _strategy.OnTestLoaded(new TestNode(xml), null);

            // Assert
            _view.OutcomeFilter.Received().Enabled = true;
        }

        [Test]
        public void OnTestUnloaded_OutcomeFilter_IsDisabled()
        {
            // Arrange + Act
            _strategy.OnTestUnloaded();

            // Assert
            _view.OutcomeFilter.Received().Enabled = false;
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
