// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using NUnit.Engine;
using TestCentric.Common;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Internal;
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
    public class TestAgency : ITestAgentProvider, ITestAgency, IService
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgency));

        private const int NORMAL_TIMEOUT = 30000;               // 30 seconds
        private const int DEBUG_TIMEOUT = NORMAL_TIMEOUT * 10;  // 5 minutes

        private readonly AgentStore _agentStore = new AgentStore();

        private ExtensionService _extensionService;

        private readonly List<IAgentLauncher> _launchers = new List<IAgentLauncher>();

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

        #region ITestAgentInfo Implementation

        /// <summary>
        /// Gets a list containing <see cref="TestAgentInfo"/> for all available agents.
        /// </summary>
        public IList<TestAgentInfo> GetAvailableAgents()
        {
            var agents = new List<TestAgentInfo>();

            foreach (var launcher in _launchers)
                agents.Add(launcher.AgentInfo);

            return agents;
        }

        /// <summary>
        /// Gets a list containing <see cref="TestAgentInfo"/> for any available agents,
        /// which are able to handle the specified package.
        /// </summary>
        /// <param name="package">A Testpackage</param>
        /// <returns>
        /// A list of suitable agents for running the package or an empty
        /// list if no agent is available for the package.
        /// </returns>
        public IList<TestAgentInfo> GetAgentsForPackage(TestPackage targetPackage)
        {
            Guard.ArgumentNotNull(targetPackage, nameof(targetPackage));

            // Initialize lists with ALL available agents
            var availableAgents = new List<TestAgentInfo>(GetAvailableAgents());
            var validAgentNames = new List<string>(availableAgents.Select(info => info.AgentName));

            // Look at each included assembly package to see if any names should be removed
            foreach (var assemblyPackage in targetPackage.Select(p => p.IsAssemblyPackage()))
            {
                // Collect names of agents that work for each assembly
                var agentsForAssembly = new List<string>();
                foreach (var launcher in _launchers)
                    if (launcher.CanCreateProcess(assemblyPackage))
                        agentsForAssembly.Add(launcher.AgentInfo.AgentName);

                // Remove agents from final result if they don't work for this assembly
                for (int index = validAgentNames.Count - 1; index >= 0; index--)
                {
                    var agentName = validAgentNames[index];
                    if (!agentsForAssembly.Contains(agentName))
                        validAgentNames.RemoveAt(index);
                }
            }

            // Finish up by deleting all unsuitable entries form the List of TestAgentInfo
            for (int index = availableAgents.Count - 1; index >= 0; index--)
            {
                var agentName = availableAgents[index].AgentName;
                if (!validAgentNames.Contains(agentName))
                    availableAgents.RemoveAt(index);
            }

            return availableAgents;
        }

        #endregion

        #region ITestAgentProvider Implementation

        /// <summary>
        /// Returns true if an agent can be found, which is suitable
        /// for running the provided test package.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        public bool IsAgentAvailable(TestPackage package)
        {
            foreach (var launcher in _launchers)
                if (launcher.CanCreateProcess(package))
                    return true;

            return false;
        }

        /// <summary>
        /// Return an agent, which best matches the criteria defined
        /// in a TestPackage.
        /// </summary>
        /// <param name="package">The test package to be run</param>
        /// <returns>An ITestAgent</returns>
        /// <exception cref="ArgumentException">If no agent is available.</exception>
        public ITestAgent GetAgent(TestPackage package)
        {
            // Target Runtime must be specified by this point
            string runtimeSetting = package.GetSetting(EnginePackageSettings.TargetRuntimeFramework, "");
            Guard.OperationValid(runtimeSetting.Length > 0, "LaunchAgentProcess called with no runtime specified");

            var targetRuntime = RuntimeFramework.Parse(runtimeSetting);
            var agentId = Guid.NewGuid();
            string agencyUrl = targetRuntime.FrameworkName.Identifier == ".NETFramework" ? RemotingUrl : TcpEndPoint;
            var agentProcess = CreateAgentProcess(agentId, agencyUrl, package);

            agentProcess.Exited += (sender, e) => OnAgentExit((Process)sender);

            Console.WriteLine(agentProcess.StartInfo.FileName);
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

                if (_agentStore.IsAvailable(agentId, out var agent))
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

        /// <summary>
        /// Releases the test agent back to the supplier, which provided it.
        /// </summary>
        /// <param name="agent">An agent previously provided by a call to GetAgent.</param>
        /// <exception cref="InvalidOperationException">
        /// If agent was never provided by the factory or was previously released.
        /// </exception>
        /// <remarks>
        /// Disposing an agent also releases it. However, this should not
        /// normally be done by the client, but by the source that created
        /// the agent in the first place.
        /// </remarks>
        public void ReleaseAgent(ITestAgent agent)
        {
            Process process;

            if (_agentStore.IsAgentActive(agent.Id, out process))
                try
                {
                    log.Debug("Stopping remote agent");
                    agent.Stop();
                }
                catch (SocketException se)
                {
                    int exitCode;

                    try
                    {
                        exitCode = process.ExitCode;
                    }
                    catch (NotSupportedException)
                    {
                        exitCode = -17;
                    }

                    if (exitCode == 0)
                    {
                        log.Warning("Agent connection was forcibly closed. Exit code was 0, so agent shutdown OK");
                    }
                    else
                    {
                        var stopError = $"Agent connection was forcibly closed. Exit code was {exitCode}. {Environment.NewLine}{ExceptionHelper.BuildMessageAndStackTrace(se)}";
                        log.Error(stopError);

                        throw;
                    }
                }
                catch (Exception e)
                {
                    var stopError = "Failed to stop the remote agent." + Environment.NewLine + ExceptionHelper.BuildMessageAndStackTrace(e);
                    log.Error(stopError);
                }
        }

        #endregion

        #region ITestAgency Implementation

        public void Register(ITestAgent agent)
        {
            _agentStore.Register(agent);
        }

        #endregion

        #region IService Implementation

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
            _extensionService = ServiceContext.GetService<ExtensionService>();

            try
            {
                // Add plugable agents first, so they can override the builtins
                if (_extensionService != null)
                    foreach (IAgentLauncher launcher in _extensionService.GetExtensions<IAgentLauncher>())
                        _launchers.Add(launcher);

                _launchers.Add(new Net20AgentLauncher());
                _launchers.Add(new Net40AgentLauncher());
                _launchers.Add(new NetCore21AgentLauncher());
                _launchers.Add(new NetCore31AgentLauncher());
                _launchers.Add(new Net50AgentLauncher());

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

        #endregion

        private Process CreateAgentProcess(Guid agentId, string agencyUrl, TestPackage package)
        {
            // Check to see if a specific agent was selected
            string selectedAgentName = package.GetSetting(EnginePackageSettings.SelectedAgentName, "DEFAULT");

            foreach (var launcher in _launchers)
            {
                var launcherName = launcher.GetType().Name;
                log.Debug($"Examining launcher {launcherName}");

                if (launcherName == selectedAgentName || selectedAgentName == "DEFAULT" && launcher.CanCreateProcess(package))
                {
                    log.Info($"Selected launcher {launcherName}");
                    return launcher.CreateProcess(agentId, agencyUrl, package);
                }
            }

            throw new NUnitEngineException($"No agent available for TestPackage {package.Name}");
        }

        internal bool IsAgentProcessActive(Guid agentId, out Process process)
        {
            return _agentStore.IsAgentActive(agentId, out process);
        }

        internal void OnAgentExit(Process process)
        {
            _agentStore.MarkProcessTerminated(process);

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
    }
}
