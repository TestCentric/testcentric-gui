// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Runtime.Versioning;
using TestCentric.Metadata;

namespace TestCentric.Extensibility
{
    internal class ExtensionAssembly : IExtensionAssembly
    {
        public ExtensionAssembly(string filePath, bool fromWildCard)
        {
            FilePath = filePath;
            FromWildCard = fromWildCard;
            Assembly = GetAssemblyDefinition();
        }

        public string FilePath { get; }
        public bool FromWildCard { get; }
        public AssemblyDefinition Assembly { get; }

        public string AssemblyName
        {
            get { return Assembly.Name.Name; }
        }

        public Version AssemblyVersion
        {
            get { return Assembly.Name.Version; }
        }

        public ModuleDefinition MainModule
        {
            get { return Assembly.MainModule; }
        }

        public FrameworkName FrameworkName
        {
            get
            {
                var framework = Assembly.GetFrameworkName();
                if (framework != null)
                    return new FrameworkName(framework);

                // No TargetFrameworkAttribute - Assume .NET Framework
                var runtimeVersion = Assembly.GetRuntimeVersion();
                return new FrameworkName(".NETFramework", new Version(runtimeVersion.Major, runtimeVersion.Minor));
            }
        }

        private AssemblyDefinition GetAssemblyDefinition()
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(FilePath));
            resolver.AddSearchDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            var parameters = new ReaderParameters { AssemblyResolver = resolver };

            return AssemblyDefinition.ReadAssembly(FilePath, parameters);
        }
    }
}
