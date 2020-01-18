// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using TestCentric.Common;
using TestCentric.Engine.Drivers;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Helpers;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The DriverService provides drivers able to load and run tests
    /// using various frameworks.
    /// </summary>
    public class DriverService : Service, IDriverService
    {
        readonly IList<IDriverFactory> _factories = new List<IDriverFactory>();

        /// <summary>
        /// Get a driver suitable for use with a particular test assembly.
        /// </summary>
        /// <param name="domain">The application domain to use for the tests</param>
        /// <param name="assemblyPath">The full path to the test assembly</param>
        /// <returns></returns>
#if NETSTANDARD1_6
        public IFrameworkDriver GetDriver(TestPackage package)
#else
        public IFrameworkDriver GetDriver(AppDomain domain, TestPackage package)
#endif
        {
            var assemblyPath = package.FullName;

            if (!File.Exists(assemblyPath))
                return new InvalidAssemblyFrameworkDriver(assemblyPath, "File not found: " + assemblyPath);

            if (!PathUtils.IsAssemblyFileType(assemblyPath))
                return new InvalidAssemblyFrameworkDriver(assemblyPath, "File type is not supported");

            bool skipNonTestAssemblies = package.GetSetting(EnginePackageSettings.SkipNonTestAssemblies, false);
            bool isNonTestAssembly = package.GetSetting(InternalEnginePackageSettings.ImageNonTestAssembly, false);
            string targetFramework = package.GetSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, (string)null);

            if (skipNonTestAssemblies && isNonTestAssembly)
                return new SkippedAssemblyFrameworkDriver(assemblyPath);

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
            if (targetFramework != null)
            {
                // This takes care of an issue with Roslyn. It may get fixed, but we still
                // have to deal with assemblies having this setting. I'm assuming that
                // any true Portable assembly would have a Profile as part of its name.
                var platform = targetFramework == ".NETPortable,Version=v5.0"
                    ? ".NETStandard"
                    : targetFramework.Split(new char[] { ',' })[0];
                if (platform == "Silverlight" || platform == ".NETPortable" || platform == ".NETStandard" || platform == ".NETCompactFramework")
                    return new InvalidAssemblyFrameworkDriver(assemblyPath, platform + " test assemblies are not supported by this version of the engine");
            }
#endif

            try
            {
                var assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath);

                var references = new List<AssemblyName>();
                foreach (var cecilRef in assemblyDef.MainModule.AssemblyReferences)
                    references.Add(new AssemblyName(cecilRef.FullName));

                foreach (var factory in _factories)
                {
                    foreach (var reference in references)
                    {
                        if (factory.IsSupportedTestFramework(reference))
#if NETSTANDARD1_6 || NETSTANDARD2_0
                            return factory.GetDriver(reference);
#else
                            return factory.GetDriver(domain, reference);
#endif
                    }
                }
            }
            catch (BadImageFormatException ex)
            {
                return new InvalidAssemblyFrameworkDriver(assemblyPath, ex.Message);
            }

            if (skipNonTestAssemblies)
                return new SkippedAssemblyFrameworkDriver(assemblyPath);
            else
                return new InvalidAssemblyFrameworkDriver(assemblyPath, string.Format("No suitable tests found in '{0}'.\n" +
                                                                              "Either assembly contains no tests or proper test driver has not been found.", assemblyPath));
        }

        public override void StartService()
        {
            Guard.OperationValid(ServiceContext != null, "Can't start DriverService outside of a ServiceContext");

            try
            {
//#if NET20 || NETSTANDARD2_0
//                var extensionService = ServiceContext.GetService<ExtensionService>();
//                if (extensionService != null)
//                {
//                    foreach (IDriverFactory factory in extensionService.GetExtensions<IDriverFactory>())
//                        _factories.Add(factory);

//#if NET20
//                    var node = extensionService.GetExtensionNode("/NUnit/Engine/NUnitV2Driver");
//                    if (node != null)
//                        _factories.Add(new NUnit2DriverFactory(node));
//#endif
//                }
//#endif

                _factories.Add(new NUnit3DriverFactory());

                Status = ServiceStatus.Started;
            }
            catch(Exception)
            {
                Status = ServiceStatus.Error;
                throw;
            }
        }
    }
}
