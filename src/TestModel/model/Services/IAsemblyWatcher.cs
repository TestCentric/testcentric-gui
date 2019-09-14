// ***********************************************************************
// Copyright (c) 2010-2018 Charlie Poole
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
