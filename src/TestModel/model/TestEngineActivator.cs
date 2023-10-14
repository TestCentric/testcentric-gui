// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using TestCentric.Engine;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// TestEngineActivator creates an instance of the test engine and returns an ITestEngine interface.
    /// </summary>
    public static class TestEngineActivator
    {
        internal static readonly Version DefaultMinimumVersion = new Version(1, 0);

        private const string DefaultAssemblyName = "testcentric.engine.dll";
        internal const string DefaultTypeName = "TestCentric.Engine.TestEngine";

#if NETSTANDARD2_0
        /// <summary>
        /// Create an instance of the test engine.
        /// </summary>
        /// <returns>An <see cref="NUnit.Engine.ITestEngine"/></returns>
        public static ITestEngine CreateInstance()
        {
            var apiLocation = typeof(TestEngineActivator).Assembly.Location;
            var directoryName = Path.GetDirectoryName(apiLocation);
            var enginePath = directoryName == null ? DefaultAssemblyName : Path.Combine(directoryName, DefaultAssemblyName);
            var assembly = Assembly.LoadFrom(enginePath);
            var engineType = assembly.GetType(DefaultTypeName);
            return Activator.CreateInstance(engineType) as ITestEngine;
        }
#else
        /// <summary>
        /// Create an instance of the test engine.
        /// </summary>
        /// <param name="unused">This parameter is no longer used but has not been removed to ensure API compatibility.</param>
        /// <exception cref="NUnitEngineNotFoundException">Thrown when a test engine of the required minimum version is not found</exception>
        /// <returns>An <see cref="NUnit.Engine.ITestEngine"/></returns>
        public static ITestEngine CreateInstance(bool unused = false)
        {
            return CreateInstance(DefaultMinimumVersion, unused);
        }

        /// <summary>
        /// Create an instance of the test engine with a minimum version.
        /// </summary>
        /// <param name="minVersion">The minimum version of the engine to return inclusive.</param>
        /// <param name="unused">This parameter is no longer used but has not been removed to ensure API compatibility.</param>
        /// <exception cref="NUnitEngineNotFoundException">Thrown when a test engine of the required minimum version is not found</exception>
        /// <returns>An <see cref="ITestEngine"/></returns>
        public static ITestEngine CreateInstance(Version minVersion, bool unused = false)
        {
            try
            {
                Assembly engine = FindEngineAssembly(minVersion);
                if (engine == null)
                {
                    throw new EngineNotFoundException();
                }
                return (ITestEngine)AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(engine.CodeBase, DefaultTypeName);
            }
            catch (EngineNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load the test engine", ex);
            }
        } 

        private static Assembly FindEngineAssembly(Version minVersion)
        {
            // Check the Application BaseDirectory first. If engine exists there, we'll try to use it
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultAssemblyName);
            if (File.Exists(path))
            {
                var assembly = Assembly.ReflectionOnlyLoadFrom(path);
                var version = assembly.GetName().Version;
                if (version >= minVersion) return assembly;
                // If minimum assembly in base directory doesn't meet minimum version requirement, that's an error
                throw new Exception($"Assembly in base directory is version {version} but minimum version required is {minVersion}");
            }

            // Check Probing Path if not found in Base Directory. This allows a runner to be executed
            // using a different application base and still function.
            if (AppDomain.CurrentDomain.RelativeSearchPath != null)
            {
                foreach (string relpath in AppDomain.CurrentDomain.RelativeSearchPath.Split(new char[] { ';' }))
                {
                    path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relpath), DefaultAssemblyName);
                    if (File.Exists(path))
                    {
                        var assembly = Assembly.ReflectionOnlyLoadFrom(path);
                        var version = assembly.GetName().Version;
                        if (version >= minVersion) return assembly;
                        // Just ignore assemblies in probing path if they don't meet minimum requirement
                        // TODO: Should we throw here as for the base directory?
                    }
                }
            }

            // Check for engine loaded as a NuGet or Chocolatey package dependency
            string thisAssemblyPath = typeof(TestEngineActivator).Assembly.Location;
            string thisAssemblyDir = Path.GetDirectoryName(thisAssemblyPath);
            // Is assembly directory 'tools'?
            if (Path.GetFileName(thisAssemblyDir) == "tools")
            {
                string packageDir = Path.GetDirectoryName(thisAssemblyDir);
                // Is it our nuget package?
                if (Path.GetFileName(packageDir).StartsWith("TestCentric.GuiRunner"))
                {
                    var parentDir = Path.GetDirectoryName(packageDir);
                    path = Path.Combine(parentDir, "TestCentric.Engine", "tools", DefaultAssemblyName);
                    if (File.Exists(path))
                    {
                        var assembly = Assembly.ReflectionOnlyLoadFrom(path);
                        var version = assembly.GetName().Version;
                        if (version >= minVersion) return assembly;
                        // TODO: Should we throw here if version does't match?
                    }
                }
            }

            throw new EngineNotFoundException();
        }

        private static Assembly CheckPathForEngine(string path, Version minVersion, ref Version newestVersionFound, Assembly newestAssemblyFound)
        {
            try
            {
                if (path != null && File.Exists(path))
                {
                    var ass = Assembly.ReflectionOnlyLoadFrom(path);
                    var ver = ass.GetName().Version;
                    if (ver >= minVersion && ver > newestVersionFound)
                    {
                        newestVersionFound = ver;
                        newestAssemblyFound = ass;
                    }
                }
            }
            catch (Exception){}
            return newestAssemblyFound;
        }

        private static string FindEngineInRegistry(RegistryKey rootKey, string subKey)
        {
            try
            {
                using (var key = rootKey.OpenSubKey(subKey, false))
                {
                    if (key != null)
                    {
                        Version newest = null;
                        string[] subkeys = key.GetValueNames();
                        foreach (string name in subkeys)
                        {
                            try
                            {
                                var current = new Version(name);
                                if (newest == null || current.CompareTo(newest) > 0)
                                {
                                    newest = current;
                                }
                            }
                            catch (Exception){}
                        }
                        if(newest != null)
                            return key.GetValue(newest.ToString()) as string;
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
#endif
    }
}
