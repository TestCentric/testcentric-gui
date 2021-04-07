// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Diagnostics;

namespace TestCentric.Engine.Services
{
    internal sealed partial class AgentStore
    {
        private struct AgentRecord
        {
            private AgentRecord(ITestAgent agent, Process process)
            {
                if (agent == null)
                    throw new ArgumentNullException(nameof(agent));

                Agent = agent;
                AgentId = agent.Id;
                Process = process;
            }

            private AgentRecord(Guid agentId, Process process)
            {
                AgentId = agentId;
                Agent = null;
                Process = process;
            }

            public Process Process { get; }
            public ITestAgent Agent { get; }
            // AgentId is needed before agent registers and after it terminates
            public Guid AgentId { get; }

            public AgentStatus Status =>
                Process is null ? AgentStatus.Terminated :
                Agent is null ? AgentStatus.Starting :
                AgentStatus.Ready;

            public static AgentRecord Starting(Guid agentId, Process process)
            {
                if (process is null) throw new ArgumentNullException(nameof(process));

                return new AgentRecord(agentId, process);
            }

            public AgentRecord Ready(ITestAgent agent)
            {
                if (agent is null) throw new ArgumentNullException(nameof(agent));

                return new AgentRecord(agent, Process);
            }

            public AgentRecord Terminated()
            {
                return new AgentRecord(AgentId, process: null);
            }
        }
    }
}
#endif
