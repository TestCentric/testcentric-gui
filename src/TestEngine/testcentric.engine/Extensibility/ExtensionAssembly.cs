// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using Mono.Cecil;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Extensibility
{
    internal class ExtensionAssembly : AssemblyView, IExtensionAssembly
    {
        public ExtensionAssembly(string filePath, bool fromWildCard)
            :base(filePath, GetReaderParameters(filePath))
        {
            FromWildCard = fromWildCard;
        }

        private static ReaderParameters GetReaderParameters(string filePath)
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(filePath));
            resolver.AddSearchDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            return new ReaderParameters { AssemblyResolver = resolver };
        }

        public bool FromWildCard { get; }

        public RuntimeFramework TargetFramework
        {
            get
            {
                if (FrameworkName != null)
                    return RuntimeFramework.FromFrameworkName(FrameworkName);

                // No TargetFrameworkAttribute - Assume .NET Framework
                return new RuntimeFramework(Runtime.Net, TargetRuntimeVersion);
            }
        }
    }
}
