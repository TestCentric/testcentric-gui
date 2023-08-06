// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using System.Xml;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Internal
{
    public class TestPackageExtensionTests
    {
        TestPackage _package;

        [OneTimeSetUp]
        public void CreatePackage()
        {
            _package = new TestPackage("test1.dll");
            _package.AddSetting("StringSetting", "SomeString");
            _package.AddSetting("IntSetting", 999);
            _package.AddSetting("BoolSetting", true);
        }

        [Test]
        public void ConvertPackageToXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_package.ToXml());

            var node = doc.DocumentElement;
            Assert.That(node.Name, Is.EqualTo("TestPackage"));
            Assert.That(node.HasAttribute("id"));
            Assert.That(node.GetAttribute("fullname"), Is.EqualTo(Path.GetFullPath("test1.dll")));

            var settings = node.FirstChild;
            Assert.That(settings.Name, Is.EqualTo("Settings"));
            Assert.That(settings.Attributes.Count, Is.EqualTo(3));
            Assert.That(settings.GetAttribute("StringSetting"), Is.EqualTo("SomeString"));
            Assert.That(int.Parse(settings.GetAttribute("IntSetting")), Is.EqualTo(999));
            Assert.That(bool.Parse(settings.GetAttribute("BoolSetting")), Is.True);
        }
    }
}
