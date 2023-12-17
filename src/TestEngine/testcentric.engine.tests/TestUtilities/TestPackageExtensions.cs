// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TestCentric.Engine.TestUtilities
{
    public static class TestPackageExtensions
    {
        public static string ToXml(this TestPackage package)
        {
            var writer = new StringWriter();
            var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings() { OmitXmlDeclaration = true });
            var serializer = new XmlSerializer(typeof(TestPackage));
            serializer.Serialize(xmlWriter, package);
            xmlWriter.Flush();
            xmlWriter.Close();

            return writer.ToString();
        }
    }
}
