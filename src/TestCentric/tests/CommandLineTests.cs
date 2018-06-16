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
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    [TestFixture]
    public class CommandLineTests
    {
        [Test]
        public void NoParametersCount()
        {
            var options = new CommandLineOptions(new string[] {});
            Assert.IsTrue(options.InputFiles.Count == 0);
        }

        [Test]
        public void Help()
        {
            var options = new CommandLineOptions(new string[] {"-help"});
            Assert.IsTrue(options.ShowHelp);
        }

        [Test]
        public void ShortHelp()
        {
            var options = new CommandLineOptions(new string[] {"-h"});
            Assert.IsTrue(options.ShowHelp);
        }

        [Test]
        public void AssemblyName()
        {
            string assemblyName = "nunit.tests.dll";
            var options = new CommandLineOptions(new string[]{ assemblyName });
            Assert.AreEqual(assemblyName, options.InputFiles[0]);
        }

        [Test]
        public void ValidateSuccessful()
        {
            var options = new CommandLineOptions(new string[] { "nunit.tests.dll" });
            Assert.IsTrue(options.Validate(), "command line should be valid");
        }

        [Test]
        public void InvalidArgs()
        {
            var options = new CommandLineOptions(new string[] { "-asembly:nunit.tests.dll" });
            Assert.IsFalse(options.Validate());
        }


        [Test] 
        public void InvalidCommandLineParms()
        {
            var parser = new CommandLineOptions(new String[]{"-garbage:TestFixture", "-assembly:Tests.dll"});
            Assert.IsFalse(parser.Validate());
        }

        //[Test] 
        //public void NoNameValuePairs()
        //{
        //	var parser = new CommandLineOptions(new String[]{"TestFixture", "Tests.dll"});
        //	Assert.IsFalse(parser.Validate());
        //}
    }
}

