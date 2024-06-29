// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Services.TestRunnerFactoryTests.Results;
using NUnit.Framework;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests.TestCases
{
#if NETFRAMEWORK
    internal static class Net20ProjectTestCases
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var processModel in Enum.GetValues(typeof(ProcessModel)).Cast<ProcessModel>())
                {
                    foreach (var domainUsage in Enum.GetValues(typeof(DomainUsage)).Cast<DomainUsage>())
                    {
                        var testName = "Single project (list ctor) - " +
                                       $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                                       $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

                        var package = TestPackageFactory.OneProjectListCtor();
                        package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
                        package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

                        var expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 2);
                        yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                        testName = "Single project (string ctor) - " +
                                       $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                                       $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

                        package = TestPackageFactory.OneProjectStringCtor();
                        package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
                        package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

                        expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 2);
                        yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                        testName = "Two projects - " +
                                       $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel} " +
                                       $"{nameof(EnginePackageSettings.DomainUsage)}:{domainUsage}";

                        package = TestPackageFactory.TwoProjects();
                        package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());
                        package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage.ToString());

                        expected = Net20ExpectedRunnerResults.ResultFor(processModel, domainUsage, 4);
                        yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
                    }
                }
            }
        }
    }
#endif
}
