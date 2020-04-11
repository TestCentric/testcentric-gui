// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using Mono.Cecil;
using NUnit.Engine;

namespace TestCentric.Engine.Internal
{
    public class TargetFrameworkHelper
    {
        private readonly AssemblyDefinition _assemblyDef;
        private readonly ModuleDefinition _module;

        public TargetFrameworkHelper(string assemblyPath)
        {
            try
            {
                _assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath);
                _module = _assemblyDef.MainModule;
            }
            catch (Exception e)
            {
                throw new NUnitEngineException($"{assemblyPath} could not be examined", e);
            }
        }

        public TargetFrameworkHelper(AssemblyDefinition assemblyDef)
        {
            _assemblyDef = assemblyDef;
            _module = _assemblyDef.MainModule;
        }

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
                foreach (var attr in _assemblyDef.CustomAttributes)
                {
                    if (attr.AttributeType.FullName != "System.Runtime.Versioning.TargetFrameworkAttribute")
                        continue;

                    var frameworkName = attr.ConstructorArguments[0].Value as string;
                    if (frameworkName != null)
                        return frameworkName;
                    break;
                }

                return null;
            }
        }


        public bool RequiresAssemblyResolver
        {
            get
            {
                foreach (var attr in _assemblyDef.CustomAttributes)
                {
                    if (attr.AttributeType.FullName == "NUnit.Framework.TestAssemblyDirectoryResolveAttribute")
                        return true;
                }

                return false;
            }
        }
    }
}
