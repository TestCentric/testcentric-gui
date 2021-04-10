// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP1_1 && !NETCOREAPP2_1
using System;
using System.IO;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Tests.Assemblies;

namespace TestCentric.Engine.Services
{
    public class DomainManagerTests
    {
        private DomainManager _domainManager;
        private TestPackage _package = new TestPackage(MockAssembly.AssemblyPath);

        [SetUp]
        public void CreateDomainManager()
        {
            _domainManager = new DomainManager();
        }

        [Test, Platform("Linux,Net", Reason = "get_SetupInformation() fails on Windows+Mono")]
        public void CanCreateDomain()
        {
            var domain = _domainManager.CreateDomain(_package);

            Assert.NotNull(domain);
            var setup = domain.SetupInformation;

            Assert.That(setup.ApplicationName, Does.StartWith("Tests_"));
            Assert.That(setup.ApplicationBase, Is.SamePath(Path.GetDirectoryName(MockAssembly.AssemblyPath)), "ApplicationBase");
            Assert.That(
                Path.GetFileName(setup.ConfigurationFile),
                Is.EqualTo("mock-assembly.dll.config").IgnoreCase,
                "ConfigurationFile");
            Assert.That(setup.PrivateBinPath, Is.EqualTo(null), "PrivateBinPath");
            Assert.That(setup.ShadowCopyFiles, Is.Null.Or.EqualTo("false"));
            //Assert.That(setup.ShadowCopyDirectories, Is.SamePath(Path.GetDirectoryName(MockAssembly.AssemblyPath)), "ShadowCopyDirectories" );
        }

        [Test, Platform("Linux,Net", Reason = "get_SetupInformation() fails on Windows+Mono")]
        public void CanCreateDomainWithApplicationBaseSpecified()
        {
            string assemblyDir = Path.GetDirectoryName(_package.FullName);
            string basePath = Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir));
            string relPath = assemblyDir.Substring(basePath.Length + 1);

            _package.Settings["BasePath"] = basePath;
            var domain = _domainManager.CreateDomain(_package);

            Assert.NotNull(domain);
            var setup = domain.SetupInformation;

            Assert.That(setup.ApplicationName, Does.StartWith("Tests_"));
            Assert.That(setup.ApplicationBase, Is.SamePath(basePath), "ApplicationBase");
            Assert.That(
                Path.GetFileName(setup.ConfigurationFile),
                Is.EqualTo("mock-assembly.dll.config").IgnoreCase,
                "ConfigurationFile");
            Assert.That(setup.PrivateBinPath, Is.SamePath(relPath), "PrivateBinPath");
            Assert.That(setup.ShadowCopyFiles, Is.Null.Or.EqualTo("false"));
        }

        [Test]
        public void CanUnloadDomain()
        {
            var domain = _domainManager.CreateDomain(_package);
            _domainManager.Unload(domain);

            CheckDomainIsUnloaded(domain);
        }

        [Test]
        public void UnloadingTwiceThrowsNUnitEngineUnloadException()
        {
            var domain = _domainManager.CreateDomain(_package);
            _domainManager.Unload(domain);

            Assert.That(() => _domainManager.Unload(domain), Throws.TypeOf<NUnitEngineUnloadException>());

            CheckDomainIsUnloaded(domain);
        }

        private void CheckDomainIsUnloaded(AppDomain domain)
        {
            // HACK: Either the Assert will succeed or the
            // exception should be thrown.
            bool unloaded = false;

            try
            {
                unloaded = domain.IsFinalizingForUnload();
            }
            catch (AppDomainUnloadedException)
            {
                unloaded = true;
            }

            Assert.That(unloaded, Is.True, "Domain was not unloaded");
        }
    }
}
#endif
