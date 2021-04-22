// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using TestCentric.Engine.Runners;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests
{
    using System.IO;
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
            projectService.Add("a.nunit", "a.dll", "b.dll");

            _services.Add(new ExtensionService());
            _services.Add(projectService);
            _services.Add(new TestRunnerFactory());
            _services.Add(new FakeRuntimeService());
            _services.Add(new TestFrameworkService());
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

        private static readonly string MOCK_ASSEMBLY = Path.Combine(TestContext.CurrentContext.TestDirectory, "mock-assembly.dll");

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
#if NETCOREAPP
                yield return new TestCaseData(
                    new TestPackage("a.dll"),
                    RunnerResult.LocalTestRunner).SetName("SingleAssembly_StringCtor");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll" }),
                    RunnerResult.LocalTestRunner).SetName("SingleAssembly_ListCtor");

                yield return new TestCaseData(
                    new TestPackage("a.junk"),
                    RunnerResult.LocalTestRunner).SetName("SingleUnknown");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll", "b.dll" }),
                    RunnerResult.AggregatingTestRunner(2)).SetName("TwoAssemblies");

                yield return new TestCaseData(
                    new TestPackage(new[] { "a.nunit" }),
                    RunnerResult.LocalTestRunner).SetName("SingleProject_ListCtor");

                yield return new TestCaseData(
                    new TestPackage("a.nunit"),
                    RunnerResult.AggregatingTestRunner(2)).SetName("SingleProject_StringConstructor");

                yield return new TestCaseData(
                    new TestPackage(new[] { "a.nunit", "a.nunit" }),
                    RunnerResult.AggregatingTestRunner(4)).SetName("TwoProjects");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.nunit", "b.dll" }),
                    RunnerResult.AggregatingTestRunner(3)).SetName("OneProjectOneAssembly");

                yield return new TestCaseData(
                    new TestPackage(new[] { "a.nunit", "b.nunit", "a.dll" }),
                    RunnerResult.AggregatingTestRunner(3)).SetName("TwoProjectsOneAssembly");

                yield return new TestCaseData(
                    new TestPackage(new[] { "a.dll", "b.dll", "a.nunit" }),
                    RunnerResult.AggregatingTestRunner(4)).SetName("TwoAssembliesOneProject");

                //yield return new TestCaseData(
                //    new TestPackage(new[] { "a.junk", "b.junk" }),
                //    RunnerResult.AggregatingTestRunner(2)).SetName("TwoUnknowns");

                //yield return new TestCaseData(
                //    new TestPackage(new[] { "a.junk", "a.dll", "a.nunit" }),
                //    RunnerResult.AggregatingTestRunner(4)).SetName("OneAssemblyOneProjectOneUnknown");
#else
                yield return new TestCaseData(
                    new TestPackage(MOCK_ASSEMBLY),
                    RunnerResult.ProcessRunner).SetName("SingleAssembly_StringCtor");

                yield return new TestCaseData(
                    new TestPackage("a.dll"),
                    RunnerResult.InvalidAssemblyRunner).SetName("SingleAssemblyNotFound_StringCtor");

                yield return new TestCaseData(
                    new TestPackage(new string[] { MOCK_ASSEMBLY }),
                    RunnerResult.ProcessRunner).SetName("SingleAssembly_ListCtor");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll" }),
                    RunnerResult.InvalidAssemblyRunner).SetName("SingleAssemblyNotFound_ListCtor");

                yield return new TestCaseData(
                    new TestPackage("a.junk"),
                    RunnerResult.InvalidAssemblyRunner).SetName("SingleUnknown");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll", "b.dll" }),
                    RunnerResult.MultipleProcessRunner(2)).SetName("TwoAssemblies");

                //yield return new TestCaseData(
                //    new TestPackage(new string[] { "a.junk", "b.junk" }),
                //    RunnerResult.MultipleProcessRunner(2)).SetName("TwoUnknowns");

                yield return new TestCaseData(
                    new TestPackage("a.nunit"),
                    RunnerResult.MultipleProcessRunner(2)).SetName("SingleProject_StringCtor");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.nunit" }),
                    RunnerResult.MultipleProcessRunner(2)).SetName("SingleProject_ListCtor");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.nunit", "a.nunit" }),
                    RunnerResult.MultipleProcessRunner(4)).SetName("TwoProjects");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.nunit", "b.dll" }),
                    RunnerResult.MultipleProcessRunner(3)).SetName("OneProjectOneAssembly");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.nunit", "a.nunit", "b.dll" }),
                    RunnerResult.MultipleProcessRunner(5)).SetName("TwoProjectsOneAssembly");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll", "b.dll", "a.nunit" }),
                    RunnerResult.MultipleProcessRunner(4)).SetName("TwoAssembliesOneProject");

                yield return new TestCaseData(
                    new TestPackage(new string[] { "a.dll", "a.nunit", "a.junk" }),
                    RunnerResult.MultipleProcessRunner(3)).SetName("OneAssemblyOneProjectOneUnknown");
#endif
            }
        }
    }
}
