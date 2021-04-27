// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using System;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Engine.Services;

namespace TestCentric.Engine.Runners
{
    public class AggregatingTestRunnerTests
    {
        [TestCase(1, null, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(3, null, 3)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 2, 2)]
        [TestCase(3, 3, 3)]
        [TestCase(20, 8, 8)]
        [TestCase(8, 20, 8)]
        public void CheckLevelOfParallelism_ListOfAssemblies(int assemblyCount, int? maxAgents, int expected)
        {
            if (maxAgents == null)
                expected = Math.Min(assemblyCount, Environment.ProcessorCount);

            var runner = CreateRunner(assemblyCount, maxAgents);
            Assert.That(runner, Has.Property(nameof(AggregatingTestRunner.LevelOfParallelism)).EqualTo(expected));
        }

        [Test]
        public void CheckLevelOfParallelism_SingleAssembly()
        {
            var package = new TestPackage("junk.dll");
            Assert.That(new AggregatingTestRunner(new ServiceContext(), package).LevelOfParallelism, Is.EqualTo(0));
        }

        // Create a MultipleTestProcessRunner with a fake package consisting of
        // some number of assemblies and with an optional MaxAgents setting.
        // Zero means that MaxAgents is not specified.
        AggregatingTestRunner CreateRunner(int assemblyCount, int? maxAgents)
        {
            // Currently, we can get away with null entries here
            var package = new TestPackage(new string[assemblyCount]);
            if (maxAgents != null)
                package.Settings[EnginePackageSettings.MaxAgents] = maxAgents;
            return new AggregatingTestRunner(new ServiceContext(), package);
        }
    }
}
#endif
