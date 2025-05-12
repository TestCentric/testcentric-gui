// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    [TestFixture]
    internal class TreeNodeImageHandlerTests
    {
        [Test]
        public void SetTreeNodeImages_NoChildNodes_Set_InitIndex()
        {
            // Arrange
            TreeNode treeNode = new TreeNode();
            var treeNodes = new List<TreeNode>() { treeNode };
            ITestTreeView treeView = Substitute.For<ITestTreeView>();

            // Act
            TreeNodeImageHandler.SetTreeNodeImages(treeView, treeNodes, false);

            // Assert
            treeView.Received().SetImageIndex(treeNode, TestTreeView.InitIndex);
        }

        [TestCase(TestTreeView.FailureIndex, TestTreeView.FailureIndex, TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.FailureIndex, TestTreeView.SuccessIndex, TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.InitIndex, TestTreeView.SuccessIndex, TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.InconclusiveIndex, TestTreeView.SuccessIndex, TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.IgnoredIndex, TestTreeView.SuccessIndex, TestTreeView.IgnoredIndex)]
        public void SetTreeNodeImages_WithChildNodes_Set_InitIndex(int childImageIndex1, int childImageIndex2, int expectedImageIndex)
        {
            // Arrange
            TreeNode treeNode = new TreeNode();

            CreateChildNode(treeNode, childImageIndex1);
            CreateChildNode(treeNode, childImageIndex2);

            var treeNodes = new List<TreeNode>() { treeNode };


            ITestTreeView treeView = Substitute.For<ITestTreeView>();

            // Act
            TreeNodeImageHandler.SetTreeNodeImages(treeView, treeNodes, false);

            // Assert
            treeView.Received().SetImageIndex(treeNode, expectedImageIndex);
        }

        [TestCase(TestTreeView.FailureIndex, TestTreeView.FailureIndex, TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.FailureIndex, TestTreeView.SuccessIndex, TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.InitIndex, TestTreeView.SuccessIndex, TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.InconclusiveIndex, TestTreeView.SuccessIndex, TestTreeView.SuccessIndex)]
        public void SetTreeNodeImagesRecursive_WithChildNodes_Set_InitIndex(int childImageIndex1, int childImageIndex2, int expectedImageIndex)
        {
            // Arrange
            TreeNode treeNode = new TreeNode();
            TreeNode childNode = CreateChildNode(treeNode, TestTreeView.InitIndex);

            CreateChildNode(childNode, childImageIndex1);
            CreateChildNode(childNode, childImageIndex2);

            var treeNodes = new List<TreeNode>() { treeNode };
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            treeView.When(t => t.SetImageIndex(Arg.Any<TreeNode>(), Arg.Any<int>())).Do(x => x.ArgAt<TreeNode>(0).ImageIndex = x.ArgAt<int>(1));

            // Act
            TreeNodeImageHandler.SetTreeNodeImages(treeView, treeNodes, true);

            // Assert
            treeView.Received().SetImageIndex(treeNode, expectedImageIndex);
            treeView.Received().SetImageIndex(childNode, expectedImageIndex);
        }

        private TreeNode CreateChildNode(TreeNode treeNode, int imageIndex)
        {
            TreeNode childNode = new TreeNode();
            childNode.ImageIndex = imageIndex;
            treeNode.Nodes.Add(childNode);

            return childNode;
        }
    }
}
