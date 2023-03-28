// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TestCentric.Engine.Agents;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Communication.Messages;
using TestCentric.Engine.Communication.Protocols;

namespace TestCentric.Engine.Communication.Transports.Tcp
{
    public class TestAgentTcpTransport : ITestAgentTransport, ITestEventListener
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgentTcpTransport));

        private string _agencyUrl;
        private Socket _clientSocket;
        private ITestEngineRunner _runner;

        public TestAgentTcpTransport(RemoteTestAgent agent, string serverUrl)
        {
            Guard.ArgumentNotNull(agent, nameof(agent));
            Agent = agent;

            Guard.ArgumentNotNullOrEmpty(serverUrl, nameof(serverUrl));
            _agencyUrl = serverUrl;

            var parts = serverUrl.Split(new char[] { ':' });
            Guard.ArgumentValid(parts.Length == 2, "Invalid server address specified. Must be a valid endpoint including the port number", nameof(serverUrl));
            ServerEndPoint = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
            log.Debug($"Using server EndPoint {ServerEndPoint}");
        }

        public TestAgent Agent { get; }

        public IPEndPoint ServerEndPoint { get; }

        public bool Start()
        {
            try
            {
                // Connect to the server
                log.Debug($"Connecting to TestAgency at {_agencyUrl}");
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.Connect(ServerEndPoint);
                log.Info($"Connected to TestAgency at {_clientSocket.RemoteEndPoint}");
            }
            catch(System.Exception ex)
            {
                log.Error(ex.ToString());
            }

            // Immediately upon connection send the agent Id as a raw byte array
            _clientSocket.Send(Agent.Id.ToByteArray());
            log.Info("Sent agent ID");

            // Start the loop that reads and executes commands
            Thread commandLoop = new Thread(CommandLoop);
            commandLoop.Start();

            return true;
        }

        public void Stop()
        {
            //Agent.StopSignal.Set();
        }

        public ITestEngineRunner CreateRunner(TestPackage package)
        {
            return Agent.CreateRunner(package);
        }

        private void CommandLoop()
        {
            bool keepRunning = true;
            var socketReader = new SocketReader(_clientSocket, new BinarySerializationProtocol());

            while (keepRunning)
            {
                log.Debug("Waiting for a command");
                var command = socketReader.GetNextMessage<CommandMessage>();
                log.Debug($"Received {command.CommandName} command");

                switch (command.CommandName)
                {
                    case "CreateRunner":
                        var package = (TestPackage)command.Arguments[0];
                        _runner = CreateRunner(package);
                        break;
                    case "Load":
                        SendResult(_runner.Load());
                        break;
                    case "Reload":
                        SendResult(_runner.Reload());
                        break;
                    case "Unload":
                        _runner.Unload();
                        break;
                    case "Explore":
                        var filter = (TestFilter)command.Arguments[0];
                        //log.Debug($"  Filter = {filter.Text}");
                        log.Debug("Calling Explore");
                        var result = _runner.Explore(filter);
                        log.Debug("Got Explore Result");
                        SendResult(result);
                        break;
                    case "CountTestCases":
                        filter = (TestFilter)command.Arguments[0];
                        log.Debug($"  Filter = {filter.Text}");
                        SendResult(_runner.CountTestCases(filter));
                        break;
                    case "Run":
                        filter = (TestFilter)command.Arguments[0];
                        log.Debug($"  Filter = {filter.Text}");
                        Thread runnerThread = new Thread(RunTests);
                        runnerThread.Start(filter);
                        break;

                    case "RunAsync":
                        filter = (TestFilter)command.Arguments[0];
                        log.Debug($"  Filter = {filter.Text}");
                        SendResult(_runner.RunAsync(this, filter));
                        break;

                    case "StopRun":
                        var force = (bool)command.Arguments[0];
                        _runner.StopRun(force);
                        break;

                    case "Stop":
                        keepRunning = false;
                        break;
                }
            }

            log.Info("Terminating command loop");
            Stop();
        }

        private void RunTests(object filter)
        {
            SendResult(_runner.Run(this, (TestFilter)filter));
        }

        private void SendResult(object result)
        {
            var resultMessage = new CommandReturnMessage(result);
            var bytes = new BinarySerializationProtocol().Encode(resultMessage);
            log.Debug($"Sending result {result.GetType().Name}, length={bytes.Length}");
            _clientSocket.Send(bytes);
        }

        public void OnTestEvent(string report)
        {
            var progressMessage = new ProgressMessage(report);
            var bytes = new BinarySerializationProtocol().Encode(progressMessage);
            _clientSocket.Send(bytes);
        }
    }
}
