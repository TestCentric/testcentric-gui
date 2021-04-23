// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric.Engine.Runners.Fakes
{
    internal class EmptyDirectTestRunner : Engine.Runners.TestAgentRunner
    {
        public EmptyDirectTestRunner(IServiceLocator services, TestPackage package) : base(package)
        {
#if !NETCOREAPP1_1
            TestDomain = AppDomain.CurrentDomain;
#endif
        }

        public new void LoadPackage()
        {
            base.LoadPackage();
        }
    }
}
