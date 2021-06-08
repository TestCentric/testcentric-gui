// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Services.Fakes
{
    public class FakeTestRunnerFactory : Service, ITestRunnerFactory
    {
        public bool CanReuse(ITestEngineRunner runner, TestPackage package)
        {
            return true;
        }

        public ITestEngineRunner MakeTestRunner(TestPackage package)
        {
            return new AssemblyRunner(ServiceContext, package);
        }
    }
}
