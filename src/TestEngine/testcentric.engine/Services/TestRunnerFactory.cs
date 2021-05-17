// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using NUnit.Engine;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// TestRunnerFactory handles creation of a suitable test
    /// runner for a given package to be loaded and run either in a
    /// separate process or within the same process.
    /// </summary>
    public class TestRunnerFactory : Service, ITestRunnerFactory
    {
        //private IProjectService _projectService;

        //public override void StartService()
        //{
        //    // TestRunnerFactory requires the ProjectService
        //    _projectService = ServiceContext.GetService<IProjectService>();

        //    // Anything returned from ServiceContext is known to be an IService
        //    Status = _projectService != null && ((IService)_projectService).Status == ServiceStatus.Started
        //        ? ServiceStatus.Started
        //        : ServiceStatus.Error;
        //}

        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>A TestRunner</returns>
        public ITestEngineRunner MakeTestRunner(TestPackage package)
        {
            var packageList = package.TerminalPackages();

            if (packageList.Count > 1)
                return new AggregatingTestRunner(ServiceContext, package);

            return MakeRunnerForSingleTestFile(packageList[0]);
        }

        private ITestEngineRunner MakeRunnerForSingleTestFile(TestPackage package)
        {
            if (!File.Exists(package.FullName))
                return new InvalidAssemblyRunner(package, "File not found: " + package.FullName);

            var ext = Path.GetExtension(package.FullName).ToLowerInvariant();
            if (ext != ".dll" && ext != ".exe")
                return new InvalidAssemblyRunner(package, "File type is not supported");

            if (package.GetSetting(EnginePackageSettings.ImageNonTestAssemblyAttribute, false))
                return new SkippedAssemblyRunner(package);

            return new ProcessRunner(ServiceContext, package);
        }

        // TODO: Review this method once used by a gui - currently unused.
        // The current implementation doesn't allow any runners to be reused.
        public bool CanReuse(ITestEngineRunner runner, TestPackage package)
        {
            return false;
        }
    }
}