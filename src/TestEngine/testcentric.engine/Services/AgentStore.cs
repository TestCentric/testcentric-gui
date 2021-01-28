// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Defines the agent tracking operations that must be done atomically.
    /// </summary>
    internal sealed partial class AgentStore
    {
        private readonly object LOCK = new object();

        private readonly Dictionary<Guid, AgentRecord> _agentIndex = new Dictionary<Guid, AgentRecord>();
        private readonly Dictionary<Process, AgentRecord> _processIndex = new Dictionary<Process, AgentRecord>();

        public void AddAgent(Guid agentId, Process process)
        {
            lock (LOCK)
            {
                if (_agentIndex.ContainsKey(agentId))
                {
                    throw new ArgumentException($"An agent has already been started with the ID '{agentId}'.", nameof(agentId));
                }

                _agentIndex[agentId] = _processIndex[process] = AgentRecord.Starting(agentId, process);
            }
        }

        public void Register(ITestAgent agent)
        {
            lock (LOCK)
            {
                if (!_agentIndex.TryGetValue(agent.Id, out var record)
                    || record.Status != AgentStatus.Starting)
                {
                    throw new ArgumentException($"Agent {agent.Id} must have a status of {AgentStatus.Starting} in order to register, but the status was {record.Status}.", nameof(agent));
                }

                _agentIndex[agent.Id] = _processIndex[record.Process] = record.Ready(agent);
            }
        }

        public bool IsReady(Guid agentId, out ITestAgent agent)
        {
            lock (LOCK)
            {
                if (_agentIndex.TryGetValue(agentId, out var record)
                    && record.Status == AgentStatus.Ready)
                {
                    agent = record.Agent;
                    return true;
                }

                agent = null;
                return false;
            }
        }

        public bool IsAgentProcessActive(Guid agentId, out Process process)
        {
            lock (LOCK)
            {
                if (_agentIndex.TryGetValue(agentId, out var record)
                    && record.Status != AgentStatus.Terminated)
                {
                    process = record.Process;
                    return process != null;
                }

                process = null;
                return false;
            }
        }

        public void MarkProcessTerminated(Process process)
        {
            lock (LOCK)
            {
                if (!_processIndex.TryGetValue(process, out var record))
                    throw new ArgumentException("An entry for the process must exist in order to mark it as terminated.", nameof(process));

                if (record.Status == AgentStatus.Terminated)
                    throw new ArgumentException("Process has already been marked as terminated");

                var agentId = record.AgentId;
                _agentIndex[agentId] = _processIndex[process] = record.Terminated();
            }
        }
    }
}
#endif
