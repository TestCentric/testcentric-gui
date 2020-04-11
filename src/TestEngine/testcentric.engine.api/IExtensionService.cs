// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine.Extensibility;

namespace TestCentric.Engine
{
    /// <summary>
    /// The IExtensionService interface allows a runner to manage extensions.
    /// </summary>
    public interface IExtensionService
    {
        /// <summary>
        /// Gets an enumeration of all ExtensionPoints in the engine.
        /// </summary>
        IEnumerable<IExtensionPoint> ExtensionPoints { get; }

        /// <summary>
        /// Gets an enumeration of all installed Extensions.
        /// </summary>
        IEnumerable<IExtensionNode> Extensions { get; }

        /// <summary>
        /// Get an ExtensionPoint based on its unique identifying path.
        /// </summary>
        IExtensionPoint GetExtensionPoint(string path);

        /// <summary>
        /// Get an enumeration of ExtensionNodes based on their identifying path.
        /// </summary>
        IEnumerable<IExtensionNode> GetExtensionNodes(string path);

        /// <summary>
        /// Enable or disable an extension
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="enabled"></param>
        void EnableExtension(string typeName, bool enabled);
    }
}

