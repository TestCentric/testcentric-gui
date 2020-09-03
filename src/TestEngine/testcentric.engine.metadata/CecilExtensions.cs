// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
    /// <summary>
    /// Extension methods that make it easier to work with Mono.Cecil.
    /// </summary>
    /// <remarks>
    /// These extensions might have been implemented directly in each of
    /// the classes they extend. However, they are extensions precisely
    /// in order to avoid modifying those classes unnecessarily.
    /// </remarks>
    public static class CecilExtensions
    {
        #region ICustomAttributeProvider Extensions

        public static bool HasAttribute(this ICustomAttributeProvider provider, string fullName)
        {
            foreach (var attr in provider.CustomAttributes)
            {
                if (attr.AttributeType.FullName == fullName)
                    return true;
            }

            return false;
        }

        public static CustomAttribute GetAttribute(this ICustomAttributeProvider provider, string fullName)
        {
            foreach (CustomAttribute attr in provider.CustomAttributes)
            {
                if (attr.AttributeType.FullName == fullName)
                    return attr;
            }

            return null;
        }

        public static IEnumerable<CustomAttribute> GetAttributes(this ICustomAttributeProvider provider, string fullName)
        {
            foreach (CustomAttribute attr in provider.CustomAttributes)
            {
                if (attr.AttributeType.FullName == fullName)
                    yield return attr;
            }
        }

        #endregion

        #region AssemblyDefinition Extensions

        public static bool RequiresX86(this AssemblyDefinition assemblyDef)
        {
            const ModuleAttributes nativeEntryPoint = (ModuleAttributes)16;
            const ModuleAttributes mask = ModuleAttributes.Required32Bit | nativeEntryPoint;

            var module = assemblyDef.MainModule;
            return module.Architecture != TargetArchitecture.AMD64 &&
                    module.Architecture != TargetArchitecture.IA64 &&
                    (module.Attributes & mask) != 0;
        }

        public static Version GetRuntimeVersion(this AssemblyDefinition assemblyDef)
        {
            var runtimeVersion = assemblyDef.MainModule.RuntimeVersion;

            if (runtimeVersion.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                runtimeVersion = runtimeVersion.Remove(0, 1);

            return new Version(runtimeVersion);
        }

        public static string GetFrameworkName(this AssemblyDefinition assemblyDef)
        {
            var attr = assemblyDef.GetAttribute("System.Runtime.Versioning.TargetFrameworkAttribute");
            return attr?.ConstructorArguments[0].Value as string;
        }

        #endregion

        #region CustomAttribute Extensions

        public static object GetNamedArgument(this CustomAttribute attr, string name)
        {
            foreach (var property in attr.Properties)
                if (property.Name == name)
                    return property.Argument.Value;

            return null;
        }

        #endregion
    }
}
