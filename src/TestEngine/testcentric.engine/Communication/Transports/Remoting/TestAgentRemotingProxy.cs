// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using NUnit.Engine;

namespace TestCentric.Engine.Communication.Transports.Remoting
{
    /// <summary>
    /// RemoteTestAgentProxy wraps a RemoteTestAgent so that certain
    /// of its properties may be accessed without remoting.
    /// </summary>
    internal class TestAgentRemotingProxy : ITestAgent
    {
        private ITestAgent _remoteAgent;

        public TestAgentRemotingProxy(ITestAgent remoteAgent, Guid id)
        {
            _remoteAgent = remoteAgent;

            Id = id;
        }

        public Guid Id { get; private set; }

        public ITestEngineRunner CreateRunner(TestPackage package)
        {
            return _remoteAgent.CreateRunner(package);
        }

        public bool Start()
        {
            return _remoteAgent.Start();
        }

        public void Stop()
        {
            _remoteAgent.Stop();
        }
    }
}
#endif
