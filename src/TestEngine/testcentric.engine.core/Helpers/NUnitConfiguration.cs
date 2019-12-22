// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using NUnit.Engine.Helpers;

namespace NUnit.Engine.Helpers
{
    /// <summary>
    /// Provides static methods for accessing configuration info
    /// </summary>
    public static class NUnitConfiguration
    {
#if !NETSTANDARD1_6

        private static string _engineDirectory;
        public static string EngineDirectory
        {
            get
            {
                if (_engineDirectory == null)
                    _engineDirectory =
                        AssemblyHelper.GetDirectoryName(Assembly.GetExecutingAssembly());

                return _engineDirectory;
            }
        }

#endif

        private static string _applicationDirectory;
        public static string ApplicationDirectory
        {
            get
            {
                if (_applicationDirectory == null)
                {
                    _applicationDirectory = Path.Combine(
#if NETSTANDARD1_6
                    Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "HOME"),
#else
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
#endif
                        "NUnit");
                }

                return _applicationDirectory;
            }
        }
    }
}
