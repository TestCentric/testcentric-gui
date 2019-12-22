// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using NUnit.Framework;

namespace NUnit.Engine
{
    public static class RuntimeTests
    {
        [Sequential]
        public static void DisplayName(
            [ValueSource(nameof(KNOWN_RUNTIMES))] string name,
            [ValueSource(nameof(DISPLAY_NAMES))] string display)
        {
            var runtime = Runtime.Parse(name);
            Assert.That(runtime.DisplayName, Is.EqualTo(display));
        }

        [Sequential]
        public static void FrameworkIdentifier(
            [ValueSource(nameof(KNOWN_RUNTIMES))] string name,
            [ValueSource(nameof(FRAMEWORK_IDENTIFIERS))] string identifier)
        {
            var runtime = Runtime.Parse(name);
            Assert.That(runtime.FrameworkIdentifier, Is.EqualTo(identifier));

            // HACK: Until we resolve issues with the Mono and Any runtimes
            if (runtime != Runtime.Mono)
            {
                var roundtrip = Runtime.FromFrameworkIdentifier(identifier);
                Assert.That(roundtrip, Is.EqualTo(runtime));
            }
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void IsKnownRuntime(string name)
        {
            Assert.That(Runtime.IsKnownRuntime(name));
        }

        [Test]
        public static void KnownRuntimes()
        {
            // If this fails, it most likely means a new runtime was added
            // without updating the tests!
            Assert.That(Runtime.KnownRuntimes, Is.EqualTo(KNOWN_RUNTIMES));
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void PublicPropertyExists(string name)
        {
            PropertyInfo prop = typeof(Runtime).GetProperty(name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);
            Assert.NotNull(prop, $"Property {name} not found");
            var runtime = prop.GetValue(null, null);
            Assert.That(runtime.ToString(), Is.EqualTo(name));
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void CanParseRuntime(string name)
        {
            var runtime = Runtime.Parse(name);
            Assert.That(runtime.ToString(), Is.EqualTo(name));
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void EqualsOverride(string name)
        {
            var rt1 = Runtime.Parse(name);
            var rt2 = Runtime.Parse(name);
            Assert.That(rt1.Equals(rt1), "Equals same object");
            Assert.That(rt1.Equals(rt2), "Equals different objects");
            Assert.That(rt1, Is.EqualTo(rt1), "EqualTo same object");
            Assert.That(rt1, Is.EqualTo(rt2), "EqualTo different objects");
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void EqualityOperator(string name)
        { 
            var rt1 = Runtime.Parse(name);

            var rt2 = Runtime.Parse(name);
#pragma warning disable 1718
            Assert.That(rt1 == rt1, "rt1 == rt1");
#pragma warning restore 1718
            Assert.That(rt1 == rt2, "rt1 == rt2");
        }

        [Test]
        public static void EqualityOperatorUsingNull()
        {
            Runtime rt = null;
            Assert.That(rt == null, "rt == null");
            Assert.That(null == rt, "null == rt");
        }

        [TestCaseSource(nameof(KNOWN_RUNTIMES))]
        public static void InequalityOperator(string name)
        {
            var rt1 = Runtime.Parse(name);
            var rt2 = name == "Net"
                ? Runtime.Parse("NetCore")
                : Runtime.Parse("Net");
            Assert.That(rt1 != rt2, "rt1 != rt2");
        }

        private static string[] KNOWN_RUNTIMES = new string[]
            { "Net", "Mono", "NetCore"};
        private static string[] DISPLAY_NAMES = new string[]
            { ".NET", "Mono", ".NETCore" };
        private static string[] FRAMEWORK_IDENTIFIERS = new string[]
            { ".NETFramework", ".NETFramework", ".NETCoreApp" };
    }
}
