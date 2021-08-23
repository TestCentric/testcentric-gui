// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using TestCentric.Engine.TestUtilities;

namespace TestCentric.Engine.Integration
{
    internal sealed class DirectoryWithNeededAssemblies : IDisposable
    {
        public string Directory { get; }

        /// <summary>
        /// Returns the transitive closure of assemblies needed to copy.
        /// Deals with assembly names rather than paths to work with runners that shadow copy.
        /// </summary>
        public DirectoryWithNeededAssemblies(params string[] assemblyNames)
        {
            Directory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(Directory);

            foreach (var neededAssembly in ShadowCopyUtils.GetAllNeededAssemblyPaths(assemblyNames))
            {
                File.Copy(neededAssembly, Path.Combine(Directory, Path.GetFileName(neededAssembly)));
            }
        }

        public void Dispose()
        {
            System.IO.Directory.Delete(Directory, true);
        }
    }
}
