// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Summary description for UserSettingsService.
    /// </summary>
    public class SettingsService : SettingsStore, IService
    {
        private const string SETTINGS_FILE = "Nunit30Settings.xml";

        public SettingsService(bool writeable)
            : base(Path.Combine(NUnitConfiguration.ApplicationDirectory, SETTINGS_FILE), writeable) { }

        public IServiceLocator ServiceContext { get; set; }

        public ServiceStatus Status { get; private set; }

        public void StartService()
        {
            try
            {
                LoadSettings();

                Status = ServiceStatus.Started;
            }
            catch
            {
                Status = ServiceStatus.Error;
                throw;
            }
        }

        public void StopService()
        {
            try
            {
                SaveSettings();
            }
            finally
            {
                Status = ServiceStatus.Stopped;
            }
        }
    }
}
