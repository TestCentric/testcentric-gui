// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace TestCentric.Gui.Model.Services
{
    public class AssemblyWatcher : IAsemblyWatcher
    {
        private List<FileSystemWatcher> fileWatchers;
        private List<FileInfo> files;

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

        public void Setup(int delay, IList<string> assemblies)
        {

            files = new List<FileInfo>();
            fileWatchers = new List<FileSystemWatcher>();

            foreach(string assemblyName in assemblies)
            {
                var fileInfo = new FileInfo(assemblyName);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    continue;
                }

                files.Add(fileInfo);

                var fileWatcher = new FileSystemWatcher();
                fileWatcher.Path = fileInfo.DirectoryName;
                fileWatcher.Filter = fileInfo.Name;
                fileWatcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;
                fileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                fileWatcher.EnableRaisingEvents = false;

                fileWatchers.Add(fileWatcher);
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
