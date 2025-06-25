// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    [TestFixture]
    internal class TreeNodeImageHandlerTests
    {
        [Test]
        public void SetTreeNodeImages_AllImagesAreSet()
        {
            // Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();

            TestGroup testGroup1 = new TestGroup("Group_1") { ImageIndex = 10 };
            TestGroup testGroup2 = new TestGroup("Group_2") { ImageIndex = 20 };
            TestGroup testGroup3 = new TestGroup("Group_3") { ImageIndex = 30 };

            // Act
            TreeNodeImageHandler.SetTreeNodeImages(treeView, new[] { testGroup1, testGroup2, testGroup3});

            // Assert
            treeView.Received().SetImageIndex(testGroup1.TreeNode, testGroup1.ImageIndex);
            treeView.Received().SetImageIndex(testGroup2.TreeNode, testGroup2.ImageIndex);
            treeView.Received().SetImageIndex(testGroup3.TreeNode, testGroup3.ImageIndex);

        }

        [TestCase(TestTreeView.InitIndex, "Passed", TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.InitIndex, "Failed", TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.SuccessIndex, "Failed", TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.SuccessIndex, "Warning", TestTreeView.WarningIndex)]
        [TestCase(TestTreeView.SuccessIndex, "Failed", TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.SuccessIndex, "Passed", TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.InconclusiveIndex, "Warning", TestTreeView.WarningIndex)]
        [TestCase(TestTreeView.WarningIndex, "Passed", TestTreeView.WarningIndex)]
        [TestCase(TestTreeView.FailureIndex, "Failed", TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.FailureIndex, "Passed", TestTreeView.FailureIndex)]
        [TestCase(TestTreeView.FailureIndex, "Warning", TestTreeView.FailureIndex)]
        public void OnTestFinished_TestCase_GroupIndexIsUpdated(int groupIndex, string outcome, int expectedGroupIndex)
        {
            // Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();

            TestGroup testGroup1 = new TestGroup("Group_1") { ImageIndex = groupIndex };
            TestGroup testGroup2 = new TestGroup("Group_2") { ImageIndex = groupIndex };
            TestGroup testGroup3 = new TestGroup("Group_3") { ImageIndex = groupIndex };

            var resultNode = new ResultNode($"<test-case id='1' result='{outcome}' />");

            // Act
            TreeNodeImageHandler.OnTestFinished(resultNode, new[] { testGroup1, testGroup2, testGroup3 }, treeView);

            // Assert
            Assert.That(testGroup1.ImageIndex, Is.EqualTo(expectedGroupIndex));
            Assert.That(testGroup2.ImageIndex, Is.EqualTo(expectedGroupIndex));
            Assert.That(testGroup3.ImageIndex, Is.EqualTo(expectedGroupIndex));
        }

        [TestCase(TestTreeView.InitIndex)]
        [TestCase(TestTreeView.SuccessIndex)]
        [TestCase(TestTreeView.FailureIndex)]
        public void OnTestFinished_TestFixture_TreeviewImageIndex_IsSet(int groupIndex)
        {
            // Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();

            TestGroup testGroup1 = new TestGroup("Group_1") { ImageIndex = groupIndex };
            TestGroup testGroup2 = new TestGroup("Group_2") { ImageIndex = groupIndex };
            TestGroup testGroup3 = new TestGroup("Group_3") { ImageIndex = groupIndex };

            var resultNode = new ResultNode($"<test-suite id='1' result='Passed' />");

            // Act
            TreeNodeImageHandler.OnTestFinished(resultNode, new[] { testGroup1, testGroup2, testGroup3 }, treeView);

            // Assert
            treeView.Received().SetImageIndex(testGroup1.TreeNode, testGroup1.ImageIndex);
            treeView.Received().SetImageIndex(testGroup2.TreeNode, testGroup2.ImageIndex);
            treeView.Received().SetImageIndex(testGroup3.TreeNode, testGroup3.ImageIndex);
        }
    }
}
