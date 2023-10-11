// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Net.Sockets;
using TestCentric.Engine.Communication.Messages;
using TestCentric.Engine.Communication.Protocols;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Communication.Transports.Tcp
{
    /// <summary>
    /// TestAgentTcpProxy wraps a RemoteTestAgent so that certain
    /// of its properties may be accessed directly.
    /// </summary>
    internal class TestAgentTcpProxy : ITestAgent, ITestEngineRunner
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgentTcpProxy));

        private Socket _socket;
        private BinarySerializationProtocol _wireProtocol = new BinarySerializationProtocol();

        public TestAgentTcpProxy(Socket socket, Guid id)
        {
           _socket = socket;
            Id = id;
        }

        public Guid Id { get; }

        public ITestEngineRunner CreateRunner(TestPackage package)
        {
            SendCommandMessage(MessageCode.CreateRunner, package.ToXml());

            // Agent also functions as the runner
            return this;
        }

        public bool Start()
        {
            // Not used for TCP since agent must already be started
            // in order to receive any messges at all.
            throw new NotImplementedException("Not used for TCP Transport");
        }

        public void Stop() => SendCommandMessage(MessageCode.StopAgent);

        public TestEngineResult Load()
        {
            SendCommandMessage(MessageCode.LoadCommand);
            return new TestEngineResult(GetCommandResult());
        }

        public void Unload() => SendCommandMessage(MessageCode.UnloadCommand);

        public TestEngineResult Reload()
        {
            SendCommandMessage(MessageCode.ReloadCommand);
            return new TestEngineResult(GetCommandResult());
        }

        public int CountTestCases(TestFilter filter)
        {
            SendCommandMessage(MessageCode.CountCasesCommand, filter.Text);
            return int.Parse(GetCommandResult());
        }

        public TestEngineResult Run(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage(MessageCode.RunCommand, filter.Text);

            return TestRunResult(listener);
        }

        public AsyncTestEngineResult RunAsync(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage(MessageCode.RunAsyncCommand, ((TestFilter)filter).Text);

            return new AsyncTestEngineResult();
        }

        public void RequestStop() => SendCommandMessage(MessageCode.RequestStopCommand);

        public void ForcedStop() => SendCommandMessage(MessageCode.ForcedStopCommand);

        public TestEngineResult Explore(TestFilter filter)
        {
            SendCommandMessage(MessageCode.ExploreCommand, filter.Text);
            return new TestEngineResult(GetCommandResult());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void SendCommandMessage(string command, string argument = null)
        {
            _socket.Send(_wireProtocol.Encode(new TestEngineMessage(command, argument)));
            log.Debug($"Sent {command} command");
        }

        private string GetCommandResult()
        {
            log.Debug("Waiting for command result");
            return new SocketReader(_socket, _wireProtocol).GetNextMessage().Data;
        }

        // Return the result of a test run as a TestEngineResult. ProgressMessages
        // preceding the final CommandReturnMessage are handled as well.
        private TestEngineResult TestRunResult(ITestEventListener listener)
        {
            var rdr = new SocketReader(_socket, _wireProtocol);
            while (true)
            {
                var message = rdr.GetNextMessage();

                switch(message.Code)
                {
                    case MessageCode.CommandResult:
                        return new TestEngineResult(message.Data);
                    case MessageCode.ProgressReport:
                        listener.OnTestEvent(message.Data);
                        break;
                    default:
                        throw new InvalidOperationException($"Expected either a ProgressMessage or a CommandReturnMessage but received a {message.Code} message");
                }
            }
        }
    }
}
