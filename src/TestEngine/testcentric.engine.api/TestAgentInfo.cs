// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Engine
{
    /// <summary>
    /// The TestAgentInfo struct provides information about an
    /// available agent for use by a runner.
    /// </summary>
    public struct TestAgentInfo
    {
        public string AgentName;
        public TestAgentType AgentType;

        public TestAgentInfo(string agentName, TestAgentType agentType)
        {
            AgentName = agentName;
            AgentType = agentType;
        }
    }
}
