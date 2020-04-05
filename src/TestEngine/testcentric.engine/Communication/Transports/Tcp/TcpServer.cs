// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Communication.Transports.Tcp
{
    public class TcpServer
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgentTcpTransport));

        private const int GUID_BUFFER_SIZE = 16;

        TcpListener _listenerSocket;
        Thread _listenerThread;
        volatile bool _running;

        public delegate void ConnectionEventHandler(Socket clientSocket, Guid id);

        public event ConnectionEventHandler ClientConnected;

        public TcpServer(int port = 0)
        {
            _listenerSocket = new TcpListener(IPAddress.Loopback, port);
        }

        public IPEndPoint EndPoint => (IPEndPoint)_listenerSocket.LocalEndpoint;

        public void Start()
        {
            _listenerSocket.Start();
            _running = true;

            _listenerThread = new Thread(WaitForClientConnections);
            _listenerThread.Start();
        }

        public void Stop()
        {
            try
            {
                _running = false;
                _listenerSocket.Stop();
            }
            catch (Exception exception)
            {
                log.Error($"Failed to stop listener socket: {exception}");
            }
        }

        private void WaitForClientConnections()
        {
            while (_running)
            {
                try
                {
                    var clientSocket = _listenerSocket.AcceptSocket();
                    if (clientSocket.Connected)
                    {
                        // Upon connection, remote agent must immediately send its Id as identification.
                        // Guid is sent as a raw byte array, without any preceding length specified.
                        var buf = new byte[GUID_BUFFER_SIZE];
                        if (clientSocket.Receive(buf) == GUID_BUFFER_SIZE)
                        {
                            Guid id = new Guid(buf);
                            ClientConnected?.Invoke(clientSocket, id);
                        }
                    }
                }
                catch
                {
                    // Two possibilities:
                    //   1. We were trying to stop the socket
                    //   2. The connection was dropped due to some external event
                    // In either case, we stop the socket and wait a while
                    _listenerSocket.Stop();

                    // If we were trying to stop, that's all
                    if (!_running)
                        return;

                    // Otherwise, wait and try to restart it. An exception here is simply logged
                    Thread.Sleep(500);
                    try
                    {
                        _listenerSocket.Start();
                    }
                    catch (Exception exception)
                    {
                        log.Error($"Unable to restart listener socket: {exception}");
                    }
                }
            }
        }
    }
}
