// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Xml;
using NUnit.Engine;

namespace TestCentric.Engine.Internal
{
    internal class TestPackageSerializer
    {
        public TestPackageSerializer() { }

        /// <summary>
        /// Convenience method to serialize a TestPackage to a StringWriter
        /// without an XML declaration.
        /// </summary>
        /// <param name="writer">An StringWriter to use</param>
        /// <param name="package">The package to be serialized</param>
        public void Serialize(StringWriter writer, TestPackage package)
        {
            // Assume we don't want the Xml declaration in our string
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            var xmlWriter = XmlWriter.Create(writer, settings);
            Serialize(xmlWriter, package);
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        /// <summary>
        /// Serialize a TestPackage to an XmlWriter
        /// </summary>
        /// <param name="xmlWriter">An XmlWriter to use</param>
        /// <param name="package">The package to be serialized</param>
        public void Serialize(XmlWriter xmlWriter, TestPackage package)
        {
            WriteTestPackage(xmlWriter, package);
        }

        private void WriteTestPackage(XmlWriter xmlWriter, TestPackage package)
        {
            xmlWriter.WriteStartElement("TestPackage");

            WritePackageAttributes();
            WriteSettings();
            WriteSubPackages();

            xmlWriter.WriteEndElement();

            void WritePackageAttributes()
            {
                xmlWriter.WriteAttributeString("id", package.ID);

                if (package.FullName != null)
                    xmlWriter.WriteAttributeString("fullname", package.FullName);
            }

            void WriteSettings()
            {
                if (package.Settings.Count != 0)
                {
                    xmlWriter.WriteStartElement("Settings");

                    foreach (var pair in package.Settings)
                        xmlWriter.WriteAttributeString(pair.Key, pair.Value.ToString());

                    xmlWriter.WriteEndElement();
                }
            }

            void WriteSubPackages()
            {
                if (package.SubPackages != null)
                    foreach (var subPackage in package.SubPackages)
                        WriteTestPackage(xmlWriter, subPackage);
            }
        }

        public TestPackage Deserialize(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var topNode = doc.DocumentElement;
            if (topNode.Name != "TestPackage")
                throw new ArgumentException("Xml provided is not a TestPackage");

            return CreateTestPackage(topNode);
        }

        private TestPackage CreateTestPackage(XmlNode topNode)
        {
            // For now, assume package is not anonymous
            var id = topNode.GetAttribute("id");
            var fullName = topNode.GetAttribute("fullname");

            var package = fullName != null
                ? new TestPackage(fullName)
                : new TestPackage(new string[0]);

            foreach (XmlNode child in topNode.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Settings":
                        foreach (XmlNode node in child.Attributes)
                            package.AddSetting(node.Name, node.Value);
                        break;
                    case "TestPackage":
                        package.AddSubPackage(CreateTestPackage(child));
                        break;
                }
            }

            return package;
        }
    }
}
