// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using System.Text;

namespace TestCentric.Engine.Internal
{
    public static class ProcessUtilsTests
    {
        private static string EscapeProcessArgument(string value, bool alwaysQuote = false)
        {
            var builder = new StringBuilder();
            ProcessUtils.EscapeProcessArgument(builder, value, alwaysQuote);
            return builder.ToString();
        }

        [Test]
        public static void EscapeProcessArgument_null()
        {
            Assert.That(EscapeProcessArgument(null), Is.EqualTo("\"\""));
        }

        [Test]
        public static void EscapeProcessArgument_null_alwaysQuote()
        {
            Assert.That(EscapeProcessArgument(null, true), Is.EqualTo("\"\""));
        }

        [Test]
        public static void EscapeProcessArgument_empty()
        {
            Assert.That(EscapeProcessArgument(string.Empty), Is.EqualTo("\"\""));
        }

        [Test]
        public static void EscapeProcessArgument_empty_alwaysQuote()
        {
            Assert.That(EscapeProcessArgument(string.Empty, true), Is.EqualTo("\"\""));
        }

        [Test]
        public static void EscapeProcessArgument_simple()
        {
            Assert.That(EscapeProcessArgument("123"), Is.EqualTo("123"));
        }

        [Test]
        public static void EscapeProcessArgument_simple_alwaysQuote()
        {
            Assert.That(EscapeProcessArgument("123", true), Is.EqualTo("\"123\""));
        }

        [Test]
        public static void EscapeProcessArgument_with_ending_backslash()
        {
            Assert.That(EscapeProcessArgument("123\\"), Is.EqualTo("123\\"));
        }

        [Test]
        public static void EscapeProcessArgument_with_ending_backslash_alwaysQuote()
        {
            Assert.That(EscapeProcessArgument("123\\", true), Is.EqualTo("\"123\\\\\""));
        }

        [Test]
        public static void EscapeProcessArgument_with_spaces_and_ending_backslash()
        {
            Assert.That(EscapeProcessArgument(" 1 2 3 \\"), Is.EqualTo("\" 1 2 3 \\\\\""));
        }

        [Test]
        public static void EscapeProcessArgument_with_spaces()
        {
            Assert.That(EscapeProcessArgument(" 1 2 3 "), Is.EqualTo("\" 1 2 3 \""));
        }

        [Test]
        public static void EscapeProcessArgument_with_quotes()
        {
            Assert.That(EscapeProcessArgument("\"1\"2\"3\""), Is.EqualTo("\"\\\"1\\\"2\\\"3\\\"\""));
        }

        [Test]
        public static void EscapeProcessArgument_with_slashes()
        {
            Assert.That(EscapeProcessArgument("1\\2\\\\3\\\\\\"), Is.EqualTo("1\\2\\\\3\\\\\\"));
        }

        [Test]
        public static void EscapeProcessArgument_with_slashes_alwaysQuote()
        {
            Assert.That(EscapeProcessArgument("1\\2\\\\3\\\\\\", true), Is.EqualTo("\"1\\2\\\\3\\\\\\\\\\\\\""));
        }

        [Test]
        public static void EscapeProcessArgument_slashes_followed_by_quotes()
        {
            Assert.That(EscapeProcessArgument("\\\\\""), Is.EqualTo("\"\\\\\\\\\\\"\""));
        }
    }
}
