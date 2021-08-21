// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestCentric.TestUtilities.Fakes;

using TestPackage = NUnit.Engine.TestPackage;

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

        [TestCase(EnginePackageSettings.RequestedRuntimeFramework, "net-2.0")]
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
