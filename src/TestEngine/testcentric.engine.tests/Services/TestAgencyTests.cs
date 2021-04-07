// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;
    using NUnit.Engine;

    public class TestAgencyTests
    {
        private ServiceContext _services;
        private TestAgency _testAgency;

        [SetUp]
        public void StartServices()
        {
            _services = new ServiceContext();
            _services.Add(new FakeRuntimeService());
            // Use a different URI to avoid conflicting with the "real" TestAgency
            _testAgency = new TestAgency("TestAgencyTest", 0);
            _services.Add(_testAgency);
            _services.ServiceManager.StartServices();
        }

        [TearDown]
        public void StopServices()
        {
            _services.ServiceManager.StopServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_testAgency.Status, Is.EqualTo(ServiceStatus.Started));
        }
    }
}
#endif
