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
        private static readonly Logger log = InternalTrace.GetLogger(typeof(AgentProcess));

        public AgentProcess(TestAgency agency, TestPackage package, Guid agentId)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.RuntimeFramework, "");
            TargetRuntime = RuntimeFramework.Parse(runtimeSetting);

            // Access other package settings
            bool runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);
            bool debugTests = package.GetSetting(EnginePackageSettings.DebugTests, false);
            bool debugAgent = package.GetSetting(EnginePackageSettings.DebugAgent, false);
            string traceLevel = package.GetSetting(EnginePackageSettings.InternalTraceLevel, "Off");
            bool loadUserProfile = package.GetSetting(EnginePackageSettings.LoadUserProfile, false);
            string workDirectory = package.GetSetting(EnginePackageSettings.WorkDirectory, string.Empty);

            AgentArgs = new StringBuilder($"{agentId} {agency.RemotingUrl} --pid={Process.GetCurrentProcess().Id}");

            // Set options that need to be in effect before the package
            // is loaded by using the command line.
            if (traceLevel != "Off")
                AgentArgs.Append(" --trace=").EscapeProcessArgument(traceLevel);
            if (debugAgent)
                AgentArgs.Append(" --debug-agent");
            if (workDirectory != string.Empty)
                AgentArgs.Append(" --work=").EscapeProcessArgument(workDirectory);

            AgentExePath = GetTestAgentExePath(TargetRuntime, runAsX86);

            log.Debug("Using testcentric-agent at " + AgentExePath);

            StartInfo.UseShellExecute = false;
            StartInfo.CreateNoWindow = true;
            EnableRaisingEvents = true;

            if (TargetRuntime.Runtime == Runtime.Mono)
            {
                StartInfo.FileName = TargetRuntime.MonoExePath;
                string monoOptions = "--runtime=v" + TargetRuntime.ClrVersion.ToString(3);
                if (debugTests || debugAgent) monoOptions += " --debug";
                StartInfo.Arguments = string.Format("{0} \"{1}\" {2}", monoOptions, AgentExePath, AgentArgs);
            }
            else if (TargetRuntime.Runtime == Runtime.Net)
            {
                StartInfo.FileName = AgentExePath;
                StartInfo.Arguments = AgentArgs.ToString();
                StartInfo.LoadUserProfile = loadUserProfile;
            }
            else
            {
                StartInfo.FileName = AgentExePath;
                StartInfo.Arguments = AgentArgs.ToString();
            }
        }

        // Internal properties exposed for testing

        internal RuntimeFramework TargetRuntime { get; }
        internal string AgentExePath { get; }
        internal StringBuilder AgentArgs { get; }

        public Process LaunchProcess()
        {
            log.Info("Getting agent for use under {1}", TargetRuntime);

            // NOTE: This could be done in the constructor, but postponing it makes testing easier.
            if (!File.Exists(AgentExePath))
                throw new FileNotFoundException(
                    $"{Path.GetFileName(AgentExePath)} could not be found.", AgentExePath);


            Start();

            return this;
        }

        public static string GetTestAgentExePath(RuntimeFramework targetRuntime, bool requires32Bit)
        {
            string engineDir = NUnitConfiguration.EngineDirectory;
            if (engineDir == null) return null;

            string agentName = requires32Bit
                ? "testcentric-agent-x86"
                : "testcentric-agent";

            switch (targetRuntime.Runtime.FrameworkIdentifier)
            {
                case ".NETFramework":
                    switch (targetRuntime.FrameworkVersion.Major)
                    {
                        case 2:
                        case 3:
                            return Path.Combine(engineDir, "agents/net20/" + agentName + ".exe");
                        case 4:
                            return Path.Combine(engineDir, "agents/net40/" + agentName + ".exe");
                    }
                    break;

#if NETCORE_SUPPORT // Future support
                case ".NETCoreApp":
                    switch (targetRuntime.FrameworkVersion.Major)
                    {
                        case 1:
                            return Path.Combine(engineDir, "agents/netcoreapp1.1/" + agentName + ".dll");

                        case 2:
                            return Path.Combine(engineDir, "agents/netcoreapp2.1/" + agentName + ".dll");
                    }
                    break;
#endif
            }

            throw new InvalidOperationException($"Unsupported runtime: {targetRuntime.Runtime}");
        }
    }
}
#endif
