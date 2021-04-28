// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric.Engine.Runners
{
    /// <summary>
    /// LocalTestRunner runs tests in the current application domain.
    /// </summary>
    public class LocalTestRunner : TestAgentRunner
    {
        public LocalTestRunner(TestPackage package) : base(package)
        {
#if !NETSTANDARD1_6
            this.TestDomain = AppDomain.CurrentDomain;
#endif
        }
    }
}
