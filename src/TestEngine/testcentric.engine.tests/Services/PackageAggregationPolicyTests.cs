// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;
    using NUnit.Engine;

    public class PackageAggregationPolicyTests
    {
        private PackageSettingsService _packageService;

        [SetUp]
        public void CreateService()
        {
            var projectService = new FakeProjectService();
            _packageService = new PackageSettingsService();

            var context = new ServiceContext();
            context.Add(projectService);
            context.Add(_packageService);

            context.ServiceManager.StartServices();
        }

        [Test]
        public void DefaultSettings()
        {
            var package = new TestPackage(new string[] { "some.dll" });
            var subPackage = package.SubPackages[0];

            _packageService.UpdatePackage(package);

            Assert.Multiple(() =>
            {
                Assert.That(package.GetSetting(EnginePackageSettings.ImageRuntimeVersion, ""), Is.EqualTo(""));
                Assert.That(package.GetSetting(EnginePackageSettings.ImageTargetFrameworkName, ""), Is.EqualTo(""));
                Assert.False(package.GetSetting(EnginePackageSettings.ImageRequiresX86, false));
                Assert.False(package.GetSetting(EnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));

                Assert.That(subPackage.GetSetting(EnginePackageSettings.ImageRuntimeVersion, ""), Is.EqualTo(""));
                Assert.That(subPackage.GetSetting(EnginePackageSettings.ImageTargetFrameworkName, ""), Is.EqualTo(""));
                Assert.False(subPackage.GetSetting(EnginePackageSettings.ImageRequiresX86, false));
                Assert.False(subPackage.GetSetting(EnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));
            });
        }

        [Test]
        public void UseHighestImageRuntimeVersion()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            // Leave subpackage 0 empty
            package.SubPackages[1].Settings[EnginePackageSettings.ImageRuntimeVersion] = new Version(4, 0, 30319);
            package.SubPackages[2].Settings[EnginePackageSettings.ImageRuntimeVersion] = new Version(2, 0, 50727);

            _packageService.UpdatePackage(package);

            Assert.That(package.Settings[EnginePackageSettings.ImageRuntimeVersion], Is.EqualTo(new Version(4, 0, 30319)));
        }

        [TestCase(".NETFramework,Version=2.0.50727", ".NETFramework,Version=v4.0.30319", ".NETFramework,Version=4.0.30319",
            ExpectedResult = ".NETFramework,Version=v4.0.30319")]
        [TestCase(".NETCoreApp,Version=2.1", ".NETCoreApp,Version=v3.1", ".NETCoreApp,Version=3.1",
            ExpectedResult = ".NETCoreApp,Version=v3.1")]
        [TestCase(".NETFramework,Version=2.0.50727", ".NETFramework,Version=v4.0.30319", "NONE",
            ExpectedResult = "NONE")]
        [TestCase(".NETFramework,Version=2.0.50727", ".NETFramework,Version=v4.0.30319", ".NETCoreApp,Version=3.1",
            ExpectedResult = "NONE")]
        public string UseHighestVersionOfSameRuntimeType(params string[] subPackageRuntimes)
        {
            var package = new TestPackage("PACKAGE");
            foreach (string runtime in subPackageRuntimes)
            {
                var subPackage = new TestPackage("SUBPACKAGE");
                if (runtime != "NONE")
                    subPackage.Settings[EnginePackageSettings.ImageTargetFrameworkName] = runtime;
                package.AddSubPackage(subPackage);
            }
            _packageService.UpdatePackage(package);

            return package.GetSetting(EnginePackageSettings.ImageTargetFrameworkName, "NONE");
        }

        [Test]
        public void RunAsX86IfAnyRequiresIt()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[1].Settings[EnginePackageSettings.ImageRequiresX86] = true;

            _packageService.UpdatePackage(package);

            Assert.True(package.GetSetting(EnginePackageSettings.ImageRequiresX86, false));
        }

        [Test]
        public void RequireResolverIfAnyRequiresIt()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[1].Settings[EnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver] = true;

            _packageService.UpdatePackage(package);

            Assert.True(package.GetSetting(EnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));
        }
    }
}
