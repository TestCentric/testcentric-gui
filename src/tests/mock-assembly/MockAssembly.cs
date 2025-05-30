// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TestCentric.Tests
{
    namespace Assemblies
    {
        /// <summary>
        /// Constant definitions for the mock-assembly dll.
        /// </summary>
        public class MockAssembly
        {
            public static int Classes = 10;
            public static int NamespaceSuites = 6; // assembly, NUnit, Tests, Assemblies, Singletons, TestAssembly

            public static int Tests = MockTestFixture.Tests
                        + Singletons.OneTestCase.Tests
                        + TestAssembly.MockTestFixture.Tests
                        + IgnoredFixture.Tests
                        + ExplicitFixture.Tests
                        + BadFixture.Tests
                        + FixtureWithTestCases.Tests
                        + ParameterizedFixture.Tests
                        + GenericFixtureConstants.Tests
                        + FixtureWithTheories.Tests;

            public static int Suites = MockTestFixture.Suites
                        + Singletons.OneTestCase.Suites
                        + TestAssembly.MockTestFixture.Suites
                        + IgnoredFixture.Suites
                        + ExplicitFixture.Suites
                        + BadFixture.Suites
                        + FixtureWithTestCases.Suites
                        + ParameterizedFixture.Suites
                        + GenericFixtureConstants.Suites
                        + FixtureWithTheories.Suites
                        + NamespaceSuites;

            public static readonly int Nodes = Tests + Suites;

            public static int ExplicitFixtures = 1;
            public static int SuitesRun = Suites - ExplicitFixtures;

            public static int Ignored = MockTestFixture.Ignored + IgnoredFixture.Tests;
            public static int Explicit = MockTestFixture.Explicit + ExplicitFixture.Tests;
            public static int NotRunnable = MockTestFixture.NotRunnable + BadFixture.Tests;
            public static int NotRun = Ignored + Explicit + NotRunnable;
            public static int TestsRun = Tests - NotRun;
            public static int ResultCount = Tests - Explicit;

            public static int Errors = MockTestFixture.Errors;
            public static int Failures = MockTestFixture.Failures;
            public static int Warnings = MockTestFixture.Warnings;

            public static int Categories = MockTestFixture.Categories;

            public static string AssemblyPath;

            static MockAssembly()
            {
                var assembly = typeof(MockAssembly).Assembly;
                string codeBase = assembly.Location;

                AssemblyPath = codeBase.ToLower().StartsWith(Uri.UriSchemeFile)
                    ? new Uri(codeBase).LocalPath
                    : assembly.Location;
            }
        }

        [TestFixture(Description = "Fake Test Fixture")]
        [Category("FixtureCategory")]
        public class MockTestFixture
        {
            public static readonly int Tests = 13;
            public static readonly int Suites = 1;

            public static readonly int Ignored = 1;
            public static readonly int Explicit = 1;
            public static readonly int NotRunnable = 2;
            public static readonly int NotRun = Ignored + Explicit + NotRunnable;
            public static readonly int TestsRun = Tests - NotRun;
            public static readonly int ResultCount = Tests - Explicit;

            public static readonly int Failures = 1;
            public static readonly int Errors = 1;
            public static readonly int Warnings = 1;

            public static readonly int Categories = 5;
            public static readonly int MockCategoryTests = 2;

            [Test(Description = "Mock Test #1")]
            public void MockTest1()
            { }

            [Test]
            [Category("MockCategory")]
            [Property("Severity", "Critical")]
            [Description("This is a really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really long description")]
            public void MockTest2()
            { }

            [Test]
            [Category("MockCategory")]
            [Category("AnotherCategory")]
            public void MockTest3()
            { Assert.Pass("Succeeded!"); }

            [Test]
            protected static void MockTest5()
            { }

            [Test]
            public void FailingTest()
            {
                Assert.Fail("Intentional failure");
            }

            [Test]
            public void WarningTest()
            {
                Assert.Warn("Consider yourself warned!");
            }

            [Test, Property("TargetMethod", "SomeClassName"), Property("Size", 5), Property("TargetType", "SomeType"), Property("Priority", "3"), Property("User", "Somebody")]
            public void TestWithManyProperties()
            { }

            [Test]
            [Ignore("ignoring this test method for now")]
            [Category("Foo")]
            public void MockTest4()
            { }

            [Test, Explicit]
            [Category("Special")]
            public void ExplicitlyRunTest()
            { }

            [Test]
            public void NotRunnableTest(int a, int b)
            {
            }

            [Test]
            public void InconclusiveTest()
            {
                Assert.Inconclusive("No valid data");
            }

            [Test]
            public void TestWithException()
            {
                MethodThrowsException();
            }

            private void MethodThrowsException()
            {
                throw new ApplicationException("Intentional Exception");
            }

            [Test]
            public void MultipleFailuresTestWithTextOutput()
            {
                Console.WriteLine("Console.WriteLine #1");
                TestContext.Progress.WriteLine("Progress #1");
                Console.WriteLine("Console.WriteLine #2");
                Console.WriteLine("Console.WriteLine #3");
                TestContext.Progress.WriteLine("Progress #2");
                TestContext.Progress.WriteLine("Progress #3");
                Console.WriteLine("Console.WriteLine #4");
                Console.WriteLine("Console.WriteLine #5");
                TestContext.Progress.WriteLine("Progress #4");
                Console.WriteLine("Console.WriteLine #6");

                Assert.Multiple( () => 
                {
                    Assert.That(2 + 2, Is.EqualTo(5));
                    Assert.That(99 < 42, "What?");
                    Assert.That("Hello World", Does.Contain("Everyone"));
                });
            }
        }
    }

    namespace Singletons
    {
        [TestFixture]
        public class OneTestCase
        {
            public static readonly int Tests = 1;
            public static readonly int Suites = 1;

            [Test]
            public virtual void TestCase()
            { }
        }
    }

    namespace TestAssembly
    {
        [TestFixture]
        public class MockTestFixture
        {
            public static readonly int Tests = 1;
            public static readonly int Suites = 1;

            [Test]
            public void MyTest()
            {
            }
        }
    }

    [TestFixture, Ignore("Testing")]
    public class IgnoredFixture
    {
        public static readonly int Tests = 3;
        public static readonly int Suites = 1;

        [Test]
        public void Test1() { }

        [Test]
        public void Test2() { }

        [Test]
        public void Test3() { }
    }

    [TestFixture, Explicit]
    public class ExplicitFixture
    {
        public static readonly int Tests = 2;
        public static readonly int Suites = 1;
        public static readonly int Nodes = Tests + Suites;

        [Test]
        public void Test1() { }

        [Test]
        public void Test2() { }
    }

    [TestFixture]
    public class BadFixture
    {
        public static readonly int Tests = 1;
        public static readonly int Suites = 1;

        public BadFixture(int val) { }

        [Test]
        public void SomeTest() { }
    }

    [TestFixture]
    public class FixtureWithTestCases
    {
        public static readonly int Tests = 4;
        public static readonly int Suites = 3;

        [TestCase(2, 2, ExpectedResult = 4)]
        [TestCase(9, 11, ExpectedResult = 20)]
        public int MethodWithParameters(int x, int y)
        {
            return x + y;
        }

        [TestCase(2, 4)]
        [TestCase(9.2, 11.7)]
        public void GenericMethod<T>(T x, T y)
        {
        }
    }

    [TestFixture(5)]
    [TestFixture(42)]
    public class ParameterizedFixture
    {
        public static readonly int Tests = 4;
        public static readonly int Suites = 3;

        public ParameterizedFixture(int num) { }

        [Test]
        public void Test1() { }

        [Test]
        public void Test2() { }
    }

    public class GenericFixtureConstants
    {
        public static readonly int Tests = 4;
        public static readonly int Suites = 3;
    }

    [TestFixture(5)]
    [TestFixture(11.5)]
    public class GenericFixture<T>
    {
        public GenericFixture(T num) { }

        [Test]
        public void Test1() { }

        [Test]
        public void Test2() { }
    }

    public class FixtureWithTheories
    {
        public static readonly int Tests = 9;
        public static readonly int Suites = 3;

        [DatapointSource]
        static int[] ValuesToUse = new int[] { 1, 2, 3 };

        [Theory]
        public void AllCasesPass(int val)
        {
            Assume.That(val != 2, $"Argument '{val}' is not valid.");
        }

        [Theory]
        public void AllCasesInconclusive(int val)
        {
            Assert.Inconclusive($"Argument '{val}' is not valid.");
        }

        [Theory]
        public void OneCaseFails(int val)
        {
            Assert.That(val, Is.Not.EqualTo(2));
        }
    }
}
