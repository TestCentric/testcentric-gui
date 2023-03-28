// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Runtime.Versioning;

namespace TestCentric.Extensibility
{
    internal interface IExtensionAssembly
    {
        bool FromWildCard { get; }
        string AssemblyName { get; }
        Version AssemblyVersion { get; }
        FrameworkName FrameworkName { get; }
    }
}
