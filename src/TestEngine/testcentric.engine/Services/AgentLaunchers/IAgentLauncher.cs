// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Diagnostics;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public interface IAgentLauncher
    {
        bool CanCreateProcess(TestPackage package);
        Process CreateProcess(Guid agentId, TestAgency agency, TestPackage package);
    }
}
#endif
