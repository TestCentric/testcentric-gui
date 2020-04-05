// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace TestCentric.Engine.Communication.Transports.Tcp
{
    public class TcpServerTests
    {
        private TcpServer _server;
        private List<Socket> _serverConnections;

        [SetUp]
        public void StartServer()
        {
            _serverConnections = new List<Socket>();
            _server = new TcpServer();
            _server.ClientConnected += (c, g) => _serverConnections.Add(c);
            _server.Start();
        }

        [TearDown]
        public void StopServer()
        {
            _server.Stop();
        }

        [Test]
        public void SingleClientConnection()
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(_server.EndPoint);
                client.Client.Send(new Guid().ToByteArray());

                Assert.That(_serverConnections.Count, Is.EqualTo(1), "Should have received 1 connection event");
                Assert.That(_serverConnections[0].Connected, "Server is not connected to client");

                Assert.True(client.Connected, "Client is not connectedToServer");
            }
        }

        [Test]
        public void SingleClientSend()
        {
            using (TcpClient client = new TcpClient())
            {
                byte[] buffer = new byte[256];
                new Random().NextBytes(buffer);

                client.Connect(_server.EndPoint);
                client.GetStream().Write(buffer, 0, 256);

                //byte[] bytes = new byte[256];
            }
        }

        [Test]
        public void MultipleClientConnections()
        {
            TcpClient[] clients = new[] { new TcpClient(), new TcpClient(), new TcpClient() };
            int num = clients.Length;

            foreach (var client in clients)
            {
                client.Connect(_server.EndPoint);
                client.Client.Send(new Guid().ToByteArray());
            }

            Assert.That(_serverConnections.Count, Is.EqualTo(num), $"Should have received {num} connection events");

            for (int i = 0; i < num; i++)
            {
                Assert.That(_serverConnections[i].Connected, $"Server is not connected to client {i+1}");
                Assert.True(clients[i].Connected, $"Client {i+1} is not connected to server");
            }
        }
    }
}
