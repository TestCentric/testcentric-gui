// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using TestCentric.Common;
using TestCentric.Engine.Agents;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;
using NUnit.Engine;
using TestCentric.Engine.Communication.Transports.Remoting;
using TestCentric.Engine.Communication.Transports.Tcp;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The TestAgency class provides RemoteTestAgents
    /// on request and tracks their status. Agents
    /// are wrapped in an instance of the TestAgent
    /// class. Multiple agent types are supported
    /// but only one, ProcessAgent is implemented
    /// at this time.
    /// </summary>
    public class TestAgency : ITestAgency, IService
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgency));

        private const int NORMAL_TIMEOUT = 30000;               // 30 seconds
        private const int DEBUG_TIMEOUT = NORMAL_TIMEOUT * 10;  // 5 minutes

        private readonly AgentStore _agentStore = new AgentStore();

        private IRuntimeFrameworkService _runtimeService;

        // Transports used for various target runtimes
        private TestAgencyRemotingTransport _remotingTransport; // .NET Framework
        private TestAgencyTcpTransport _tcpTransport; // .NET Standard 2.0

        internal virtual string RemotingUrl => _remotingTransport.ServerUrl;
        internal virtual string TcpEndPoint => _tcpTransport.ServerUrl;

        public TestAgency() : this("TestAgency", 0) { }

        public TestAgency(string uri, int port )
        {
            _remotingTransport = new TestAgencyRemotingTransport(this, uri, port);
            _tcpTransport = new TestAgencyTcpTransport(this, port);
        }

        public void Register(ITestAgent agent)
        {
            _agentStore.Register(agent);
        }

        public ITestAgent GetAgent(TestPackage package)
        {
            // Target Runtime must be specified by this point
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            Guard.OperationValid(runtimeSetting.Length > 0, "LaunchAgentProcess called with no runtime specified");

            // If target runtime is not available, something went wrong earlier
            var targetRuntime = RuntimeFramework.Parse(runtimeSetting);
            if (!_runtimeService.IsAvailable(targetRuntime.Id))
                throw new ArgumentException(
                    string.Format("The {0} framework is not available", targetRuntime),
                    "framework");

            var agentProcess = CreateAgentProcess(this, package);
            var agentId = agentProcess.AgentId;

            agentProcess.Exited += (sender, e) => OnAgentExit((Process)sender, ((AgentProcess)sender).AgentId);

            agentProcess.Start();
            log.Debug("Launched Agent process {0} - see testcentric-agent_{0}.log", agentProcess.Id);
            log.Debug("Command line: \"{0}\" {1}", agentProcess.StartInfo.FileName, agentProcess.StartInfo.Arguments);

            _agentStore.AddAgent(agentId, agentProcess);

            log.Debug($"Waiting for agent {agentId:B} to register");

            const int pollTime = 200;

            // Increase the timeout to give time to attach a debugger
            bool debug = package.GetSetting(EnginePackageSettings.DebugAgent, false) ||
                         package.GetSetting(EnginePackageSettings.PauseBeforeRun, false);

            int waitTime = debug ? DEBUG_TIMEOUT : NORMAL_TIMEOUT;

            // Wait for agent registration based on the agent actually getting processor time to avoid falling over
            // under process starvation.
            while (waitTime > agentProcess.TotalProcessorTime.TotalMilliseconds && !agentProcess.HasExited)
            {
                Thread.Sleep(pollTime);

                if (_agentStore.IsReady(agentId, out var agent))
                {
                    log.Debug($"Returning new agent {agentId:B}");

                    switch (targetRuntime.Runtime.FrameworkIdentifier)
                    {
                        case FrameworkIdentifiers.NetFramework:
                            return new TestAgentRemotingProxy(agent, agentId);

                        case FrameworkIdentifiers.NetCoreApp:
                            return agent;

                        default:
                            throw new InvalidOperationException($"Invalid runtime: {targetRuntime.Runtime.FrameworkIdentifier}");
                    }
                }
            }

            return null;
        }

        internal static AgentProcess CreateAgentProcess(TestAgency agency, TestPackage package)
        {
            var process = new AgentProcess();

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
                ? agency.TcpEndPoint
                : agency.RemotingUrl;

            var sb = new StringBuilder($"{process.AgentId} {agencyUrl} --pid={Process.GetCurrentProcess().Id}");

            // Set options that need to be in effect before the package
            // is loaded by using the command line.
            if (traceLevel != "Off")
                sb.Append(" --trace=").EscapeProcessArgument(traceLevel);
            if (debugAgent)
                sb.Append(" --debug-agent");
            if (workDirectory != string.Empty)
                sb.Append(" --work=").EscapeProcessArgument(workDirectory);

            string agentExePath = AgentProcess.GetTestAgentExePath(targetRuntime, runAsX86);
            string agentArgs = sb.ToString();

            log.Debug("Using testcentric-agent at " + agentExePath);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            process.EnableRaisingEvents = true;

            if (targetRuntime.Runtime == Runtime.Mono)
            {
                process.StartInfo.FileName = targetRuntime.MonoExePath;
                string monoOptions = "--runtime=v" + targetRuntime.ClrVersion.ToString(3);
                if (debugTests || debugAgent) monoOptions += " --debug";
                process.StartInfo.Arguments = string.Format("{0} \"{1}\" {2}", monoOptions, agentExePath, agentArgs);
            }
            else if (targetRuntime.Runtime == Runtime.Net)
            {
                process.StartInfo.FileName = agentExePath;
                process.StartInfo.Arguments = agentArgs;
                process.StartInfo.LoadUserProfile = loadUserProfile;
            }
            else if (targetRuntime.Runtime == Runtime.NetCore)
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"\"{agentExePath}\" {agentArgs}";
            }
            else
            {
                process.StartInfo.FileName = agentExePath;
                process.StartInfo.Arguments = agentArgs;
            }

            return process;
        }

        internal bool IsAgentProcessActive(Guid agentId, out Process process)
        {
            return _agentStore.IsAgentProcessActive(agentId, out process);
        }

        internal void OnAgentExit(Process process, Guid agentId)
        {
            _agentStore.MarkTerminated(agentId);

            string errorMsg;

            switch (process.ExitCode)
            {
                case AgentExitCodes.OK:
                    return;
                case AgentExitCodes.PARENT_PROCESS_TERMINATED:
                    errorMsg = "Remote test agent believes agency process has exited.";
                    break;
                case AgentExitCodes.UNEXPECTED_EXCEPTION:
                    errorMsg = "Unhandled exception on remote test agent. " +
                               "To debug, try using --trace=debug to output logs.";
                    break;
                case AgentExitCodes.FAILED_TO_START_REMOTE_AGENT:
                    errorMsg = "Failed to start remote test agent.";
                    break;
                case AgentExitCodes.DEBUGGER_SECURITY_VIOLATION:
                    errorMsg = "Debugger could not be started on remote agent due to System.Security.Permissions.UIPermission not being set.";
                    break;
                case AgentExitCodes.DEBUGGER_NOT_IMPLEMENTED:
                    errorMsg = "Debugger could not be started on remote agent as not available on platform.";
                    break;
                case AgentExitCodes.UNABLE_TO_LOCATE_AGENCY:
                    errorMsg = "Remote test agent unable to locate agency process.";
                    break;
                default:
                    errorMsg = $"Remote test agent exited with non-zero exit code {process.ExitCode}";
                    break;
            }

            throw new NUnitEngineException(errorMsg);
        }

        public IServiceLocator ServiceContext { get; set; }

        public ServiceStatus Status { get; private set; }

        // TODO: it would be better if we had a list of transports to start and stop!

        public void StopService()
        {
            try
            {
                _remotingTransport.Stop();
                _tcpTransport.Stop();
            }
            finally
            {
                Status = ServiceStatus.Stopped;
            }
        }

        public void StartService()
        {
            _runtimeService = ServiceContext.GetService<IRuntimeFrameworkService>();
            if (_runtimeService == null)
                Status = ServiceStatus.Error;
            else
            try
            {
                _remotingTransport.Start();
                _tcpTransport.Start();
                Status = ServiceStatus.Started;
            }
            catch
            {
                Status = ServiceStatus.Error;
                throw;
            }
        }
    }
}
#endif
