// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using NUnit.Framework;
using NSubstitute;
using System.IO;
using System.Diagnostics;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    [TestFixture("net-2.0", false)]
    [TestFixture("net-2.0", true)]
    [TestFixture("net-3.0", false)]
    [TestFixture("net-3.0", true)]
    [TestFixture("net-3.5", false)]
    [TestFixture("net-3.5", true)]
    [TestFixture("net-4.0", false)]
    [TestFixture("net-4.0", true)]
    [TestFixture("net-4.5", false)]
    [TestFixture("netcore-5.0", false)]
    [TestFixture("netcore-5.0", true)]
    public class AgentProcessTests
    {
        private string _runtime;
        private bool _x86; 

        private TestAgency _agency;
        private TestPackage _package;
        private const string REMOTING_URL = "tcp://127.0.0.1:1234/TestAgency";
        private readonly string REQUIRED_ARGS = $"{REMOTING_URL} --pid={Process.GetCurrentProcess().Id}";

        public AgentProcessTests(string runtime, bool x86)
        {
            _runtime = runtime;
            _x86 = x86;
        }

        [SetUp]
        public void SetUp()
        {
            _agency = Substitute.For<TestAgency>();
            _agency.RemotingUrl.ReturnsForAnyArgs(REMOTING_URL);
            _package = new TestPackage("junk.dll");
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = _runtime;
            _package.Settings[EnginePackageSettings.RunAsX86] = _x86;
        }

        [Test]
        public void AgentSelection()
        {
            var agentProcess = GetAgentProcess();
            var ext = _runtime.StartsWith("netcore") ? ".dll" : ".exe";
            var agentName = (_x86 ? "testcentric-agent-x86" : "testcentric-agent") + ext;
            var dir = TargetRuntimeDirectory;

            // NOTE: the file doesn't actually exist at this location during unit
            // testing, but it's where it will be found once the app is installed.
            var agentPath = Path.Combine(TestContext.CurrentContext.TestDirectory, $"agents/{dir}/{agentName}");

            if (_runtime.StartsWith("netcore"))
            {
                Assert.That(agentProcess.StartInfo.FileName, Is.EqualTo("dotnet"));
                Assert.That(agentProcess.StartInfo.Arguments, Does.StartWith($"\"{agentPath}"));
            }
            else
                Assert.That(agentProcess.StartInfo.FileName, Is.SamePath(agentPath));
        }

        [Test]
        public void DefaultValues()
        {
            var process = GetAgentProcess();

            Assert.True(process.EnableRaisingEvents, "EnableRaisingEvents");

            var startInfo = process.StartInfo;
            Assert.False(startInfo.UseShellExecute, "UseShellExecute");
            Assert.True(startInfo.CreateNoWindow, "CreateNoWindow");
            Assert.False(startInfo.LoadUserProfile, "LoadUserProfile");
        }

        [Test]
        public void DebugTests()
        {
            _package.Settings[EnginePackageSettings.DebugTests] = true;
            var agentProcess = GetAgentProcess();

            // Not reflected in args because framework handles it
            Assert.That(agentProcess.StartInfo.Arguments, Does.Not.Contain("--debug-tests"));
        }

        [Test]
        public void DebugAgent()
        {
            _package.Settings[EnginePackageSettings.DebugAgent] = true;
            var agentProcess = GetAgentProcess();
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--debug-agent"));
        }


        //[Test]
        public void LoadUserProfile()
        {
            _package.Settings[EnginePackageSettings.LoadUserProfile] = true;
            var agentProcess = GetAgentProcess();
            Assert.That(agentProcess.StartInfo.Arguments, Is.EqualTo($"{agentProcess.AgentId} {REQUIRED_ARGS}"));
        }

        [Test]
        public void TraceLevel()
        {
            _package.Settings[EnginePackageSettings.InternalTraceLevel] = "Debug";
            var agentProcess = GetAgentProcess();
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--trace=Debug"));
        }

        [Test]
        public void WorkDirectory()
        {
            _package.Settings[EnginePackageSettings.WorkDirectory] = "WORKDIRECTORY";
            var agentProcess = GetAgentProcess();
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--work=WORKDIRECTORY"));
        }

        [TestCase(".NetFramework,Version=2.0")]
        [TestCase(".NetFramework,Version=4.5")]
        [TestCase(".NetFramework,Version=4.8")]
        [TestCase(".NetCoreApp,Version=2.1")]
        [TestCase(".NetCoreApp,Version=2.1")]
        [TestCase(".NetCoreApp,Version=2.1")]
        public void WorkingDirectory(string targetRuntime)
        {
            _package.Settings[EnginePackageSettings.ImageTargetFrameworkName] = targetRuntime;
            Assert.That(GetAgentProcess().StartInfo.WorkingDirectory, Is.EqualTo(Environment.CurrentDirectory));
        }

        private AgentProcess GetAgentProcess() => TestAgency.CreateAgentProcess(_agency, _package);

        private string TargetRuntimeDirectory
        {
            get
            {
                if (_runtime == "netcore-5.0")
                    return "net5.0";
                else if (_runtime.StartsWith("netcore3"))
                    return "netcoreapp3.1";
                else if (_runtime.StartsWith("netcore"))
                    return "netcoreapp2.1";
                else if (_runtime.StartsWith("net-4"))
                    return "net40";
                else 
                    return "net20";
            }
        }
    }
}
#endif
