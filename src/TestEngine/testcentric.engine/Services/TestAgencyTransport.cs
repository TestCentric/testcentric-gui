// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD2_0
using System;
using System.Diagnostics;
using System.IO;
using NUnit.Engine;
using TestCentric.Engine.Helpers;

namespace TestCentric.Engine.Services
{
    public abstract class TestAgencyTransport : MarshalByRefObject, ITestAgency, IDisposable
    {
        protected TestAgency _agency;

        public TestAgencyTransport(TestAgency agency)
        {
            _agency = agency;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract Process LaunchAgentProcess(TestPackage package, Guid agentId);

        public void Register(ITestAgent agent)
        {
            _agency.Register(agent);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Stop();

                _disposed = true;
            }
        }

        protected static string GetTestAgentExePath(RuntimeFramework targetRuntime, bool requires32Bit)
        {
            string engineDir = NUnitConfiguration.EngineDirectory;
            if (engineDir == null) return null;

            string agentName = requires32Bit
                ? "testcentric-agent-x86.exe"
                : "testcentric-agent.exe";

            switch (targetRuntime.Runtime.FrameworkIdentifier)
            {
                case ".NETFramework":
                    return Path.Combine(engineDir, "agents/net20/" + agentName);
                default:
                    throw new InvalidOperationException($"Unsupported runtime type: {targetRuntime.Runtime.FrameworkIdentifier}");
            }
        }

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
