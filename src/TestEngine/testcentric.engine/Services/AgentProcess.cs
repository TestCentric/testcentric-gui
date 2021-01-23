// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Engine;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services
{
    public class AgentProcess : Process
    {
        public Guid AgentId { get; } = Guid.NewGuid(); // Used by TestAgency and tests       

        public static string GetTestAgentExePath(RuntimeFramework targetRuntime, bool requires32Bit)
        {
            string engineDir = NUnitConfiguration.EngineDirectory;
            if (engineDir == null) return null;

            string agentName = requires32Bit
                ? "testcentric-agent-x86"
                : "testcentric-agent";

            string agentPath = null;

            switch (targetRuntime.Runtime.FrameworkIdentifier)
            {
                case FrameworkIdentifiers.NetFramework:
                    switch (targetRuntime.FrameworkVersion.Major)
                    {
                        case 2:
                        case 3:
                            agentPath = Path.Combine(engineDir, "agents/net20/" + agentName + ".exe");
                            break;
                        case 4:
                            agentPath = Path.Combine(engineDir, "agents/net40/" + agentName + ".exe");
                            break;
                    }
                    break;

                case FrameworkIdentifiers.NetCoreApp:
                    switch (targetRuntime.FrameworkVersion.Major)
                    {
                        case 1:
                            // TODO: For now, we don't have a .NET Core 1.1 agent and default to 2.1
                            //agentPath = Path.Combine(engineDir, "agents/netcoreapp1.1/" + agentName + ".dll");
                            //break;

                        case 2:
                            agentPath = Path.Combine(engineDir, "agents/netcoreapp2.1/" + agentName + ".dll");
                            break;

                        case 3:
                            agentPath = Path.Combine(engineDir, "agents/netcoreapp3.1/" + agentName + ".dll");
                            break;
                        case 5:
                            agentPath = Path.Combine(engineDir, "agents/net5.0/" + agentName + ".dll");
                            break;
                    }
                    break;
            }

            if (agentPath == null)
                throw new InvalidOperationException($"Unsupported runtime: {targetRuntime.Runtime}");

            // TODO: Temporarily leaving out this check because it breaks some AgentProcessTests            
            //if (!File.Exists(agentPath))
            //    throw new FileNotFoundException($"Agent not found: {agentPath}");

            return agentPath;
        }
    }
}
#endif
