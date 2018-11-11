// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

            Assert.AreEqual("mytest.dll", node.Text);
            Assert.AreEqual("Assembly", node.TestType);
        }

        [Test]
        public void CanConstructFromTestFixture()
        {
            var testFixture = new TestNode("<test-suite id='123' type='TestFixture' name='SomeFixture' fullname='/A/B/C/mytest.dll' testcasecount='42' runstate='Runnable'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testFixture);

            Assert.AreEqual("SomeFixture", node.Text);
            Assert.AreEqual("TestFixture", node.TestType);
        }

        [Test]
        public void CanConstructFromTestCase()
        {
            var testCase = new TestNode("<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='Ignored'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testCase);

            Assert.AreEqual("SomeTest", node.Text);
            Assert.AreEqual("TestCase", node.TestType);
        }

        [TestCase("Unknown", TestSuiteTreeNode.InitIndex)]
        [TestCase("Ignored", TestSuiteTreeNode.IgnoredIndex)]
        [TestCase("NotRunnable", TestSuiteTreeNode.FailureIndex)]
        public void WhenResultIsNotSet_IndexReflectsRunState(string runState, int expectedIndex)
        {
            var testCase = new TestNode($"<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='{runState}'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(testCase);

            Assert.AreEqual(expectedIndex, node.ImageIndex);
            Assert.AreEqual(expectedIndex, node.SelectedImageIndex);
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

            Assert.AreEqual(expectedIndex, node.ImageIndex);
            Assert.AreEqual(expectedIndex, node.SelectedImageIndex);
            Assert.AreEqual(outcome, node.StatusText);
        }

        [TestCase("Unknown", TestSuiteTreeNode.InitIndex)]
        [TestCase("Ignored", TestSuiteTreeNode.IgnoredIndex)]
        [TestCase("NotRunnable", TestSuiteTreeNode.FailureIndex)]
        public void WhenResultIsCleared_IndexReflectsRunState(string runState, int expectedIndex)
        {
            var result = new ResultNode($"<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='{runState}' result='Failed'/>");
            TestSuiteTreeNode node = new TestSuiteTreeNode(result);
            Assert.AreEqual(TestSuiteTreeNode.FailureIndex, node.ImageIndex);
            Assert.AreEqual(TestSuiteTreeNode.FailureIndex, node.SelectedImageIndex);

            node.ClearResults();
            Assert.AreEqual(null, node.Result);
            Assert.AreEqual(expectedIndex, node.ImageIndex);
            Assert.AreEqual(expectedIndex, node.SelectedImageIndex);
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
