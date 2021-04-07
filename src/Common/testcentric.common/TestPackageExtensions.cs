// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Runtime.Versioning;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Common
{
    public delegate bool TestPackageSelectorDelegate(TestPackage p);

    /// <summary>
    /// Extension methods for use with TestPackages
    /// </summary>
    public static class TestPackageExtensions
    {
        public static bool IsAssemblyPackage(this TestPackage package)
        {
            return package.FullName != null && IsAssemblyFileType(package.FullName);
        }

        public static IList<TestPackage> AssemblyPackages(this TestPackage package)
        {
            return package.Select(p => p.IsAssemblyPackage());
        }

        public static bool HasSubPackages(this TestPackage package)
        {
            return package.SubPackages.Count > 0;
        }

        public static IList<TestPackage> Select(this TestPackage package, TestPackageSelectorDelegate selector)
        {
            var selection = new List<TestPackage>();

            AccumulatePackages(package, selection, selector);

            return selection;
        }

        public static string GetActiveConfig(this TestPackage package)
        {
            return package.GetSetting(EnginePackageSettings.ActiveConfig, string.Empty);
        }

        public static void SetActiveConfig(this TestPackage package, string configName)
        {
            package.Settings[EnginePackageSettings.ActiveConfig] = configName;
        }

        public static string[] GetConfigNames(this TestPackage package)
        {
            return package.GetSetting(EnginePackageSettings.ConfigNames, new string[0]);
        }

        public static void SetConfigNames(this TestPackage package, string[] configNames)
        {
            package.Settings[EnginePackageSettings.ConfigNames] = configNames;
        }

        public static string GetRequestedRuntimeFramework(this TestPackage package)
        {
            return package.GetSetting(EnginePackageSettings.RequestedRuntimeFramework, "DEFAULT");
        }

        public static string GetImageTargetFrameworkName(this TestPackage package)
        {
            return package.GetSetting(EnginePackageSettings.ImageTargetFrameworkName, string.Empty);
        }

        public static bool HasImageTargetFrameworkName(this TestPackage package)
        {
            return HasSetting(package, EnginePackageSettings.ImageTargetFrameworkName);
        }

        public static bool IsNetFrameworkPackage(this TestPackage package)
        {
            return !HasImageTargetFrameworkName(package) // Implies .NET 2.0 or lower
                || new FrameworkName(GetImageTargetFrameworkName(package)).Identifier == ".NETFramework";
        }

        public static bool IsNetCorePackage(this TestPackage package)
        {
            return HasImageTargetFrameworkName(package)
                && new FrameworkName(GetImageTargetFrameworkName(package)).Identifier == ".NETCoreApp";
        }

        public static bool HasSetting(this TestPackage package, string name)
        {
            return package.Settings.ContainsKey(name);
        }

        #region Helper Methods

        private static bool IsAssemblyFileType(string path)
        {
            string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            return ext == ".dll" || ext == ".exe";
        }

        private static void AccumulatePackages(TestPackage package, IList<TestPackage> selection, TestPackageSelectorDelegate selector)
        {
            if (selector(package))
                selection.Add(package);

            foreach (var subPackage in package.SubPackages)
                AccumulatePackages(subPackage, selection, selector);
        }

        #endregion
    }
}
