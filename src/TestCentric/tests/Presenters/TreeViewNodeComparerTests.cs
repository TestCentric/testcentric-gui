// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters
{
    [TestFixture]
    public class TreeViewNodeComparerTests
    {
        [TestCase(TreeViewNodeComparer.Name, TreeViewNodeComparer.Ascending, false, "NameComparer")]
        [TestCase(TreeViewNodeComparer.Name, TreeViewNodeComparer.Ascending, true, "FullnameComparer")]
        [TestCase(TreeViewNodeComparer.Name, TreeViewNodeComparer.Descending, false, "NameComparer")]
        [TestCase(TreeViewNodeComparer.Name, TreeViewNodeComparer.Descending, true, "FullnameComparer")]
        [TestCase(TreeViewNodeComparer.Duration, TreeViewNodeComparer.Ascending, false, "DurationComparer")]
        [TestCase(TreeViewNodeComparer.Duration, TreeViewNodeComparer.Descending, true, "DurationComparer")]
        public void GetComparer(string sortMode, string sortDirection, bool showNamespace, string expectedComparerName)
        {
            // Arrange
            ITestModel model = Substitute.For<ITestModel>();

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, sortMode, sortDirection, showNamespace);

            // Assert
            string classname = comparer.GetType().Name;
            Assert.That(classname, Is.EqualTo(expectedComparerName));
        }

        [TestCase("a", "b", -1)]
        [TestCase("aba", "aaa", 1)]
        [TestCase("abc", "abc", 0)]
        [TestCase("", "", 0)]
        public void Namecompare_Ascending_ReturnsExpectedResult(string text1, string text2, int expectedCompareResult)
        {
            // Arrange
            ITestModel model = Substitute.For<ITestModel>();
            TreeNode treeNode1 = new TreeNode(text1);
            TreeNode treeNode2 = new TreeNode(text2);


            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Name, TreeViewNodeComparer.Ascending, false);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("a", "b", 1)]
        [TestCase("aba", "aaa", -1)]
        [TestCase("abc", "abc", 0)]
        [TestCase("", "", 0)]
        public void Namecompare_Descending_ReturnsExpectedResult(string text1, string text2, int expectedCompareResult)
        {
            // Arrange
            ITestModel model = Substitute.For<ITestModel>();
            TreeNode treeNode1 = new TreeNode(text1);
            TreeNode treeNode2 = new TreeNode(text2);


            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Name, TreeViewNodeComparer.Descending, false);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("Namespace1.A", "Namespace1.B", -1)]
        [TestCase("Namespace1.B", "Namespace1.A", 1)]
        [TestCase("Namespace1.A", "Namespace1.A", 0)]
        [TestCase("Namespace1.A", "Namespace1.a", 1)]
        [TestCase("a", "b", -1)]
        [TestCase("aba", "aaa", 1)]
        [TestCase("", "", 0)]
        public void Fullnamecompare_Ascending_ReturnsExpectedResult(string text1, string text2, int expectedCompareResult)
        {
            // Arrange
            ITestModel model = Substitute.For<ITestModel>();
            TestNode testNode1 = new TestNode($"<test-start fullname='{text1}'/>");
            TestNode testNode2 = new TestNode($"<test-start fullname='{text2}'/>");

            TreeNode treeNode1 = new TreeNode() { Tag = testNode1 };
            TreeNode treeNode2 = new TreeNode() { Tag = testNode2 };

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Name, TreeViewNodeComparer.Ascending, true);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("Namespace1.A", "Namespace1.B", 1)]
        [TestCase("Namespace1.B", "Namespace1.A", -1)]
        [TestCase("Namespace1.A", "Namespace1.A", 0)]
        [TestCase("Namespace1.A", "Namespace1.a", -1)]
        [TestCase("", "", 0)]
        public void Fullnamecompare_Descending_ReturnsExpectedResult(string text1, string text2, int expectedCompareResult)
        {
            // Arrange
            ITestModel model = Substitute.For<ITestModel>();

            TestNode testNode1 = new TestNode($"<test-start fullname='{text1}'/>");
            TestNode testNode2 = new TestNode($"<test-start fullname='{text2}'/>");

            TreeNode treeNode1 = new TreeNode() { Tag = testNode1 };
            TreeNode treeNode2 = new TreeNode() { Tag = testNode2 };

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Name, TreeViewNodeComparer.Descending, true);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("2", "4", -1)]
        [TestCase("0.5", "0.8", -1)]
        [TestCase("0.8", "1.2", -1)]
        [TestCase("0.8", "0.5", 1)]
        [TestCase("0.8", "0.8", 0)]
        public void Durationcompare_Ascending_ReturnsExpectedResult(string duration1, string duration2, int expectedCompareResult)
        {
            // Arrange
            TestNode testNode1 = new TestNode($"<test-start id='1'/>");
            TestNode testNode2 = new TestNode($"<test-start id='2'/>");
            var resultNode1 = new ResultNode($"<test-case id='1' duration='{duration1}'/>");
            var resultNode2 = new ResultNode($"<test-case id='2' duration='{duration2}'/>");
            ITestModel model = Substitute.For<ITestModel>();
            model.GetResultForTest("1").Returns(resultNode1);
            model.GetResultForTest("2").Returns(resultNode2);

            TreeNode treeNode1 = new TreeNode() { Tag = testNode1 };
            TreeNode treeNode2 = new TreeNode() { Tag = testNode2 };

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Duration, TreeViewNodeComparer.Ascending, true);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("2", "4", 1)]
        [TestCase("0.5", "0.8", 1)]
        [TestCase("0.8", "1.2", 1)]
        [TestCase("0.8", "0.5", -1)]
        [TestCase("0.8", "0.8", 0)]
        public void Durationcompare_Descending_ReturnsExpectedResult(string duration1, string duration2, int expectedCompareResult)
        {
            // Arrange
            TestNode testNode1 = new TestNode($"<test-start id='1'/>");
            TestNode testNode2 = new TestNode($"<test-start id='2'/>");
            var resultNode1 = new ResultNode($"<test-case id='1' duration='{duration1}'/>");
            var resultNode2 = new ResultNode($"<test-case id='2' duration='{duration2}'/>");
            ITestModel model = Substitute.For<ITestModel>();
            model.GetResultForTest("1").Returns(resultNode1);
            model.GetResultForTest("2").Returns(resultNode2);

            TreeNode treeNode1 = new TreeNode() { Tag = testNode1 };
            TreeNode treeNode2 = new TreeNode() { Tag = testNode2 };

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Duration, TreeViewNodeComparer.Descending, true);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }

        [TestCase("TestA", "TestB", TreeViewNodeComparer.Ascending, -1)]
        [TestCase("TestA", "TestB", TreeViewNodeComparer.Descending, 1)]
        [TestCase("TestA", "TestA", TreeViewNodeComparer.Descending, 0)]
        public void Durationcompare_NoTestResultAvailable_ReturnsExpectedResult(string text1, string text2, string sortDirection, int expectedCompareResult)
        {
            // Arrange
            TestNode testNode1 = new TestNode($"<test-start id='1'/>");
            TestNode testNode2 = new TestNode($"<test-start id='2'/>");
            ITestModel model = Substitute.For<ITestModel>();
            model.GetResultForTest("1").Returns((ResultNode)null);
            model.GetResultForTest("2").Returns((ResultNode)null);

            TreeNode treeNode1 = new TreeNode(text1) { Tag = testNode1 };
            TreeNode treeNode2 = new TreeNode(text2) { Tag = testNode2 };

            // Act
            IComparer comparer = TreeViewNodeComparer.GetComparer(model, TreeViewNodeComparer.Duration, sortDirection, true);
            int result = comparer.Compare(treeNode1, treeNode2);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCompareResult));
        }
    }
}
