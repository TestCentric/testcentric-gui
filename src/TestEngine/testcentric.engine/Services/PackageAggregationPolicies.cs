// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Runtime.Versioning;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    abstract class PackageAggregationPolicy
    {
        public abstract void ApplyTo(TestPackage package);
    }

    class UseHighestImageRuntimeVersion : PackageAggregationPolicy
    {
        static readonly string VERSION_SETTING = InternalEnginePackageSettings.ImageRuntimeVersion;

        public override void ApplyTo(TestPackage package)
        {
            Version maxVersion = new Version(0, 0);

            foreach (var subpackage in package.SubPackages)
            {
                var version = subpackage.GetSetting(VERSION_SETTING, maxVersion);
                if (version > maxVersion)
                    maxVersion = version;
            }

            if (maxVersion.Major > 0)
                package.Settings[VERSION_SETTING] = maxVersion;
        }
    }

    class UseHighestVersionOfSameRuntimeType : PackageAggregationPolicy
    {
        static readonly string TARGET_FRAMEWORK_SETTING = InternalEnginePackageSettings.ImageTargetFrameworkName;

        public override void ApplyTo(TestPackage package)
        {
            FrameworkName targetRuntime = null;

            foreach (var subpackage in package.SubPackages)
            {
                string fn = subpackage.GetSetting(TARGET_FRAMEWORK_SETTING, string.Empty);
                if (fn == string.Empty)
                    continue;

                var runtime = new FrameworkName(fn);
                if (targetRuntime == null ||
                    targetRuntime.Identifier == runtime.Identifier && targetRuntime.Version < runtime.Version)
                {
                    targetRuntime = runtime;
                }
                // TODO: Should we add an else that notes the runtimes are not compatible?
            }

            if (targetRuntime != null)
                package.Settings[TARGET_FRAMEWORK_SETTING] = targetRuntime.FullName;
        }
    }

    class RunAsX86IfAnyAssemblyRequiresIt : PackageAggregationPolicy
    {
        public override void ApplyTo(TestPackage package)
        {
            bool requiresX86 = false;

            foreach (var subpackage in package.SubPackages)
            {
                if (subpackage.GetSetting(InternalEnginePackageSettings.ImageRequiresX86, false))
                    requiresX86 = true;
            }

            if (requiresX86)
            {
                package.Settings[InternalEnginePackageSettings.ImageRequiresX86] = true;
                package.Settings[EnginePackageSettings.RunAsX86] = true;
            }
        }
    }

    class UseAssemblyResolverIfAnyAssemblyRequiresIt : PackageAggregationPolicy
    {
        public override void ApplyTo(TestPackage package)
        {
            bool requiresResolver = false;

            foreach (var subpackage in package.SubPackages)
            {
                if (subpackage.GetSetting(InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false))
                    requiresResolver = true;
            }

            if (requiresResolver)
                package.Settings[InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver] = true;
        }
    }
}
