// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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
using System.IO;
using System.Timers;

namespace TestCentric.Gui.Model
{
    public class AssemblyWatcher:IAsemblyWatcher,IDisposable
    {
        private FileSystemWatcher[] fileWatchers;
        private FileInfo[] files;

        protected System.Timers.Timer timer;
        protected string changedAssemblyPath;

        protected FileInfo GetFileInfo(int index)
        {
            return files[index];
        }

        public void Setup(int delay, string assemblyFileName)
        {
            Setup(delay, new string[] { assemblyFileName });
        }

        public void Setup(int delay, IList assemblies)
        {

            files = new FileInfo[assemblies.Count];
            fileWatchers = new FileSystemWatcher[assemblies.Count];

            for (int i = 0; i < assemblies.Count; i++)
            {

                files[i] = new FileInfo((string)assemblies[i]);

                fileWatchers[i] = new FileSystemWatcher();
                fileWatchers[i].Path = files[i].DirectoryName;
                fileWatchers[i].Filter = files[i].Name;
                fileWatchers[i].NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;
                fileWatchers[i].Changed += new FileSystemEventHandler(OnChanged);
                fileWatchers[i].EnableRaisingEvents = false;
            }

            timer = new System.Timers.Timer(delay);
            timer.AutoReset = false;
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
        }

        public void Start()
        {
            EnableWatchers(true);
        }

        public void Stop()
        {
            EnableWatchers(false);
        }

        private void EnableWatchers(bool enable)
        {
            if (fileWatchers != null)
                foreach (FileSystemWatcher watcher in fileWatchers)
                    watcher.EnableRaisingEvents = enable;
        }

        public void Dispose()
        {

            Stop();

            if (fileWatchers != null)
            {
                foreach (FileSystemWatcher watcher in fileWatchers)
                {
                    if (watcher != null)
                    {
                        watcher.Changed -= new FileSystemEventHandler(OnChanged);
                        watcher.Dispose();
                    }
                }
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Close();
            }

            fileWatchers = null;
            timer = null;
        }

        public event AssemblyChangedHandler AssemblyChanged;

        protected void OnTimer(Object source, ElapsedEventArgs e)
        {
            lock (this)
            {
                PublishEvent();
                timer.Enabled = false;
            }
        }

        protected void OnChanged(object source, FileSystemEventArgs e)
        {

            changedAssemblyPath = e.FullPath;
            if (timer != null)
            {
                lock (this)
                {
                    if (!timer.Enabled)
                        timer.Enabled = true;
                    timer.Start();
                }
            }
            else
            {
                PublishEvent();
            }
        }

        protected void PublishEvent()
        {
            if (AssemblyChanged != null)
            {
                AssemblyChanged(changedAssemblyPath);
            }
        }
    }
}
