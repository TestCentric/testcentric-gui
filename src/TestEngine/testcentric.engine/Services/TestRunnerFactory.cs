// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using NUnit.Engine;
using TestCentric.Engine.Helpers;
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
            if (package.SubPackages.Count == 1)
                return MakeTestRunner(package.SubPackages[0]);

#if NETSTANDARD2_0
            if (package.SubPackages.Count > 1)
                return new AggregatingTestRunner(ServiceContext, package);

            return new LocalTestRunner(ServiceContext, package);
#else
            if (package.AssemblyPackages().Count > 1)
                return new MultipleTestProcessRunner(this.ServiceContext, package);

            return MakeRunnerForSingleTestFile(package);
#endif
        }

        private ITestEngineRunner MakeRunnerForSingleTestFile(TestPackage package)
        {
            if (!File.Exists(package.FullName))
                return new InvalidAssemblyRunner(package, "File not found: " + package.FullName);

            if (!PathUtils.IsAssemblyFileType(package.FullName))
                return new InvalidAssemblyRunner(package, "File type is not supported");

            if (package.GetSetting(EnginePackageSettings.ImageNonTestAssemblyAttribute, false))
                return new SkippedAssemblyRunner(package);

            return new ProcessRunner(this.ServiceContext, package);
        }

        // TODO: Review this method once used by a gui - the implementation is
        // overly simplistic. It is not currently used by any known runner.
        public bool CanReuse(ITestEngineRunner runner, TestPackage package)
        {
#if NETSTANDARD2_0
            return false;
#else
            return runner is MultipleTestProcessRunner;
#endif
        }
    }
}
