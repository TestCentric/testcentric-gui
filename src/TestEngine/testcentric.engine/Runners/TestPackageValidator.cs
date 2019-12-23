// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine.Runners
{
    internal class TestPackageValidator
    {
        private IRuntimeFrameworkService _runtimeService;

        public TestPackageValidator(IRuntimeFrameworkService runtimeService)
        {
            _runtimeService = runtimeService;
        }

        // Any Errors thrown from this method indicate that the client
        // runner is putting invalid values into the package.
        public void Validate(TestPackage package)
        {
#if !NETSTANDARD1_6 && !NETSTANDARD2_0  // TODO: How do we validate runtime framework for .NET Standard 2.0?
            var processModel = package.GetSetting(EnginePackageSettings.ProcessModel, "Default").ToLower();
            var runningInProcess = processModel == "inprocess";
            var frameworkSetting = package.GetSetting(EnginePackageSettings.RuntimeFramework, "");
            var runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);

            if (frameworkSetting.Length > 0)
            {
                // Check requested framework is actually available
                if (!_runtimeService.IsAvailable(frameworkSetting))
                    throw new NUnitEngineException($"The requested framework {frameworkSetting} is unknown or not available.");

                // If running in process, check requested framework is compatible
                if (runningInProcess)
                {
                    var currentFramework = RuntimeFramework.CurrentFramework;

                    RuntimeFramework requestedFramework;
                    if (!RuntimeFramework.TryParse(frameworkSetting, out requestedFramework))
                        throw new NUnitEngineException("Invalid or unknown framework requested: " + frameworkSetting);

                    if (!currentFramework.Supports(requestedFramework))
                        throw new NUnitEngineException(string.Format(
                            "Cannot run {0} framework in process already running {1}.", frameworkSetting, currentFramework));
                }
            }

            if (runningInProcess && runAsX86 && IntPtr.Size == 8)
                throw new NUnitEngineException("Cannot run tests in process - a 32 bit process is required.");
#endif
        }
    }
}
