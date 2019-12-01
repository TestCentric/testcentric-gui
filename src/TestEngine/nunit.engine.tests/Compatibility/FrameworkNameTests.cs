// ***********************************************************************
// Copyright (c) 2015 Charlie Poole, Rob Prouse
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

#if NET35
using System;
using System.Runtime.Versioning;
using NUnit.Framework;

namespace NUnit.Engine.Compatibility
{
    /// <summary>
    /// Tests of our compatible implementation of FrameworkName, based on corefx implementation
    /// </summary>
    public static class FrameworkNameTests
    {
        [TestCase("SomeFramework,Version=v1.2.3.4,Profile=PROFILE", "SomeFramework,Version=v1.2.3.4,Profile=PROFILE", "SomeFramework", "1.2.3.4", "PROFILE")]
        [TestCase("SomeFramework,Version=v1.2.3,Profile=PROFILE", "SomeFramework,Version=v1.2.3,Profile=PROFILE", "SomeFramework", "1.2.3", "PROFILE")]
        [TestCase("SomeFramework,Version=v1.2,Profile=PROFILE", "SomeFramework,Version=v1.2,Profile=PROFILE", "SomeFramework", "1.2", "PROFILE")]
        [TestCase(" SomeFramework , Version = v1.2    ,Profile    =    PROFILE", "SomeFramework,Version=v1.2,Profile=PROFILE", "SomeFramework", "1.2", "PROFILE")]
        [TestCase("SomeFramework,Version=1.2,Profile=PROFILE", "SomeFramework,Version=v1.2,Profile=PROFILE", "SomeFramework", "1.2", "PROFILE")]
        [TestCase("SomeFramework,Version=1.2,Profile=", "SomeFramework,Version=v1.2", "SomeFramework", "1.2", "")]
        [TestCase("SomeFramework,Version=1.2", "SomeFramework,Version=v1.2", "SomeFramework", "1.2", "")]
        public static void ConstructFromString(string ctorArg, string expectedName, string expectedIdentifier, string expectedVersion, string expectedProfile)
        {
            var name = new FrameworkName(ctorArg);
            VerifyConstructor(name, expectedName, expectedIdentifier, new Version(expectedVersion), expectedProfile);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(" ,A")]
        [TestCase("A")]
        [TestCase("A,B")]
        [TestCase("A,B,C")]
        [TestCase("A,Version=1.0.0.0,C")]
        [TestCase("A,1.0.0.0,Profile=C")]
        [TestCase("A,Version=1.z.0.0,Profile=C")]
        [TestCase("A,Something=1.z.0.0,Profile=C")]
        [TestCase("A,Profile=C")]
        [TestCase("A,======")]
        [TestCase("A,    =B=")]
        [TestCase("A,1  =2=3")]
        public static void ConstructFromStringThrowsArgumentException(string arg)
        {
            Assert.Throws<ArgumentException>(() => new FrameworkName(arg));
        }

