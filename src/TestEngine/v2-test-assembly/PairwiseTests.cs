// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using NUnit.Framework;

namespace TestCentric.Tests
{
    [TestFixture]
    public class PairwiseTest
    {
        [TestFixture]
        public class LiveTest
        {
            public Hashtable pairsTested = new Hashtable();

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                pairsTested = new Hashtable();
            }

            [TestFixtureTearDown]
            public void TestFixtureTearDown()
            {
                Assert.That(pairsTested.Count, Is.EqualTo(16));
            }

            [Test, Pairwise]
            public void Test(
                [Values("a", "b", "c")] string a,
                [Values("+", "-")] string b,
                [Values("x", "y")] string c)
            {
                Console.WriteLine("Pairwise: {0} {1} {2}", a, b, c);

                pairsTested[a + b] = null;
                pairsTested[a + c] = null;
                pairsTested[b + c] = null;
            }
        }
    }
}
