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
using System.Linq;
using System.Reflection;
using NUnit.Engine;
using NUnit.Framework;
using NUnit.TestUtilities.Fakes;

namespace TestCentric.Gui.Model
{
    public class TestModelPackageTests
    {
        private TestModel _model;

        [SetUp]
        public void CreateModel()
        {
            _model = new TestModel(new MockTestEngine());
        }

        [TestCase("my.test.assembly.dll")]
        [TestCase("one.dll", "two.dll", "three.dll")]
        public void PackageContainsOneSubPackagePerAssembly(params string[] assemblies)
        {
            TestPackage package = _model.MakeTestPackage(assemblies);

            Assert.That(package.SubPackages.Select(p => p.Name), Is.EqualTo(assemblies));
        }

        [TestCase(EnginePackageSettings.ProcessModel, "Single")]
        [TestCase(EnginePackageSettings.DomainUsage, "Multiple")]
        [TestCase(EnginePackageSettings.RuntimeFramework, "net-2.0")]
        [TestCase(EnginePackageSettings.MaxAgents, 8)]
        [TestCase(EnginePackageSettings.ShadowCopyFiles, false)]
        public void PackageReflectsPackageSettings(string key, object value)
        {
            _model.PackageSettings[key] = value;
            TestPackage package = _model.MakeTestPackage(new[] { "my.dll" });

            Assert.That(package.Settings.ContainsKey(key));
            Assert.That(package.Settings[key], Is.EqualTo(value));
        }

        [Test]
        public void DisplayShadowCopySettings()
        {
            Console.WriteLine($"Current AppDomain has ShadowCopyFiles = {AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles}");
        }

        [TestCase("my.dll")]
        [TestCase("my.sln")]
        [TestCase("my.dll,my.sln")]
        [TestCase("my.sln,my.dll")]
        [TestCase("my.sln,another.sln")]
        public void PackageForSolutionFileHasSkipNonTestAssemblies(string files)
        {
            TestPackage package = _model.MakeTestPackage(files.Split(','));
            string skipKey = EnginePackageSettings.SkipNonTestAssemblies;

            foreach (var subpackage in package.SubPackages)
            {
                if (subpackage.Name.EndsWith(".sln"))
                {
                    Assert.That(subpackage.Settings, Does.ContainKey(skipKey));
                    Assert.That(subpackage.Settings[skipKey], Is.True);
                }
                else
                    Assert.That(subpackage.Settings, Does.Not.ContainKey(skipKey));
            }
        }

        [Test]
        public void RunAsX86_GuiSettingFalse_DefaultsPackageSettingToFalse()
        {
            MockTestEngine testEngine = new MockTestEngine().WithSetting("Gui.Options." + EnginePackageSettings.RunAsX86, false);

            TestModel model = new TestModel(testEngine);

            Assert.That(model.PackageSettings[EnginePackageSettings.RunAsX86], Is.EqualTo(false));
        }

        [Test]
        public void RunAsX86_GuiSettingTrue_DefaultsPackageSettingToTrue()
        {
            MockTestEngine testEngine = new MockTestEngine().WithSetting("Gui.Options." + EnginePackageSettings.RunAsX86, true);

            TestModel model = new TestModel(testEngine);

            Assert.That(model.PackageSettings[EnginePackageSettings.RunAsX86], Is.EqualTo(true));
        }
    }
}
