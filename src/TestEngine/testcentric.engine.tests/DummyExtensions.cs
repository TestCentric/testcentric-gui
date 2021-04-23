// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace TestCentric.Engine
{
    [Extension]
    public class DummyProjectLoaderExtension : IProjectLoader
    {
        public bool CanLoadFrom(string path)
        {
            throw new NotImplementedException();
        }

        public IProject LoadFrom(string path)
        {
            throw new NotImplementedException();
        }
    }

    [Extension]
    public class DummyResultWriterExtension : IResultWriter
    {
        public void CheckWritability(string outputPath)
        {
            throw new NotImplementedException();
        }

        public void WriteResultFile(XmlNode resultNode, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public void WriteResultFile(XmlNode resultNode, string outputPath)
        {
            throw new NotImplementedException();
        }
    }

    [Extension]
    public class DummyEventListenerExtension : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            throw new NotImplementedException();
        }
    }

    [Extension]
    public class DummyServiceExtension : IService
    {
        public IServiceLocator ServiceContext
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ServiceStatus Status
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void StartService()
        {
            throw new NotImplementedException();
        }

        public void StopService()
        {
            throw new NotImplementedException();
        }
    }

    [Extension(Enabled=false)]
    public class DummyDisabledExtension : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            throw new NotImplementedException();
        }
    }
}
