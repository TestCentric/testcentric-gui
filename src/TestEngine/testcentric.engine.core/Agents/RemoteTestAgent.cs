// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
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
    /// <remarks>
    /// In the current implementation, we use remoting for agents targeting the .NET framework
    /// and our own TCP protocol for agents targeting .NET Core.
    /// </remarks>
    public class RemoteTestAgent : TestAgent
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(RemoteTestAgent));

        /// <summary>
        /// Construct a RemoteTestAgent.
        /// </summary>
        /// <remarks>
        /// The constructor is not referenced in the engine project because the engine doesn't
        /// create RemoteTestAgents. All agents are now pluggable, so only the plugins use this.
        /// </remarks>
        public RemoteTestAgent(Guid agentId)
            : base(agentId) { }

        public ITestAgentTransport Transport;

        public int ProcessId => System.Diagnostics.Process.GetCurrentProcess().Id;

        public override bool Start()
        {
            Guard.OperationValid(Transport != null, "Transport must be set before calling Start().");
            log.Debug("Starting");
            return Transport.Start();
        }

#if NETFRAMEWORK
        // For remoting, the agent stops the transport, but for TCP the transport is stopped directly
        public override void Stop()
        {
            log.Debug("Stopping");
            Transport.Stop();
        }
#endif

        public override ITestEngineRunner CreateRunner(TestPackage package)
        {
            Console.WriteLine("Creating the runner");
#if NETFRAMEWORK
            var runner = new TestDomainRunner(package);
#else
            var runner = new LocalTestRunner(package);
#endif
            log.Debug($"Returning {runner.GetType().Name}");
            return runner;
        }
    }
}
