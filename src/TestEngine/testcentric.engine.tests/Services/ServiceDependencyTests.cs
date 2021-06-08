// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;
    using NUnit.Engine;

    public class ServiceDependencyTests
    {
        private ServiceContext _services;
        private FakeService _fake1;
        private FakeService _fake2;

        [SetUp]
        public void CreateServiceContext()
        {
            _fake1 = new FakeService1();
            _fake2 = new FakeService2();
            _services = new ServiceContext();
        }

        [Test]
        public void BothServicesStart()
        {
            _services.Add(_fake1);
            _services.Add(_fake2);
            _services.ServiceManager.StartServices();
            Assert.That(_fake1.Status, Is.EqualTo(ServiceStatus.Started));
            Assert.That(_fake2.Status, Is.EqualTo(ServiceStatus.Started));
        }

        [Test]
        public void FirstServiceFailsToStart()
        {
            _services.Add(_fake1);
            _services.Add(_fake2);
            _fake1.FailToStart = true;
            Assert.That(
                () => _services.ServiceManager.StartServices(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(_fake1.Status, Is.EqualTo(ServiceStatus.Error));
            Assert.That(_fake2.Status, Is.EqualTo(ServiceStatus.Stopped));
        }

        [Test]
        public void SecondServiceFailsToStart()
        {
            _services.Add(_fake1);
            _services.Add(_fake2);
            _fake2.FailToStart = true;
            Assert.That(
                () => _services.ServiceManager.StartServices(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(_fake1.Status, Is.EqualTo(ServiceStatus.Started));
            Assert.That(_fake2.Status, Is.EqualTo(ServiceStatus.Error));
        }

        [Test]
        public void Service1NotAdded()
        {
            _services.Add(_fake2);
            Assert.That(
                () => _services.ServiceManager.StartServices(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(_fake2.Status, Is.EqualTo(ServiceStatus.Error));
        }

        [Test]
        public void ServicesAddedInWrongOrder()
        {
            _services.Add(_fake2);
            _services.Add(_fake1);
            Assert.That(
                () => _services.ServiceManager.StartServices(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(_fake1.Status, Is.EqualTo(ServiceStatus.Stopped));
            Assert.That(_fake2.Status, Is.EqualTo(ServiceStatus.Error));
        }

        private class FakeService1 : FakeService { }

        private class FakeService2 : FakeService
        {
            public override void StartService()
            {
                var fake1 = ServiceContext.GetService<FakeService1>();
                if (fake1 == null || fake1.Status != ServiceStatus.Started)
                    FailToStart = true;

                base.StartService();
            }
        }
    }
}
