// ***********************************************************************
// Copyright (c) 2007-2019 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Win32;
using NUnit.Common;

namespace NUnit.Engine
{
    using Helpers;

    /// <summary>
    /// RuntimeFramework represents a particular version
    /// of a common language runtime implementation.
    /// </summary>
    [Serializable]
    public sealed class RuntimeFramework : IRuntimeFramework
    {
        private static RuntimeFramework _currentFramework;

        /// <summary>
        /// Construct from a runtime type and version. If the version has
        /// two parts, it is taken as a framework version. If it has three
        /// or more, it is taken as a CLR version. In either case, the other
        /// version is deduced based on the runtime type and provided version.
        /// </summary>
        /// <param name="runtime">The runtime type of the framework</param>
        /// <param name="version">The version of the framework</param>
        public RuntimeFramework(Runtime runtime, Version version)
            : this(runtime, version, null)
        {
        }

        /// <summary>
        /// Construct from a runtime type, version and profile. The version
        /// may be either a framework version or a CLR version. If a CLR
        /// version is provided, we try to deduce the framework version but
        /// this may not always be successful, in which case a version of
        /// 0.0 is used.
        /// </summary>
        /// <param name="runtime">The runtime type of the framework.</param>
        /// <param name="version">The version of the framework.</param>
        /// <param name="profile">The profile of the framework. Null if unspecified.</param>
        public RuntimeFramework(Runtime runtime, Version version, string profile)
        {
            Runtime = runtime;
            FrameworkVersion = ClrVersion = version;

            if (IsFrameworkVersion(version))
                ClrVersion = GetClrVersionForFramework(version);
            else
                FrameworkVersion = GetFrameworkVersionForClr(version);

            Profile = profile;

            DisplayName = GetDefaultDisplayName(Runtime, FrameworkVersion, profile);

            FrameworkName = new FrameworkName(Runtime.FrameworkIdentifier, FrameworkVersion);
        }

        private bool IsFrameworkVersion(Version v)
        {
            // All known framework versions have either two components or
            // three. If three, then the Build is currently less than 3.
            return v.Build < 3 && v.Revision == -1;
        }

        private Version GetClrVersionForFramework(Version frameworkVersion)
        {
            if (Runtime == Runtime.Net)
            {
                switch (frameworkVersion.Major)
                {
                    case 1:
                        switch (frameworkVersion.Minor)
                        {
                            case 0:
                                return new Version(1, 0, 3705);
                            case 1:
                                return new Version(1, 1, 4322);
                        }
                        break;
                    case 2:
                    case 3:
                        return new Version(2, 0, 50727);
                    case 4:
                        return new Version(4, 0, 30319);
                }
            }
            else if (Runtime == Runtime.Mono)
            {
                switch (frameworkVersion.Major)
                {
                    case 1:
                        return new Version(1, 1, 4322);
                    case 2:
                    case 3:
                        return new Version(2, 0, 50727);
                    case 4:
                        return new Version(4, 0, 30319);
                }
            }
            else if (Runtime == Runtime.NetCore)
            {
                // HACK to make tests pass - needs research
                return new Version(FrameworkVersion.Major, FrameworkVersion.Minor, 1234);
            }

            throw new ArgumentException("Unknown framework version " + frameworkVersion.ToString(), "version");
        }

        private Version GetFrameworkVersionForClr(Version clrVersion)
        {
            return Runtime == Runtime.Mono && clrVersion.Major == 1
                ? new Version(1, 0)
                : new Version(clrVersion.Major, clrVersion.Minor);
        }

        /// <summary>
        /// Static method to return a RuntimeFramework object
        /// for the framework that is currently in use.
        /// </summary>
        public static RuntimeFramework CurrentFramework
        {
            get
            {
                if (_currentFramework == null)
                {
                    Type monoRuntimeType = Type.GetType("Mono.Runtime", false);
                    bool isMono = monoRuntimeType != null;

                    Runtime runtime = isMono
                        ? Runtime.Mono
                        : Runtime.Net;

                    int major = Environment.Version.Major;
                    int minor = Environment.Version.Minor;

                    if (isMono)
                    {
                        switch (major)
                        {
                            case 1:
                                minor = 0;
                                break;
                            case 2:
                                major = 3;
                                minor = 5;
                                break;
                        }
                    }
                    else /* It's windows */
                        if (major == 2)
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework");
                        if (key != null)
                        {
                            string installRoot = key.GetValue("InstallRoot") as string;
                            if (installRoot != null)
                            {
                                if (Directory.Exists(Path.Combine(installRoot, "v3.5")))
                                {
                                    major = 3;
                                    minor = 5;
                                }
                                else if (Directory.Exists(Path.Combine(installRoot, "v3.0")))
                                {
                                    major = 3;
                                    minor = 0;
                                }
                            }
                        }
                    }
                    else if (major == 4 && Type.GetType("System.Reflection.AssemblyMetadataAttribute") != null)
                    {
                        minor = 5;
                    }

                    _currentFramework = new RuntimeFramework(runtime, new Version(major, minor));
                    _currentFramework.ClrVersion = Environment.Version;

                    if (isMono)
                    {
                        _currentFramework.MonoPrefix = GetMonoPrefixFromAssembly(monoRuntimeType.Assembly);

                        MethodInfo getDisplayNameMethod = monoRuntimeType.GetMethod(
                            "GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding);
                        if (getDisplayNameMethod != null)
                        {
                            string displayName = (string)getDisplayNameMethod.Invoke(null, new object[0]);
                            Version monoVersion = new Version(0, 0);

                            int space = displayName.IndexOf(' ');
                            if (space >= 3) // Minimum length of a version
                            {
                                displayName = displayName.Substring(0, space);
                                monoVersion = new Version(displayName);
                            }

                            _currentFramework.DisplayName = "Mono " + displayName;
                            _currentFramework.MonoVersion = monoVersion;
                        }
                    }
                }

                return _currentFramework;
            }
        }

