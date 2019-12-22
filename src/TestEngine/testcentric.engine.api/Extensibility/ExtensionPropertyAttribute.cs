// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace NUnit.Engine.Extensibility
{
    /// <summary>
    /// The ExtensionPropertyAttribute is used to specify named properties for an extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public class ExtensionPropertyAttribute : Attribute
    {
        /// <summary>
        /// Construct an ExtensionPropertyAttribute
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The property value</param>
        public ExtensionPropertyAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The property value
        /// </summary>
        public string Value { get; private set; }
    }
}
