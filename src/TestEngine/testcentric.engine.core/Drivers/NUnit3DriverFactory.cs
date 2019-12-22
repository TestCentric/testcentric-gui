// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using NUnit.Common;
using NUnit.Engine.Extensibility;

namespace NUnit.Engine.Drivers
{
    public class NUnit3DriverFactory : IDriverFactory
    {
        private const string NUNIT_FRAMEWORK = "nunit.framework";

        /// <summary>
        /// Gets a flag indicating whether a given assembly name and version
        /// represent a test framework supported by this factory.
        /// </summary>
        /// <param name="reference">An AssemblyName referring to the possible test framework.</param>
        public bool IsSupportedTestFramework(AssemblyName reference)
        {
            return NUNIT_FRAMEWORK.Equals(reference.Name, StringComparison.OrdinalIgnoreCase) && reference.Version.Major == 3;
        }

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
        /// <summary>
        /// Gets a driver for a given test assembly and a framework
        /// which the assembly is already known to reference.
        /// </summary>
        /// <param name="domain">The domain in which the assembly will be loaded</param>
        /// <param name="reference">An AssemblyName referring to the test framework.</param>
        /// <returns></returns>
        public IFrameworkDriver GetDriver(AppDomain domain, AssemblyName reference)
        {
            Guard.ArgumentValid(IsSupportedTestFramework(reference), "Invalid framework", "reference");

            return new NUnit3FrameworkDriver(domain, reference);
        }
#else
        /// <summary>
        /// Gets a driver for a given test assembly and a framework
        /// which the assembly is already known to reference.
        /// </summary>
        /// <param name="domain">The domain in which the assembly will be loaded</param>
        /// <param name="reference">An AssemblyName referring to the test framework.</param>
        /// <returns></returns>
        public IFrameworkDriver GetDriver(AssemblyName reference)
        {
            Guard.ArgumentValid(IsSupportedTestFramework(reference), "Invalid framework", "reference");

            return new NUnitNetStandardDriver();
        }
#endif
    }
}
