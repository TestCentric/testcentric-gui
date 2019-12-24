// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Services.Tests
{
    public class FakeSettingsService : SettingsStore, IService
    {
        IServiceLocator IService.ServiceContext { get; set; }

        private ServiceStatus _status;
        ServiceStatus IService.Status
        {
            get { return _status; }
        }

        void IService.StartService()
        {
            _status = FailToStart
                ? ServiceStatus.Error
                : ServiceStatus.Started;
        }

        void IService.StopService()
        {
            _status = ServiceStatus.Stopped;
            if (FailedToStop)
                throw new ArgumentException(nameof(FailedToStop));
        }

        public bool FailToStart { get; set; }

        public bool FailedToStop { get; set; }
    }
}
