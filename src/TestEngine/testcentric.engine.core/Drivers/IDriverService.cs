// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine.Extensibility;

namespace TestCentric.Engine.Drivers
{
    /// <summary>
    /// The IDriverService interface is implemented by the driver service, which is able
    /// to provide drivers for loading and running tests using various frameworks.
    /// </summary>
    public interface IDriverService
    {
        /// <summary>
        /// Get a driver suitable for loading and running tests in the specified assembly.
        /// </summary>
        /// <param name="domain">The application domain in which to run the tests</param>
        /// <param name="assemblyPath">The path to the test assembly</param>
        /// <param name="targetFramework">The value of any TargetFrameworkAttribute on the assembly, or null</param>
        /// <param name="skipNonTestAssemblies">True if non-test assemblies should simply be skipped rather than reporting an error</param>
        /// <returns></returns>
        IFrameworkDriver GetDriver(AppDomain domain, string assemblyPath, string targetFramework, bool skipNonTestAssemblies);
    }
}
