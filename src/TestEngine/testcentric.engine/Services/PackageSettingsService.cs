// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using Mono.Cecil;
using NUnit.Engine;
using TestCentric.Common;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The PackageExpansionService expands packages to ensure that all information
    /// needed by lower levels of the engine is explicitly specified.
    /// 
    /// Several kinds of information expansion take place:
    /// 
    /// 1. Project Packages are expanded, using the ProjectService, to have
    /// subpackages for each included assembly.
    /// 
    /// 2. Assembly packages are annotated with internal properties that give info
    /// about how that assembly expects to be run.
    /// 
    /// 3. Aggregate packages (those with subpackages) are also annotated with the
    /// best set of property values that will allow all the asssemblies under them
    /// to run in the same Process and/or AppDomain. If this is __not__ possible,
    /// no properties are added.
    /// </summary>
    public class PackageSettingsService : Service
    {
        static Logger log = InternalTrace.GetLogger(typeof(PackageSettingsService));

        private IProjectService _projectService;
        private ITestFrameworkService _testFrameworkService;

        public override void StartService()
        {
            _projectService = ServiceContext.GetService<IProjectService>();
            _testFrameworkService = ServiceContext.GetService<ITestFrameworkService>();
            if (_projectService == null || _testFrameworkService == null)
                Status = ServiceStatus.Error;
        }

        PackageAggregationPolicy[] _aggregationPolicies = new PackageAggregationPolicy[]
        {
            new UseHighestImageRuntimeVersion(),
            new UseHighestVersionOfSameRuntimeType(),
            new RunAsX86IfAnyAssemblyRequiresIt(),
            new UseAssemblyResolverIfAnyAssemblyRequiresIt()
        };

        public void UpdatePackage(TestPackage package)
        {
            Guard.ArgumentNotNull(package, nameof(package));

            // Expand any packages for projects
            ExpandProjectPackages(package);

            if (package.SubPackages.Count > 0)
            {
                // First update subpackages, recursively
                foreach (var subPackage in package.SubPackages)
                    UpdatePackage(subPackage);

                // Then apply policies for combining settings, where possible
                foreach (var policy in _aggregationPolicies)
                    policy.ApplyTo(package);
            }
            else if (File.Exists(package.FullName) && PathUtils.IsAssemblyFileType(package.FullName))
            {
                ApplyImageSettings(package);
            }
        }

        private void ExpandProjectPackages(TestPackage package)
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
        /// Use Mono.Cecil to get information about an assembly and
        /// apply it to the package using special internal keywords.
        /// </summary>
        /// <param name="package"></param>
        private void ApplyImageSettings(TestPackage package)
        {
            Guard.ArgumentNotNull(package, nameof(package));

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
