// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters
{
    using System.Collections.Generic;
    using NSubstitute;
    using NUnit.Framework;
    using System.Linq;
    using Model;
    using Views;

    [TestFixture]
    public class OutcomeGroupingTests
    {
        [Test]
        public void Constructor_DefaultGroups_AreCreated()
        {
            // 1. Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            ITestModel model = Substitute.For<ITestModel>();
            GroupDisplayStrategy strategy = Substitute.For<GroupDisplayStrategy>(treeView, model);

            // 2. Act
            OutcomeGrouping grouping = new OutcomeGrouping(strategy);

            // 3. Assert
            Assert.That(grouping.Groups.Count, Is.EqualTo(6));
        }

        [Test]
        public void ID_Is_Outcome()
        {
            // 1. Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            ITestModel model = Substitute.For<ITestModel>();
            GroupDisplayStrategy strategy = Substitute.For<GroupDisplayStrategy>(treeView, model);

            // 2. Act
            OutcomeGrouping grouping = new OutcomeGrouping(strategy);

            // 3. Assert
            Assert.That(grouping.ID, Is.EqualTo("OUTCOME"));
        }

        [Test]
        [TestCase("Passed", "", "Passed")]
        [TestCase("Failed", "", "Failed")]
        [TestCase("Skipped", "", "Skipped")]
        [TestCase("Skipped", "Ignored", "Ignored")]
        [TestCase("Inconclusive", "", "Inconclusive")]
        public void Load_ResultExistsForTestNode_TestNodeIsInsideGroup(string resultState, string outcomeLabel, string expectedGroupName)
        {
            // 1. Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            ITestModel model = Substitute.For<ITestModel>();
            GroupDisplayStrategy strategy = Substitute.For<GroupDisplayStrategy>(treeView, model);

            var resultNode = new ResultNode($"<test-case id='1' result='{resultState}' label='{outcomeLabel}'/>");

            var testNode = new TestNode($"<test-case id='1' />");
            var tests = new List<TestNode> { testNode };

            model.GetResultForTest("1").Returns(resultNode);

            // 2. Act
            OutcomeGrouping grouping = new OutcomeGrouping(strategy);
            grouping.Load(tests);

            // 3. Assert
            var expectedGroup = grouping.Groups.FirstOrDefault(g => g.Name == expectedGroupName);
            Assert.That(expectedGroup, Contains.Item(testNode));
        }

        [Test]
        public void Load_NoResultExistsForTestNode_TestNodeIsInsideNotRunGroup()
        {
            // 1. Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            ITestModel model = Substitute.For<ITestModel>();
            GroupDisplayStrategy strategy = Substitute.For<GroupDisplayStrategy>(treeView, model);

            var testNode = new TestNode($"<test-case id='1' />");
            var tests = new List<TestNode> { testNode };

            model.GetResultForTest("1").Returns((ResultNode)null);

            // 2. Act
            OutcomeGrouping grouping = new OutcomeGrouping(strategy);
            grouping.Load(tests);

            // 3. Assert
            var expectedGroup = grouping.Groups.FirstOrDefault(g => g.Name == "Not Run");
            Assert.That(expectedGroup, Contains.Item(testNode));
        }

        [Test]
        public void OnTestFinished_Calls_ApplyResultToGroup()
        {
            // 1. Arrange
            ITestTreeView treeView = Substitute.For<ITestTreeView>();
            ITestModel model = Substitute.For<ITestModel>();
            GroupDisplayStrategy strategy = Substitute.For<GroupDisplayStrategy>(treeView, model);
            ResultNode result = new ResultNode("<test-case id='1'/>");

            // 2. Act
            OutcomeGrouping grouping = new OutcomeGrouping(strategy);
            grouping.OnTestFinished(result);

            // 3. Assert
            strategy.Received().ApplyResultToGroup(result);
        }
    }
}
