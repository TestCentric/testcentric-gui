// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    class TestAgentService : Service
    {
        private IList<ITestAgentProvider> _agentSources = new List<ITestAgentProvider>();
        private IList<TestAgentInfo> _availableAgents = new List<TestAgentInfo>();

        #region ITestAgentFactory Implementation

        public IList<TestAgentInfo> AvailableAgents { get; } = new List<TestAgentInfo>();

        public bool IsAgentAvailable(TestPackage package)
        {
            foreach (var agentSource in _agentSources)
                if (agentSource.IsAgentAvailable(package))
                    return true;

            return false;
        }

        public ITestAgent GetAgent(TestPackage package)
        {
            foreach (var agentSource in _agentSources)
                if (agentSource.IsAgentAvailable(package))
                    return agentSource.GetAgent(package);

            throw new InvalidOperationException("No available agent matches the TestPackage");
        }

        public void ReleaseAgent(ITestAgent agent)
        {
            // TODO: save the source rather than trying all sources
            foreach(var agentSource in _agentSources)
                agentSource.ReleaseAgent(agent);
        }

        #endregion

        #region Service Overrides

        public override void StartService()
        {
            // TEMP for testing
            _agentSources.Add(new DummyAgentFactory());

            ITestAgentProvider testAgency = ServiceContext.GetService<TestAgency>();
            if (testAgency != null)
            {
                _agentSources.Add(testAgency);
                foreach (var info in testAgency.AvailableAgents)
                    _availableAgents.Add(info);
                Status = ServiceStatus.Started;
            }
            else
                Status = ServiceStatus.Error;
        }

        #endregion

        #region

        class DummyAgentFactory : ITestAgentProvider
        {
            IList<TestAgentInfo> ITestAgentProvider.AvailableAgents => new TestAgentInfo[0];

            public TestAgentType AgentType => TestAgentType.LocalProcess;

            ITestAgent ITestAgentProvider.GetAgent(TestPackage package)
            {
                throw new NotImplementedException();
            }

            bool ITestAgentProvider.IsAgentAvailable(TestPackage package) => false;

            void ITestAgentProvider.ReleaseAgent(ITestAgent agent)
            {
            }
        }

        #endregion
    }
}
