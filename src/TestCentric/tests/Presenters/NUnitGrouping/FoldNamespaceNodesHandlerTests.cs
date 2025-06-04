// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    [TestFixture]
    internal class FoldNamespaceNodesHandlerTests
    {
        [Test]
        public void IsNamespaceNode_NullInput_ReturnsFalse()
        {
            // Arrange

            // Act
            bool isNamespace = FoldNamespaceNodesHandler.IsNamespaceNode(null);

            // Assert
            Assert.That(isNamespace, Is.False);
        }

        [TestCase("TestSuite")]
        [TestCase("SetUpFixture")]
        public void IsNamespaceNode_TestNodeIsSuite_ReturnsTrue(string testSuiteType)
        {
            // Arrange
            TestNode testNode = new TestNode($"<test-suite type='{testSuiteType}' />");

            // Act
            bool isNamespace = FoldNamespaceNodesHandler.IsNamespaceNode(testNode);

            // Assert
            Assert.That(isNamespace, Is.True);
        }

        [Test]
        public void FoldNamespaceNodes_OneNamespaceNode_ReturnsNode()
        {
            // Arrange
            TestNode testNode = new TestNode($"<test-suite type='TestSuite' name='NamespaceA' />");

            // Act
            IList<TestNode> nodes = FoldNamespaceNodesHandler.FoldNamespaceNodes(testNode);

            // Assert
            Assert.That(nodes.Count, Is.EqualTo(1));
        }

        [Test]
        public void FoldNamespaceNodes_AllNamespaceNodesCanBeFolded_ReturnsAllNodes()
        {
            // Arrange
            string xml = 
                "<test-suite type='TestSuite' name='NamespaceA' >" +
                    "<test-suite type='TestSuite' name='NamespaceB' >" +
                        "<test-suite type='TestSuite' name='NamespaceC' />" +
                    "</test-suite>" +
            "</test-suite>";

            TestNode testNode = new TestNode(xml);

            // Act
            IList<TestNode> nodes = FoldNamespaceNodesHandler.FoldNamespaceNodes(testNode);

            // Assert
            Assert.That(nodes.Count, Is.EqualTo(3));
        }

        [Test]
        public void FoldNamespaceNodes_NotAllNamespaceNodesCanBeFolded_ReturnsNodes()
        {
            // Arrange
            string xml =
                "<test-suite type='TestSuite' name='NamespaceA' >" +
                    "<test-suite type='TestSuite' name='NamespaceB' >" +
                        "<test-suite type='TestSuite' name='NamespaceC' />" +
                        "<test-suite type='TestSuite' name='NamespaceD' />" +
                    "</test-suite>" +
            "</test-suite>";

            TestNode testNode = new TestNode(xml);

            // Act
            IList<TestNode> nodes = FoldNamespaceNodesHandler.FoldNamespaceNodes(testNode);

            // Assert
            Assert.That(nodes.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetFoldedNamespaceName_AllNamespaceNodesCanBeFolded_ReturnFoldedName()
        {
            // Arrange
            string xml =
                "<test-suite type='TestSuite' name='NamespaceA' >" +
                    "<test-suite type='TestSuite' name='NamespaceB' >" +
                        "<test-suite type='TestSuite' name='NamespaceC' />" +
                    "</test-suite>" +
            "</test-suite>";

            TestNode testNode = new TestNode(xml);

            // Act
            IList<TestNode> nodes = FoldNamespaceNodesHandler.FoldNamespaceNodes(testNode);
            string foldedName = FoldNamespaceNodesHandler.GetFoldedNamespaceName(nodes);

            // Assert
            Assert.That(foldedName, Is.EqualTo("NamespaceA.NamespaceB.NamespaceC"));
        }

        [Test]
        public void GetFoldedNamespaceName_NotAllNamespaceNodesCanBeFolded_ReturnFoldedName()
        {
            // Arrange
            string xml =
                "<test-suite type='TestSuite' name='NamespaceA' >" +
                    "<test-suite type='TestSuite' name='NamespaceB' >" +
                        "<test-suite type='TestSuite' name='NamespaceC' />" +
                        "<test-suite type='TestSuite' name='NamespaceD' />" +
                    "</test-suite>" +
            "</test-suite>";

            TestNode testNode = new TestNode(xml);

            // Act
            IList<TestNode> nodes = FoldNamespaceNodesHandler.FoldNamespaceNodes(testNode);
            string foldedName = FoldNamespaceNodesHandler.GetFoldedNamespaceName(nodes);

            // Assert
            Assert.That(foldedName, Is.EqualTo("NamespaceA.NamespaceB"));
        }
    }
}
