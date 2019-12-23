// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using NUnit.Framework;

namespace NUnit.Engine
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
