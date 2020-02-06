// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using TestCentric.Common;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services.Transports
{
    /// <summary>
    /// Summary description for TestAgencyRemotingTransport.
    /// </summary>
    public class TestAgencyRemotingTransport : TestAgencyTransport
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(TestAgencyRemotingTransport));

        private string _uri;
        private int _port;

        private TcpChannel _channel;
        private bool _isMarshalled;

        private object _theLock = new object();

        public TestAgencyRemotingTransport(TestAgency agency, string uri, int port)
            : base (agency)
        {
            _uri = uri;
            _port = port;
        }

        public string ServerUrl => string.Format("tcp://127.0.0.1:{0}/{1}", _port, _uri);

        public override void Start()
        {
            if (_uri != null && _uri != string.Empty)
            {
                lock (_theLock)
                {
                    _channel = TcpChannelUtils.GetTcpChannel(_uri + "Channel", _port, 100);

                    RemotingServices.Marshal(this, _uri);
                    _isMarshalled = true;
                }

                if (_port == 0)
                {
                    ChannelDataStore store = this._channel.ChannelData as ChannelDataStore;
                    if (store != null)
                    {
                        string channelUri = store.ChannelUris[0];
                        _port = int.Parse(channelUri.Substring(channelUri.LastIndexOf(':') + 1));
                    }
                }
            }
        }

        [System.Runtime.Remoting.Messaging.OneWay]
        public override void Stop()
        {
            lock( _theLock )
            {
                if ( this._isMarshalled )
                {
                    RemotingServices.Disconnect( this );
                    this._isMarshalled = false;
                }

                if ( this._channel != null )
                {
                    try
                    {
                        ChannelServices.UnregisterChannel(this._channel);
                        this._channel = null;
                    }
                    catch (RemotingException)
                    {
                        // Mono 4.4 appears to unregister the channel itself
                        // so don't do anything here.
                    }
                }

                Monitor.PulseAll( _theLock );
            }
        }

        public void WaitForStop()
        {
            lock( _theLock )
            {
                Monitor.Wait( _theLock );
            }
        }
    }
}
#endif
