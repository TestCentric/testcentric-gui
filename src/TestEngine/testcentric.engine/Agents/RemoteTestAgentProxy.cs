// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine.Agents
{
    /// <summary>
    /// RemoteTestAgentProxy wraps a RemoteTestAgent so that certain
    /// of its properties may be accessed without remoting.
    /// </summary>
    internal class RemoteTestAgentProxy : ITestAgent
    {
        private ITestAgent _remoteAgent;

        public RemoteTestAgentProxy(ITestAgent remoteAgent, Guid id)
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
