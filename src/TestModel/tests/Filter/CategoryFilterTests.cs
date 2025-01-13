// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCentric.Gui.Model.Filter
{
    [TestFixture]
    internal class CategoryFilterTests
    {
        [Test]
        public void Create_TestFilter_ConditionIsEmpty()
        {
            // 1. Arrange + Act
            ITestModel testModel = Substitute.For<ITestModel>();
            CategoryFilter filter = new CategoryFilter(testModel);

            // 2. Assert
            Assert.That(filter.Condition, Is.Empty);
        }

        [TestCase(new[] { "CategoryA" }, new[] { "CategoryA" })]
        [TestCase(new[] { "CategoryA", "CategoryB" }, new[] { "CategoryB" })]
        [TestCase(new[] { "CategoryB" }, new[] { "CategoryA", "CategoryB" })]
        [TestCase(new[] { CategoryFilter.NoCategory }, new string[]{ })]

        public void IsMatching_CategoryMatchesCategoryFilter_ReturnsTrue(IList<string> categoryFilter, IList<string> testCategories)
        {
            // 1. Arrange
            ITestModel testModel = Substitute.For<ITestModel>();
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Condition = categoryFilter;

            string xml = CreateTestcaseXml("1", "TestA", testCategories);
            TestNode testNode = new TestNode(xml);

            // 2. Act
            bool isMatch = filter.IsMatching(testNode);

            // 3. Assert
            Assert.That(isMatch, Is.True);
        }

        [TestCase(new[] { "CategoryA" }, new[] { "CategoryB" })]
        [TestCase(new[] { "CategoryA" }, new[] { "" })]
        [TestCase(new[] { CategoryFilter.NoCategory }, new[] { "CategoryB" })]
        public void IsMatching_CategoryNotMatchesCategoryFilter_ReturnsFalse(IList<string> categoryFilter, IList<string> testCategories)
        {
            // 1. Arrange
            ITestModel testModel = Substitute.For<ITestModel>();
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Condition = categoryFilter;

            string xml = CreateTestcaseXml("1", "TestA", testCategories);
            TestNode testNode = new TestNode(xml);

            // 2. Act
            bool isMatch = filter.IsMatching(testNode);

            // 3. Assert
            Assert.That(isMatch, Is.False);
        }

        [Test]
        public void Init_Condition_ContainsAllCategories()
        {
            // 1. Arrange
            var availableCategories = new List<string>() { "CategoryA", "CategoryB" };
            ITestModel testModel = Substitute.For<ITestModel>();
            testModel.AvailableCategories.Returns(availableCategories);
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Condition = new[] { "CategoryA" };

            // 2. Act
            filter.Init();

            // 3. Assert
            Assert.That(filter.Condition, Has.Exactly(3).Items);
            Assert.That(filter.Condition, Does.Contain("CategoryA"));
            Assert.That(filter.Condition, Does.Contain("CategoryB"));
            Assert.That(filter.Condition, Does.Contain(CategoryFilter.NoCategory));
        }

        [Test]
        public void Reset_Condition_ContainsAllCategories()
        {
            // 1. Arrange
            var availableCategories = new List<string>() { "CategoryA", "CategoryB" };
            ITestModel testModel = Substitute.For<ITestModel>();
            testModel.AvailableCategories.Returns(availableCategories);
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Condition = new[] { "CategoryA" };

            // 2. Act
            filter.Reset();

            // 3. Assert
            Assert.That(filter.Condition, Has.Exactly(3).Items);
            Assert.That(filter.Condition, Does.Contain("CategoryA"));
            Assert.That(filter.Condition, Does.Contain("CategoryB"));
            Assert.That(filter.Condition, Does.Contain(CategoryFilter.NoCategory));
        }

        [Test]
        public void IsActive_Condition_IsSet_ReturnsTrue()
        {
            // 1. Arrange
            var availableCategories = new List<string>() { "CategoryA", "CategoryB" };
            ITestModel testModel = Substitute.For<ITestModel>();
            testModel.AvailableCategories.Returns(availableCategories);
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Init();

            // 2. Act
            filter.Condition = new[] { "CategoryA" };

            // 3. Assert
            Assert.That(filter.IsActive, Is.EqualTo(true));
        }

        [Test]
        public void IsActive_FilterIsReset_ReturnsFalse()
        {
            // 1. Arrange
            var availableCategories = new List<string>() { "CategoryA", "CategoryB" };
            ITestModel testModel = Substitute.For<ITestModel>();
            testModel.AvailableCategories.Returns(availableCategories);
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Init();
            filter.Condition = new[] { "CategoryA" };

            // 2. Act
            filter.Reset();

            // 3. Assert
            Assert.That(filter.IsActive, Is.False);
        }

        [Test]
        public void IsActive_FilterIsInit_ReturnsFalse()
        {
            // 1. Arrange
            var availableCategories = new List<string>() { "CategoryA", "CategoryB" };
            ITestModel testModel = Substitute.For<ITestModel>();
            testModel.AvailableCategories.Returns(availableCategories);
            CategoryFilter filter = new CategoryFilter(testModel);
            filter.Init();
            filter.Condition = new[] { "CategoryA" };

            // 2. Act
            filter.Init();

            // 3. Assert
            Assert.That(filter.IsActive, Is.False);
        }

        private string CreateTestcaseXml(string testId, string testName, IList<string> categories)
        {
            string str = $"<test-case id='{testId}' fullname='{testName}'> ";

            str += "<properties> ";
            foreach (string category in categories)
                str += $"<property name='Category' value='{category}' /> ";
            str += "</properties> ";

            str += "</test-case> ";

            return str;
        }
    }
}
