// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            new TestCaseData(MessageType.CreateRunner, TEST_PACKAGE),
            new TestCaseData(MessageType.LoadCommand, null),
            new TestCaseData(MessageType.ReloadCommand, null),
            new TestCaseData(MessageType.UnloadCommand, null),
            new TestCaseData(MessageType.ExploreCommand, EMPTY_FILTER),
            new TestCaseData(MessageType.CountCasesCommand, EMPTY_FILTER),
            new TestCaseData(MessageType.RunCommand, EMPTY_FILTER),
            new TestCaseData(MessageType.RunAsyncCommand, EMPTY_FILTER),
            new TestCaseData(MessageType.RequestStopCommand, null),
            new TestCaseData(MessageType.ForcedStopCommand, null)
        };

        [TestCaseSource(nameof(MessageTestData))]
        public void CommandMessageConstructionTests(string commandName, string argument)
        {
            var cmd = new TestEngineMessage(commandName, argument);
            Assert.That(cmd.Type, Is.EqualTo(commandName));
            Assert.That(cmd.CommandName, Is.EqualTo(commandName));
            Assert.That(cmd.Argument, Is.EqualTo(argument));
        }

        [TestCaseSource(nameof(MessageTestData))]
        public void CommandMessageEncodingTests(string commandName, string argument)
        {
            var cmd = new TestEngineMessage(commandName, argument);

            var bytes = _wireProtocol.Encode(cmd);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0];
            Assert.That(decoded.Type, Is.EqualTo(commandName));
            Assert.That(decoded.CommandName, Is.EqualTo(commandName));
            Assert.That(decoded.Argument, Is.EqualTo(argument));
        }

        [Test]
        public void ProgressMessageTest()
        {
            const string REPORT = "Progress report";
            var msg = new TestEngineMessage(MessageType.ProgressReport, REPORT);
            Assert.That(msg.Type, Is.EqualTo(MessageType.ProgressReport));
            Assert.That(msg.Data, Is.EqualTo(REPORT));
            var bytes = _wireProtocol.Encode(msg);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0];
            Assert.That(decoded.Type, Is.EqualTo(MessageType.ProgressReport));
            Assert.That(decoded.Data, Is.EqualTo(REPORT));
        }

        [Test]
        public void CommandReturnMessageTest()
        {
            const string RESULT = "Result text";
            var msg = new TestEngineMessage(MessageType.CommandResult, RESULT);
            Assert.That(msg.Type, Is.EqualTo(MessageType.CommandResult));
            Assert.That(msg.Data, Is.EqualTo(RESULT));
            var bytes = _wireProtocol.Encode(msg);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0];
            Assert.That(decoded.Type, Is.EqualTo(MessageType.CommandResult));
            Assert.That(decoded.Data, Is.EqualTo(RESULT));
        }
    }
}
