// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public interface ITestAgentFactory
    {
        ITestAgent GetAgent(TestPackage package);
        bool IsAgentActive(Guid agentId);
    }
}
