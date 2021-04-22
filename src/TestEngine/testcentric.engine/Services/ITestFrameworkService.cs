// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The ITestFrameworkService interface is able to detect known
    /// test frameworks referenced by an assembly.
    /// </summary>
    public interface ITestFrameworkService
    {
        /// <summary>
        /// Returns a list of test frameworks known to this service.
        /// </summary>
        IList<string> KnownFrameworks { get; }

        /// <summary>
        /// Returns information about the first test framework referenced
        /// by the named assembly.
        /// </summary>
        ITestFrameworkReference GetFrameworkReference(string assemblyName);
    }

    /// <summary>
    /// The ITestFramework interface represents a reference
    /// to a known test framework found in a test assembly.
    /// </summary>
    public interface ITestFrameworkReference
    {
        /// <summary>
        /// Gets the name of the test framework.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a reference to a known test framework, which was found
        /// in a test assembly or returns null if none was found.
        /// </summary>
        AssemblyName FrameworkReference { get; }

        /// <summary>
        /// Gets the AssemblyQualifiedName of the framework driver
        /// to be used for loading and running the tests.
        /// </summary>
        string FrameworkDriver { get; }
    }
}
