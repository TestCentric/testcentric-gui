// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Text;
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
            var sb = new StringBuilder();

#if !NETSTANDARD2_0  // TODO: How do we validate runtime framework for .NET Standard 2.0?
            var frameworkSetting = package.GetSetting(EnginePackageSettings.RequestedRuntimeFramework, "");
            var runAsX86 = package.GetSetting(EnginePackageSettings.RunAsX86, false);

            if (frameworkSetting.Length > 0)
            {
                // Check requested framework is actually available
                if (!_runtimeService.IsAvailable(frameworkSetting))
                    sb.Append($"\n* The requested framework {frameworkSetting} is unknown or not available.\n");
            }
#endif

            // At this point, any unsupported settings in the TestPackage were
            // put there by the user, so we consider the package invalid. This
            // will be checked again as each project package is expanded and
            // treated as a warning in that case.
            foreach (string setting in package.Settings.Keys)
            {
                if (EnginePackageSettings.IsObsoleteSetting(setting))
                    sb.Append($"\n* The {setting} setting is no longer supported.\n");
            }

            if (sb.Length > 0)
                throw new NUnitEngineException($"The following errors were detected in the TestPackage:\n{sb}");
        }
    }
}
