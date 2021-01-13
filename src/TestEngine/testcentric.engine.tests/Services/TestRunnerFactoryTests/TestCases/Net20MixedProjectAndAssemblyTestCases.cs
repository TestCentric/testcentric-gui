// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Services.TestRunnerFactoryTests.Results;
using NUnit.Framework;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests.TestCases
{
#if !NETCOREAPP
    internal class Net20MixedProjectAndAssemblyTestCases
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var processModel in Enum.GetValues(typeof(ProcessModel)).Cast<ProcessModel>())
                {
                    var testName = "One project, one assembly - " +
                                    $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    var package = TestPackageFactory.OneProjectOneAssembly();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    var expected =
                        Net20ExpectedRunnerResults.ResultFor(processModel, 3);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                    testName = "Two projects, one assembly - " +
                                $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    package = TestPackageFactory.TwoProjectsOneAssembly();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    expected = Net20ExpectedRunnerResults.ResultFor(processModel, 5);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                    testName = "Two assemblies, one project - " +
                                $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    package = TestPackageFactory.TwoAssembliesOneProject();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    expected = Net20ExpectedRunnerResults.ResultFor(processModel, 4);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                    testName = "One assembly, one project, one unknown - " +
                                $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    package = TestPackageFactory.OneAssemblyOneProjectOneUnknown();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    expected = Net20ExpectedRunnerResults.ResultFor(processModel, 3);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
                }
            }
        }
    }
#endif
}
