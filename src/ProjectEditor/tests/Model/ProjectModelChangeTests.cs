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

using System.IO;

namespace NUnit.ProjectEditor.Tests.Model
{
    class ProjectModelChangeTests
    {
        static readonly string xmlfile = "MyProject.nunit";

        private ProjectDocument doc;
        private ProjectModel project;
        private bool gotChangeNotice;

        [SetUp]
        public void SetUp()
        {
            doc = new ProjectDocument(xmlfile);
            project = new ProjectModel(doc);

            doc.ProjectChanged += OnProjectChange;
            gotChangeNotice = false;
        }

        private void OnProjectChange()
        {
            gotChangeNotice = true;
        }

        [Test]
        public void RenameConfigMakesProjectDirty()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.Configs[0].Name = "New";
            Assert.IsTrue(doc.HasUnsavedChanges);
        }

        [Test]
        public void RenameConfigFiresChangedEvent()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.Configs[0].Name = "New";
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void RenamingActiveConfigChangesActiveConfigName()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.ActiveConfigName = "Debug";

            project.Configs[0].Name = "New";

            Assert.AreEqual("New", project.ActiveConfigName);
        }

        [Test]
        public void RemoveConfigMakesProjectDirty()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.RemoveConfig("Debug");
            Assert.IsTrue(doc.HasUnsavedChanges);
        }

        [Test]
        public void RemoveConfigFiresChangedEvent()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.RemoveConfig("Debug");
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void RemovingActiveConfigRemovesActiveConfigNameAttribute()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.ActiveConfigName = "Debug";
            project.RemoveConfig("Debug");
            Assert.AreEqual(null, project.ActiveConfigName);
        }

        [Test]
        public void SettingActiveConfigMakesProjectDirty()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.ActiveConfigName = "Release";
            Assert.IsTrue(doc.HasUnsavedChanges);
        }

        [Test]
        public void SettingActiveConfigFiresChangedEvent()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.ActiveConfigName = "Release";
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void CanSetActiveConfig()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.ActiveConfigName = "Release";
            Assert.AreEqual("Release", project.ActiveConfigName);
        }

        [Test]
        public void CanAddAssemblies()
        {
            doc.LoadXml(NUnitProjectXml.EmptyConfigs);
            project.Configs["Debug"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly1.dll"));
            project.Configs["Debug"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly2.dll"));
            project.Configs["Release"].Assemblies.Add(Path.GetFullPath(@"bin\debug\assembly3.dll"));

            Assert.AreEqual(2, project.Configs.Count);
            Assert.AreEqual(2, project.Configs["Debug"].Assemblies.Count);
            Assert.AreEqual(1, project.Configs["Release"].Assemblies.Count);
        }

        [Test]
        public void AddingAssemblyFiresChangedEvent()
        {
            doc.LoadXml(NUnitProjectXml.EmptyConfigs);
            project.Configs["Debug"].Assemblies.Add("assembly1.dll");
            Assert.IsTrue(gotChangeNotice);
        }

        [Test]
        public void RemoveAssemblyFiresChangedEvent()
        {
            doc.LoadXml(NUnitProjectXml.NormalProject);
            project.Configs["Debug"].Assemblies.Remove("assembly1.dll");
            Assert.IsTrue(gotChangeNotice);
        }
    }
}
