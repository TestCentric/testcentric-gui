// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Gui.Model.Filter
{
    [TestFixture]
    internal class TextFilterTests
    {
        [Test]
        public void Create_TestFilter_ConditionIsEmpty()
        {
            // 1. Arrange + Act
            TextFilter filter = new TextFilter();

            // 2. Assert
            Assert.That(filter.Condition, Has.Exactly(1).EqualTo(""));
        }

        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("", "TestA")]
        [TestCase("TestA", "TestA")]
        [TestCase("TESTA", "TestA")]
        [TestCase("A", "TestA")]
        public void IsMatching_TestNodeNameMatchesFilterText_ReturnsTrue(string filterText, string nodeName)
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { filterText };
            TestNode testNode = new TestNode($"<test-case id='1' fullname='${nodeName}' />");

            // 2. Act
            bool isMatch = filter.IsMatching(testNode);

            // 3. Assert
            Assert.That(isMatch, Is.True);
        }

        [TestCase("A", "")]
        [TestCase("B", "TestA")]
        [TestCase("TestAB", "TestA")]
        public void IsMatching_TestNodeNameNotMatchesFilterText_ReturnsFalse(string filterText, string nodeName)
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { filterText };
            TestNode testNode = new TestNode($"<test-case id='1' fullname='${nodeName}' />");

            // 2. Act
            bool isMatch = filter.IsMatching(testNode);

            // 3. Assert
            Assert.That(isMatch, Is.False);
        }

        [Test]
        public void Init_ConditionIsEmpty()
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { "TextFilter" };

            // 2. Act
            filter.Init();

            // 3. Assert
            Assert.That(filter.Condition, Has.Exactly(1).EqualTo(""));
        }

        [Test]
        public void Reset_ConditionIsEmpty()
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { "TextFilter" };

            // 2. Act
            filter.Reset();

            // 3. Assert
            Assert.That(filter.Condition, Has.Exactly(1).EqualTo(""));
        }

        [TestCase("FilterText", true)]
        [TestCase("A", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsActive_Condition_IsSet_ReturnsTrue(string filterText, bool expectedIsActive)
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();

            // 2. Act
            filter.Condition = new[] { filterText };

            // 3. Assert
            Assert.That(filter.IsActive, Is.EqualTo(expectedIsActive));
        }

        [Test]
        public void IsActive_FilterIsReset_ReturnsFalse()
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { "FilterText" };

            // 2. Act
            filter.Reset();

            // 3. Assert
            Assert.That(filter.IsActive, Is.False);
        }

        [Test]
        public void IsActive_FilterIsInit_ReturnsFalse()
        {
            // 1. Arrange
            TextFilter filter = new TextFilter();
            filter.Condition = new[] { "FilterText" };

            // 2. Act
            filter.Init();

            // 3. Assert
            Assert.That(filter.IsActive, Is.False);
        }
    }
}
