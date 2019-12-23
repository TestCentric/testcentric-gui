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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestCentric.Engine;
using NUnit.Framework;
using TestCentric.TestUtilities.Fakes;

namespace TestCentric.Gui.Model
{
    public class MakeTestPackageTests
    {
        private TestModel _model;

        [SetUp]
        public void CreateModel()
        {
            _model = new TestModel(new MockTestEngine());
        }

        [TestCase("my.test.assembly.dll")]
        [TestCase("one.dll", "two.dll", "three.dll")]
        [TestCase("tests.nunit")]
        public void PackageContainsOneSubPackagePerTestFile(params string[] testFiles)
        {
            TestPackage package = _model.MakeTestPackage(testFiles);

            Assert.That(package.SubPackages.Select(p => p.Name), Is.EqualTo(testFiles));
        }

        [TestCase(EnginePackageSettings.ProcessModel, "Single")]
        [TestCase(EnginePackageSettings.DomainUsage, "Multiple")]
        [TestCase(EnginePackageSettings.RuntimeFramework, "net-2.0")]
        [TestCase(EnginePackageSettings.MaxAgents, 8)]
        [TestCase(EnginePackageSettings.ShadowCopyFiles, false)]
        public void PackageReflectsPackageSettings(string key, object value)
        {
            _model.PackageOverrides[key] = value;
            TestPackage package = _model.MakeTestPackage(new[] { "my.dll" });

            Assert.That(package.Settings.ContainsKey(key));
            Assert.That(package.Settings[key], Is.EqualTo(value));
        }

        [Test]
        public void PackageReflectsTestParameters()
        {
            var testParms = new Dictionary<string, string>();
            testParms.Add("parm1", "value1");
            testParms.Add("parm2", "value2");
            _model.PackageOverrides.Add("TestParametersDictionary", testParms);

            TestPackage package = _model.MakeTestPackage(new[] { "my.dll" });

            Assert.That(package.Settings.ContainsKey("TestParametersDictionary"));
            var parms = package.Settings["TestParametersDictionary"] as IDictionary<string, string>;
            Assert.NotNull(parms);

            Assert.That(parms, Contains.Key("parm1"));
            Assert.That(parms, Contains.Key("parm2"));
            Assert.That(parms["parm1"], Is.EqualTo("value1"));
            Assert.That(parms["parm2"], Is.EqualTo("value2"));
        }

        [Test]
        public void DisplayShadowCopySettings()
        {
            Console.WriteLine($"Current AppDomain has ShadowCopyFiles = {AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles}");
        }

        [Test]
        public void DisplayTestParameters()
        {
            Console.WriteLine("Test Parameters for this run:");
            foreach (string name in TestContext.Parameters.Names)
                Console.WriteLine($"{name}={TestContext.Parameters[name]}");
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
    }
}
