// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace TestCentric.Engine.Api
{
    // NOTE: These are tests of the NUnit Engine rather than TestCentric.
    // If they fail, something has changed in the NUnit Engine.

    [TestFixture("test.dll")]
    [TestFixture("test1.dll,test2.dll,test3.dll")]
    public class TestPackageTests
    {
        private string[] _fileNames;
        private TestPackage _package;
        private string _packageXml;

        private const string HEADER = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
        private const string SETTINGS = "<Settings StringSetting=\"xyz\" IntSetting=\"123\" />";

        public TestPackageTests(string fileNames)
        {
            _fileNames = fileNames.Split(new char[] { ',' });
            _package = new TestPackage(_fileNames);
            _package.AddSetting("StringSetting", "xyz");
            _package.AddSetting("IntSetting", 123);
            int nextId = int.Parse(_package.ID);

            var sb = new StringBuilder();
            sb.Append($"<TestPackage id=\"{nextId++}\">" + SETTINGS);
            foreach (string fileName in _fileNames)
            {
                string fullName = Path.GetFullPath(fileName);
                sb.Append($"<TestPackage id=\"{nextId++}\" fullname=\"{fullName}\">" + SETTINGS);
                sb.Append("</TestPackage>");
            }
            sb.Append("</TestPackage>");

            _packageXml = sb.ToString();
        }

        [Test]
        public void PackageIDsAreUnique()
        {
            var another = new TestPackage(_fileNames);
            Assert.That(another.ID, Is.Not.EqualTo(_package.ID));
        }

        [Test]
        public void PackageIsAnonymous()
        {
            Assert.Null(_package.Name);
            Assert.Null(_package.FullName);
            Assert.False(_package.IsAssemblyPackage);
        }

        [Test]
        public void PackageHasSettings()
        {
            Assert.That(_package.Settings["StringSetting"], Is.EqualTo("xyz"));
            Assert.That(_package.Settings["IntSetting"], Is.EqualTo(123));
        }

        [Test]
        public void PackageHasSubPackages()
        {
            Assert.That(_package.HasSubPackages);
        }

        [Test]
        public void HasSubPackageForEachFile()
        {
            Assert.That(_package.SubPackages.Count, Is.EqualTo(_fileNames.Length));

            for (int i = 0; i < _fileNames.Length; i++)
            {
                TestPackage subPackage = _package.SubPackages[i];
                string fileName = _fileNames[i];

                Assert.That(subPackage.Name, Is.EqualTo(fileName));
                Assert.That(subPackage.FullName, Is.EqualTo(Path.GetFullPath(fileName)));
                Assert.That(subPackage.IsAssemblyPackage);
            }
        }

        [Test]
        public void SubPackagesHaveNoSubPackages()
        {
            foreach (TestPackage subPackage in _package.SubPackages)
            {
                Assert.That(subPackage.SubPackages.Count, Is.EqualTo(0));
                Assert.False(subPackage.HasSubPackages);
            }
        }

        [Test]
        public void CanSelectPackages()
        {
            var selection = _package.Select(p => p.Name != null && p.Name.StartsWith("test"));

            Assert.That(selection, Is.EquivalentTo(_package.SubPackages));
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

            Assert.That(writer.ToString(), Is.EqualTo(_packageXml));
        }

        [Test]
        public void CanDeserializePackage()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestPackage));
            StringReader reader = new StringReader(_packageXml);
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
    }
}
