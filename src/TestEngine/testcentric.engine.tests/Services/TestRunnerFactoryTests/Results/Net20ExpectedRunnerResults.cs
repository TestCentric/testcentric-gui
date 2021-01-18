// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests.Results
{
#if !NETCOREAPP
    internal static class Net20ExpectedRunnerResults
    {
        public static RunnerResult ResultFor(ProcessModel processModel, int numAssemblies)
        {
            switch (processModel)
            {
                default:
                case ProcessModel.Default:
                case ProcessModel.Multiple:
                    return numAssemblies > 1
                        ? new RunnerResult()
                        {
                            TestRunner = typeof(MultipleTestProcessRunner),
                            SubRunners = GetSubRunners(RunnerResult.ProcessRunner, numAssemblies)
                        }
                        : RunnerResult.ProcessRunner;
                case ProcessModel.Separate:
                    return RunnerResult.ProcessRunner;
            }
        }

        private static RunnerResult[] GetSubRunners(RunnerResult subRunner, int count)
        {
            var subRunners = new RunnerResult[count];
            for (int i = 0; i < count; i++)
                subRunners[i] = subRunner;

            return subRunners;
        }
    }
#endif
}
