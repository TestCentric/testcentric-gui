// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Runtime.Versioning;

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
        public string TargetRuntime;

        public TestAgentInfo(string agentName, TestAgentType agentType, string targetRuntime)
        {
            AgentName = agentName;
            AgentType = agentType;
            TargetRuntime = targetRuntime;
        }
    }
}
