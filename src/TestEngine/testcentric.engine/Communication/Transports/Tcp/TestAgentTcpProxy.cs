// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Net.Sockets;
using NUnit.Engine;
using TestCentric.Engine.Communication.Messages;
using TestCentric.Engine.Communication.Protocols;
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
            SendCommandMessage("CreateRunner", package.ToXml());

            // Agent also functions as the runner
            return this;
        }

        public bool Start()
        {
            SendCommandMessage("Start");
            return bool.Parse(GetCommandResult());
        }

        public void Stop()
        {
            SendCommandMessage("Stop");
        }

        public TestEngineResult Load()
        {
            SendCommandMessage("Load");
            return new TestEngineResult(GetCommandResult());
        }

        public void Unload()
        {
            SendCommandMessage("Unload");
        }

        public TestEngineResult Reload()
        {
            SendCommandMessage("Reload");
            return new TestEngineResult(GetCommandResult());
        }

        public int CountTestCases(TestFilter filter)
        {
            SendCommandMessage("CountTestCases", filter.Text);
            return int.Parse(GetCommandResult());
        }

        public TestEngineResult Run(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage("Run", filter.Text);

            return TestRunResult(listener);
        }

        public AsyncTestEngineResult RunAsync(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage("RunAsync", ((TestFilter)filter).Text);

            return new AsyncTestEngineResult();
        }

        public void RequestStop()
        {
            SendCommandMessage("RequestStop");
        }

        public void ForcedStop()
        {
            SendCommandMessage("ForcedStop");
        }

        public TestEngineResult Explore(TestFilter filter)
        {
            SendCommandMessage("Explore", filter.Text);
            return new TestEngineResult(GetCommandResult());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void SendCommandMessage(string command, string argument = null)
        {
            _socket.Send(_wireProtocol.Encode(new CommandMessage(command, argument)));
            log.Debug($"Sent {command} command");
        }

        private string GetCommandResult()
        {
            log.Debug("Waiting for command result");
            return new SocketReader(_socket, _wireProtocol).GetNextMessage().MessageData;
        }

        // Return the result of a test run as a TestEngineResult. ProgressMessages
        // preceding the final CommandReturnMessage are handled as well.
        private TestEngineResult TestRunResult(ITestEventListener listener)
        {
            var rdr = new SocketReader(_socket, _wireProtocol);
            while (true)
            {
                var message = rdr.GetNextMessage();

                switch(message.MessageType)
                {
                    case "RSLT":
                        return new TestEngineResult(message.MessageData);
                    case "PROG":
                        listener.OnTestEvent(message.MessageData);
                        break;
                    default:
                        throw new InvalidOperationException($"Expected either a ProgressMessage or a CommandReturnMessage but received a {message.MessageType} message");
                }
            }
        }
    }
}
