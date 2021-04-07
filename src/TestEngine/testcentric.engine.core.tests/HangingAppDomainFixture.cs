// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Threading;
using NUnit.Framework;

namespace TestCentric.Engine
{
    [Explicit]
    public class HangingAppDomainFixture
    {
        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }

        ~HangingAppDomainFixture()
        {
            Thread.Sleep(TimeSpan.FromDays(1));
        }
    }
}
