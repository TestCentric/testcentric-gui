// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using TestCentric.Engine.Runners;
using TestCentric.Engine.Services.TestRunnerFactoryTests.TestCases;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests
{
    using Fakes;

    /// <summary>
    /// Tests of ITestRunner tree structure for different combinations
    /// of TestPackage and PackageSettings. Some tests are overkill for
    /// current capabilities. For example, it's really not necessary to
    /// verify that the subrunners of a MultipleTestProcessRunner are
    /// all ProcessRunners. However, this approach will support future
    /// scenarios, where different types of runners are aggregated.
    /// </summary>
    public class RunnerSelectionTests
    {
        private ServiceContext _services;

        [OneTimeSetUp]
        public void SetUp()
        {
            _services = new ServiceContext();

            var projectService = new FakeProjectService();
            projectService.Add(TestPackageFactory.FakeProject, "a.dll", "b.dll");

            _services.Add(new ExtensionService());
            _services.Add(projectService);
            _services.Add(new DefaultTestRunnerFactory());
            _services.Add(new FakeRuntimeService());
            _services.Add(new PackageSettingsService());

            _services.ServiceManager.StartServices();
        }

        [TestCaseSource(nameof(TestCases))]
        public void RunnerSelectionTest(TestPackage package, RunnerResult expected)
        {
            var masterRunner = new MasterTestRunner(_services, package);
            var runner = masterRunner.GetEngineRunner();
            var result = GetRunnerResult(runner);
            Assert.That(result, Is.EqualTo(expected).Using(RunnerResultComparer.Instance));
        }

        private static RunnerResult GetRunnerResult(ITestEngineRunner runner)
        {
            var result = new RunnerResult { TestRunner = runner.GetType() };

            if (runner is AggregatingTestRunner aggRunner)
                result.SubRunners = aggRunner.Runners.Select(GetRunnerResult).ToList();

            return result;
        }

#if NETCOREAPP
        private static IEnumerable<TestCaseData> TestCases => NetStandardTestCases.TestCases;
#else
        private static IEnumerable<TestCaseData> TestCases => Net20AssemblyTestCases.TestCases
            .Concat(Net20ProjectTestCases.TestCases)
            .Concat(Net20MixedProjectAndAssemblyTestCases.TestCases);
#endif
    }
}
