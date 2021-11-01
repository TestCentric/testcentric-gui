// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;
using TestCentric.Engine.Agents;

namespace TestCentric.Engine.Communication.Transports
{
    public interface ITestAgentTransport : ITransport
    {
        TestAgent Agent { get; }
        ITestEngineRunner CreateRunner(TestPackage package);
    }
}
