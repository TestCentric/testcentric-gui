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
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var processModel in Enum.GetValues(typeof(ProcessModel)).Cast<ProcessModel>())
                {
                    yield return SingleAssemblyStringCtorTest(processModel);
                    yield return SingleAssemblyListCtorTest(processModel);
                    yield return SingleUnknownExtensionTest(processModel);
                    yield return TwoAssembliesTest(processModel);
                    //yield return TwoUnknownsTest(processModel);
                }
            }
        }

        private static TestCaseData SingleAssemblyStringCtorTest(ProcessModel processModel)
        {
            var testName = "Single assembly (string ctor) - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

            var package = TestPackageFactory.OneAssemblyStringCtor();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 1);
            var testCase = new TestCaseData(package, expected).SetName($"{{m}}({testName})");
            return testCase;
        }

        private static TestCaseData SingleAssemblyListCtorTest(ProcessModel processModel)
        {
            var testName = "Single assembly (list ctor) - " +
                              $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

            var package = TestPackageFactory.OneAssemblyListCtor();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 1);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData SingleUnknownExtensionTest(ProcessModel processModel)
        {
            var testName = "Single unknown - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

            var package = TestPackageFactory.OneUnknownExtension();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 1);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData TwoAssembliesTest(ProcessModel processModel)
        {
            var testName = "Two assemblies - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

            var package = TestPackageFactory.TwoAssemblies();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 2);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }

        private static TestCaseData TwoUnknownsTest(ProcessModel processModel)
        {
            var testName = "Two unknown extensions - " +
                           $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

            var package = TestPackageFactory.TwoUnknownExtension();
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

            var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 2);
            return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
        }
    }
#endif
}
