// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
using TestCentric.Common;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Communication.Transports;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Agents
{
    /// <summary>
    /// RemoteTestAgent represents a remote agent executing in another process
    /// and communicating with NUnit by TCP. Although it is similar to a
    /// TestServer, it does not publish a Uri at which clients may connect
    /// to it. Rather, it reports back to the sponsoring TestAgency upon
    /// startup so that the agency may in turn provide it to clients for use.
    /// </summary>
    public class RemoteTestAgent : TestAgent
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(RemoteTestAgent));

        /// <summary>
        /// Construct a RemoteTestAgent
        /// </summary>
        public RemoteTestAgent(Guid agentId)
            : base(agentId) { }

        public ITestAgentTransport Transport;

        public int ProcessId => System.Diagnostics.Process.GetCurrentProcess().Id;

        public override bool Start()
        {
            Guard.OperationValid(Transport != null, "Transport must be set before calling Start().");
            return Transport.Start();
        }

        public override void Stop()
        {
            Transport.Stop();
        }

        public override ITestEngineRunner CreateRunner(TestPackage package)
        {
#if NETFRAMEWORK
            return new TestDomainRunner(package);
#else
            return new LocalTestRunner(package);
#endif
        }
    }
}
