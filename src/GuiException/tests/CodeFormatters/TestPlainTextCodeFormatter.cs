// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NUnit.UiException.CodeFormatters;

namespace NUnit.UiException.Tests.CodeFormatters
{
    [TestFixture]
    public class TestPlainTextCodeFormatter
    {
        private PlainTextCodeFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new PlainTextCodeFormatter();

            return;
        }

        [Test]
        public void Test_Language()
        {
            Assert.That(_formatter.Language, Is.EqualTo("Plain text"));
        }

        [Test]
        public void Test_PreProcess()
        {
            // PreProcess is expected to remove '\t' sequences.
            // This test expects that normal strings are left untouched.

            Assert.That(_formatter.PreProcess("hello world"), Is.EqualTo("hello world"));

            // This test expects to see differences
            Assert.That(_formatter.PreProcess("hello\tworld"), Is.EqualTo("hello    world"));

            // test to fail: passing null has no effect.
            Assert.That(_formatter.PreProcess(null), Is.Null);

            return;
        }

        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void Format_Can_Throw_CodeNullException()
        //{
        //    _formatter.Format(null); // throws exception
        //}

        [Test]
        public void Format_HelloWorld()
        {
            FormattedCode res;
            FormattedCode exp;

            res = _formatter.Format("Hello world!");

            exp = new FormattedCode(
                "Hello world!",
                new int[] { 0 },
                new byte[] { 0 },
                new int[] { 0 });

            Assert.That(res, Is.EqualTo(exp));

            return;
        }

        [Test]
        public void Format_Lines()
        {
            FormattedCode res;
            FormattedCode exp;

            res = _formatter.Format(
                "line 1\r\n" +
                "line 2\r\n" +
                "line 3\r\n");

            exp = new FormattedCode(
                res.Text,
                new int[] { 0, 8, 16 },
                new byte[] { 0, 0, 0 },
                new int[] { 0, 1, 2 });

            Assert.That(res, Is.EqualTo(exp));

            return;
        }

        [TestCase("\n\nStart")]
        [TestCase("Start\n\nEnd")]
        [TestCase("End\n\n")]
        [TestCase("\n\n\nStart")]
        [TestCase("Start\n\n\nEnd")]
        [TestCase("End\n\n\n")]
        public void Format_Lines_MultipleLinuxNewLines(string input)
        {
            Assert.DoesNotThrow(() => _formatter.Format(input));
        }

        [TestCase("\r\n\r\nStart")]
        [TestCase("Start\r\n\r\nEnd")]
        [TestCase("End\r\n\r\n")]
        [TestCase("\r\n\r\n\r\nStart")]
        [TestCase("Start\r\n\r\n\r\nEnd")]
        [TestCase("End\r\n\r\n\r\n")]

        public void Format_Lines_MultipleWindowsNewLines(string input)
        {
            Assert.DoesNotThrow(() => _formatter.Format(input));
        }
    }
}
