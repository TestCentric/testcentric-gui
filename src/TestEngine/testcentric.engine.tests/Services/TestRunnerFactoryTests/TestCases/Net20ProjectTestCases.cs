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
#if !NETCOREAPP
    internal static class Net20ProjectTestCases
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var processModel in Enum.GetValues(typeof(ProcessModel)).Cast<ProcessModel>())
                {
                    var testName = "Single project (list ctor) - " +
                                    $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    var package = TestPackageFactory.OneProjectListCtor();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    var expected = Net20ExpectedRunnerResults.ResultFor(processModel, 2);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                    testName = "Single project (string ctor) - " +
                                    $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    package = TestPackageFactory.OneProjectStringCtor();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    expected = Net20ExpectedRunnerResults.ResultFor(processModel, 2);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");

                    testName = "Two projects - " +
                                    $"{nameof(EnginePackageSettings.ProcessModel)}:{processModel}";

                    package = TestPackageFactory.TwoProjects();
                    package.AddSetting(EnginePackageSettings.ProcessModel, processModel.ToString());

                    expected = Net20ExpectedRunnerResults.ResultFor(processModel, 4);
                    yield return new TestCaseData(package, expected).SetName($"{{m}}({testName})");
                }
            }
        }
    }
#endif
}
