// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Linq;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    public class TestAgentServiceTests
    {
        private ServiceContext _services;
        private ITestAgentInfo _agentService;

        [SetUp]
        public void StartServices()
        {
            _services = new ServiceContext();
            _services.Add(new TestAgency("TestAgentServiceTests", 0));
            _services.Add(new TestAgentService());
            _services.ServiceManager.StartServices();

            _agentService = _services.GetService<ITestAgentInfo>();
        }

        [TearDown]
        public void StopServices()
        {
            _services.ServiceManager.StopServices();
        }

        [Test]
        public void AvailableAgents()
        {
            Assert.That(_agentService.GetAvailableAgents().Select((info) => info.AgentName),
                Is.EqualTo(TestAgencyTests.BUILTIN_AGENTS));
        }

        [TestCase("net-2.0", "Net20AgentLauncher", "Net40AgentLauncher")]
        [TestCase("net-3.5", "Net20AgentLauncher", "Net40AgentLauncher")]
        [TestCase("net-4.0", "Net40AgentLauncher")]
        [TestCase("net-4.8", "Net40AgentLauncher")]
        [TestCase("netcore-1.1", "NetCore21AgentLauncher", "NetCore31AgentLauncher", "Net50AgentLauncher")]
        [TestCase("netcore-2.0", "NetCore21AgentLauncher", "NetCore31AgentLauncher", "Net50AgentLauncher")]
        [TestCase("netcore-2.1", "NetCore21AgentLauncher", "NetCore31AgentLauncher", "Net50AgentLauncher")]
        [TestCase("netcore-3.0", "NetCore31AgentLauncher", "Net50AgentLauncher")]
        [TestCase("netcore-3.1", "NetCore31AgentLauncher", "Net50AgentLauncher")]
        [TestCase("netcore-5.0", "Net50AgentLauncher")]
        public void GetAgentsForPackage(string targetRuntime, params string[] expectedAgents)
        {
            var package = new TestPackage("some.dll");
            package.AddSetting(EnginePackageSettings.TargetRuntimeFramework, targetRuntime);
            
            Assert.That(_agentService.GetAvailableAgents(package).Select((info) => info.AgentName),
                Is.EqualTo(expectedAgents));
        }
    }
}
