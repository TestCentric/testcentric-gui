// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using NUnit.Engine;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Runners
{
    /// <summary>
    /// TestDomainRunner loads and runs tests in a separate
    /// domain whose lifetime it controls.
    /// </summary>
    public class TestDomainRunner : TestAgentRunner
    {
        private DomainManager _domainManager;

        public TestDomainRunner(TestPackage package) : base(package)
        {
            _domainManager = new DomainManager();
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
