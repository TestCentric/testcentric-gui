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

using System;
using System.IO;
using NUnit.Framework;

namespace NUnit.ProjectEditor.Tests.Model
{
    [TestFixture]
    public class ProjectDocumentTests
    {
        static readonly string xmlfile = "test.nunit";

        private ProjectDocument doc;
        private bool gotChangeNotice;

        [SetUp]
        public void SetUp()
        {
            doc = new ProjectDocument();
            doc.CreateNewProject();
            doc.ProjectChanged += new ActionDelegate(OnProjectChange);

            gotChangeNotice = false;
        }

        [TearDown]
        public void EraseFile()
        {
            if (File.Exists(xmlfile))
                File.Delete(xmlfile);
        }

        private void OnProjectChange()
        {
            gotChangeNotice = true;
        }

        [Test]
        public void AddingElementMakesProjectDirty()
        {
            XmlHelper.AddElement(doc.RootNode, "Settings");
            Assert.True(doc.HasUnsavedChanges);
        }

        [Test]
        public void AddingElementFiresChangedEvent()
        {
            XmlHelper.AddElement(doc.RootNode, "Settings");
            Assert.True(gotChangeNotice);
        }

        [Test]
        public void AddingAttributeMakesProjectDirty()
        {
            XmlHelper.AddAttribute(doc.RootNode, "Version", "1.0");
            Assert.True(doc.HasUnsavedChanges);
        }

        [Test]
        public void AddingAttributeFiresChangedEvent()
        {
            XmlHelper.AddAttribute(doc.RootNode, "Version", "1.0");
            Assert.True(gotChangeNotice);
        }

    }
}
