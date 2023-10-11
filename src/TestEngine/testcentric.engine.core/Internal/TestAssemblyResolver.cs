// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if NETCOREAPP3_1_OR_GREATER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.Win32;

namespace TestCentric.Engine.Internal
{
    internal sealed class TestAssemblyResolver : IDisposable
    {
        static Logger log = InternalTrace.GetLogger("TestAssemblyResolver");

        private readonly ICompilationAssemblyResolver _assemblyResolver;
        private readonly DependencyContext _dependencyContext;
        private readonly AssemblyLoadContext _loadContext;

        private static readonly string INSTALL_DIR = GetDotNetInstallDirectory();
        private static readonly string WINDOWS_DESKTOP_DIR = Path.Combine(INSTALL_DIR, "shared", "Microsoft.WindowsDesktop.App");
        private static readonly string ASP_NET_CORE_DIR = Path.Combine(INSTALL_DIR, "shared", "Microsoft.AspNetCore.App");
        private static readonly List<string> AdditionalFrameworkDirectories;

        static TestAssemblyResolver()
        {
            AdditionalFrameworkDirectories = new List<string>();
            if (Directory.Exists(WINDOWS_DESKTOP_DIR))
                AdditionalFrameworkDirectories.Add(WINDOWS_DESKTOP_DIR);
            if (Directory.Exists(ASP_NET_CORE_DIR))
                AdditionalFrameworkDirectories.Add(ASP_NET_CORE_DIR);
        }

        public TestAssemblyResolver(AssemblyLoadContext loadContext, string assemblyPath)
        {
            _loadContext = loadContext;
            _dependencyContext = DependencyContext.Load(loadContext.LoadFromAssemblyPath(assemblyPath));

            _assemblyResolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
            {
                new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(assemblyPath)),
                new ReferenceAssemblyPathResolver(),
                new PackageCompilationAssemblyResolver()
            });

            _loadContext.Resolving += OnResolving;
        }

        public void Dispose()
        {
            _loadContext.Resolving -= OnResolving;
        }

        private Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            log.Info($"Resolving {name}");

            foreach (var library in _dependencyContext.RuntimeLibraries)
            {
                var wrapper = new CompilationLibrary(
                    library.Type,
                    library.Name,
                    library.Version,
                    library.Hash,
                    library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                    library.Dependencies,
                    library.Serviceable);

                var assemblies = new List<string>();
                _assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies);

                foreach (var assemblyPath in assemblies)
                {
                    if (name.Name == Path.GetFileNameWithoutExtension(assemblyPath))
                    {
                        log.Info($"Using {assemblyPath}");
                        return _loadContext.LoadFromAssemblyPath(assemblyPath);
                    }
                }
            }

            foreach (string frameworkDirectory in AdditionalFrameworkDirectories)
            {
                var versionDir = FindBestVersionDir(frameworkDirectory, name.Version);
                if (versionDir != null)
                {
                    string candidate = Path.Combine(frameworkDirectory, versionDir, name.Name + ".dll");
                    if (File.Exists(candidate))
                    {
                        log.Info($"Using {candidate}");
                        return _loadContext.LoadFromAssemblyPath(candidate);
                    }
                }
            }

            return null;
        }

        private static string GetDotNetInstallDirectory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Running on Windows so use registry
                RegistryKey key = Environment.Is64BitProcess
                    ? Registry.LocalMachine.OpenSubKey(@"Software\dotnet\SetUp\InstalledVersions\x64\sharedHost\")
                    : Registry.LocalMachine.OpenSubKey(@"Software\dotnet\SetUp\InstalledVersions\x86\sharedHost\");
                return (string)key?.GetValue("Path");
            }
            else
                return "/usr/shared/dotnet/";
        }

        private static string FindBestVersionDir(string libraryDir, Version targetVersion)
        {
            if (targetVersion == null)
                return null;

            targetVersion = new Version(targetVersion.Major, targetVersion.Minor, targetVersion.Build);
            Version bestVersion = new Version(0, 0);
            string bestVersionDir = null;

            foreach (var subdir in Directory.GetDirectories(libraryDir))
            {
                Version version;
                if (TryGetVersionFromString(Path.GetFileName(subdir), out version))
                    if (version >= targetVersion)
                        if (bestVersion.Major == 0 || bestVersion > version)
                        {
                            bestVersion = version;
                            bestVersionDir = subdir;
                        }
            }

            return bestVersionDir;
        }

        private static bool TryGetVersionFromString(string text, out Version newVersion)
        {
            const string VERSION_CHARS = ".0123456789";

            int len = 0;
            foreach (char c in text)
            {
                if (VERSION_CHARS.IndexOf(c) >= 0)
                    len++;
                else
                    break;
            }

            try
            {
                newVersion = new Version(text.Substring(0, len));
                return true;
            }
            catch
            {
                newVersion = new Version();
                return false;
            }
        }
    }
}
#endif
