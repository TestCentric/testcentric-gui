// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;
using System.Collections.Generic;
using System.Reflection;

// TODO: This class was used in an alternate implementation of the extension service,
// which was based on reflection. It's currently unused and may be removed when and
// if we decide not to persue that alternative.

namespace TestCentric.Engine.Helpers
{
    /// <summary>
    /// Extension methods that make it easier to perform reflection operations.
    /// </summary>
    internal static class ReflectionExtensions
    {
        public static List<Attribute> GetAttributes(this Type type, string fullName, bool inherit)
        {
            var attributes = new List<Attribute>();

            foreach (Attribute attr in type.GetCustomAttributes(inherit))
            {
                if (attr.GetType().FullName == fullName)
                    attributes.Add(attr);
            }

            return attributes;
        }

        public static List<Attribute> GetAttributes(this Assembly assembly, string fullName, bool inherit)
        {
            var attributes = new List<Attribute>();

            foreach (Attribute attr in assembly.GetCustomAttributes(inherit))
            {
                if (attr.GetType().FullName == fullName)
                    attributes.Add(attr);
            }

            return attributes;
        }

        public static Attribute GetAttribute(this Type type, string fullName, bool inherit)
        {
            foreach (Attribute attr in type.GetCustomAttributes(inherit))
            {
                if (attr.GetType().FullName == fullName)
                    return attr;
            }

            return null;
        }

        public static object GetPropertyValue(this Attribute attr, string name)
        {
            return attr.GetType().GetProperty(name)?.GetValue(attr, null);
        }

        public static T GetPropertyValue<T>(this Attribute attr, string name)
        {
            return (T)attr.GetPropertyValue(name);
        }
    }
}
#endif
