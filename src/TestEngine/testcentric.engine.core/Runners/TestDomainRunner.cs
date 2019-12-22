// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using NUnit.Engine.Services;

namespace NUnit.Engine.Runners
{
    /// <summary>
    /// TestDomainRunner loads and runs tests in a separate
    /// domain whose lifetime it controls.
    /// </summary>
    public class TestDomainRunner : DirectTestRunner
    {
        private DomainManager _domainManager;

        public TestDomainRunner(IServiceLocator services, TestPackage package) : base(services, package)
        {
            _domainManager = Services.GetService<DomainManager>();
        }

        protected override TestEngineResult LoadPackage()
        {
            TestDomain = _domainManager.CreateDomain(TestPackage);

            return base.LoadPackage();
        }

        /// <summary>
        /// Unload any loaded TestPackage as well as the application domain.
        /// </summary>
        public override void UnloadPackage()
        {
            if (this.TestDomain != null)
            {
                _domainManager.Unload(this.TestDomain);
                this.TestDomain = null;
            }
        }
    }
}
#endif