        public static void ConstructFromStringThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkName(null));
        }

        [Test]
        public static void ConstructFromIdentifierVersion()
        {
            var name = new FrameworkName("SomeFramework", new Version(1, 2, 3, 0));
            VerifyConstructor(name, "SomeFramework,Version=v1.2.3.0", "SomeFramework", new Version(1, 2, 3, 0), "");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("   \r\n\t")]
        public static void ConstructFromIdentifierVersionThrowsArgumentException(string identifier)
        {
            Assert.Throws<ArgumentException>(() => new FrameworkName(identifier, new Version(1, 2, 3, 4)));
        }

        [TestCase(null, "1.2.3.4")]
        [TestCase("SomeFramework", null)]
        public static void ConstructFromIdentifierVersionThrowsArgumentNullException(string identifier, string version)
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkName(identifier, new Version(version)));
        }

        [TestCase("SomeFramework", "1.2.3.4", "TestProfile", "SomeFramework,Version=v1.2.3.4,Profile=TestProfile")]
        [TestCase("SomeFramework", "1.2.3.4", "", "SomeFramework,Version=v1.2.3.4")]
        [TestCase("SomeFramework", "1.2.3.4", null, "SomeFramework,Version=v1.2.3.4")]
        public static void ConstructFromIdentifierVersionProfile(string identifier, string version, string profile, string expectedName)
        {
            var name = new FrameworkName(identifier, new Version(version), profile);
            VerifyConstructor(name, expectedName, identifier, new Version(version), profile ?? "");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("   \r\n\t")]
        public static void ConstructFromIdentifierVersionProfileThrowsArgumentException(string identifier)
        {
            Assert.Throws<ArgumentException>(() => new FrameworkName(identifier, new Version(1, 2, 3, 4), "PROFILE"));
        }

        [TestCase(null, "1.2.3.4", "PROFILE")]
        [TestCase("SomeFramework", null, "PROFILE")]
        public static void ConstructFromIdentifierVersionProfileThrowsArgumentNullException(string identifier, string version, string profile)
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkName(identifier, new Version(version), profile));
        }

        [TestCase("1.2", -1, -1)]
        [TestCase("1.2.3", 3, -1)]
        [TestCase("1.2.3.4", 3, 4)]
        public static void ConstructFromVersionWithoutBuildRevision(string version, int expectedBuild, int expectedRevision)
        {
            var name = new FrameworkName("SomeFramework", new Version(version));

            Assert.AreEqual(expectedBuild, name.Version.Build);
            Assert.AreEqual(expectedRevision, name.Version.Revision);
        }

        static readonly string SAMPLE_FRAMEWORK_NAME_STRING = "SomeFramework,Version=v1.2.3.4,Profile=PROFILE";
        static readonly FrameworkName SAMPLE_FRAMEWORK_NAME = new FrameworkName(SAMPLE_FRAMEWORK_NAME_STRING);

        static TestCaseData[] EqualityTestCases = new TestCaseData[]
        {
            new TestCaseData(null, null),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, SAMPLE_FRAMEWORK_NAME),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, new FrameworkName(SAMPLE_FRAMEWORK_NAME_STRING)),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, new FrameworkName("SomeFramework", new Version(1, 2, 3, 4), "PROFILE"))
        };

        [TestCaseSource(nameof(EqualityTestCases))]
        public static void Equality(FrameworkName x, FrameworkName y)
        {
            Assert.True(x == y);
            Assert.False(x != y);
            if (x != null)
            {
                Assert.True(x.Equals(y));
                Assert.True(x.Equals((object)y));
                Assert.True(((IEquatable<FrameworkName>)x).Equals(y));
                Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
            }
        }

        static TestCaseData[] InequalityTestCases = new TestCaseData[]
        {
            new TestCaseData(null, SAMPLE_FRAMEWORK_NAME),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, null),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, new FrameworkName("NotTheTestIdentifier", new Version(1, 2, 3, 4), "PROFILE")),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, new FrameworkName("SomeFramework", new Version(1, 2, 3, 4))),
            new TestCaseData(SAMPLE_FRAMEWORK_NAME, new FrameworkName("SomeFramework", new Version(9, 8, 7, 6), "PROFILE"))
        };

        [TestCaseSource(nameof(InequalityTestCases))]
        public static void Inequality(FrameworkName x, FrameworkName y)
        {
            Assert.True(x != y);
            Assert.False(x == y);
            if (x != null)
            {
                Assert.False(x.Equals(y));
                Assert.False(x.Equals((object)y));
                Assert.False(((IEquatable<FrameworkName>)x).Equals(y));
            }
        }

        private static void VerifyConstructor(FrameworkName name, string expectedName, string expectedIdentifier, Version expectedVersion, string expectedProfile)
        {
            Assert.AreEqual(expectedName, name.FullName);
            Assert.AreEqual(expectedName, name.ToString());
            Assert.AreEqual(expectedIdentifier, name.Identifier);
            Assert.AreEqual(expectedProfile, name.Profile);
            Assert.AreEqual(expectedVersion, name.Version);
        }
    }
}
#endif
