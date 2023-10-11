// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Extensibility;
using TestCentric.Engine.Extensibility;

// Extension points supported by the engine

[assembly: ExtensionPoint("/NUnit/Engine/NUnitV2Driver", typeof(IFrameworkDriver),
    Description="Driver for NUnit tests using the V2 framework.")]
