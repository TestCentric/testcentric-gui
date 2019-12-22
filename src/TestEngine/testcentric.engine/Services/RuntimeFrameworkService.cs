// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Mono.Cecil;
using NUnit.Common;
using NUnit.Engine.Helpers;
using NUnit.Engine.Internal;

namespace NUnit.Engine.Services
{
    public class RuntimeFrameworkService : Service, IRuntimeFrameworkService, IAvailableRuntimes
    {
        private static readonly string DEFAULT_WINDOWS_MONO_DIR =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono");

        private static readonly Version AnyVersion = new Version(0, 0);

        static Logger log = InternalTrace.GetLogger(typeof(RuntimeFrameworkService));

        List<RuntimeFramework> _availableRuntimes = new List<RuntimeFramework>();

        #region Service Overrides

        /// <summary>
        /// Start the service, initializing available runtimes.
        /// </summary>
        public override void StartService()
        {
            FindAvailableRuntimes();
            base.StartService();
        }

        #endregion

        #region IAvailableRuntimes Implementation

        /// <summary>
        /// Gets a list of available runtimes.
        /// </summary>
        public IList<IRuntimeFramework> AvailableRuntimes
        {
            get { return _availableRuntimes.ToArray(); }
        }

        #endregion

        #region IRuntimeFrameworkService Implementation

        /// <summary>
        /// Returns true if the runtime framework represented by
        /// the string passed as an argument is available.
        /// </summary>
        /// <param name="name">A string representing a framework, like 'net-4.0'</param>
        /// <returns>True if the framework is available, false if unavailable or nonexistent</returns>
        public bool IsAvailable(string name)
        {
            Guard.ArgumentNotNullOrEmpty(name, nameof(name));

            RuntimeFramework requestedFramework;
            if (!RuntimeFramework.TryParse(name, out requestedFramework))
                throw new NUnitEngineException("Invalid or unknown framework requested: " + name);

            return IsAvailable(requestedFramework);
        }

        /// <summary>
        /// Selects a target runtime framework for a TestPackage based on
        /// the settings in the package and the assemblies themselves.
        /// The package RuntimeFramework setting may be updated as a result
        /// and a string representing the selected runtime is returned.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        /// <returns>A string representing the selected RuntimeFramework</returns>
        public string SelectRuntimeFramework(TestPackage package)
        {
            // Evaluate package target framework
            ApplyImageData(package);

            var targetFramework = SelectRuntimeFrameworkInner(package);
            return targetFramework.ToString();
        }

        #endregion

        #region Other Public Methods

        public bool IsAvailable(RuntimeFramework requestedFramework)
        {
            foreach (var framework in _availableRuntimes)
                if (FrameworksMatch(requestedFramework, framework))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the best available framework that matches a target framework.
        /// If the target framework has a build number specified, then an exact
        /// match is needed. Otherwise, the matching framework with the highest
        /// build number is used.
        /// </summary>
        public RuntimeFramework GetBestAvailableFramework(RuntimeFramework target)
        {
            RuntimeFramework result = target;

            foreach (RuntimeFramework framework in _availableRuntimes)
                if (framework.Supports(target))
                {
                    if (framework.ClrVersion.Build > result.ClrVersion.Build)
                        result = framework;
                }

            return result;
        }

        #endregion

        #region Helper Methods

        private static bool FrameworksMatch(RuntimeFramework f1, RuntimeFramework f2)
        {
            var rt1 = f1.Runtime;
            var rt2 = f2.Runtime;

            if (!rt1.Matches(rt2))
                return false;

            var v1 = f1.ClrVersion;
            var v2 = f2.ClrVersion;

            if (v1 == AnyVersion || v2 == AnyVersion)
                return true;

            return v1.Major == v2.Major &&
                   v1.Minor == v2.Minor &&
                   (v1.Build < 0 || v2.Build < 0 || v1.Build == v2.Build) &&
                   (v1.Revision < 0 || v2.Revision < 0 || v1.Revision == v2.Revision) &&
                   f1.FrameworkVersion.Major == f2.FrameworkVersion.Major &&
                   f1.FrameworkVersion.Minor == f2.FrameworkVersion.Minor;
        }

        private RuntimeFramework SelectRuntimeFrameworkInner(TestPackage package)
        {
            foreach (var subPackage in package.SubPackages)
            {
                SelectRuntimeFrameworkInner(subPackage);
            }

            // Examine the provided settings
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            log.Debug("Current framework is " + currentFramework);

            string requestedFrameworkSetting = package.GetSetting(EnginePackageSettings.RuntimeFramework, "");

            if (requestedFrameworkSetting.Length > 0)
            {
                RuntimeFramework requestedFramework;
                if (!RuntimeFramework.TryParse(requestedFrameworkSetting, out requestedFramework))
                    throw new NUnitEngineException("Invalid or unknown framework requested: " + requestedFrameworkSetting);

                log.Debug($"Requested framework for {package.Name} is {requestedFramework}");

                if (!IsAvailable(requestedFramework))
                    throw new NUnitEngineException("Requested framework is not available: " + requestedFrameworkSetting);

                return requestedFramework;
            }

            log.Debug($"No specific framework requested for {package.Name}");

            string imageTargetFrameworkNameSetting = package.GetSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, "");

            RuntimeFramework targetFramework;

            // HACK: handling the TargetFrameworkName does not currently work outside of windows
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && imageTargetFrameworkNameSetting.Length > 0)
            {
                targetFramework = RuntimeFramework.FromFrameworkName(imageTargetFrameworkNameSetting);

                // TODO: temporary exception thrown until we implement .NET Core
                if (targetFramework.Runtime == Runtime.NetCore)
                    throw new NotImplementedException("The GUI does not yet support .NET Core tests");
            }
            else
            {
                var targetVersion = package.GetSetting(InternalEnginePackageSettings.ImageRuntimeVersion, currentFramework.FrameworkVersion);
                targetFramework = new RuntimeFramework(currentFramework.Runtime, targetVersion);
            }

