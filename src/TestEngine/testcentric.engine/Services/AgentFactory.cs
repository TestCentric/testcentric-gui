// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Engine;
using TestCentric.Engine.Helpers;

namespace TestCentric.Engine.Services
{
    internal class AgentFactory
    {
        private TestAgency _agency;

        public AgentFactory(TestAgency agency)
        {
            _agency = agency;
        }

        private IAgentFactory[] _launchers = new IAgentFactory[]
        {
            new Net20AgentFactory(),
            new Net40AgentFactory(),
            new NetCore21AgentFactory(),
            new NetCore31AgentFactory(),
            new Net50AgentFactory()
        };

        public Process CreateProcess(TestPackage package, Guid agentId)
        {
            foreach(var launcher in _launchers)
            {
                if (launcher.CanCreateProcess(package))
                {
                    // Get target runtime
                    string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
                    var targetRuntime = RuntimeFramework.Parse(runtimeSetting);

                    // Access other package settings
                    bool runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);
                    bool debugTests = package.GetSetting(EnginePackageSettings.DebugTests, false);
                    bool debugAgent = package.GetSetting(EnginePackageSettings.DebugAgent, false);
                    string traceLevel = package.GetSetting(EnginePackageSettings.InternalTraceLevel, "Off");
                    bool loadUserProfile = package.GetSetting(EnginePackageSettings.LoadUserProfile, false);
                    string workDirectory = package.GetSetting(EnginePackageSettings.WorkDirectory, string.Empty);

                    string agencyUrl = targetRuntime.Runtime == Runtime.NetCore
                        ? _agency.TcpEndPoint
                        : _agency.RemotingUrl;

                    var sb = new StringBuilder($"{agentId} {agencyUrl} --pid={Process.GetCurrentProcess().Id}");

                    // Set options that need to be in effect before the package
                    // is loaded by using the command line.
                    if (traceLevel != "Off")
                        sb.Append(" --trace=").EscapeProcessArgument(traceLevel);
                    if (debugAgent)
                        sb.Append(" --debug-agent");
                    if (workDirectory != string.Empty)
                        sb.Append(" --work=").EscapeProcessArgument(workDirectory);

                    Process process = launcher.CreateProcess(sb.ToString(), runAsX86);

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    process.EnableRaisingEvents = true;

                    return process;
                }
            }

            throw new NUnitEngineException($"No agent available for TestPackage {package.Name}");
        }
    }

    public interface IAgentFactory
    {
        bool CanCreateProcess(TestPackage package);
        Process CreateProcess(string agentArgs, bool x86);
    }

    public class Net20AgentFactory : IAgentFactory
    {
        public bool CanCreateProcess(TestPackage package)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            var framework = RuntimeFramework.Parse(runtimeSetting).FrameworkName;
            return framework.Identifier == ".NETFramework" && framework.Version.Major < 4;
        }

        public Process CreateProcess(string agentArgs, bool x86)
        {
            var process = new Process();
            var info = process.StartInfo;
            var agentName = x86 ? "testcentric-agent-x86.exe" : "testcentric-agent.exe";
            var agentPath = System.IO.Path.Combine(NUnitConfiguration.EngineDirectory, $"agents/net20/{agentName}");

            info.FileName = agentPath;
            info.Arguments = agentArgs;

            return process;
        }
    }

    public class Net40AgentFactory : IAgentFactory
    {
        public bool CanCreateProcess(TestPackage package)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            var framework = RuntimeFramework.Parse(runtimeSetting).FrameworkName;
            return framework.Identifier == ".NETFramework" && framework.Version.Major <= 4;
        }

        public Process CreateProcess(string agentArgs, bool x86)
        {
            var process = new Process();
            var info = process.StartInfo;
            var agentName = x86 ? "testcentric-agent-x86.exe" : "testcentric-agent.exe";
            var agentPath = System.IO.Path.Combine(NUnitConfiguration.EngineDirectory, $"agents/net40/{agentName}");

            info.FileName = agentPath;
            info.Arguments = agentArgs;

            return process;
        }
    }

    public class NetCore21AgentFactory : IAgentFactory
    {
        public bool CanCreateProcess(TestPackage package)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            var framework = RuntimeFramework.Parse(runtimeSetting).FrameworkName;
            return framework.Identifier == ".NETCoreApp" && framework.Version.Major <= 2;
        }

        public Process CreateProcess(string agentArgs, bool x86)
        {
            var process = new Process();
            var info = process.StartInfo;
            var agentName = x86 ? "testcentric-agent-x86.dll" : "testcentric-agent.dll";
            var agentPath = System.IO.Path.Combine(NUnitConfiguration.EngineDirectory, $"agents/netcoreapp2.1/{agentName}");

            info.FileName = "dotnet";
            info.Arguments = $"{agentPath} {agentArgs}";

            return process;
        }
    }

    public class NetCore31AgentFactory : IAgentFactory
    {
        public bool CanCreateProcess(TestPackage package)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            var framework = RuntimeFramework.Parse(runtimeSetting).FrameworkName;
            return framework.Identifier == ".NETCoreApp" && framework.Version.Major <= 3;
        }

        public Process CreateProcess(string agentArgs, bool x86)
        {
            var process = new Process();
            var info = process.StartInfo;
            var agentName = x86 ? "testcentric-agent-x86.dll" : "testcentric-agent.dll";
            var agentPath = System.IO.Path.Combine(NUnitConfiguration.EngineDirectory, $"agents/netcoreapp3.1/{agentName}");

            info.FileName = "dotnet";
            info.Arguments = $"{agentPath} {agentArgs}";

            return process;
        }
    }

    public class Net50AgentFactory : IAgentFactory
    {
        public bool CanCreateProcess(TestPackage package)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            var framework = RuntimeFramework.Parse(runtimeSetting).FrameworkName;
            return framework.Identifier == ".NETCoreApp" && framework.Version.Major <= 5;
        }

        public Process CreateProcess(string agentArgs, bool x86)
        {
            var process = new Process();
            var info = process.StartInfo;
            var agentName = x86 ? "testcentric-agent-x86.dll" : "testcentric-agent.dll";
            var agentPath = System.IO.Path.Combine(NUnitConfiguration.EngineDirectory, $"agents/net5.0/{agentName}");

            info.FileName = "dotnet";
            info.Arguments = $"{agentPath} {agentArgs}";

            return process;
        }
    }
}
#endif
