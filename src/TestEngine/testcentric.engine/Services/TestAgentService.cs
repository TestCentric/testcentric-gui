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
        private IList<ITestAgentProvider> _providers = new List<ITestAgentProvider>();

        #region ITestAgentFactory Implementation

        public bool IsAgentAvailable(TestPackage package)
        {
            foreach (var agentSource in _providers)
                if (agentSource.IsAgentAvailable(package))
                    return true;

            return false;
        }

        public ITestAgent GetAgent(TestPackage package)
        {
            foreach (var agentSource in _providers)
                if (agentSource.IsAgentAvailable(package))
                    return agentSource.GetAgent(package);

            throw new InvalidOperationException("No available agent matches the TestPackage");
        }

        public void ReleaseAgent(ITestAgent agent)
        {
            // TODO: save the source rather than trying all sources
            foreach(var agentSource in _providers)
                agentSource.ReleaseAgent(agent);
        }

        #endregion

        #region Service Overrides

        public override void StartService()
        {
            ITestAgentProvider testAgency = ServiceContext.GetService<TestAgency>();
            if (testAgency != null)
            {
                _providers.Add(testAgency);
                Status = ServiceStatus.Started;
            }
            else
                Status = ServiceStatus.Error;
        }

        #endregion
    }
}
