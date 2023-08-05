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
            return CommandResult<bool>();
        }

        public void Stop()
        {
            SendCommandMessage("Stop");
        }

        public TestEngineResult Load()
        {
            SendCommandMessage("Load");
            return CommandResult<TestEngineResult>();
        }

        public void Unload()
        {
            SendCommandMessage("Unload");
        }

        public TestEngineResult Reload()
        {
            SendCommandMessage("Reload");
            return CommandResult<TestEngineResult>();
        }

        public int CountTestCases(TestFilter filter)
        {
            SendCommandMessage("CountTestCases", filter.Text);
            return CommandResult<int>();
        }

        public TestEngineResult Run(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage("Run", filter.Text);

            return TestRunResult(listener);
        }

        public AsyncTestEngineResult RunAsync(ITestEventListener listener, TestFilter filter)
        {
            SendCommandMessage("RunAsync", filter.Text);
            // TODO: Should we get the async result from the agent or just use our own?
            return CommandResult<AsyncTestEngineResult>();
            //return new AsyncTestEngineResult();
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
            return CommandResult<TestEngineResult>();
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

        private T CommandResult<T>()
        {
            log.Debug("Waiting for command result");
            return (T)new SocketReader(_socket, _wireProtocol).GetNextMessage<CommandReturnMessage>().ReturnValue;
        }

        // Return the result of a test run as a TestEngineResult. ProgressMessages
        // preceding the final CommandReturnMessage are handled as well.
        private TestEngineResult TestRunResult(ITestEventListener listener)
        {
            var rdr = new SocketReader(_socket, _wireProtocol);
            while (true)
            {
                var receivedMessage = rdr.GetNextMessage();
                var receivedType = receivedMessage.GetType();

                var returnMessage = receivedMessage as CommandReturnMessage;
                if (returnMessage != null)
                    return (TestEngineResult)returnMessage.ReturnValue;

                if (receivedMessage.MessageType != "PROG")
                //var progressMessage = receivedMessage as ProgressMessage;
                //if (progressMessage == null)
                    throw new InvalidOperationException($"Expected either a ProgressMessage or a CommandReturnMessage but received a {receivedType}");

                listener.OnTestEvent(receivedMessage.MessageData);
            }
        }
    }
}
