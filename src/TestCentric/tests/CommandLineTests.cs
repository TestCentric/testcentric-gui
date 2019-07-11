// ***********************************************************************
// Copyright (c) 2018-2019 Charlie Poole
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
using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    [TestFixture]
    public class CommandLineTests
    {
        [TestCase]
        [TestCase("tests.dll")]
        [TestCase("one.dll", "two.dll", "three.dll")]
        public void InputFiles(params string[] files)
        {
            var options = new CommandLineOptions(files);

            Assert.That(options.InputFiles.Count, Is.EqualTo(files.Length));
            for (int i = 0; i < files.Length; i++)
                Assert.That(options.InputFiles[i], Is.EqualTo(files[i]));

            Assert.True(options.Validate());
        }

        [TestCase("NoLoad", false)]
        [TestCase("RunAllTests", false)]
        [TestCase("RunAsX86", false)]
        [TestCase("ShowHelp", false)]
        [TestCase("ProcessModel", null)]
        [TestCase("DomainUsage", null)]
        [TestCase("InternalTraceLevel", NUnit.Engine.InternalTraceLevel.Default)]
        public void DefaultOptionValues(string propertyName, object val)
        {
            var property = GetPropertyInfo(propertyName);
            var options = new CommandLineOptions();

            Assert.That(property.GetValue(options, null), Is.EqualTo(val));
        }

        [TestCase("NoLoad", "--noload", true)]
        [TestCase("RunAllTests", "--run", true)]
        [TestCase("ProcessModel", "--process:Single")]
        [TestCase("ProcessModel", "--process:Separate")]
        [TestCase("ProcessModel", "--process:Multiple")]
        [TestCase("ProcessModel", "--inprocess", "Single")]
        [TestCase("DomainUsage", "--domain:Single")]
        [TestCase("DomainUsage", "--domain:Separate")]
        [TestCase("DomainUsage", "--domain:Multiple")]
        [TestCase("RunAsX86", "--x86", true)]
        [TestCase("MaxAgents", "--agents:8", 8)]
        [TestCase("InternalTraceLevel", "--trace:Off", NUnit.Engine.InternalTraceLevel.Off)]
        [TestCase("InternalTraceLevel", "--trace:Error", NUnit.Engine.InternalTraceLevel.Error)]
        [TestCase("InternalTraceLevel", "--trace:Warning", NUnit.Engine.InternalTraceLevel.Warning)]
        [TestCase("InternalTraceLevel", "--trace:Info", NUnit.Engine.InternalTraceLevel.Info)]
        [TestCase("InternalTraceLevel", "--trace:Verbose", NUnit.Engine.InternalTraceLevel.Verbose)]
        [TestCase("InternalTraceLevel", "--trace:Debug", NUnit.Engine.InternalTraceLevel.Debug)]
        [TestCase("ShowHelp", "--help", true)]
        [TestCase("ShowHelp", "-h", true)]
        public void ValidOptionsAreRecognized(string propertyName, string option, object expected = null)
        {
            var property = GetPropertyInfo(propertyName);
            var options = new CommandLineOptions(option);

            if (expected == null)
            {
                var index = option.IndexOf(':');
                expected = option.Substring(index + 1);
            }

            Assert.That(options.Validate(), Is.True, $"Should be valid: {option}");
            Assert.That(property.GetValue(options, null), Is.EqualTo(expected));
        }

        [TestCase("--process")]
        [TestCase("--agents")]
        [TestCase("--domain")]
        [TestCase("--trace")]
        public void InvalidOptionsAreDetectedByMonoOptions(string option)
        {
            // We would prefer to handle all errors ourselves so
            // this will eventually change. The test documents
            // the current behavior.
            Assert.Throws<Mono.Options.OptionException>(
                () => new CommandLineOptions(option));
        }

        [TestCase("--assembly:nunit.tests.dll")]
        [TestCase("--garbage")]
        [TestCase("--process:Unknown")]
        [TestCase("--agents:XYZ")]
        [TestCase("--domain:Junk")]
        [TestCase("--trace:Something")]
        public void InvalidOptionsAreDetected(string option)
        {
            var options = new CommandLineOptions(option);
            Assert.IsFalse(options.Validate());
            Assert.That(options.ErrorMessages.Count, Is.EqualTo(1));
        }

        private static PropertyInfo GetPropertyInfo(string propertyName)
        {
            PropertyInfo property = typeof(CommandLineOptions).GetProperty(propertyName);
            Assert.IsNotNull(property, "The property '{0}' is not defined", propertyName);
            return property;
        }
    }
}

