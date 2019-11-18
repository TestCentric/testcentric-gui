// ***********************************************************************
// Copyright (c) 2019 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.Engine.Internal;
using NUnit.Engine.Runners;

namespace NUnit.Engine.Tests.Services.TestRunnerFactoryTests.Results
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