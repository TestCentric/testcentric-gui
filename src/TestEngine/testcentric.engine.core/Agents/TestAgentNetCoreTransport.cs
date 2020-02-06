// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETSTANDARD2_0
using System;

namespace TestCentric.Engine.Agents
{
    public class TestAgentNetCoreTransport : TestAgentTransport
    {
        public TestAgentNetCoreTransport(RemoteTestAgent agent) : base(agent) { }

        // TODO: Start and Stop essentially do nothing in .NET Core
        // at this point. They need to be implemented using an appropriate
        // communications protoc0l before we actually call the code.
        
        public override bool Start() { return true; }

        public override void Stop() { _agent.StopSignal.Set(); }

        public override ITestEngineRunner CreateRunner(TestPackage package)
        {
            return _agent.CreateRunner(package);
        }
    }
}
#endif
