// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace NUnit.TestUtilities.Fakes
{
    public class ExtensionService : IExtensionService
    {
        private List<IExtensionPoint> _extensionPoints;
        private List<IExtensionNode> _extensions;

        public ExtensionService()
        {
            _extensionPoints = new List<IExtensionPoint>();
            _extensions = new List<IExtensionNode>();

            // ExtensionPoints are all known, so we add in constructor. Extensions
            // may vary, so we use a method to add them.
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/NUnitV2Driver", "NUnit.Engine.Extensibility.IDriverFactory", "Driver for NUnit tests using the V2 framework."));
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/TypeExtensions/IService", "NUnit.Engine.Extensibility.IService", "Provides a service within the engine and possibly externally as well."));
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/TypeExtensions/ITestEventListener", "NUnit.Engine.Extensibility.ITestEventListener", "Allows an extension to process progress reports and other events from the test."));
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/TypeExtensions/IDriverFactory", "NUnit.Engine.Extensibility.IDriverFactory", "Supplies a driver to run tests that use a specific test framework."));
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/TypeExtensions/IProjectLoader", "NUnit.Engine.Extensibility.IProjectLoader", "Recognizes and loads assemblies from various types of project formats."));
            _extensionPoints.Add(new ExtensionPoint(
                "/NUnit/Engine/TypeExtensions/IResultWriter", "NUnit.Engine.Extensibility.IResultWriter", "Supplies a writer to write the result of a test to a file using a specific format."));
        }

        public void AddExtensions(params IExtensionNode[] extensions)
        {
            _extensions.AddRange(extensions);
        }

        public IEnumerable<IExtensionPoint> ExtensionPoints { get { return _extensionPoints; } }

        public IEnumerable<IExtensionNode> Extensions { get; } = new List<IExtensionNode>();

        public void EnableExtension(string typeName, bool enabled)
        {
        }

        public IEnumerable<IExtensionNode> GetExtensionNodes(string path)
        {
            return new IExtensionNode[0];
        }

        public IExtensionPoint GetExtensionPoint(string path)
        {
            return null;
        }

        // ExtensionPoint class is nested since the list of extension points is fixed
        public class ExtensionPoint : IExtensionPoint
        {
            public ExtensionPoint(string path, string typeName, string description)
            {
                Path = path;
                TypeName = typeName;
                Description = description;
            }

            public string Description { get; }

            public IEnumerable<IExtensionNode> Extensions { get; } = new List<IExtensionNode>();

            public string Path { get; }

            public string TypeName { get; }
        }
    }

    public class ExtensionNode : IExtensionNode
    {
        public ExtensionNode(string path, string typeName, string description)
        {
            Path = path;
            TypeName = typeName;
            Description = description;
        }

        public string Description { get; }

        public bool Enabled { get; }

        public string Path { get; }

        public IEnumerable<string> PropertyNames { get; }

        public string TypeName { get; }

        public IRuntimeFramework TargetFramework { get; }

        public IEnumerable<string> GetValues(string name)
        {
            return new string[0];
        }
    }
}
