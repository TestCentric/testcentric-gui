// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    [TestFixture]
    internal class TreeNodeNameHandlerTests
    {
        [Test]
        public void UpdateTreeNodeName_GroupWithoutNodes()
        {
            // Arrange
            TestGroup testGroup = new TestGroup("CategoryA");

            TreeNode tn = new TreeNode();
            tn.Tag = testGroup;

            // Act
            TreeNodeNameHandler.UpdateTreeNodeName(tn);

            // Assert
            Assert.That(tn.Text, Is.EqualTo("CategoryA (0)"));
        }

        [Test]
        public void UpdateTreeNodeName_GroupWithNodes()
        {
            // Arrange
            TestNode testNode1 = new TestNode($"<test-case name='TestA' /> ");
            TestNode testNode2 = new TestNode($"<test-case name='TestA' /> ");

            TestGroup testGroup = new TestGroup("CategoryA") { testNode1, testNode2 };

            TreeNode tn = new TreeNode();
            tn.Tag = testGroup;

            // Act
            TreeNodeNameHandler.UpdateTreeNodeName(tn);

            // Assert
            Assert.That(tn.Text, Is.EqualTo("CategoryA (2)"));
        }

        [Test]
        public void UpdateTreeNodeName_TestNode()
        {
            // Arrange
            TestNode testNode = new TestNode($"<test-case name='TestA' /> ");

            TreeNode tn = new TreeNode("TreeNodeName");
            tn.Tag = testNode;

            // Act
            TreeNodeNameHandler.UpdateTreeNodeName(tn);

            // Assert
            Assert.That(tn.Text, Is.EqualTo("TreeNodeName"));
        }
    }
}
