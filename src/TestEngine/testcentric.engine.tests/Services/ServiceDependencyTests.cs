// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;

    public class ServiceDependencyTests
    {
        private ServiceContext _services;

        [SetUp]
        public void CreateServiceContext()
        {
            _services = new ServiceContext();
        }

        [Test]
        public void DefaultTestRunnerFactory_ProjectServiceError()
        {
            var fake = new FakeProjectService();
            fake.FailToStart = true;
            _services.Add(fake);
            var service = new DefaultTestRunnerFactory();
            _services.Add(service);
            ((IService)fake).StartService();
            service.StartService();
            Assert.That(service.Status, Is.EqualTo(ServiceStatus.Error));
        }

#if !NETCOREAPP1_1
        [Test]
        public void DefaultTestRunnerFactory_ProjectServiceMissing()
        {
            var service = new DefaultTestRunnerFactory();
            _services.Add(service);
            service.StartService();
            Assert.That(service.Status, Is.EqualTo(ServiceStatus.Error));
        }
#endif
    }
}
