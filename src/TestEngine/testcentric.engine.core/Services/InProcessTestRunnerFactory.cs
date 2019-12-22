// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.Engine.Helpers;
using NUnit.Engine.Internal;
using NUnit.Engine.Runners;

namespace NUnit.Engine.Services
{
    /// <summary>
    /// InProcessTestRunnerFactory handles creation of a suitable test
    /// runner for a given package to be loaded and run within the
    /// same process.
    /// </summary>
    public class InProcessTestRunnerFactory : Service, ITestRunnerFactory
    {
        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>An ITestEngineRunner</returns>
        public virtual ITestEngineRunner MakeTestRunner(TestPackage package)
        {
#if NETSTANDARD1_6 || NETSTANDARD2_0
            return new LocalTestRunner(ServiceContext, package);
#else
            DomainUsage domainUsage = (DomainUsage)System.Enum.Parse(
                typeof(DomainUsage),
                package.GetSetting(EnginePackageSettings.DomainUsage, "Default"));

            switch (domainUsage)
            {
                default:
                case DomainUsage.Default:
                case DomainUsage.Multiple:
                    if (package.AssemblyPackages().Count > 1)
                        return new MultipleTestDomainRunner(this.ServiceContext, package);
                    else
                        return new TestDomainRunner(this.ServiceContext, package);

                case DomainUsage.None:
                    return new LocalTestRunner(ServiceContext, package);

                case DomainUsage.Single:
                    return new TestDomainRunner(ServiceContext, package);
            }
#endif
        }

        public virtual bool CanReuse(ITestEngineRunner runner, TestPackage package)
        {
            return false;
        }
    }
}