            if (!IsAvailable(targetFramework))
            {
                log.Debug("Preferred target framework {0} is not available.", targetFramework);
                if (currentFramework.Supports(targetFramework))
                {
                    targetFramework = currentFramework;
                    log.Debug($"Using {currentFramework}");
                }
            }

            package.Settings[EnginePackageSettings.RuntimeFramework] = targetFramework.ToString();

            log.Debug($"Test will use {targetFramework} for {package.Name}");
            return targetFramework;
        }


        /// <summary>
        /// Use Mono.Cecil to get information about all assemblies and
        /// apply it to the package using special internal keywords.
        /// </summary>
        /// <param name="package"></param>
        private static void ApplyImageData(TestPackage package)
        {
            string packageName = package.FullName;

            Version targetVersion = new Version(0, 0);
            string frameworkName = null;
            bool requiresX86 = false;
            bool requiresAssemblyResolver = false;

            // We are doing two jobs here: (1) in the else clause (below)
            // we get information about a single assembly and record it,
            // (2) in the if clause, we recursively examine all subpackages
            // and then apply policies for promulgating each setting to
            // a containing package. We could implement the policy part at
            // a higher level, but it seems simplest to do it right here.
            if (package.SubPackages.Count > 0)
            {
                foreach (var subPackage in package.SubPackages)
                {
                    ApplyImageData(subPackage);

                    // Collect the highest version required
                    Version v = subPackage.GetSetting(InternalEnginePackageSettings.ImageRuntimeVersion, new Version(0, 0));
                    if (v > targetVersion) targetVersion = v;

                    // Collect highest framework name
                    // TODO: This assumes lexical ordering is valid - check it
                    string fn = subPackage.GetSetting(InternalEnginePackageSettings.ImageTargetFrameworkName, "");
                    if (fn != "")
                    {
                        if (frameworkName == null || fn.CompareTo(frameworkName) < 0)
                            frameworkName = fn;
                    }

                    // If any assembly requires X86, then the aggregate package requires it
                    if (subPackage.GetSetting(InternalEnginePackageSettings.ImageRequiresX86, false))
                        requiresX86 = true;

                    if (subPackage.GetSetting(InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver, false))
                        requiresAssemblyResolver = true;
                }
            }
            else if (File.Exists(packageName) && PathUtils.IsAssemblyFileType(packageName))
            {
                var assembly = new TargetFrameworkHelper(packageName);

                targetVersion = assembly.TargetRuntimeVersion;
                log.Debug($"Assembly {packageName} uses version {targetVersion}");

                frameworkName = assembly.FrameworkName;
                log.Debug($"Assembly {packageName} targets {frameworkName}");

                if (assembly.RequiresX86)
                {
                    requiresX86 = true;
                    log.Debug($"Assembly {packageName} will be run x86");
                }

                if (assembly.RequiresAssemblyResolver)
                {
                    requiresAssemblyResolver = true;
                    log.Debug($"Assembly {packageName} requires default app domain assembly resolver");
                }
            }

            if (targetVersion.Major > 0)
                package.Settings[InternalEnginePackageSettings.ImageRuntimeVersion] = targetVersion;

            if (!string.IsNullOrEmpty(frameworkName))
                package.Settings[InternalEnginePackageSettings.ImageTargetFrameworkName] = frameworkName;

            package.Settings[InternalEnginePackageSettings.ImageRequiresX86] = requiresX86;
            if (requiresX86)
                package.Settings[EnginePackageSettings.RunAsX86] = true;

            package.Settings[InternalEnginePackageSettings.ImageRequiresDefaultAppDomainAssemblyResolver] = requiresAssemblyResolver;
        }

