// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using System;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace NUnit.Engine.Internal
{
    /// <summary>
    /// Summary description for ServerBase.
    /// </summary>
    public abstract class ServerBase : MarshalByRefObject, IDisposable
    {
        protected string uri;
        protected int port;

        private TcpChannel channel;
        private bool isMarshalled;

        private object theLock = new object();

        protected ServerBase()
        {
        }

        protected ServerBase(string uri, int port)
        {
            this.uri = uri;
            this.port = port;
        }

        public string ServerUrl
        {
            get { return string.Format("tcp://127.0.0.1:{0}/{1}", port, uri); }
        }

        public virtual void Start()
        {
            if (uri != null && uri != string.Empty)
            {
                lock (theLock)
                {
                    this.channel = TcpChannelUtils.GetTcpChannel(uri + "Channel", port, 100);

                    RemotingServices.Marshal(this, uri);
                    this.isMarshalled = true;
                }

                if (this.port == 0)
                {
                    ChannelDataStore store = this.channel.ChannelData as ChannelDataStore;
                    if (store != null)
                    {
                        string channelUri = store.ChannelUris[0];
                        this.port = int.Parse(channelUri.Substring(channelUri.LastIndexOf(':') + 1));
                    }
                }
            }
        }

        [System.Runtime.Remoting.Messaging.OneWay]
        public virtual void Stop()
        {
            lock( theLock )
            {
                if ( this.isMarshalled )
                {
                    RemotingServices.Disconnect( this );
                    this.isMarshalled = false;
                }

                if ( this.channel != null )
                {
                    try
                    {
                        ChannelServices.UnregisterChannel(this.channel);
                        this.channel = null;
                    }
                    catch (RemotingException)
                    {
                        // Mono 4.4 appears to unregister the channel itself
                        // so don't do anything here.
                    }
                }

                Monitor.PulseAll( theLock );
            }
        }

        public void WaitForStop()
        {
            lock( theLock )
            {
                Monitor.Wait( theLock );
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Stop();

                _disposed = true;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
#endif
