// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

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
#if !NETSTANDARD2_0  // TODO: How do we validate runtime framework for .NET Standard 2.0?
            var processModel = package.GetSetting(EnginePackageSettings.ProcessModel, "Default").ToLower();
            var frameworkSetting = package.GetSetting(EnginePackageSettings.RequestedRuntimeFramework, "");
            var runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);

            if (frameworkSetting.Length > 0)
            {
                // Check requested framework is actually available
                if (!_runtimeService.IsAvailable(frameworkSetting))
                    throw new NUnitEngineException($"The requested framework {frameworkSetting} is unknown or not available.");
            }
#endif
        }
    }
}
