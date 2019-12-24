// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Engine
{
    /// <summary>
    /// Enumeration identifying all known (to the engine) runtimes
    /// </summary>
    public enum RuntimeType
    {
        /// <summary>Microsoft .NET Framework</summary>
        Net,
        /// <summary>Mono</summary>
        // TODO: Determine if Mono should be a separate runtime
        // type or just a specific instance of .NET Framework.
        Mono,
        /// <summary>NetCore</summary>
        NetCore
    }
}
