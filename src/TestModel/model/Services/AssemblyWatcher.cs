// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.IO;
using System.Timers;

namespace TestCentric.Gui.Model.Services
{
    public class AssemblyWatcher : IAsemblyWatcher
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
