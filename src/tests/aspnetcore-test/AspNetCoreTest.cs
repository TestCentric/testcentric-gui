// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using Microsoft.AspNetCore.Components.Forms;

// Test which resolves issue #1203
namespace Test1
{
    [TestFixture]
    public class AspNetCoreTest
    {
        [Test]
        public void WithoutFramework()
        {
            Assert.Pass();
        }

        [Test]
        public void WithFramework()
        {
            InputCheckbox checkbox = new InputCheckbox();
            Assert.Pass();
        }
    }
}