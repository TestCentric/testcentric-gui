// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    class TestAgentService : Service, ITestAgentService
    {
        private IList<ITestAgentSource> _agentSources = new List<ITestAgentSource>();
        private IList<TestAgentInfo> _availableAgents = new List<TestAgentInfo>();

        #region ITestAgentFactory Implementation

        IList<TestAgentInfo> ITestAgentService.AvailableAgents { get; } = new List<TestAgentInfo>();

        bool ITestAgentService.IsAgentAvailable(TestPackage package)
        {
            foreach (var agentSource in _agentSources)
                if (agentSource.IsAgentAvailable(package))
                    return true;

            return false;
        }

        ITestAgent ITestAgentService.GetAgent(TestPackage package)
        {
            foreach (var agentSource in _agentSources)
                if (agentSource.IsAgentAvailable(package))
                    return agentSource.GetAgent(package);

            throw new InvalidOperationException("No available agent matches the TestPackage");
        }

        ITestAgent ITestAgentService.SelectAgent(int index)
        {
            foreach (var agentSource in _agentSources)
            {
                int sourceCount = agentSource.AvailableAgents.Count;
                if (index >= sourceCount)
                    index -= sourceCount;
                else
                    return agentSource.SelectAgent(index);
            }

            throw new InvalidOperationException("No agent named {agentName} is available.");
        }

        void ITestAgentService.ReleaseAgent(ITestAgent agent)
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

            ITestAgentSource testAgency = ServiceContext.GetService<TestAgency>();
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

        class DummyAgentFactory : ITestAgentSource
        {
            IList<TestAgentInfo> ITestAgentSource.AvailableAgents => new TestAgentInfo[0];

            public TestAgentType AgentType => TestAgentType.LocalProcess;

            ITestAgent ITestAgentSource.GetAgent(TestPackage package)
            {
                throw new NotImplementedException();
            }

            ITestAgent ITestAgentSource.SelectAgent(int index)
            {
                throw new NotImplementedException();
            }

            bool ITestAgentSource.IsAgentAvailable(TestPackage package) => false;

            void ITestAgentSource.ReleaseAgent(ITestAgent agent)
            {
            }
        }

        #endregion
    }
}
