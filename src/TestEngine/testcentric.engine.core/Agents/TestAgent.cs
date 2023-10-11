// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Threading;

namespace TestCentric.Engine.Agents
{
    /// <summary>
    /// Abstract base for all types of TestAgents.
    /// A TestAgent provides services of locating,
    /// loading and running tests in a particular
    /// context such as an application domain or process.
    /// </summary>
    public abstract class TestAgent : ITestAgent, IDisposable
    {
        internal readonly ManualResetEvent StopSignal = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAgent"/> class.
        /// </summary>
        /// <param name="agentId">The identifier of the agent.</param>
        /// <param name="services">The services available to the agent.</param>
        public TestAgent(Guid agentId)
        {
            Id = agentId;
        }

        /// <summary>
        /// Gets a Guid that uniquely identifies this agent.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Starts the agent, performing any required initialization
        /// </summary>
        /// <returns><c>true</c> if the agent was started successfully.</returns>
        public abstract bool Start();

        /// <summary>
        /// Stops the agent, releasing any resources
        /// </summary>
        public virtual void Stop()
        {
            // Override if any action is needed
        }

        /// <summary>
        ///  Creates a test runner for a TestPackage
        /// </summary>
        /// <param name="package">The TestPackage for which a runner is to be created</param>
        /// <returns>An ITestEngineRunner</returns>
        public abstract ITestEngineRunner CreateRunner(TestPackage package);

        /// <summary>
        /// Wait for the agent to complete it's work and stop
        /// </summary>
        /// <remarks>
        /// This is not called by the engine but only by our pluggable agents.
        /// </remarks>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitForStop(int timeout)
        {
            return StopSignal.WaitOne(timeout);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private bool _disposed = false;

        /// <summary>
        /// Dispose is overridden to stop the agent
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Stop();

                _disposed = true;
            }
        }
    }
}
