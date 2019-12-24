// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using TestCentric.Engine.Runners;
using TestCentric.Engine.Services.TestRunnerFactoryTests.TestCases;
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
        private DefaultTestRunnerFactory _factory;
        private ServiceContext _services;

        [OneTimeSetUp]
        public void SetUp()
        {
            _services = new ServiceContext();
#if !NETCOREAPP1_1
            _services.Add(new ExtensionService());
            var projectService = new FakeProjectService();
            ((IService)projectService).StartService();
            projectService.Add(TestPackageFactory.FakeProject, "a.dll", "b.dll");
            _services.Add(projectService);
            Assert.That(((IService)projectService).Status, Is.EqualTo(ServiceStatus.Started));
#endif
            _factory = new DefaultTestRunnerFactory();
            _services.Add(_factory);
            _factory.StartService();
            Assert.That(_factory.Status, Is.EqualTo(ServiceStatus.Started));

            var fakeRuntimeService = new FakeRuntimeService();
            ((IService)fakeRuntimeService).StartService();
            _services.Add(fakeRuntimeService);
            Assert.That(((IService)fakeRuntimeService).Status, Is.EqualTo(ServiceStatus.Started));
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
