// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;
using System.IO;
using Mono.Cecil;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Extensibility
{
    internal class ExtensionAssembly : IExtensionAssembly
    {
        private readonly TargetFrameworkHelper _targetFrameworkHelper;

        public ExtensionAssembly(string filePath, bool fromWildCard)
        {
            FilePath = filePath;
            FromWildCard = fromWildCard;
            Assembly = GetAssemblyDefinition();
            _targetFrameworkHelper = new TargetFrameworkHelper(Assembly);
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

#if !NETSTANDARD2_0
        public RuntimeFramework TargetFramework
        {
            get
            {
                var frameworkName = _targetFrameworkHelper.FrameworkName;
                if (frameworkName != null)
                    return RuntimeFramework.FromFrameworkName(frameworkName);

                // No TargetFrameworkAttribute - Assume .NET Framework
                return new RuntimeFramework(Runtime.Net, _targetFrameworkHelper.TargetRuntimeVersion);
            }
        }
#endif

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
#endif
