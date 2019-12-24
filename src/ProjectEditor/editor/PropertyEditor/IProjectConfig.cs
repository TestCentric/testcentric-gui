// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor
{
    public interface IProjectConfig
    {
        string Name { get; set; }

        string BasePath { get; set; }

        string RelativeBasePath { get; }

        string EffectiveBasePath { get; }

        string ConfigurationFile { get; set; }

        string PrivateBinPath { get; set; }

        BinPathType BinPathType { get; set; }

        AssemblyList Assemblies { get; }

        RuntimeFramework RuntimeFramework { get; set; }
    }
}
