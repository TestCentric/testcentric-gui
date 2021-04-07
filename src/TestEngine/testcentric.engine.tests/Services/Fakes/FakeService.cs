// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;

namespace TestCentric.Engine.Services.Fakes
{
    public class FakeService : IService
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
        }

        // Set to true to cause the service to give
        // an error result when started
        public bool FailToStart { get; set; }

        // Set to true to cause the service to give
        // an error result when stopped
        public bool FailedToStop { get; set; }
    }
}
