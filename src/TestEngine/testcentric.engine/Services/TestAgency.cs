// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text;
using TestCentric.Common;
using TestCentric.Engine.Agents;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Helpers;

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
    public class TestAgency : ServerBase, ITestAgency, IService
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgency));

        private readonly AgentStore _agents = new AgentStore();

        private IRuntimeFrameworkService _runtimeService;

        public TestAgency() : this( "TestAgency", 0 ) { }

        public TestAgency( string uri, int port ) : base( uri, port ) { }

        //public override void Stop()
        //{
        //    foreach( KeyValuePair<Guid,AgentRecord> pair in agentData )
        //    {
        //        AgentRecord r = pair.Value;

        //        if ( !r.Process.HasExited )
        //        {
        //            if ( r.Agent != null )
        //            {
        //                r.Agent.Stop();
        //                r.Process.WaitForExit(10000);
        //            }

        //            if ( !r.Process.HasExited )
        //                r.Process.Kill();
        //        }
        //    }

        //    agentData.Clear();

        //    base.Stop ();
        //}

        public void Register(ITestAgent agent)
        {
            _agents.Register(agent);
        }

        public ITestAgent GetAgent(TestPackage package, int waitTime)
        {
            // TODO: Decide if we should reuse agents
            return CreateRemoteAgent(package, waitTime);
        }

        internal bool IsAgentProcessActive(Guid agentId, out Process process)
        {
            return _agents.IsAgentProcessActive(agentId, out process);
        }

        private Process LaunchAgentProcess(TestPackage package, Guid agentId)
        {
            // Target Runtime must be specified by this point
            string runtimeSetting = package.GetSetting(EnginePackageSettings.RuntimeFramework, "");
            Guard.OperationValid(runtimeSetting.Length > 0, "LaunchAgentProcess called with no runtime specified");
            var targetRuntime = RuntimeFramework.Parse(runtimeSetting);

            // Access other package settings
            bool useX86Agent = package.GetSetting(EnginePackageSettings.RunAsX86, false);
            bool debugTests = package.GetSetting(EnginePackageSettings.DebugTests, false);
            bool debugAgent = package.GetSetting(EnginePackageSettings.DebugAgent, false);
            string traceLevel = package.GetSetting(EnginePackageSettings.InternalTraceLevel, "Off");
            bool loadUserProfile = package.GetSetting(EnginePackageSettings.LoadUserProfile, false);
            string workDirectory = package.GetSetting(EnginePackageSettings.WorkDirectory, string.Empty);

            var agentArgs = new StringBuilder();

            // Set options that need to be in effect before the package
            // is loaded by using the command line.
            agentArgs.Append("--pid=").Append(Process.GetCurrentProcess().Id);
            if (traceLevel != "Off")
                agentArgs.Append(" --trace:").EscapeProcessArgument(traceLevel);
            if (debugAgent)
                agentArgs.Append(" --debug-agent");
            if (workDirectory != string.Empty)
                agentArgs.Append(" --work=").EscapeProcessArgument(workDirectory);

            log.Info("Getting {0} agent for use under {1}", useX86Agent ? "x86" : "standard", targetRuntime);

            if (!_runtimeService.IsAvailable(targetRuntime.Id))
                throw new ArgumentException(
                    string.Format("The {0} framework is not available", targetRuntime),
                    "framework");

            string agentExePath = GetTestAgentExePath(targetRuntime, useX86Agent);

            if (!File.Exists(agentExePath))
                throw new FileNotFoundException(
                    $"{Path.GetFileName(agentExePath)} could not be found.", agentExePath);

            log.Debug("Using testcentric-agent at " + agentExePath);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.EnableRaisingEvents = true;
            p.Exited += (sender, e) => OnAgentExit((Process)sender, agentId);
            string arglist = agentId.ToString() + " " + ServerUrl + " " + agentArgs;

            targetRuntime = ServiceContext.GetService<RuntimeFrameworkService>().GetBestAvailableFramework(targetRuntime);

            if (targetRuntime.Runtime == Runtime.Mono)
            {
                p.StartInfo.FileName = targetRuntime.MonoExePath;
                string monoOptions = "--runtime=v" + targetRuntime.ClrVersion.ToString(3);
                if (debugTests || debugAgent) monoOptions += " --debug";
                p.StartInfo.Arguments = string.Format("{0} \"{1}\" {2}", monoOptions, agentExePath, arglist);
            }
            else if (targetRuntime.Runtime == Runtime.Net)
            {
                p.StartInfo.FileName = agentExePath;
                p.StartInfo.Arguments = arglist;
                p.StartInfo.LoadUserProfile = loadUserProfile;
            }
            else
            {
                p.StartInfo.FileName = agentExePath;
                p.StartInfo.Arguments = arglist;
            }

            p.Start();
            log.Debug("Launched Agent process {0} - see testcentric-agent_{0}.log", p.Id);
            log.Debug("Command line: \"{0}\" {1}", p.StartInfo.FileName, p.StartInfo.Arguments);

            _agents.Start(agentId, p);
            return p;
        }

        private ITestAgent CreateRemoteAgent(TestPackage package, int waitTime)
        {
            var agentId = Guid.NewGuid();
            var process = LaunchAgentProcess(package, agentId);

            log.Debug($"Waiting for agent {agentId:B} to register");

            const int pollTime = 200;

            // Wait for agent registration based on the agent actually getting processor time to avoid falling over
            // under process starvation.
            while (waitTime > process.TotalProcessorTime.TotalMilliseconds && !process.HasExited)
            {
                Thread.Sleep(pollTime);

                if (_agents.IsReady(agentId, out var agent))
                {
                    log.Debug($"Returning new agent {agentId:B}");
                    return new RemoteTestAgentProxy(agent, agentId);
                }
            }

            return null;
        }

        private static string GetTestAgentExePath(RuntimeFramework targetRuntime, bool requires32Bit)
        {
            string engineDir = NUnitConfiguration.EngineDirectory;
            if (engineDir == null) return null;

            string agentName = requires32Bit
                ? "testcentric-agent-x86.exe"
                : "testcentric-agent.exe";

            switch (targetRuntime.Runtime.FrameworkIdentifier)
            {
                case FrameworkIdentifiers.Net:
                    return targetRuntime.FrameworkVersion.Major >= 4
                        ? Path.Combine(engineDir, "agents/net40/" + agentName)
                        : Path.Combine(engineDir, "agents/net20/" + agentName);
                default:
                    return Path.Combine(engineDir, "agents/net40/" + agentName);
           }
        }

        private void OnAgentExit(Process process, Guid agentId)
        {
            _agents.MarkTerminated(agentId);

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
                               "To debug, try running with the --inprocess flag, or using --trace=debug to output logs.";
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

        public void StopService()
        {
            try
            {
                Stop();
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
                Start();
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
