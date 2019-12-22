// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace NUnit.Engine.Api
{
    public class TestFilterTests
    {
        [Test]
        public void EmptyFilter()
        {
            TestFilter filter = TestFilter.Empty;
            Assert.That(filter.Text, Is.EqualTo("<filter/>"));
        }

        [Test]
        public void FilterWithOneTest()
        {
            string text = "<filter><tests><test>My.Test.Name</test></tests></filter>";
            TestFilter filter = new TestFilter(text);
            Assert.That(filter.Text, Is.EqualTo(text));
        }

        [Test]
        public void FilterWithThreeTests()
        {
            string text = "<filter><tests><test>My.First.Test</test><test>My.Second.Test</test><test>My.Third.Test</test></tests></filter>";
            TestFilter filter = new TestFilter(text);
            Assert.That(filter.Text, Is.EqualTo(text));
        }
    }
}
