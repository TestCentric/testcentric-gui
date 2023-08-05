// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Components.DictionaryAdapter;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Engine.Communication.Messages;
using TestCentric.Engine.Communication.Protocols;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Communication.Messages
{
    public class MessageTests
    {
        const string EMPTY_FILTER = "</filter>";
        static readonly string TEST_PACKAGE = new TestPackage("mock-assembly.dll").ToXml();

        private BinarySerializationProtocol _wireProtocol = new BinarySerializationProtocol();

        static TestCaseData[] MessageTestData = new TestCaseData[]
        {
            new TestCaseData("StartAgent", null),
            new TestCaseData("StopAgent", null),
            new TestCaseData("CreateRunner", TEST_PACKAGE),
            new TestCaseData("Load", null),
            new TestCaseData("Reload", null),
            new TestCaseData("Unload", null),
            new TestCaseData("Explore", EMPTY_FILTER),
            new TestCaseData("CountTestCases", EMPTY_FILTER),
            new TestCaseData("Run", EMPTY_FILTER),
            new TestCaseData("RunAsync", EMPTY_FILTER),
            new TestCaseData("RequestStop", null),
            new TestCaseData("ForcedStop", null)
        };

        [TestCaseSource(nameof(MessageTestData))]
        public void CommandMessageConstructionTests(string commandName, string argument)
        {
            var cmd = new CommandMessage(commandName, argument);
            Assert.That(cmd.MessageType, Is.EqualTo("CMND"));
            Assert.That(cmd.CommandName, Is.EqualTo(commandName));
            Assert.That(cmd.Argument, Is.EqualTo(argument));
        }

        [TestCaseSource(nameof(MessageTestData))]
        public void CommandMessageEncodingTests(string commandName, string argument)
        {
            var cmd = new CommandMessage(commandName, argument);

            var bytes = _wireProtocol.Encode(cmd);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0] as CommandMessage;
            Assert.That(decoded.MessageType, Is.EqualTo("CMND"));
            Assert.That(decoded.CommandName, Is.EqualTo(commandName));
            Assert.That(decoded.Argument, Is.EqualTo(argument));
        }

        [Test]
        public void ProgressMessageTest()
        {
            var msg = new TestEngineMessage("PROG", "PROGRESS");
            Assert.That(msg.MessageType, Is.EqualTo("PROG"));
            Assert.That(msg.MessageData, Is.EqualTo("PROGRESS"));
            var bytes = _wireProtocol.Encode(msg);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0];
            Assert.That(decoded.MessageType, Is.EqualTo("PROG"));
            Assert.That(decoded.MessageData, Is.EqualTo("PROGRESS"));
        }

        [Test]
        public void CommandReturnMessageTest()
        {
            var msg = new CommandReturnMessage("RESULT");
            Assert.That(msg.MessageType, Is.EqualTo("RSLT"));
            Assert.That(msg.ReturnValue, Is.EqualTo("RESULT"));
            var bytes = _wireProtocol.Encode(msg);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0] as CommandReturnMessage;
            Assert.That(decoded.MessageType, Is.EqualTo("RSLT"));
            Assert.That(decoded.ReturnValue, Is.EqualTo("RESULT"));
        }
    }
}
