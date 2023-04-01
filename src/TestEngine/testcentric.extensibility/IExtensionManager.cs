// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Engine.Extensibility;

namespace TestCentric.Extensibility
{
    public interface IExtensionManager
    {
        void Initialize(string startDirectory);

        IEnumerable<IExtensionPoint> ExtensionPoints { get; }

        IEnumerable<IExtensionNode> Extensions { get; }

        IExtensionPoint GetExtensionPoint(string path);

        IEnumerable<T> GetExtensions<T>();

        IEnumerable<IExtensionNode> GetExtensionNodes(string path);

        IEnumerable<ExtensionNode> GetExtensionNodes<T>(bool includeDisabled = false);

        void EnableExtension(string typeName, bool enabled);
    }
}
