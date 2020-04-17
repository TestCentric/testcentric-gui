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
                Assert.That(package.GetSetting(InternalEnginePackageSettings.ImageRuntimeVersion, ""), Is.EqualTo(""));
                Assert.That(package.GetSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, ""), Is.EqualTo(""));
                Assert.False(package.GetSetting(InternalEnginePackageSettings.ImageRequiresX86, false));
                Assert.False(package.GetSetting(InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));

                Assert.That(subPackage.GetSetting(InternalEnginePackageSettings.ImageRuntimeVersion, ""), Is.EqualTo(""));
                Assert.That(subPackage.GetSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, ""), Is.EqualTo(""));
                Assert.False(subPackage.GetSetting(InternalEnginePackageSettings.ImageRequiresX86, false));
                Assert.False(subPackage.GetSetting(InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));
            });
        }

        [Test]
        public void UseHighestImageRuntimeVersion()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            // Leave subpackage 0 empty
            package.SubPackages[1].Settings[InternalEnginePackageSettings.ImageRuntimeVersion] = new Version(4, 0, 30319);
            package.SubPackages[2].Settings[InternalEnginePackageSettings.ImageRuntimeVersion] = new Version(2, 0, 50727);

            _packageService.UpdatePackage(package);

            Assert.That(package.Settings[InternalEnginePackageSettings.ImageRuntimeVersion], Is.EqualTo(new Version(4, 0, 30319)));
        }

        [Test]
        public void UseHighestVersionOfSameRuntimeType()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[0].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NETFramework,Version=2.0.50727";
            package.SubPackages[1].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NETFramework,Version=v4.0.30319";
            package.SubPackages[2].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NETFramework,Version=4.0.30319";

            _packageService.UpdatePackage(package);

            Assert.That(package.Settings[InternalEnginePackageSettings.ImageTargetFrameworkName], Is.EqualTo(".NETFramework,Version=v4.0.30319"));
        }

        [Test, Ignore("Not yet implemented")]
        public void DiferentRuntimeTypesMayNotBeAggregated()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[0].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NETFramework,Version=2.0.50727";
            package.SubPackages[1].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NetCoreApp,Version=v3.1";
            package.SubPackages[2].Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = ".NETFramework,Version=4.0.30319";

            _packageService.UpdatePackage(package);

            // We need to decide how aggregate packages should indicate that the contained assemblies
            // are able to run or not run in the same project or AppDomain
            Assert.That(package.Settings[InternalEnginePackageSettings.ImageTargetFrameworkName], Is.Null);
        }

        [Test]
        public void RunAsX86IfAnyRequiresIt()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[1].Settings[InternalEnginePackageSettings.ImageRequiresX86] = true;

            _packageService.UpdatePackage(package);

            Assert.True(package.GetSetting(InternalEnginePackageSettings.ImageRequiresX86, false));
        }

        [Test]
        public void RequireResolverIfAnyRequiresIt()
        {
            var package = new TestPackage(new string[] { "one.dll", "two.dll", "three.dll" });

            package.SubPackages[1].Settings[InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver] = true;

            _packageService.UpdatePackage(package);

            Assert.True(package.GetSetting(InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false));
        }
    }
}
