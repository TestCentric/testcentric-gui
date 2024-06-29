// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using NUnit.Framework;
#if NET20 || NET35
using FrameworkName = TestCentric.Engine.Compatibility.FrameworkName;
#else
using FrameworkName = System.Runtime.Versioning.FrameworkName;
#endif

namespace TestCentric.Engine
{
    using Helpers;

    [TestFixture]
    public class RuntimeFrameworkTests
    {
        static readonly Runtime CURRENT_RUNTIME =
            Type.GetType("Mono.Runtime", false) != null
                ? Runtime.Mono
                : Runtime.Net;

        [Test]
        public void CanGetCurrentFramework()
        {
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;

            Assert.That(framework.Runtime, Is.EqualTo(CURRENT_RUNTIME));
            Assert.That(framework.ClrVersion, Is.EqualTo(Environment.Version));
        }

        [Test]
        public void CurrentFrameworkHasBuildSpecified()
        {
            Assert.That(RuntimeFramework.CurrentFramework.ClrVersion.Build, Is.GreaterThan(0));
        }

        [TestCaseSource(nameof(frameworkData))]
        public void CanCreateUsingFrameworkVersion(FrameworkData data)
        {
            RuntimeFramework framework = new RuntimeFramework(data.Runtime, data.FrameworkVersion);
            Assert.That(framework.Runtime, Is.EqualTo(data.Runtime));
            Assert.That(framework.FrameworkVersion, Is.EqualTo(data.FrameworkVersion));
            Assert.That(framework.ClrVersion, Is.EqualTo(data.ClrVersion));
            Assert.That(framework.FrameworkName, Is.EqualTo(data.FrameworkName));
        }

        [TestCaseSource(nameof(frameworkData))]
        public void CanParseRuntimeFramework(FrameworkData data)
        {
            RuntimeFramework framework = RuntimeFramework.Parse(data.Representation);
            Assert.That(framework.Runtime, Is.EqualTo(data.Runtime));
            Assert.That(framework.ClrVersion, Is.EqualTo(data.ClrVersion));
            Assert.That(framework.FrameworkName, Is.EqualTo(data.FrameworkName));
        }

        [TestCaseSource(nameof(frameworkData))]
        public void CanDisplayFrameworkAsString(FrameworkData data)
        {
            RuntimeFramework framework = new RuntimeFramework(data.Runtime, data.FrameworkVersion);
            Assert.That(framework.ToString(), Is.EqualTo(data.Representation));
            Assert.That(framework.DisplayName, Is.EqualTo(data.DisplayName));
        }

        [TestCaseSource(nameof(frameworkData))]
        public void CanCreateFromFrameworkName(FrameworkData data)
        {
            Assume.That(data.Runtime != Runtime.Mono);

            var framework = RuntimeFramework.FromFrameworkName(data.FrameworkName);
            Assert.That(framework.ToString(), Is.EqualTo(data.Representation));
        }

        [TestCaseSource(nameof(matchData))]
        public bool CanMatchRuntimes(RuntimeFramework f1, RuntimeFramework f2)
        {
            return f1.Supports(f2);
        }

        [TestCaseSource(nameof(CanLoadData))]
        public bool CanLoad(RuntimeFramework f1, RuntimeFramework f2)
        {
            return f1.CanLoad(f2);
        }

#pragma warning disable 414
        static TestCaseData[] matchData = new TestCaseData[] {
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(3,5)),
                new RuntimeFramework(Runtime.Net, new Version(2,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Net, new Version(3,5)))
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(3,5)),
                new RuntimeFramework(Runtime.Net, new Version(3,5)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Net, new Version(2,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Mono, new Version(2,0)))
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Net, new Version(1,1)))
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(6,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(5,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(3,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(2,1)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.NetCore, new Version(7,0)),
                new RuntimeFramework(Runtime.NetCore, new Version(1,1)))
                .Returns(true),
            };

        private static readonly TestCaseData[] CanLoadData = {
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Net, new Version(2,0)))
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(2,0)),
                new RuntimeFramework(Runtime.Net, new Version(4,0)))
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(Runtime.Net, new Version(4,0)),
                new RuntimeFramework(Runtime.Net, new Version(2,0)))
                .Returns(true)
            };
#pragma warning restore 414

        public struct FrameworkData
        {
            public Runtime Runtime;
            public Version FrameworkVersion;
            public Version ClrVersion;
            public string Representation;
            public string DisplayName;
            public FrameworkName FrameworkName;

            public FrameworkData(Runtime runtime, Version frameworkVersion, Version clrVersion,
                string representation, string displayName, string frameworkName)
            {
                Runtime = runtime;
                FrameworkVersion = frameworkVersion;
                ClrVersion = clrVersion;
                Representation = representation;
                DisplayName = displayName;
                FrameworkName = frameworkName != null
                    ? new FrameworkName(frameworkName)
                    : null;
            }

            public override string ToString()
            {
                return string.Format("<{0}-{1}>", this.Runtime, this.FrameworkVersion);
            }
        }

