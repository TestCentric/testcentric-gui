// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Linq;
using NUnit.Framework;
using System.IO;
using System.Diagnostics;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public abstract class AgentLauncherTests<TLauncher> where TLauncher : IAgentLauncher
    {
        protected static readonly Guid AGENTID = Guid.NewGuid();

        protected TLauncher _launcher;
        protected TestPackage _package;
        protected const string REMOTING_URL = "tcp://127.0.0.1:1234/TestAgency";
        protected readonly string REQUIRED_ARGS = $"{REMOTING_URL} --pid={Process.GetCurrentProcess().Id}";

        public static string[] Runtimes = new string[]
        {
            "net-2.0", "net-3.0", "net-3.5", "net-4.0", "net-4.5",
            "netcore-1.1", "netcore-2.1", "netcore-3.1", "netcore-5.0"
        };

        protected abstract string[] SupportedRuntimes { get; }

        [SetUp]
        public void SetUp()
        {
            _launcher = (TLauncher)Activator.CreateInstance(typeof(TLauncher));
            _package = new TestPackage("junk.dll");
        }

        [TestCaseSource(nameof(Runtimes))]
        public void CanCreateProcess(string runtime)
        {
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.RunAsX86] = false;

            bool supported = SupportedRuntimes.Contains(runtime);
            Assert.That(_launcher.CanCreateProcess(_package), Is.EqualTo(supported));
        }

        [TestCaseSource(nameof(Runtimes))]
        public void CanCreateX86Process(string runtime)
        {
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.RunAsX86] = true;

            bool supported = SupportedRuntimes.Contains(runtime);
            Assert.That(_launcher.CanCreateProcess(_package), Is.EqualTo(supported));
        }

        [TestCaseSource(nameof(Runtimes))]
        public void CreateProcess(string runtime)
        {
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.RunAsX86] = false;

            if (SupportedRuntimes.Contains(runtime))
            {
                var process = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
                CheckStandardProcessSettings(process);
                CheckAgentPath(process, false);
            }
            else
            {
                Assert.That(_launcher.CreateProcess(AGENTID, REMOTING_URL, _package), Is.Null);
            }
        }

        protected abstract void CheckAgentPath(Process process, bool x86);

        [TestCaseSource(nameof(Runtimes))]
        public void CreateX86Process(string runtime)
        {
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.RunAsX86] = true;

            if (SupportedRuntimes.Contains(runtime))
            {
                var process = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
                CheckStandardProcessSettings(process);
                CheckAgentPath(process, true);
            }
            else
            {
                Assert.That(_launcher.CreateProcess(AGENTID, REMOTING_URL, _package), Is.Null);
            }
        }

        private void CheckStandardProcessSettings(Process process)
        {
            Assert.NotNull(process);
            Assert.True(process.EnableRaisingEvents, "EnableRaisingEvents");

            var startInfo = process.StartInfo;
            Assert.False(startInfo.UseShellExecute, "UseShellExecute");
            Assert.True(startInfo.CreateNoWindow, "CreateNoWindow");
            Assert.False(startInfo.LoadUserProfile, "LoadUserProfile");
            Assert.That(startInfo.WorkingDirectory, Is.EqualTo(Environment.CurrentDirectory));

            var arguments = startInfo.Arguments;
            Assert.That(arguments, Does.Not.Contain("--trace="));
            Assert.That(arguments, Does.Not.Contain("--debug-agent"));
            Assert.That(arguments, Does.Not.Contain("--work="));
        }

        [Test]
        public void DebugAgentSetting()
        {
            var runtime = SupportedRuntimes[0];
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.DebugAgent] = true;
            var agentProcess = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--debug-agent"));
        }

        [Test]
        public void TraceLevelSetting()
        {
            var runtime = SupportedRuntimes[0];
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.InternalTraceLevel] = "Debug";
            var agentProcess = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--trace=Debug"));
        }

        [Test]
        public void WorkDirectorySetting()
        {
            var runtime = SupportedRuntimes[0];
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.WorkDirectory] = "WORKDIRECTORY";
            var agentProcess = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
            Assert.That(agentProcess.StartInfo.Arguments, Does.Contain("--work=WORKDIRECTORY"));
        }

        [Test]
        public void LoadUserProfileSetting()
        {
            var runtime = SupportedRuntimes[0];
            _package.Settings[EnginePackageSettings.TargetRuntimeFramework] = runtime;
            _package.Settings[EnginePackageSettings.LoadUserProfile] = true;
            var agentProcess = _launcher.CreateProcess(AGENTID, REMOTING_URL, _package);
            Assert.True(agentProcess.StartInfo.LoadUserProfile);
        }
    }

    public class Net40AgentLauncherTests : AgentLauncherTests<Net40AgentLauncher>
    {
        protected override string[] SupportedRuntimes => new string[] { "net-2.0", "net-3.0", "net-3.5", "net-4.0", "net-4.5" };

        private string AgentDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "agents/net40");
        private string AgentName = "testcentric-agent.exe";
        private string AgentNameX86 = "testcentric-agent-x86.exe";

        protected override void CheckAgentPath(Process process, bool x86)
        {
            string agentPath = Path.Combine(AgentDir, x86 ? AgentNameX86 : AgentName);
            Assert.That(process.StartInfo.FileName, Is.SamePath(agentPath));
        }
    }

    public class NetCore21AgentLauncherTests : AgentLauncherTests<NetCore21AgentLauncher>
    {
        protected override string[] SupportedRuntimes => new string[] { "netcore-1.1", "netcore-2.1" };

        private string AgentDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "agents/netcoreapp2.1/").Replace('\\', '/');
        private string AgentName = "testcentric-agent.dll";
        private string AgentNameX86 = "testcentric-agent-x86.dll";

        protected override void CheckAgentPath(Process process, bool x86)
        {
            string agentPath = AgentDir + (x86 ? AgentNameX86 : AgentName);
            Assert.That(process.StartInfo.FileName, Is.EqualTo("dotnet"));
            Assert.That(process.StartInfo.Arguments.Replace('\\', '/'), Does.StartWith(agentPath));
        }
    }

    public class NetCore31AgentLauncherTests : AgentLauncherTests<NetCore31AgentLauncher>
    {
        protected override string[] SupportedRuntimes => new string[] { "netcore-1.1", "netcore-2.1", "netcore-3.1" };

        private string AgentDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "agents/netcoreapp3.1/").Replace('\\', '/');
        private string AgentName = "testcentric-agent.dll";
        private string AgentNameX86 = "testcentric-agent-x86.dll";

        protected override void CheckAgentPath(Process process, bool x86)
        {
            string agentPath = AgentDir + (x86 ? AgentNameX86 : AgentName);
            Assert.That(process.StartInfo.FileName, Is.EqualTo("dotnet"));
            Assert.That(process.StartInfo.Arguments.Replace('\\', '/'), Does.StartWith(agentPath));
        }
    }

    public class Net50AgentLauncherTests : AgentLauncherTests<Net50AgentLauncher>
    {
        protected override string[] SupportedRuntimes => new string[] { "netcore-1.1", "netcore-2.1", "netcore-3.1", "netcore-5.0" };

        private string AgentDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "agents/net5.0/").Replace('\\', '/');

        private string AgentName = "testcentric-agent.dll";
        private string AgentNameX86 = "testcentric-agent-x86.dll";

        protected override void CheckAgentPath(Process process, bool x86)
        {
            string agentPath = AgentDir + (x86 ? AgentNameX86 : AgentName);
            Assert.That(process.StartInfo.FileName, Is.EqualTo("dotnet"));
            Assert.That(process.StartInfo.Arguments.Replace('\\', '/'), Does.StartWith(agentPath));
        }
    }
}
#endif
