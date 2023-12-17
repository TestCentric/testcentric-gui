// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using TestCentric.Engine.Communication.Messages;
using TestCentric.Engine.TestUtilities;

namespace TestCentric.Engine.Communication.Protocols
{
    public class BinarySerializationProtocolTests
    {
        private BinarySerializationProtocol wireProtocol = new BinarySerializationProtocol();

        [Test]
        public void WriteAndReadBytes()
        {
            int SIZE = 1024;

            var bytes = new byte[SIZE];
            new Random().NextBytes(bytes);

            var stream = new MemoryStream();
            stream.Write(bytes, 0, SIZE);

            Assert.That(stream.Length, Is.EqualTo(SIZE));
            var copy = new byte[SIZE];

            stream.Position = 0;
            Assert.That(stream.Read(copy, 0, SIZE), Is.EqualTo(SIZE));

            Assert.That(copy, Is.EqualTo(bytes));
        }

        [Test]
        public void DecodeSingleMessage()
        {
            var originalPackage = new TestPackage("mock-assembly.dll", "notest-assembly.dll");
            var originalMessage = new TestEngineMessage(MessageCode.CommandResult, originalPackage.ToXml());

            var bytes = wireProtocol.Encode(originalMessage);
            Console.WriteLine($"Serialized {bytes.Length} bytes.");

            var messages = new List<TestEngineMessage>(wireProtocol.Decode(bytes));
            Assert.That(messages.Count, Is.EqualTo(1));
            var message = messages[0];

            Assert.That(message.Code, Is.EqualTo(MessageCode.CommandResult));
            Assert.That(message.Data, Is.EqualTo(originalPackage.ToXml()));
            var newPackage = DeserializePackage(message.Data);
            ComparePackages(newPackage, originalPackage);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void DecodeSplitMessages(int numMessages)
        {
            const int SPLIT_SIZE = 1000;

            var originalPackage = new TestPackage("mock-assembly.dll", "notest-assembly.dll");
            var originalMessage = new TestEngineMessage(MessageCode.CommandResult, originalPackage.ToXml());

            var msgBytes = wireProtocol.Encode(originalMessage);
            var msgLength = msgBytes.Length;
            var allBytes = new byte[msgLength * numMessages];
            for (int i = 0; i < numMessages; i++)
                Array.Copy(msgBytes, 0, allBytes, i * msgLength, msgLength);

            Console.WriteLine($"Serialized {numMessages} messages in {allBytes.Length} bytes.");

            var messages = new List<TestEngineMessage>();

            for (int index = 0; index < allBytes.Length; index += SPLIT_SIZE)
            {
                var bytesToSend = Math.Min(allBytes.Length - index, SPLIT_SIZE);
                var buffer = new byte[bytesToSend];
                Array.Copy(allBytes, index, buffer, 0, bytesToSend);
                messages.AddRange(wireProtocol.Decode(buffer));
                Console.WriteLine($"Decoded {bytesToSend} bytes, message count is now {messages.Count}");
                var expectedCount = (index + bytesToSend) / msgLength;
                Assert.That(messages.Count, Is.EqualTo(expectedCount));
            }

            foreach (TestEngineMessage message in messages)
            {
                Assert.That(message.Code, Is.EqualTo(MessageCode.CommandResult));
                var newPackage = DeserializePackage(message.Data);
                ComparePackages(newPackage, originalPackage);
            }
        }

        //[Test]
        public void DecodeMultipleMessages()
        {
            var commands = new string[] { "CMD1", "CMD2", "CMD3", "CMD4", "CMD5", "CMD6" };

            var stream = new MemoryStream();

            foreach (var command in commands)
            {
                var buffer = wireProtocol.Encode(new TestEngineMessage(command, null));
                stream.Write(buffer, 0, buffer.Length);
            }

            var received = new List<TestEngineMessage>(wireProtocol.Decode(stream.ToArray()));
            Assert.That(received.Count, Is.EqualTo(commands.Length));

            for (int i = 0; i < commands.Length; i++)
                Assert.That(received[i].CommandName, Is.EqualTo(commands[i]));
        }

        //[Test]
        public void WriteAndReadTestPackageAsXml()
        {
            var originalPackage = new TestPackage(new string[] { "mock-assembly.dll", "notest-assembly.dll" });

            Stream stream = new MemoryStream();
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TestPackage));
            serializer.Serialize(stream, originalPackage);
            Console.WriteLine($"Serialized {stream.Length} bytes to memory stream.");

            stream.Position = 0;
            object newPackage = serializer.Deserialize(stream);

            Assert.That(newPackage, Is.TypeOf<TestPackage>());
            ComparePackages((TestPackage)newPackage, originalPackage);
        }

        private TestPackage DeserializePackage(string xml)
        {
            var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(TestPackage));
            return serializer.Deserialize(reader) as TestPackage;
        }

        private void ComparePackages(TestPackage newPackage, TestPackage oldPackage)
        {
            Assert.That(newPackage.Name, Is.EqualTo(oldPackage.Name));
            Assert.That(newPackage.FullName, Is.EqualTo(oldPackage.FullName));
            Assert.That(newPackage.Settings.Count, Is.EqualTo(oldPackage.Settings.Count));
            Assert.That(newPackage.SubPackages.Count, Is.EqualTo(oldPackage.SubPackages.Count));

            foreach (var key in oldPackage.Settings.Keys)
            {
                Assert.That(newPackage.Settings.ContainsKey(key));
                Assert.That(newPackage.Settings[key], Is.EqualTo(oldPackage.Settings[key]));
            }

            for (int i = 0; i < oldPackage.SubPackages.Count; i++)
                ComparePackages(newPackage.SubPackages[i], oldPackage.SubPackages[i]);
        }
    }
}
