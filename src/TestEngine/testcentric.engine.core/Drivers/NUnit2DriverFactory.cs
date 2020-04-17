// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using System;
using System.Reflection;
using NUnit.Engine.Extensibility;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Drivers
{
    public class NUnit2DriverFactory : IDriverFactory
    {
        private const string NUNIT_FRAMEWORK = "nunit.framework";
        private const string NUNITLITE_FRAMEWORK = "nunitlite";
        private ExtensionNode _driverNode;

        // TODO: This should be a central service but for now it's local
        private ProvidedPathsAssemblyResolver _resolver;
        bool _resolverInstalled;

        public NUnit2DriverFactory(ExtensionNode driverNode)
        {
            _driverNode = driverNode;
            _resolver = new ProvidedPathsAssemblyResolver();
        }

        /// <summary>
        /// Gets a flag indicating whether a given assembly name and version
        /// represent a test framework supported by this factory.
        /// </summary>
        /// <param name="reference">An AssemblyName referring to the possible test framework.</param>
        public bool IsSupportedTestFramework(AssemblyName reference)
        {
            return NUNIT_FRAMEWORK.Equals(reference.Name, StringComparison.OrdinalIgnoreCase) && reference.Version.Major == 2
                || NUNITLITE_FRAMEWORK.Equals(reference.Name, StringComparison.OrdinalIgnoreCase) && reference.Version.Major == 1;
        }

        /// <summary>
        /// Gets a driver for a given test assembly and a framework
        /// which the assembly is already known to reference.
        /// </summary>
        /// <param name="domain">The domain in which the assembly will be loaded</param>
        /// <param name="reference">The name of the test framework reference</param>
        /// <returns></returns>
        public IFrameworkDriver GetDriver(AppDomain domain, AssemblyName reference)
        {
            if (!IsSupportedTestFramework(reference))
                throw new ArgumentException("Invalid framework", "reference");

            if (!_resolverInstalled)
            {
                _resolver.Install();
                _resolverInstalled = true;
                _resolver.AddPathFromFile(_driverNode.AssemblyPath);
            }

            return _driverNode.CreateExtensionObject(domain) as IFrameworkDriver;
        }
    }
}
#endif
