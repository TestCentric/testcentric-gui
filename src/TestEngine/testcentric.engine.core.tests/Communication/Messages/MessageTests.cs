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

namespace TestCentric.Engine.Communication.Messages
{
    public class MessageTests
    {
        static readonly object[] NO_ARGS = new object[0];

        const string EMPTY_FILTER = "</filter>";
        static readonly object[] FILTER_ARGS = new object[] { EMPTY_FILTER };

        static readonly TestPackage TEST_PACKAGE = new TestPackage("mock-assembly.dll");
        static readonly object[] PACKAGE_ARGS = new object[] { TEST_PACKAGE };

        private BinarySerializationProtocol _wireProtocol = new BinarySerializationProtocol();

        [TestCase("StartAgent")]
        [TestCase("StopAgent")]
        [TestCase("CreateRunner", "TESTPACKAGE")]
        [TestCase("Load")]
        [TestCase("Reload")]
        [TestCase("Unload")]
        [TestCase("Explore", EMPTY_FILTER)]
        [TestCase("CountTestCases", EMPTY_FILTER)]
        [TestCase("Run", EMPTY_FILTER)]
        [TestCase("RunAsync", EMPTY_FILTER)]
        [TestCase("StopRun", "FORCE")]
        public void CommandMessageTests(string commandName, object argument=null)
        {
            var cmd = new CommandMessage(commandName, argument);
            Assert.That(cmd.CommandName, Is.EqualTo(commandName));
            Assert.That(cmd.Argument, Is.EqualTo(argument));

            var bytes = _wireProtocol.Encode(cmd);
            var messages = new List<TestEngineMessage>(_wireProtocol.Decode(bytes));
            var decoded = messages[0] as CommandMessage;
            Assert.That(decoded.CommandName, Is.EqualTo(commandName));
            Assert.That(decoded.Argument, Is.EqualTo(argument));
        }
    }
}
