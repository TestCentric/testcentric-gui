// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The TestAgentInfo struct provides information about an
    /// available agent for use by a runner.
    /// </summary>
    public struct TestAgentInfo
    {
        public string AgentName;
        public TestAgentType AgentType;
        public string TargetRuntime;

        public TestAgentInfo(string agentName, TestAgentType agentType, string targetRuntime)
        {
            //Guard.ArgumentNotNullOrEmpty(agentName, nameof(agentName));
            //Guard.ArgumentValid(agentType != TestAgentType.Any, "TargetAgentType.Any may not be used to describe an agent", nameof(agentType));
            //Guard.ArgumentNotNullOrEmpty(targetRuntime, nameof(targetRuntime));

            AgentName = agentName;
            AgentType = agentType;
            TargetRuntime = targetRuntime;
        }
    }
}