#pragma warning disable 414
        static FrameworkData[] frameworkData = new FrameworkData[] {
            new FrameworkData(Runtime.Net, new Version(1,0), new Version(1,0,3705), "net-1.0", ".NET 1.0", ".NETFramework,Version=v1.0"),
            new FrameworkData(Runtime.Net, new Version(1,1), new Version(1,1,4322), "net-1.1", ".NET 1.1", ".NETFramework,Version=v1.1"),
            new FrameworkData(Runtime.Net, new Version(2,0), new Version(2,0,50727), "net-2.0", ".NET 2.0", ".NETFramework,Version=2.0"),
            new FrameworkData(Runtime.Net, new Version(3,0), new Version(2,0,50727), "net-3.0", ".NET 3.0", ".NETFramework,Version=3.0"),
            new FrameworkData(Runtime.Net, new Version(3,5), new Version(2,0,50727), "net-3.5", ".NET 3.5", ".NETFramework,Version=3.5"),
            new FrameworkData(Runtime.Net, new Version(4,0), new Version(4,0,30319), "net-4.0", ".NET 4.0", ".NETFramework,Version=4.0"),
            new FrameworkData(Runtime.Net, new Version(4,5), new Version(4,0,30319), "net-4.5", ".NET 4.5", ".NETFramework,Version=4.5"),
            new FrameworkData(Runtime.Net, new Version(4,5,1), new Version(4,0,30319), "net-4.5.1", ".NET 4.5.1", ".NETFramework,Version=4.5.1"),
            new FrameworkData(Runtime.Net, new Version(4,5,2), new Version(4,0,30319), "net-4.5.2", ".NET 4.5.2", ".NETFramework,Version=4.5.2"),
            new FrameworkData(Runtime.Net, new Version(4,6), new Version(4,0,30319), "net-4.6", ".NET 4.6", ".NETFramework,Version=4.6"),
            new FrameworkData(Runtime.Net, new Version(4,6,1), new Version(4,0,30319), "net-4.6.1", ".NET 4.6.1", ".NETFramework,Version=4.6.1"),
            new FrameworkData(Runtime.Net, new Version(4,6,2), new Version(4,0,30319), "net-4.6.2", ".NET 4.6.2", ".NETFramework,Version=4.6.2"),
            new FrameworkData(Runtime.Net, new Version(4,7), new Version(4,0,30319), "net-4.7", ".NET 4.7", ".NETFramework,Version=4.7"),
            new FrameworkData(Runtime.Net, new Version(4,7,1), new Version(4,0,30319), "net-4.7.1", ".NET 4.7.1", ".NETFramework,Version=4.7.1"),
            new FrameworkData(Runtime.Net, new Version(4,7,2), new Version(4,0,30319), "net-4.7.2", ".NET 4.7.2", ".NETFramework,Version=4.7.2"),
            new FrameworkData(Runtime.Net, new Version(4,8), new Version(4,0,30319), "net-4.8", ".NET 4.8", ".NETFramework,Version=4.8"),
            new FrameworkData(Runtime.Mono, new Version(1,0), new Version(1,1,4322), "mono-1.0", "Mono 1.0", ".NETFramework,Version=1.0"),
            new FrameworkData(Runtime.Mono, new Version(2,0), new Version(2,0,50727), "mono-2.0", "Mono 2.0", ".NETFramework,Version=2.0"),
            new FrameworkData(Runtime.Mono, new Version(3,5), new Version(2,0,50727), "mono-3.5", "Mono 3.5", ".NETFramework,Version=3.5"),
            new FrameworkData(Runtime.Mono, new Version(4,0), new Version(4,0,30319), "mono-4.0", "Mono 4.0", ".NETFramework,Version=4.0"),
            new FrameworkData(Runtime.NetCore, new Version(1,0), new Version(4,0,30319), "netcore-1.0", ".NETCore 1.0", ".NETCoreApp,Version=1.0"),
            new FrameworkData(Runtime.NetCore, new Version(1,1), new Version(4,0,30319), "netcore-1.1", ".NETCore 1.1", ".NETCoreApp,Version=1.1"),
            new FrameworkData(Runtime.NetCore, new Version(2,0), new Version(4,0,30319), "netcore-2.0", ".NETCore 2.0", ".NETCoreApp,Version=2.0"),
            new FrameworkData(Runtime.NetCore, new Version(2,1), new Version(4,0,30319), "netcore-2.1", ".NETCore 2.1", ".NETCoreApp,Version=2.1"),
            new FrameworkData(Runtime.NetCore, new Version(3,0), new Version(3,1,10), "netcore-3.0", ".NETCore 3.0", ".NETCoreApp,Version=3.0"),
        };
#pragma warning restore 414
    }
}
#endif
