// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TestCentric.Gui.Model.Services
{
    public delegate void AssemblyChangedHandler(string fullPath);
    public interface IAsemblyWatcher : IDisposable
    {
        /// <summary>
        /// Stops watching for changes.
        /// To release resources call FreeResources.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts watching for assembly changes.
        /// You need to call Setup before start watching.
        /// </summary>
        void Start();

        /// <summary>
        /// Initializes the watcher with assemblies to observe for changes.
        /// </summary>
        /// <param name="delayInMs">The delay in ms.</param>
        /// <param name="assemblies">The assemblies.</param>

        void Setup(int delayInMs, IList assemblies);


        /// <summary>
        /// Initializes the watcher with assemblies to observe for changes.
        /// </summary>
        /// <param name="delayInMs">The delay in ms.</param>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        void Setup(int delayInMs, string assemblyFileName);

        /// <summary>
        /// Releases all resources held by the watcher.
        /// </summary>

        /// <summary>
        /// Occurs when an assembly being watched has changed.
        /// </summary>
        event AssemblyChangedHandler AssemblyChanged;
    }
}
