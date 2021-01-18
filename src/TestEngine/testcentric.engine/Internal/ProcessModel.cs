// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;

namespace TestCentric.Engine.Internal
{
    /// <summary>
    /// Represents the manner in which test assemblies are
    /// distributed across processes.
    /// </summary>
    public enum ProcessModel
    {
        /// <summary>
        /// Use the default setting, depending on the runner
        /// and the nature of the tests to be loaded.
        /// </summary>
        Default,
        /// <summary>
        /// Run tests in a single separate process
        /// </summary>
        Separate,
        /// <summary>
        /// Run tests in a separate process per assembly
        /// </summary>
        Multiple
    }
}
#endif
