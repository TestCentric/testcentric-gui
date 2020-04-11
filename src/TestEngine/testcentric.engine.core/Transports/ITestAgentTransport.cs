// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Engine;
using TestCentric.Engine.Agents;

namespace TestCentric.Engine.Transports
{
    public interface ITestAgentTransport
    {
        TestAgent Agent { get; }
        bool Start();
        void Stop();
        ITestEngineRunner CreateRunner(TestPackage package);
    }
}
