// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Tests
{
    [TestFixture]
    public class ExplicitFiltering
    {
        [Test, Explicit]
        [Category("ExplicitCategory")]
        public void ExplicitTest() { }

        [Test]
        [Category("AnotherCategory")]
        public void AnotherTest() { }

        [Test]
        [Category("AnotherCategory")]
        public void YetAnotherTest() { }
    }
}
