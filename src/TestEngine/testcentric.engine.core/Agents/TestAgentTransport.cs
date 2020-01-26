// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine.Agents
{
    public abstract class TestAgentTransport : MarshalByRefObject, ITestAgent
    {
        protected TestAgent _agent;

        public TestAgentTransport(TestAgent agent)
        {
            _agent = agent;
        }

        public Guid Id => _agent.Id;

        public abstract bool Start();

        public abstract void Stop();

        public abstract ITestEngineRunner CreateRunner(TestPackage package);

        /// <summary>
        /// Overridden to cause object to live indefinitely
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
#endif
