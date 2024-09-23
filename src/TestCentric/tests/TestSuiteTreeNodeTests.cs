// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Tests
{
    using Views;

    /// <summary>
    /// Summary description for TestSuiteTreeNodeTests.
    /// </summary>
    [TestFixture]
    public class TestSuiteTreeNodeTests
    {
        //TestSuite testSuite;
        //Test testFixture;
        //Test testCase;

        [SetUp]
        public void SetUp()
        {
            //testSuite = new TestSuite("MyTestSuite");
            //testFixture = TestFixtureBuilder.BuildFrom( typeof( MockTestFixture ) );
            //testSuite.Add( testFixture );

            //testCase = TestFinder.Find("MockTest1", testFixture, false);
        }

        [Test]
        public void CanConstructFromAssembly()
        {
            var testSuite = new TestNode("<test-suite id='123' type='Assembly' name='mytest.dll' fullname='/A/B/C/mytest.dll' testcasecount='42' runstate='Runnable'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testSuite);

            Assert.That(node.Text, Is.EqualTo("mytest.dll"));
            Assert.That(node.TestType, Is.EqualTo("Assembly"));
        }

        [Test]
        public void CanConstructFromTestFixture()
        {
            var testFixture = new TestNode("<test-suite id='123' type='TestFixture' name='SomeFixture' fullname='/A/B/C/mytest.dll' testcasecount='42' runstate='Runnable'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testFixture);

            Assert.That(node.Text, Is.EqualTo("SomeFixture"));
            Assert.That(node.TestType, Is.EqualTo("TestFixture"));
        }

        [Test]
        public void CanConstructFromTestCase()
        {
            var testCase = new TestNode("<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='Ignored'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testCase);

            Assert.That(node.Text, Is.EqualTo("SomeTest"));
            Assert.That(node.TestType, Is.EqualTo("TestCase"));
        }

        [TestCase("Unknown", TestSuiteTreeNode.InitIndex)]
        [TestCase("Ignored", TestSuiteTreeNode.IgnoredIndex)]
        [TestCase("NotRunnable", TestSuiteTreeNode.FailureIndex)]
        public void WhenResultIsNotSet_IndexReflectsRunState(string runState, int expectedIndex)
        {
            var testCase = new TestNode($"<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='{runState}'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testCase);

            Assert.That(node.ImageIndex, Is.EqualTo(expectedIndex));
            Assert.That(node.SelectedImageIndex, Is.EqualTo(expectedIndex));
        }

        [TestCase("Inconclusive", TestSuiteTreeNode.InconclusiveIndex)]
        [TestCase("Skipped", TestSuiteTreeNode.SkippedIndex)]
        [TestCase("Skipped:Ignored", TestSuiteTreeNode.IgnoredIndex)]
        [TestCase("Passed", TestSuiteTreeNode.SuccessIndex)]
        [TestCase("Failed", TestSuiteTreeNode.FailureIndex)]
        [TestCase("Failed:Error", TestSuiteTreeNode.FailureIndex)]
        [TestCase("Failed:Cancelled", TestSuiteTreeNode.FailureIndex)]
        [TestCase("Failed:NotRunnable", TestSuiteTreeNode.FailureIndex)]
        [TestCase("Warning", TestSuiteTreeNode.WarningIndex)]
        public void WhenResultIsSet_IndexReflectsResultState(string outcome, int expectedIndex)
        {
            int colon = outcome.IndexOf(':');
            string resultPart = colon >= 0
                ? $"result='{outcome.Substring(0, colon)}' label='{outcome.Substring(colon + 1)}'"
                : $"result='{outcome}'";

            var result = new ResultNode($"<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='Runnable' {resultPart}/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(result);

            Assert.That(node.ImageIndex, Is.EqualTo(expectedIndex));
            Assert.That(node.SelectedImageIndex, Is.EqualTo(expectedIndex));
            Assert.That(node.StatusText, Is.EqualTo(outcome));
        }

        [TestCase("Unknown", TestSuiteTreeNode.InitIndex)]
        [TestCase("Ignored", TestSuiteTreeNode.IgnoredIndex)]
        [TestCase("NotRunnable", TestSuiteTreeNode.FailureIndex)]
        public void WhenResultIsCleared_IndexReflectsRunState(string runState, int expectedIndex)
        {
            var result = new ResultNode($"<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='{runState}' result='Failed'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(result);
            Assert.That(node.ImageIndex, Is.EqualTo(TestSuiteTreeNode.FailureIndex));
            Assert.That(node.SelectedImageIndex, Is.EqualTo(TestSuiteTreeNode.FailureIndex));

            node.ClearResults();
            Assert.That(node.Result, Is.Null);
            Assert.That(node.ImageIndex, Is.EqualTo(expectedIndex));
            Assert.That(node.SelectedImageIndex, Is.EqualTo(expectedIndex));
        }

        //[Test]
        //public void WhenResultIsCleared_NestedResultsAreAlsoCleared()
        //{
        //	TestResult testCaseResult = new TestResult( testCase );
        //	testCaseResult.Success();
        //	TestResult testSuiteResult = new TestResult( testFixture );
        //	testSuiteResult.AddResult( testCaseResult );
        //          testSuiteResult.Success();

        //	TestSuiteTreeNode node1 = new TestSuiteTreeNode( testSuiteResult );
        //	TestSuiteTreeNode node2 = new TestSuiteTreeNode( testCaseResult );
        //	node1.Nodes.Add( node2 );

        //	Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node1.ImageIndex );
        //	Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node1.SelectedImageIndex );
        //	Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node2.ImageIndex );
        //	Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node2.SelectedImageIndex );

        //	node1.ClearResults();

        //	Assert.AreEqual( TestSuiteTreeNode.InitIndex, node1.ImageIndex );
        //  Assert.AreEqual( TestSuiteTreeNode.InitIndex, node1.SelectedImageIndex );
        //	Assert.AreEqual( TestSuiteTreeNode.InitIndex, node2.ImageIndex );
        //	Assert.AreEqual( TestSuiteTreeNode.InitIndex, node2.SelectedImageIndex );
        //}
    }
}
