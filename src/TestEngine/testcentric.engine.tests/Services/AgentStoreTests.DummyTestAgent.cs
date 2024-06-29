// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public partial class AgentStoreTests
    {
        private sealed class DummyTestAgent : ITestAgent
        {
            public DummyTestAgent(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }

            public ITestEngineRunner CreateRunner(TestPackage package)
            {
                throw new NotImplementedException();
            }

            public bool Start()
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new NotImplementedException();
            }
        }
    }
}
#endif
