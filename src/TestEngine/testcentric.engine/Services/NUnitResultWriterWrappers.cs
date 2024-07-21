// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using System.Xml;
using TestCentric.Engine.Extensibility;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Wrapper class for listeners based on the NUnit API
    /// </summary>
    public class NUnitTestEventListenerWrapper : ITestEventListener
    {
        private NUnit.Engine.ITestEventListener _listener;

        public NUnitTestEventListenerWrapper(NUnit.Engine.ITestEventListener listener)
        {
            _listener = listener;
        }

        public void OnTestEvent(string report)
        {
            _listener.OnTestEvent(report);
        }
    }

    /// <summary>
    /// Wrapper class for result writers based on the NUnit API
    /// </summary>
    public class NUnitResultWriterWrapper : IResultWriter
    {
        private NUnit.Engine.Extensibility.IResultWriter _writer;

        public NUnitResultWriterWrapper(NUnit.Engine.Extensibility.IResultWriter writer)
        {
            _writer = writer;
        }

        public void CheckWritability(string outputPath)
        {
            _writer.CheckWritability(outputPath);
        }

        public void WriteResultFile(XmlNode resultNode, string outputPath)
        {
            _writer.WriteResultFile(resultNode, outputPath);
        }

        public void WriteResultFile(XmlNode resultNode, TextWriter writer)
        {
            _writer.WriteResultFile(resultNode, writer);
        }
    }

    /// <summary>
    /// Wrapper class for project loaders which use the NUnit Engine API
    /// </summary>
    public class NUnitProjectLoaderWrapper : IProjectLoader
    {
        private NUnit.Engine.Extensibility.IProjectLoader _projectLoader;

        public NUnitProjectLoaderWrapper(NUnit.Engine.Extensibility.IProjectLoader projectLoader)
        {
            _projectLoader = projectLoader;
        }

        public bool CanLoadFrom(string path)
        {
            return _projectLoader.CanLoadFrom(path);
        }

        public IProject LoadFrom(string path)
        {
            return new Projects.NUnitProject(_projectLoader.LoadFrom(path));
        }
    }
}
