// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Extensibility
{
    /// <summary>
    /// The ExtensionAttribute is used to identify a class that is intended
    /// to serve as an extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class ExtensionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCentric.Engine.Extensibility.ExtensionAttribute"/> class.
        /// </summary>
        public ExtensionAttribute()
        {
            Enabled = true;
        }

        /// <summary>
        /// A unique string identifying the ExtensionPoint for which this Extension is 
        /// intended. This is an optional field provided NUnit is able to deduce the
        /// ExtensionPoint from the Type of the extension class.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// An optional description of what the extension does.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Flag indicating whether the extension is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// The minimum Engine version for which this extension is designed
        /// </summary>
        public string EngineVersion { get; set; }
    }
}
