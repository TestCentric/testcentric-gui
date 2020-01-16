// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace TestCentric.Engine.Helpers
{
    /// <summary>
    /// Extension methods that make it easier to work with Mono.Cecil.
    /// </summary>
    internal static class CecilExtensions
    {
        public static List<CustomAttribute> GetAttributes(this TypeDefinition type, string fullName)
        {
            var attributes = new List<CustomAttribute>();

            foreach (CustomAttribute attr in type.CustomAttributes)
            {
                if (attr.AttributeType.FullName == fullName)
                    attributes.Add(attr);
            }

            return attributes;
        }

        public static CustomAttribute GetAttribute(this TypeDefinition type, string fullName)
        {
            foreach (CustomAttribute attr in type.CustomAttributes)
            {
                if (attr.AttributeType.FullName == fullName)
                    return attr;
            }

            return null;
        }

        public static object GetNamedArgument(this CustomAttribute attr, string name)
        {
            foreach (var property in attr.Properties)
                if (property.Name == name)
                    return property.Argument.Value;

            return null;
        }
    }
}
