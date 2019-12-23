// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCOREAPP1_1 && !NETCOREAPP2_1
using System.Diagnostics;
using TestCentric.Engine.TestUtilities;
using NUnit.Framework;

namespace TestCentric.Engine.Integration
{
    public sealed class RemoteAgentTests : IntegrationTests
    {
        [Test, Ignore("Won't work with ShadowCopy")]
        public void Explore_does_not_throw_SocketException()
        {
            using (var runner = new RunnerInDirectoryWithoutFramework())
            using (var test = new MockAssemblyInDirectoryWithFramework())
            {
                for (var times = 0; times < 3; times++)
                {
                    var result = ProcessRunner.Run(new ProcessStartInfo(runner.ConsoleExe, $"--explore \"{test.MockAssemblyDll}\""));
                    Assert.That(result.StandardStreamData, Has.None.With.Property("Data").Contains("System.Net.Sockets.SocketException"));
                    Assert.That(result.StandardStreamData, Has.None.With.Property("IsError").True);
                    Assert.That(result, Has.Property("ExitCode").Zero);
                }
            }
        }
    }
}
#endif
