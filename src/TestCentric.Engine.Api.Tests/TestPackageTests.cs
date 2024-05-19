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
using NUnit.Framework;

namespace TestCentric.Engine.Api
{
    // NOTE: These are tests of the NUnit Engine rather than TestCentric.
    // If they fail, something has changed in the NUnit Engine.

    [TestFixtureSource(nameof(FixtureData))]
    public class TestPackageTests
    {
        private TestPackage _package;
        private string _expectedXml;

        private const string HEADER = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

        public TestPackageTests(TestPackage package, string expectedXml)
        {
            _package = package;
            _expectedXml = expectedXml;
        }

        [Test]
        public void TestPackageIsXmlSerializable()
        {
            Assert.That(_package, Is.XmlSerializable);
        }

        [Test]
        public void PackageIDsAreUnique()
        {
            string[] fileNames = _package.SubPackages.Select(p => p.FullName).ToArray();
            var anotherPackage = new TestPackage(fileNames);
            Assert.That(anotherPackage.ID, Is.Not.EqualTo(_package.ID));
        }

        [Test]
        public void TopLevelPackageIsAnonymous()
        {
            Assert.Null(_package.Name);
            Assert.Null(_package.FullName);
        }

        [Test]
        public void TopLevelPackageHasSubPackages()
        {
            Assert.That(_package.HasSubPackages);
        }

        [Test]
        public void TopLevelPackageIsNotAnAssembly()
        {
            Assert.False(_package.IsAssemblyPackage);
        }

        [Test]
        public void CanSelectPackages()
        {
            var selection = _package.Select(p => p.IsAssemblyPackage);

            Assert.That(selection.Count, Is.GreaterThan(0));
            foreach (var package in selection)
                Assert.That(package.IsAssemblyPackage);
        }

        [Test]
        public void CanSerializePackage()
        {
            StringWriter writer = new StringWriter();
            // Creating our own XmlWriter makes the test simpler. Using
            // the StringWriter directly would introduce the XML declaration
            // and format the output into multiple indented lines.
            XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings() { OmitXmlDeclaration = true });
            XmlSerializer serializer = new XmlSerializer(typeof(TestPackage));
            serializer.Serialize(xmlWriter, _package);

            Assert.That(writer.ToString(), Is.EqualTo(_expectedXml));
        }

        [Test]
        public void CanDeserializePackage()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestPackage));
            StringReader reader = new StringReader(_expectedXml);
            TestPackage newPackage = (TestPackage)serializer.Deserialize(reader);

            CheckPackage(newPackage, _package);
        }

        [Test]
        public void RoundTrip()
        {
            // This test performs a round trip serializing and then
            // deserializing the original package using default settings.
            XmlSerializer serializer = new XmlSerializer(typeof(TestPackage));

            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, _package);

            StringReader reader = new StringReader(writer.ToString());
            TestPackage newPackage = (TestPackage)serializer.Deserialize(reader);

            CheckPackage(newPackage, _package);
        }

        private void CheckPackage(TestPackage actual, TestPackage expected)
        {
            Assert.That(actual.ID, Is.EqualTo(expected.ID));
            Assert.That(actual.FullName, Is.EqualTo(expected.FullName));
            Assert.That(actual.Settings.Keys, Is.EqualTo(expected.Settings.Keys));
            foreach (var key in expected.Settings.Keys)
                Assert.That(actual.Settings[key], Is.EqualTo(expected.Settings[key].ToString()));
            Assert.That(actual.SubPackages.Count, Is.EqualTo(expected.SubPackages.Count));

            for (int i = 0; i < expected.SubPackages.Count; i++)
                CheckPackage(actual.SubPackages[i], expected.SubPackages[i]);
        }

        static IEnumerable<TestFixtureData> FixtureData()
        {
            TestPackage package;

            package = new TestPackage("test.dll");
            yield return new TestFixtureData(package, GetExpectedXml(package)) { TestName = "TestPackageTests(Single Assembly, no Settings)" };

            package = new TestPackage("test.dll");
            package.AddSetting("StringSetting", "xyz");
            package.AddSetting("IntSetting", 123);
            yield return new TestFixtureData(package, GetExpectedXml(package)) { TestName = "TestPackageTests(Single Assembly with Settings)" };

            package = new TestPackage("test1.dll", "test2.dll", "test3.dll");
            yield return new TestFixtureData(package, GetExpectedXml(package)) { TestName = "TestPackageTests(Three Assemblies, no Settings)" };

            package = new TestPackage("test1.dll", "test2.dll", "test3.dll");
            package.AddSetting("StringSetting", "xyz");
            package.AddSetting("IntSetting", 123);
            package.SubPackages[0].AddSetting("Comment", "This is test1");
            package.SubPackages[1].AddSetting("Comment", "This is test2");
            package.SubPackages[2].AddSetting("Comment", "This is test3");
            yield return new TestFixtureData(package, GetExpectedXml(package)) { TestName = "TestPackageTests(Three Assemblies with Settings)" };

            package = new TestPackage("test1.dll", "project.nunit");
            package.SubPackages[1].AddSubPackage("test2.dll");
            package.SubPackages[1].AddSubPackage("test3.dll");
            yield return new TestFixtureData(package, GetExpectedXml(package)) { TestName = "TestPackageTests(One Assembly and One Project))" };
        }

        private static string GetExpectedXml(TestPackage package)
        {
            int nextId = int.Parse(package.ID);

            var sb = new StringBuilder();
            sb.Append($"<TestPackage id=\"{package.ID}\"");
            if (package.FullName != null)
                sb.Append($" fullname=\"{package.FullName}\"");

            if (package.Settings.Count == 0 && package.SubPackages.Count == 0)
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">");

                if (package.Settings.Count > 0)
                {
                    // Terminate TestPackage and start Settings
                    sb.Append("<Settings");
                    foreach (string key in package.Settings.Keys)
                        sb.Append($" {key}=\"{package.Settings[key]}\"");
                    sb.Append(" />");
                }

                foreach (TestPackage subPackage in package.SubPackages)
                    sb.Append(GetExpectedXml(subPackage));

                sb.Append("</TestPackage>");
            }

            return sb.ToString();
        }
    }
}
