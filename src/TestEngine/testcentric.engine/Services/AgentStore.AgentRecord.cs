// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
            private AgentRecord(Process process, ITestAgent agent)
            {
                Process = process;
                Agent = agent;
            }

            public Process Process { get; }
            public ITestAgent Agent { get; }

            public AgentStatus Status =>
                Process is null ? AgentStatus.Terminated :
                Agent is null ? AgentStatus.Starting :
                AgentStatus.Ready;

            public static AgentRecord Starting(Process process)
            {
                if (process is null) throw new ArgumentNullException(nameof(process));

                return new AgentRecord(process, agent: null);
            }

            public AgentRecord Ready(ITestAgent agent)
            {
                if (agent is null) throw new ArgumentNullException(nameof(agent));

                return new AgentRecord(Process, agent);
            }

            public AgentRecord Terminated()
            {
                return new AgentRecord(process: null, agent: null);
            }
        }
    }
}
#endif
