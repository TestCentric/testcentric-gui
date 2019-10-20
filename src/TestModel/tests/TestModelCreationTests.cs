// ***********************************************************************
// Copyright (c) 2019 Charlie Poole
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
using System.Text;
using System.Threading.Tasks;
using NUnit.Engine;
using NUnit.Framework;
using NUnit.TestUtilities.Fakes;

namespace TestCentric.Gui.Model
{
    public class TestModelCreationTests
    {
        [TestCase("dummy.dll")]
        [TestCase("dummy.dll", "--work=/Path/To/Directory")]
        [TestCase("dummy.dll", "--trace=Off")]
        [TestCase("dummy.dll", "--trace=Error", "--work=/Path/To/Directory")]
        [TestCase("dummy.dll", "--trace=Warning")]
        [TestCase("dummy.dll", "--trace=Info", "--work=/Path/To/Directory")]
        [TestCase("dummy.dll", "--trace=Debug")]
        [TestCase("dummy.dll", "--process:Multiple", "--work=/Some/Directory", "==agents:32")]
        [TestCase("dummy.dll", "--process:Separate", "--domain:Multiple", "--X86")]
        [TestCase("dummy.dll", "--process:Single")]
        [TestCase("dummy.dll", "--process:InProcess")]
        [TestCase("dummy.dll", "--inprocess")]
        [TestCase("dummy.dll", "--domain:Multiple")]
        [TestCase("dummy.dll", "--domain:Single")]
        [TestCase("dummy.dll", "--domain:None")]
        [TestCase("dummy.dll", "--agents:5")]
        [TestCase("dummy.dll", "--X86")]
        public void CreateTestModel(params string[] args)
        {
            ITestEngine engine = new MockTestEngine();
            var options = new CommandLineOptions(args);
            var model = TestModel.CreateTestModel(engine, options);

            Assert.NotNull(model, "Unable to create TestModel");

            string expectedTraceLevel = options.InternalTraceLevel ?? "Off";
            string actualTraceLevel = model.PackageOverrides[EnginePackageSettings.InternalTraceLevel] as string;
            Assert.That(actualTraceLevel, Is.EqualTo(expectedTraceLevel));

            Assert.That(engine.WorkDirectory, Is.EqualTo(options.WorkDirectory));

            var checker = new PackageOverridesChecker(model);
            checker.CheckSetting(options.ProcessModel, EnginePackageSettings.ProcessModel);
            checker.CheckSetting(options.DomainUsage, EnginePackageSettings.DomainUsage);
            checker.CheckSetting(options.MaxAgents, EnginePackageSettings.MaxAgents);
            checker.CheckSetting(options.RunAsX86, EnginePackageSettings.RunAsX86);
        }

        private class PackageOverridesChecker
        {
            private IDictionary<string, object> _dictionary;

            public PackageOverridesChecker(ITestModel model)
            {
                _dictionary = model.PackageOverrides;
            }

            public void CheckSetting(bool option, string key)
            {
                if (option || _dictionary.ContainsKey(key))
                {
                    Assert.That(_dictionary, Contains.Key(key));
                    Assert.That(_dictionary[key], Is.EqualTo(option));
                }
            }

            public void CheckSetting<T>(T option, string key)
            {
                if (option != null)
                {
                    Assert.That(_dictionary, Contains.Key(key));
                    Assert.That(_dictionary[key], Is.EqualTo(option));
                }
                else
                    Assert.That(_dictionary, Does.Not.ContainKey(key));
            }
        }
    }
}