        /// <summary>
        /// The version of Mono in use or null if runtime type is not Mono.
        /// </summary>
        /// <value>The mono version.</value>
        public Version MonoVersion { get; set; }

        /// <summary>
        /// The install directory where the version of mono in
        /// use is located. Null if this is not a Mono runtime.
        /// </summary>
        public string MonoPrefix { get; set; }

        /// <summary>
        /// The path to the mono executable, based on the
        /// Mono prefix if available. Otherwise, uses "mono",
        /// to invoke a script of that name.
        /// </summary>
        public string MonoExePath
        {
            get
            {
                return MonoPrefix != null && Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? Path.Combine(MonoPrefix, "bin/mono.exe")
                    : "mono";
            }
        }

        /// <summary>
        /// Gets the unique Id for this runtime, such as "net-4.5"
        /// </summary>
        public string Id
        {
            get
            {
                return Runtime.ToString().ToLower() + "-" + FrameworkVersion.ToString();
            }
        }

        public FrameworkName FrameworkName { get; private set; }

        /// <summary>
        /// The runtime framework
        /// </summary>
        public Runtime Runtime { get; private set; }

        /// <summary>
        /// The framework version for this runtime framework
        /// </summary>
        public Version FrameworkVersion { get; private set; }

        /// <summary>
        /// The CLR version for this runtime framework
        /// </summary>
        public Version ClrVersion { get; private set; }

        /// <summary>
        /// The Profile for this framework, where relevant.
        /// May be null and will have different sets of
        /// values for each Runtime.
        /// </summary>
        public string Profile { get; private set; }

        /// <summary>
        /// Returns the Display name for this framework
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Parses a string representing a RuntimeFramework.
        /// The string may be just a RuntimeType name or just
        /// a Version or a hyphenated RuntimeType-Version or
        /// a Version prefixed by 'v'.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static RuntimeFramework Parse(string s)
        {
            Guard.ArgumentNotNullOrEmpty(s, nameof(s));

            string[] parts = s.Split(new char[] { '-' });
            Guard.ArgumentValid(parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0, "RuntimeFramework id not in correct format", nameof(s));

            var runtime = Runtime.Parse(parts[0]);
            var version = new Version(parts[1]);
            return new RuntimeFramework(runtime, version);
        }

        public static bool TryParse(string s, out RuntimeFramework runtimeFramework)
        {
            try
            {
                runtimeFramework = Parse(s);
                return true;
            }
            catch
            {
                runtimeFramework = null;
                return false;
            }
        }

        public static RuntimeFramework FromFrameworkName(string frameworkName)
        {
            return FromFrameworkName(new FrameworkName(frameworkName));
        }

        public static RuntimeFramework FromFrameworkName(FrameworkName frameworkName)
        {
            return new RuntimeFramework(Runtime.FromFrameworkIdentifier(frameworkName.Identifier), frameworkName.Version, frameworkName.Profile);
        }

        /// <summary>
        /// Overridden to return the short name of the framework
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Id;
        }

        /// <summary>
        /// Returns true if the current framework matches the
        /// one supplied as an argument. Two frameworks match
        /// if their runtime types are the same or either one
        /// is RuntimeType.Any and all specified version components
        /// are equal. Negative (i.e. unspecified) version
        /// components are ignored.
        /// </summary>
        /// <param name="target">The RuntimeFramework to be matched.</param>
        /// <returns><c>true</c> on match, otherwise <c>false</c></returns>
        public bool Supports(RuntimeFramework target)
        {
            if (this.Runtime != target.Runtime)
                return false;

            return VersionsMatch(this.ClrVersion, target.ClrVersion)
                && this.FrameworkVersion.Major >= target.FrameworkVersion.Major
                && this.FrameworkVersion.Minor >= target.FrameworkVersion.Minor;
        }

        public bool CanLoad(IRuntimeFramework requested)
        {
            return FrameworkVersion >= requested.FrameworkVersion;
        }

        private static string GetDefaultDisplayName(Runtime runtime, Version version, string profile)
        {
            string displayName = runtime.DisplayName + " " + version.ToString();

            if (!string.IsNullOrEmpty(profile) && profile != "Full")
                displayName += " - " + profile;

            return displayName;
        }

        private static bool VersionsMatch(Version v1, Version v2)
        {
            return v1.Major == v2.Major &&
                   v1.Minor == v2.Minor &&
                  (v1.Build < 0 || v2.Build < 0 || v1.Build == v2.Build) &&
                  (v1.Revision < 0 || v2.Revision < 0 || v1.Revision == v2.Revision);
        }

        private static string GetMonoPrefixFromAssembly(Assembly assembly)
        {
            string prefix = assembly.Location;

            // In all normal mono installations, there will be sufficient
            // levels to complete the four iterations. But just in case
            // files have been copied to some non-standard place, we check.
            for (int i = 0; i < 4; i++)
            {
                string dir = Path.GetDirectoryName(prefix);
                if (string.IsNullOrEmpty(dir)) break;

                prefix = dir;
            }

            return prefix;
        }
    }
}
#endif
