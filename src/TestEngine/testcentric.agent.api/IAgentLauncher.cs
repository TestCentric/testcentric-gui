// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if NETFRAMEWORK
using System;
using System.Diagnostics;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace TestCentric.Engine.Services
{
    [TypeExtensionPoint(
        Description = "Launches an Agent Process for supported target runtimes")]
    public interface IAgentLauncher
    {
        bool CanCreateProcess(TestPackage package);
        Process CreateProcess(Guid agentId, string agencyUrl, TestPackage package);
    }
}
#endif
