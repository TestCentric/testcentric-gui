// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
using TestCentric.Engine.Agents;

namespace TestCentric.Engine.Communication.Transports.Tcp
{
    // Placeholder class representing future TCP transport.
    // At this point, we are assuming will work for all build
    // targets, but that could change.
    public class TestAgentTcpTransport : ITestAgentTransport
    {
        public TestAgentTcpTransport(RemoteTestAgent agent) { }

        // TODO: Start and Stop essentially do nothing at this point. 

        public TestAgent Agent { get; }

        public bool Start() { return true; }

        public void Stop() { Agent.StopSignal.Set(); }

        public ITestEngineRunner CreateRunner(TestPackage package)
        {
            return Agent.CreateRunner(package);
        }
    }
}
