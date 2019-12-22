// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine.Internal;
using NUnit.Engine.Runners;

namespace NUnit.Engine.Services.TestRunnerFactoryTests.Results
{
#if !NETCOREAPP
    internal static class Net20ExpectedRunnerResults
    {
        private static readonly string ExceptionMessage =
            $"No expected Test result provided for this {nameof(ProcessModel)}/{nameof(DomainUsage)} combination.";

        public static RunnerResult ResultFor(ProcessModel processModel, DomainUsage domainUsage, int numAssemblies)
        {
            switch (processModel)
            {
                case ProcessModel.Default:
                case ProcessModel.Multiple:
                    switch (domainUsage)
                    {
                        case DomainUsage.Default:
                        case DomainUsage.None:
                        case DomainUsage.Single:
                        case DomainUsage.Multiple:
                            return numAssemblies > 1
                                ? new RunnerResult()
                                {
                                    TestRunner = typeof(MultipleTestProcessRunner),
                                    SubRunners = GetSubRunners(RunnerResult.ProcessRunner, numAssemblies)
                                }
                                : RunnerResult.ProcessRunner;
                    }
                    break;
                case ProcessModel.InProcess:
                    switch (domainUsage)
                    {
                        case DomainUsage.Default:
                        case DomainUsage.Multiple:
                            return numAssemblies > 1
                                ? new RunnerResult
                                {
                                    TestRunner = typeof(MultipleTestDomainRunner),
                                    SubRunners = GetSubRunners(RunnerResult.TestDomainRunner, numAssemblies)
                                }
                                : RunnerResult.TestDomainRunner;
                        case DomainUsage.None:
                            return RunnerResult.LocalTestRunner;
                        case DomainUsage.Single:
                            return RunnerResult.TestDomainRunner;
                    }
                    break;
                case ProcessModel.Separate:
                    switch (domainUsage)
                    {
                        case DomainUsage.Default:
                        case DomainUsage.None:
                        case DomainUsage.Single:
                        case DomainUsage.Multiple:
                            return RunnerResult.ProcessRunner;
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(domainUsage), domainUsage, ExceptionMessage);
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
