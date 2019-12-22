// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Engine.Runners;

namespace NUnit.Engine.Services.TestRunnerFactoryTests
{
    public class RunnerResult
    {
#if !NETCOREAPP
        public static RunnerResult TestDomainRunner => new RunnerResult { TestRunner = typeof(TestDomainRunner) };
        public static RunnerResult ProcessRunner => new RunnerResult { TestRunner = typeof(ProcessRunner) };
#endif
        public static RunnerResult LocalTestRunner => new RunnerResult { TestRunner = typeof(LocalTestRunner) };

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
    }
}
