// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Gui.Model
{
    public class TestNodeTests
    {
        [Test]
        public void CreateTestCase()
        {
            var testNode = new TestNode("<test-case id='123' name='SomeTest' fullname='A.B.C.SomeTest' runstate='Ignored'/>");

            Assert.That(testNode.Id, Is.EqualTo("123"));
            Assert.That(testNode.Name, Is.EqualTo("SomeTest"));
            Assert.That(testNode.FullName, Is.EqualTo("A.B.C.SomeTest"));
            Assert.That(testNode.Type, Is.EqualTo("TestCase"));
            Assert.That(testNode.TestCount, Is.EqualTo(1));
            Assert.That(testNode.RunState, Is.EqualTo(RunState.Ignored));
            Assert.False(testNode.IsSuite);
        }

        [Test]
        public void CreateTestSuite()
        {
            var testNode = new TestNode("<test-suite id='123' type='Assembly' name='mytest.dll' fullname='/A/B/C/mytest.dll' testcasecount='42' runstate='Runnable'/>");

            Assert.That(testNode.Id, Is.EqualTo("123"));
            Assert.That(testNode.Name, Is.EqualTo("mytest.dll"));
            Assert.That(testNode.FullName, Is.EqualTo("/A/B/C/mytest.dll"));
            Assert.That(testNode.Type, Is.EqualTo("Assembly"));
            Assert.That(testNode.TestCount, Is.EqualTo(42));
            Assert.That(testNode.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(testNode.IsSuite);
        }

        [Test]
        public void CreateTestRun()
        {
            var testNode = new TestNode("<test-run id='123' testcasecount='99'/>");

            Assert.That(testNode.Id, Is.EqualTo("123"));
            Assert.That(testNode.Name, Is.EqualTo("TestRun"));
            Assert.That(testNode.FullName, Is.EqualTo("TestRun"));
            Assert.That(testNode.TestCount, Is.EqualTo(99));
            Assert.That(testNode.IsSuite);
        }
    }
}
