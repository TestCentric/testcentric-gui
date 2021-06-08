// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    public class RuntimeFrameworkServiceTests
    {
        private RuntimeFrameworkService _runtimeService;
        private TestPackageAnalyzer _packageAnalyzer;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            services.Add(new Fakes.FakeProjectService());
            services.Add(new RuntimeFrameworkService());
            services.Add(new TestFrameworkService());
            services.Add(new TestPackageAnalyzer());
            services.ServiceManager.StartServices();
            _runtimeService = services.GetService<RuntimeFrameworkService>();
            _packageAnalyzer = services.GetService<TestPackageAnalyzer>();
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

            _packageAnalyzer.ApplyImageSettings(package);
            var runtimeFramework = _runtimeService.SelectRuntimeFramework(package);

            Assert.That(package.GetSetting("TargetRuntimeFramework", ""), Is.EqualTo(runtimeFramework));
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
            package.AddSetting(EnginePackageSettings.ImageTargetFrameworkName, framework);
            package.AddSetting(EnginePackageSettings.ImageRuntimeVersion, new Version(majorVersion, minorVersion));
            package.AddSetting(EnginePackageSettings.RequestedRuntimeFramework, requested);

            _runtimeService.SelectRuntimeFramework(package);
            Assert.That(package.GetSetting<string>(EnginePackageSettings.TargetRuntimeFramework, null), Is.EqualTo(requested));
        }

        //[Test, Platform(Exclude ="Linux")]
        //public void RuntimeFrameworkIsSetForSubpackages()
        //{
        //    var topLevelPackage = new TestPackage(new [] {"a.dll", "b.dll"});

        //    var net20Package = topLevelPackage.SubPackages[0];
        //    net20Package.Settings.Add(EnginePackageSettings.ImageRuntimeVersion, new Version("2.0.50727"));
        //    var net40Package = topLevelPackage.SubPackages[1];
        //    net40Package.Settings.Add(EnginePackageSettings.ImageRuntimeVersion, new Version("4.0.30319"));

        //    var platform = Environment.OSVersion.Platform;

        //    _packageAnalyzer.ApplyImageSettings(topLevelPackage);
        //    _runtimeService.SelectRuntimeFramework(topLevelPackage);

        //    Assert.Multiple(() =>
        //    {
        //        // UPDATE: No longer working on Linux - Excluded for now
        //        // HACK: this test will pass on a windows system with .NET 2.0 and .NET 4.0 installed or on a 
        //        // linux system with a newer version of Mono with no 2.0 profile.
        //        // TODO: Test should not depend on the availability of specific runtimes
        //        if (platform == PlatformID.Win32NT)
        //        {
        //            Assert.That(net20Package.Settings[EnginePackageSettings.TargetRuntimeFramework], Is.EqualTo("net-2.0"));
        //            Assert.That(net40Package.Settings[EnginePackageSettings.TargetRuntimeFramework], Is.EqualTo("net-4.0"));
        //        }
        //        else
        //        {
        //            Assert.That(net20Package.Settings[EnginePackageSettings.TargetRuntimeFramework], Is.EqualTo("mono-2.0"));
        //            Assert.That(net40Package.Settings[EnginePackageSettings.TargetRuntimeFramework], Is.EqualTo("mono-4.0"));
        //        }
        //    });
        //}

        [TestCase("1.1", "2.1", "3.1", ExpectedResult = new[] { "netcore-1.1", "netcore-2.1", "netcore-3.1" })]
        [TestCase("1.1.14", "2.1.508", "3.1.201", ExpectedResult = new[] { "netcore-1.1", "netcore-2.1", "netcore-3.1" })]
        [TestCase("1.1.14", "2.1.202", "2.1.508", "2.1.509", "2.1.512", "3.1.201", ExpectedResult = new[] { "netcore-1.1", "netcore-2.1", "netcore-3.1" })]
        [TestCase("1.0.1", "2.0.7", "2.0.9", "2.1.2", "2.1.3-servicing-26724-03", "2.1.4", "2.1.5", ExpectedResult = new[] { "netcore-1.0", "netcore-2.0", "netcore-2.1" })]
        [TestCase("1.0.1", "2.0.7", "2.0.9", "2.1.3-servicing-26724-03", ExpectedResult = new[] { "netcore-1.0", "netcore-2.0", "netcore-2.1" })]
        public string[] GetNetCoreRuntimesFromDirectoryNames(params string[] dirNames)
        {
            return _runtimeService.GetNetCoreRuntimesFromDirectoryNames(dirNames).Select((r) => r.Id).ToArray();
        }
    }
}
#endif
