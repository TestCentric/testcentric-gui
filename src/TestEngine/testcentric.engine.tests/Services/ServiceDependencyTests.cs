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

        [SetUp]
        public void CreateServiceContext()
        {
            _services = new ServiceContext();
        }

        [Test]
        public void BothServicesStart()
        {
            _services.Add(new FakeService1());
            _services.Add(new FakeService2());
            _services.ServiceManager.StartServices();
            var fake1 = _services.GetService<FakeService1>();
            var fake2 = _services.GetService<FakeService2>();
            Assert.That(fake1.Status, Is.EqualTo(ServiceStatus.Started));
            Assert.That(fake2.Status, Is.EqualTo(ServiceStatus.Started));
        }

        [Test]
        public void FirstServiceFailsToStart()
        {
            _services.Add(new FakeService1() { FailToStart = true });
            _services.Add(new FakeService2());
            _services.ServiceManager.StartServices();
            Assert.That(
                () => _services.GetService<FakeService1>(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(
                () => _services.GetService<FakeService2>(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void SecondServiceFailsToStart()
        {
            _services.Add(new FakeService1());
            _services.Add(new FakeService2() { FailToStart = true });
            _services.ServiceManager.StartServices();
            var fake1 = _services.GetService<FakeService1>();
            Assert.That(fake1.Status, Is.EqualTo(ServiceStatus.Started));
            Assert.That(
                () => _services.GetService<FakeService2>(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Service1NotAdded()
        {
            _services.Add(new FakeService2());
            _services.ServiceManager.StartServices();
            var fake1 = _services.GetService<FakeService1>();
            Assert.Null(fake1);
            Assert.That(
                () => _services.GetService<FakeService2>(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ServicesAddedInWrongOrder()
        {
            _services.Add(new FakeService2());
            _services.Add(new FakeService1());
            _services.ServiceManager.StartServices();
            var fake1 = _services.GetService<FakeService1>();
            var fake2 = _services.GetService<FakeService2>();
            Assert.That(fake1.Status, Is.EqualTo(ServiceStatus.Started));
            Assert.That(fake2.Status, Is.EqualTo(ServiceStatus.Started));
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
