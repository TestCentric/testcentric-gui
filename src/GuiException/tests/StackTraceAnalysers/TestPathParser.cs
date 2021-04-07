// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NUnit.UiException.StackTraceAnalysers;
using NUnit.UiException.StackTraceAnalyzers;

namespace NUnit.UiException.Tests.StackTraceAnalyzers
{
    [TestFixture]
    public class TestPathParser :
        TestIErrorParser
    {
        private PathCompositeParser _parser;

        [SetUp]
        public new void SetUp()
        {
            _parser = new PathCompositeParser();

            Assert.That(_parser.UnixPathParser, Is.Not.Null);
            Assert.That(_parser.WindowsPathParser, Is.Not.Null);

            return;
        }

        [Test]
        public void Test_Ability_To_Handle_Windows_Path_Like_Values()
        {
            RawError res;

            res = AcceptValue(_parser, "à get_Text() dans C:\\folder\\file1:line 1");
            Assert.That(res.Path, Is.EqualTo("C:\\folder\\file1"));

            return;
        }

        [Test]
        public void Test_Ability_To_Handle_Unix_Path_Like_Values()
        {
            RawError res;

            res = AcceptValue(_parser, "à get_Text() dans /home/ihottier/folder/file1:line 1");
            Assert.That(res.Path, Is.EqualTo("/home/ihottier/folder/file1"));

            return;
        }
    }
}
