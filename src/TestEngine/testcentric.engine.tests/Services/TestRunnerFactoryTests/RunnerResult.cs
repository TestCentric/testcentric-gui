// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests
{
    public class RunnerResult
    {
#if !NETCOREAPP
        public static RunnerResult ProcessRunner => new RunnerResult { TestRunner = typeof(ProcessRunner) };

        public static RunnerResult MultipleProcessRunner(int numAssemblies)
        {
            return new RunnerResult()
            {
                TestRunner = typeof(MultipleTestProcessRunner),
                SubRunners = GetSubRunners(ProcessRunner, numAssemblies)
            };
        }
#endif

        public static RunnerResult LocalTestRunner => new RunnerResult { TestRunner = typeof(LocalTestRunner) };

        public static RunnerResult AggregatingTestRunner(int numAssemblies)
        {
            return new RunnerResult()
            {
                TestRunner = typeof(AggregatingTestRunner),
                SubRunners = GetSubRunners(RunnerResult.LocalTestRunner, numAssemblies)
            };
        }

        public Type TestRunner { get; set; }

        public ICollection<RunnerResult> SubRunners { get; set; } = new List<RunnerResult>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"TestRunner: {TestRunner.Name}");

            if (SubRunners.Count == 0)
                return sb.ToString().Trim();

            sb.AppendLine("SubRunners:");
            sb.AppendLine("[");

            foreach (var subRunner in SubRunners)
            {
                sb.AppendLine($"\t{subRunner}");
            }
            sb.AppendLine("]");
            return sb.ToString().Trim();
        }
        private static RunnerResult[] GetSubRunners(RunnerResult subRunner, int count)
        {
            var subRunners = new RunnerResult[count];
            for (int i = 0; i < count; i++)
                subRunners[i] = subRunner;

            return subRunners;
        }
    }
}
