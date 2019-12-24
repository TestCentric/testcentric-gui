// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Runners;
using TestCentric.Engine.Services.TestRunnerFactoryTests.Results;
using NUnit.Framework;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests.TestCases
{
#if !NETCOREAPP
    internal static class Net20AssemblyTestCases
    {
        private static readonly string ExceptionMessage =
            $"No expected Test result provided for this {nameof(ProcessModel)}/{nameof(DomainUsage)} combination.";

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var processModel in Enum.GetValues(typeof(ProcessModel)).Cast<ProcessModel>())
                {
                    foreach (var domainUsage in Enum.GetValues(typeof(DomainUsage)).Cast<DomainUsage>())
                    {
                        yield return SingleAssemblyStringCtorTest(processModel, domainUsage);
                        yield return SingleAssemblyListCtorTest(processModel, domainUsage);
                        yield return SingleUnknownExtensionTest(processModel, domainUsage);
                        yield return TwoAssembliesTest(processModel, domainUsage);
                        //yield return TwoUnknownsTest(processModel, domainUsage);
                    }
                }
            }
        }

        private static TestCaseData SingleAssemblyStringCtorTest(ProcessModel processModel, DomainUsage domainUsage)
        {
            var testName = "Single assembly (string ctor) - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                           $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

            var package = TestPackageFactory.OneAssemblyStringCtor();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 1);
            var testCase = new TestCaseData(package, expected).SetName($"{{m}}({testName})");
            return testCase;
        }

        private static TestCaseData SingleAssemblyListCtorTest(ProcessModel processModel, DomainUsage domainUsage)
        {
            var testName = "Single assembly (list ctor) - " +
                              $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                              $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

            var package = TestPackageFactory.OneAssemblyListCtor();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 1);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData SingleUnknownExtensionTest(ProcessModel processModel, DomainUsage domainUsage)
        {
            var testName = "Single unknown - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                           $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

            var package = TestPackageFactory.OneUnknownExtension();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 1);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData TwoAssembliesTest(ProcessModel processModel, DomainUsage domainUsage)
        {
            var testName = "Two assemblies - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                           $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

            var package = TestPackageFactory.TwoAssemblies();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 2);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData TwoUnknownsTest(ProcessModel processModel, DomainUsage domainUsage)
        {
            var testName = "Two unknown extensions - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                           $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

            var package = TestPackageFactory.TwoUnknownExtension();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 2);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }
    }
#endif
}
