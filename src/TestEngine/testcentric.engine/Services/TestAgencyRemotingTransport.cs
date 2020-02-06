// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using TestCentric.Common;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Summary description for TestAgencyRemotingTransport.
    /// </summary>
    public class TestAgencyRemotingTransport : TestAgencyTransport
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgencyRemotingTransport));

        private string _uri;
        private int _port;

        private TcpChannel _channel;
        private bool _isMarshalled;

        private object _theLock = new object();

        public TestAgencyRemotingTransport(TestAgency agency, string uri, int port)
            : base (agency)
        {
            _uri = uri;
            _port = port;
        }

        public string ServerUrl => string.Format("tcp://127.0.0.1:{0}/{1}", _port, _uri);

        public override void Start()
        {
            if (_uri != null && _uri != string.Empty)
            {
                lock (_theLock)
                {
                    _channel = TcpChannelUtils.GetTcpChannel(_uri + "Channel", _port, 100);

                    RemotingServices.Marshal(this, _uri);
                    _isMarshalled = true;
                }

                if (_port == 0)
                {
                    ChannelDataStore store = this._channel.ChannelData as ChannelDataStore;
                    if (store != null)
                    {
                        string channelUri = store.ChannelUris[0];
                        _port = int.Parse(channelUri.Substring(channelUri.LastIndexOf(':') + 1));
                    }
                }
            }
        }

        [System.Runtime.Remoting.Messaging.OneWay]
        public override void Stop()
        {
            lock( _theLock )
            {
                if ( this._isMarshalled )
                {
                    RemotingServices.Disconnect( this );
                    this._isMarshalled = false;
                }

                if ( this._channel != null )
                {
                    try
                    {
                        ChannelServices.UnregisterChannel(this._channel);
                        this._channel = null;
                    }
                    catch (RemotingException)
                    {
                        // Mono 4.4 appears to unregister the channel itself
                        // so don't do anything here.
                    }
                }

                Monitor.PulseAll( _theLock );
            }
        }

        public override Process LaunchAgentProcess(TestPackage package, Guid agentId)
        {
            // Get target runtime
            string runtimeSetting = package.GetSetting(EnginePackageSettings.RuntimeFramework, "");
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

            string agentExePath = GetTestAgentExePath(targetRuntime, useX86Agent);

            if (!File.Exists(agentExePath))
                throw new FileNotFoundException(
                    $"{Path.GetFileName(agentExePath)} could not be found.", agentExePath);

            log.Debug("Using testcentric-agent at " + agentExePath);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.EnableRaisingEvents = true;
            p.Exited += (sender, e) => _agency.OnAgentExit((Process)sender, agentId);
            string arglist = agentId.ToString() + " " + ServerUrl + " " + agentArgs;

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
                // Override the COMPLUS_Version env variable, this would cause CLR meta host to run a CLR of the specific version
                string envVar = "v" + targetRuntime.ClrVersion.ToString(3);
                p.StartInfo.EnvironmentVariables["COMPLUS_Version"] = envVar;
                // Leave a marker that we have changed this variable, so that the agent could restore it for any code or child processes running within the agent
                string cpvOriginal = Environment.GetEnvironmentVariable("COMPLUS_Version");
                p.StartInfo.EnvironmentVariables["TestAgency_COMPLUS_Version_Original"] = string.IsNullOrEmpty(cpvOriginal) ? "NULL" : cpvOriginal;
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

            return p;
        }

        public void WaitForStop()
        {
            lock( _theLock )
            {
                Monitor.Wait( _theLock );
            }
        }
    }
}
#endif
