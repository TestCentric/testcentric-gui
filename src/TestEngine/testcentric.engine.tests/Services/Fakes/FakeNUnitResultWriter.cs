// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestCentric.Engine.Services.Fakes
{
    [NUnit.Engine.Extensibility.Extension]
    [NUnit.Engine.Extensibility.ExtensionProperty("Format", "custom")]
    public class FakeNUnitResultWriter : NUnit.Engine.Extensibility.IResultWriter
    {
        public string Output { get; private set; }
        public string OutputPath { get; private set; }

        public void CheckWritability(string outputPath)
        {
            OutputPath = outputPath;
            Output = $"Able to write to {outputPath}";
        }

        public void WriteResultFile(XmlNode resultNode, string outputPath)
        {
            Output = resultNode.OuterXml;
            OutputPath = outputPath;
        }

        public void WriteResultFile(XmlNode resultNode, TextWriter writer)
        {
            Output = resultNode.OuterXml;
            OutputPath = null;
            writer.Write(Output);
        }
    }
}
