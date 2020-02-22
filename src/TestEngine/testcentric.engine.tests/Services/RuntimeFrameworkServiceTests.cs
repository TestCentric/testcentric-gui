// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    public class RuntimeFrameworkServiceTests
    {
        private RuntimeFrameworkService _runtimeService;
        private PackageSettingsService _packageManager;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            services.Add(new Fakes.FakeProjectService());
            _runtimeService = new RuntimeFrameworkService();
            services.Add(_runtimeService);
            _packageManager = new PackageSettingsService();
            services.Add(_packageManager);
            services.ServiceManager.StartServices();
        }

        [TearDown]
        public void StopService()
        {
            _runtimeService.StopService();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_runtimeService.Status, Is.EqualTo(ServiceStatus.Started));
        }

        // TODO: Review whether this test is contributing anything
        [TestCase("mock-assembly.dll", false)]
        [TestCase("../../agents/net20/testcentric-agent.exe", false, ExcludePlatform = "Linux")]
        [TestCase("../../agents/net20/testcentric-agent-x86.exe", true, ExcludePlatform = "Linux")]
        public void SelectRuntimeFramework(string assemblyName, bool runAsX86)
        {
            var assemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, assemblyName);
            FileAssert.Exists(assemblyPath, $"File not found: {assemblyPath}");
            var package = new TestPackage(assemblyPath);

            _packageManager.UpdatePackage(package);
            var runtimeFramework = _runtimeService.SelectRuntimeFramework(package);

            Assert.That(package.GetSetting("RuntimeFramework", ""), Is.EqualTo(runtimeFramework));
            Assert.That(package.GetSetting("RunAsX86", false), Is.EqualTo(runAsX86));
        }

        [Test]
        public void AvailableFrameworks()
        {
            var available = _runtimeService.AvailableRuntimes;
            Assert.That(available.Count, Is.GreaterThan(0));
            foreach (var framework in available)
                Console.WriteLine("Available: {0}", framework.DisplayName);
        }

        [Test]
        public void CurrentFrameworkMustBeAvailable()
        {
            var current = RuntimeFramework.CurrentFramework;
            Console.WriteLine("Current framework is {0} ({1})", current.DisplayName, current.Id);
            Assert.That(_runtimeService.IsAvailable(current), "{0} not available", current);
        }

        [Test]
        public void AvailableFrameworksListContainsNoDuplicates()
        {
            var names = new List<string>();
            foreach (var framework in _runtimeService.AvailableRuntimes)
                names.Add(framework.DisplayName);
            Assert.That(names, Is.Unique);
        }

        [TestCase("mono", 2, 0, "net-4.0")]
        [TestCase("net", 4, 0, "net-4.0")]
        [TestCase("net", 3, 5, "net-4.0")]
        public void EngineOptionPreferredOverImageTarget(string framework, int majorVersion, int minorVersion, string requested)
        {
            var package = new TestPackage("test");
            package.AddSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, framework);
            package.AddSetting(InternalEnginePackageSettings.ImageRuntimeVersion, new Version(majorVersion, minorVersion));
            package.AddSetting(EnginePackageSettings.RuntimeFramework, requested);

            _runtimeService.SelectRuntimeFramework(package);
            Assert.That(package.GetSetting<string>(EnginePackageSettings.RuntimeFramework, null), Is.EqualTo(requested));
        }

        [Test, Platform(Exclude ="Linux")]
        public void RuntimeFrameworkIsSetForSubpackages()
        {
            var topLevelPackage = new TestPackage(new [] {"a.dll", "b.dll"});

            var net20Package = topLevelPackage.SubPackages[0];
            net20Package.Settings.Add(InternalEnginePackageSettings.ImageRuntimeVersion, new Version("2.0.50727"));
            var net40Package = topLevelPackage.SubPackages[1];
            net40Package.Settings.Add(InternalEnginePackageSettings.ImageRuntimeVersion, new Version("4.0.30319"));

            var platform = Environment.OSVersion.Platform;

            _packageManager.UpdatePackage(topLevelPackage);
            _runtimeService.SelectRuntimeFramework(topLevelPackage);

            Assert.Multiple(() =>
            {
                // UPDATE: No longer working on Linux - Excluded for now
                // HACK: this test will pass on a windows system with .NET 2.0 and .NET 4.0 installed or on a 
                // linux system with a newer version of Mono with no 2.0 profile.
                // TODO: Test should not depend on the availability of specific runtimes
                if (platform == PlatformID.Win32NT)
                {
                    Assert.That(net20Package.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("net-2.0"));
                    Assert.That(net40Package.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("net-4.0"));
                    Assert.That(topLevelPackage.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("net-4.0"));
                }
                else
                {
                    Assert.That(net20Package.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("mono-2.0"));
                    Assert.That(net40Package.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("mono-4.0"));
                    Assert.That(topLevelPackage.Settings[EnginePackageSettings.RuntimeFramework], Is.EqualTo("mono-4.0"));
                }
            });
        }
    }
}
#endif