        private void FindAvailableRuntimes()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                FindDotNetFrameworks();

            FindDefaultMonoFramework();
            FindDotNetCoreFrameworks();
        }

        // Note: this method cannot be generalized past V4, because (a)  it has
        // specific code for detecting .NET 4.5 and (b) we don't know what
        // microsoft will do in the future
        private void FindDotNetFrameworks()
        {
            // Handle Version 1.0, using a different registry key
            FindExtremelyOldDotNetFrameworkVersions();

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP");
            if (key != null)
            {
                foreach (string name in key.GetSubKeyNames())
                {
                    if (name.StartsWith("v") && name != "v4.0") // v4.0 is a duplicate, legacy key
                    {
                        var versionKey = key.OpenSubKey(name);
                        if (versionKey == null) continue;

                        if (name.StartsWith("v4", StringComparison.Ordinal))
                        {
                            // Version 4 and 4.5
                            FindDotNetFourFrameworkVersions(versionKey);
                        }
                        else if (CheckInstallDword(versionKey))
                        {
                            // Versons 1.1, 2.0, 3.0 and 3.5 are possible here
                            _availableRuntimes.Add(new RuntimeFramework(Runtime.Net, new Version(name.Substring(1, 3))));
                        }
                    }
                }
            }
        }

        private void FindExtremelyOldDotNetFrameworkVersions()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\policy\v1.0");
            if (key != null)
                foreach (var build in key.GetValueNames())
                    _availableRuntimes.Add(new RuntimeFramework(Runtime.Net, new Version("1.0." + build)));
        }

        private struct MinimumRelease
        {
            public readonly int Release;
            public readonly Version Version;

            public MinimumRelease(int release, Version version)
            {
                Release = release;
                Version = version;
            }
        }

        private static readonly MinimumRelease[] ReleaseTable = new MinimumRelease[]
        {
            // TODO: Make 3-component versions work correctly
            new MinimumRelease(378389, new Version(4, 5)),
            new MinimumRelease(378675, new Version(4, 5, 1)),
            new MinimumRelease(379893, new Version(4, 5, 2)),
            new MinimumRelease(393295, new Version(4, 6)),
            new MinimumRelease(394254, new Version(4, 6, 1)),
            new MinimumRelease(394802, new Version(4, 6, 2)),
            new MinimumRelease(460798, new Version(4, 7)),
            new MinimumRelease(461308, new Version(4, 7, 1)),
            new MinimumRelease(461808, new Version(4, 7, 2)),
            new MinimumRelease(528040, new Version(4, 8))
        };

        private void FindDotNetFourFrameworkVersions(RegistryKey versionKey)
        {
            foreach (string profile in new string[] { "Full", "Client" })
            {
                var profileKey = versionKey.OpenSubKey(profile);
                if (profileKey == null) continue;

                if (CheckInstallDword(profileKey))
                {
                    _availableRuntimes.Add(new RuntimeFramework(Runtime.Net, new Version(4, 0), profile));

                    var release = (int)profileKey.GetValue("Release", 0);
                    foreach (var entry in ReleaseTable)
                        if (release >= entry.Release)
                            _availableRuntimes.Add(new RuntimeFramework(Runtime.Net, entry.Version));

                    break;     //If full profile found don't check for client profile
                }
            }
        }

        private static bool CheckInstallDword(RegistryKey key)
        {
            return ((int)key.GetValue("Install", 0) == 1);
        }

        private void FindDefaultMonoFramework()
        {
            if (RuntimeFramework.CurrentFramework.Runtime == Runtime.Mono)
                UseCurrentMonoFramework();
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                FindBestMonoFrameworkOnWindows();
        }

        private void UseCurrentMonoFramework()
        {
            var current = RuntimeFramework.CurrentFramework;
            Debug.Assert(current.Runtime == Runtime.Mono && current.MonoPrefix != null && current.MonoVersion != null);

            // Multiple profiles are no longer supported with Mono 4.0
            if (current.MonoVersion.Major < 4 && FindAllMonoProfiles(current.MonoVersion, current.MonoPrefix) > 0)
                return;

            // If Mono 4.0+ or no profiles found, just use current runtime
            _availableRuntimes.Add(current);
        }

        private void FindBestMonoFrameworkOnWindows()
        {
            // First, look for recent frameworks that use the Software\Mono Key
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Mono");

            if (key != null && (int)key.GetValue("Installed", 0) == 1)
            {
                string version = key.GetValue("Version") as string;
                string monoPrefix = key.GetValue("SdkInstallRoot") as string;

                if (version != null)
                {
                    AddMonoFramework(new Version(4, 5), new Version(version), monoPrefix, null);
                    return;
                }
            }

            // Some later 3.x Mono releases didn't use the registry
            // so check in standard location for them.
            if (Directory.Exists(DEFAULT_WINDOWS_MONO_DIR))
            {
                AddMonoFramework(new Version(4, 5), new Version(0, 0), DEFAULT_WINDOWS_MONO_DIR, null);
                return;
            }

            // Look in the Software\Novell key for older versions
            key = Registry.LocalMachine.OpenSubKey(@"Software\Novell\Mono");
            if (key != null)
            {
                string version = key.GetValue("DefaultCLR") as string;
                if (version != null)
                {
                    RegistryKey subKey = key.OpenSubKey(version);
                    if (subKey != null)
                    {
                        string monoPrefix = subKey.GetValue("SdkInstallRoot") as string;
                        Version monoVersion = new Version(version);

                        FindAllMonoProfiles(monoVersion, monoPrefix);
                    }
                }
            }
        }

        private int FindAllMonoProfiles(Version monoVersion, string monoPrefix)
        {
            int count = 0;

            if (monoPrefix != null)
            {
                if (File.Exists(Path.Combine(monoPrefix, "lib/mono/1.0/mscorlib.dll")))
                {
                    AddMonoFramework(new Version(1, 1, 4322), monoVersion, monoPrefix, "1.0");
                    count++;
                }

                if (File.Exists(Path.Combine(monoPrefix, "lib/mono/2.0/mscorlib.dll")))
                {
                    AddMonoFramework(new Version(2, 0), monoVersion, monoPrefix, "2.0");
                    count++;
                }

                if (Directory.Exists(Path.Combine(monoPrefix, "lib/mono/3.5")))
                {
                    AddMonoFramework(new Version(3, 5), monoVersion, monoPrefix, "3.5");
                    count++;
                }

                if (File.Exists(Path.Combine(monoPrefix, "lib/mono/4.0/mscorlib.dll")))
                {
                    AddMonoFramework(new Version(4, 0), monoVersion, monoPrefix, "4.0");
                    count++;
                }

                if (File.Exists(Path.Combine(monoPrefix, "lib/mono/4.5/mscorlib.dll")))
                {
                    AddMonoFramework(new Version(4, 5), monoVersion, monoPrefix, "4.5");
                    count++;
                }
            }

            return count;
        }

        private void AddMonoFramework(Version frameworkVersion, Version monoVersion, string monoPrefix, string profile)
        {
            var framework = new RuntimeFramework(Runtime.Mono, frameworkVersion, profile)
            {
                MonoVersion = monoVersion,
                MonoPrefix = monoPrefix,
                DisplayName = monoVersion != null
                    ? "Mono " + monoVersion.ToString() + " - " + profile + " Profile"
                    : "Mono - " + profile + " Profile"
            };

            _availableRuntimes.Add(framework);
        }

        private void FindDotNetCoreFrameworks()
        {
            const string WINDOWS_INSTALL_DIR = "C:\\Program Files\\dotnet\\";
            const string LINUX_INSTALL_DIR = "/usr/shared/dotnet/";
            string INSTALL_DIR = Path.DirectorySeparatorChar == '\\'
                ? WINDOWS_INSTALL_DIR
                : LINUX_INSTALL_DIR;

            if (!Directory.Exists(INSTALL_DIR))
                return;
            if (!File.Exists(Path.Combine(INSTALL_DIR, "dotnet.exe")))
                return;

            string runtimeDir = Path.Combine(INSTALL_DIR, "shared\\Microsoft.NETCore.App");
            if (!Directory.Exists(runtimeDir))
                return;

            var runtimes = new List<RuntimeFramework>();
            int count = 0;
            
            foreach (var dir in new DirectoryInfo(runtimeDir).GetDirectories())
            {
                var fullVersion = new Version(dir.Name);
                var newVersion = new Version(fullVersion.Major, fullVersion.Minor);
                if (count > 0 && runtimes[count - 1].FrameworkVersion == newVersion)
                        continue;

                runtimes.Add(new RuntimeFramework(Runtime.NetCore, newVersion));
                count++;
            }

            _availableRuntimes.AddRange(runtimes);
        }

        #endregion
    }
}
#endif
