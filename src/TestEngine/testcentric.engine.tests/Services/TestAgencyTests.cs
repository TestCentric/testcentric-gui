// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCOREAPP1_1 && !NETCOREAPP2_1
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;

    public class TestAgencyTests
    {
        private TestAgency _testAgency;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            services.Add(new FakeRuntimeService());
            // Use a different URI to avoid conflicting with the "real" TestAgency
            _testAgency = new TestAgency("TestAgencyTest", 0);
            services.Add(_testAgency);
            services.ServiceManager.StartServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_testAgency.Status, Is.EqualTo(ServiceStatus.Started));
        }
    }
}
#endif
