// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Collections.Generic;
using Mono.Cecil;

namespace TestCentric.Engine.Internal
{
    public class AssemblyView
    {
        private readonly AssemblyDefinition _assemblyDef;
        private readonly ModuleDefinition _module;

        public static AssemblyView ReadAssembly(string assemblyPath)
        {
            return new AssemblyView(assemblyPath);
        }

        public static AssemblyView ReadAssembly(string assemblyPath, ReaderParameters parameters)
        {
            return new AssemblyView(assemblyPath, parameters);
        }

        protected AssemblyView(string assemblyPath, ReaderParameters parameters = null)
        {
            AssemblyPath = assemblyPath;

            try
            {
                _assemblyDef = parameters != null
                    ? AssemblyDefinition.ReadAssembly(assemblyPath, parameters)
                    : AssemblyDefinition.ReadAssembly(assemblyPath);
                _module = _assemblyDef.MainModule;
            }
            catch (Exception e)
            {
                throw new NUnitEngineException($"{assemblyPath} could not be examined", e);
            }
        }

        public AssemblyDefinition AssemblyDefinition => _assemblyDef;
        public ModuleDefinition MainModule => _module;

        public string AssemblyPath { get; }
        public AssemblyName AssemblyName => new AssemblyName(FullName);
        public string Name => _assemblyDef.Name.Name;
        public string FullName => _assemblyDef.Name.FullName;
        public Version AssemblyVersion => _assemblyDef.Name.Version;

        public bool RequiresAssemblyResolver => HasAttribute("NUnit.Framework.TestAssemblyDirectoryResolveAttribute");

        public bool RequiresX86
        {
            get
            {
                const ModuleAttributes nativeEntryPoint = (ModuleAttributes)16;
                const ModuleAttributes mask = ModuleAttributes.Required32Bit | nativeEntryPoint;

                return _module.Architecture != TargetArchitecture.AMD64 &&
                       _module.Architecture != TargetArchitecture.IA64 &&
                       (_module.Attributes & mask) != 0;
            }
        }

        public Version TargetRuntimeVersion
        {
            get
            {
                var runtimeVersion = _module.RuntimeVersion;

                if (runtimeVersion.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                    runtimeVersion = runtimeVersion.Remove(0, 1);

                return new Version(runtimeVersion);
            }
        }

        public string FrameworkName
        {
            get
            {
                return GetCustomAttribute(
                    "System.Runtime.Versioning.TargetFrameworkAttribute")
                        ?.ConstructorArguments[0].Value as string;
            }
        }

        public bool HasAttribute(string attrName)
        {
            foreach (var attr in _assemblyDef.CustomAttributes)
                if (attr.AttributeType.FullName == attrName)
                    return true;

            return false;
        }

        private CustomAttribute GetCustomAttribute(string attrName)
        {
            foreach (var attr in _assemblyDef.CustomAttributes)
                if (attr.AttributeType.FullName == attrName)
                    return attr;

            return null;
        }

        public IEnumerable<AssemblyName> AssemblyReferences
        {
            get
            {
                foreach (var reference in _module.AssemblyReferences)
                    yield return new AssemblyName(reference.FullName); ;
            }
        }
    }
}
