// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;

namespace TestCentric.Engine.Extensibility
{
    internal interface IExtensionAssembly
    {
        bool FromWildCard { get; }
        string AssemblyName { get; }
        Version AssemblyVersion { get; }
#if !NETSTANDARD2_0
        RuntimeFramework TargetFramework { get; }
#endif
    }
}
#endif
