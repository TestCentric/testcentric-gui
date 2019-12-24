// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;
using TestCentric.Engine.Runners;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// DefaultTestRunnerFactory handles creation of a suitable test
    /// runner for a given package to be loaded and run either in a
    /// separate process or within the same process.
    /// </summary>
    public class DefaultTestRunnerFactory : InProcessTestRunnerFactory, ITestRunnerFactory
    {
#if !NETSTANDARD1_6
        private IProjectService _projectService;
#endif

        public override void StartService()
        {
#if !NETSTANDARD1_6
            // TestRunnerFactory requires the ProjectService
            _projectService = ServiceContext.GetService<IProjectService>();

            // Anything returned from ServiceContext is known to be an IService
            Status = _projectService != null && ((IService)_projectService).Status == ServiceStatus.Started
                ? ServiceStatus.Started
                : ServiceStatus.Error;
#else
            Status = ServiceStatus.Started;
#endif
        }

        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>A TestRunner</returns>
        public override ITestEngineRunner MakeTestRunner(TestPackage package)
        {
#if NETSTANDARD1_6 || NETSTANDARD2_0
            if (package.SubPackages.Count > 1)
                return new AggregatingTestRunner(ServiceContext, package);

            return base.MakeTestRunner(package);
        }
#else

            ProcessModel processModel = GetTargetProcessModel(package);

            switch (processModel)
            {
                default:
                case ProcessModel.Default:
                case ProcessModel.Multiple:
                    if (package.AssemblyPackages().Count > 1)
                        return new MultipleTestProcessRunner(this.ServiceContext, package);
                    else
                        return new ProcessRunner(this.ServiceContext, package);

                case ProcessModel.Separate:
                    return new ProcessRunner(this.ServiceContext, package);

                case ProcessModel.InProcess:
                    return base.MakeTestRunner(package);
            }
        }

        // TODO: Review this method once used by a gui - the implementation is
        // overly simplistic. It is not currently used by any known runner.
        public override bool CanReuse(ITestEngineRunner runner, TestPackage package)
        {
            ProcessModel processModel = GetTargetProcessModel(package);

            switch (processModel)
            {
                case ProcessModel.Default:
                case ProcessModel.Multiple:
                    return runner is MultipleTestProcessRunner;
                case ProcessModel.Separate:
                    return runner is ProcessRunner;
                default:
                    return base.CanReuse(runner, package);
            }
        }
#endif

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
        /// <summary>
        /// Get the specified target process model for the package.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        /// <returns>The string representation of the process model or "Default" if none was specified.</returns>
        private ProcessModel GetTargetProcessModel(TestPackage package)
        {
            return (ProcessModel)System.Enum.Parse(
                typeof(ProcessModel),
                package.GetSetting(EnginePackageSettings.ProcessModel, "Default"));
        }
#endif
    }
}
