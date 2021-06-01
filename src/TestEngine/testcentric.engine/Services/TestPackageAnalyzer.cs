// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using Mono.Cecil;
using NUnit.Engine;
using TestCentric.Common;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The TestPackageAnalyzer analyzes and expands packages in order to
    /// ensure that all information needed by lower levels of the engine
    /// is explicitly specified.
    /// </summary>
    public class TestPackageAnalyzer : Service
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestPackageAnalyzer));

        private IProjectService _projectService;
        private ITestFrameworkService _testFrameworkService;
        private IRuntimeFrameworkService _runtimeService;

        public override void StartService()
        {
            _projectService = ServiceContext.GetService<IProjectService>();
            _testFrameworkService = ServiceContext.GetService<ITestFrameworkService>();
            _runtimeService = ServiceContext.GetService<IRuntimeFrameworkService>();
            if (_projectService == null || _testFrameworkService == null || _runtimeService == null)
                Status = ServiceStatus.Error;
        }

        /// <summary>
        /// Examine package settings to see if they are valid. An
        /// <see cref="NUnitEngineException"> is thrown if errors
        /// are found.
        /// </summary>
        /// <param name="package">The package whose settings are to be validated</param>
        public void ValidatePackageSettings(TestPackage package)
        {
            var sb = new StringBuilder();

#if !NETSTANDARD2_0  // TODO: How do we validate runtime framework for .NET Standard 2.0?
            var frameworkSetting = package.GetSetting(EnginePackageSettings.RequestedRuntimeFramework, "");
            var runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);

            if (frameworkSetting.Length > 0)
            {
                // Check requested framework is actually available
                if (!_runtimeService.IsAvailable(frameworkSetting))
                    sb.Append($"\n* The requested framework {frameworkSetting} is unknown or not available.\n");
            }
#endif

            // At this point, any unsupported settings in the TestPackage were
            // put there by the user, so we consider the package invalid. This
            // will be checked again as each project package is expanded and
            // treated as a warning in that case.
            foreach (string setting in package.Settings.Keys)
            {
                if (EnginePackageSettings.IsObsoleteSetting(setting))
                    sb.Append($"\n* The {setting} setting is no longer supported.\n");
            }

            if (sb.Length > 0)
                throw new NUnitEngineException($"The following errors were detected in the TestPackage:\n{sb}");
        }

        /// <summary>
        /// Expand any supported project packages so that all assemblies
        /// included appear as subpackages. The <see cref="ProjectService"/>
        /// is used to expand the projects.
        /// </summary>
        /// <param name="package"></param>
        public void ExpandProjectPackages(TestPackage package)
        {
            if (package == null) throw new ArgumentNullException("package");

            foreach (var subPackage in package.SubPackages)
            {
                ExpandProjectPackages(subPackage);
            }

            if (package.SubPackages.Count == 0 &&
                !string.IsNullOrEmpty(package.FullName) &&
                _projectService.CanLoadFrom(package.FullName))
            {
                _projectService.ExpandProjectPackage(package);
            }
        }

        /// <summary>
        /// Use metadata to get information about an assembly and
        /// apply it to the package using special internal keywords.
        /// </summary>
        /// <param name="package"></param>
        public void ApplyImageSettings(TestPackage package)
        {
            Guard.ArgumentNotNull(package, nameof(package));
            Guard.ArgumentValid(package.IsAssemblyPackage(), "ApplyImageSettings called for non-assembly", nameof(package));

            var assembly = AssemblyDefinition.ReadAssembly(package.FullName);

            var targetVersion = assembly.GetRuntimeVersion();
            if (targetVersion.Major > 0)
            {
                log.Debug($"Assembly {package.FullName} uses version {targetVersion}");
                package.Settings[EnginePackageSettings.ImageRuntimeVersion] = targetVersion;
            }

            var frameworkName = assembly.GetFrameworkName();
            if (!string.IsNullOrEmpty(frameworkName))
            {
                log.Debug($"Assembly {package.FullName} targets {frameworkName}");
                // This takes care of an old issue with Roslyn
                if (frameworkName == ".NETPortable,Version=v5.0")
                {
                    frameworkName = ".NETStandard,Version=v1.0";
                    log.Debug($"Using {frameworkName} instead");
                }
                package.Settings[EnginePackageSettings.ImageTargetFrameworkName] = frameworkName;
            }

            package.Settings[EnginePackageSettings.ImageRequiresX86] = assembly.RequiresX86();
            if (assembly.RequiresX86())
            {
                log.Debug($"Assembly {package.FullName} will be run x86");
                package.Settings[EnginePackageSettings.RunAsX86] = true;
            }

            bool requiresAssemblyResolver = assembly.HasAttribute("NUnit.Framework.TestAssemblyDirectoryResolveAttribute");
            package.Settings[EnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver] = requiresAssemblyResolver;
            if (requiresAssemblyResolver)
            {
                log.Debug($"Assembly {package.FullName} requires default app domain assembly resolver");
            }

            bool nonTestAssembly = assembly.HasAttribute("NUnit.Framework.NonTestAssemblyAttribute");
            if (nonTestAssembly)
            {
                log.Debug($"Assembly {package.FullName} has NonTestAssemblyAttribute");
                package.Settings[EnginePackageSettings.ImageNonTestAssemblyAttribute] = true;
            }

            var testFrameworkReference = _testFrameworkService.GetFrameworkReference(package.FullName);
            if (testFrameworkReference != null)
            {
                package.Settings[EnginePackageSettings.ImageTestFrameworkReference] = testFrameworkReference.FrameworkReference.FullName;
                if (testFrameworkReference.FrameworkDriver != null)
                    package.Settings[EnginePackageSettings.ImageFrameworkDriverReference] = testFrameworkReference.FrameworkDriver;
            }
        }
    }
}
