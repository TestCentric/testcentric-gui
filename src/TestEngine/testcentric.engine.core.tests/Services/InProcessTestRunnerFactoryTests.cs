// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine.Runners;
using NUnit.Framework;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public class InProcessTestRunnerFactoryTests
    {
        private InProcessTestRunnerFactory _factory;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            _factory = new InProcessTestRunnerFactory();
            services.Add(_factory);
            services.ServiceManager.StartServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_factory.Status, Is.EqualTo(ServiceStatus.Started), "Failed to start service");
        }

#if NETCOREAPP1_1 || NETCOREAPP2_1
        [TestCase("x.dll", typeof(LocalTestRunner))]
        [TestCase("x.dll y.dll", typeof(LocalTestRunner))]
        [TestCase("x.dll y.dll z.dll", typeof(LocalTestRunner))]
#else
        [TestCase("x.dll", typeof(TestDomainRunner))]
        [TestCase("x.dll y.dll", typeof(MultipleTestDomainRunner))]
        [TestCase("x.dll y.dll z.dll", typeof(MultipleTestDomainRunner))]
#endif
        public void CorrectRunnerIsUsed(string files, Type expectedType)
        {
            var package = new TestPackage(files.Split(new char[] { ' ' }));
            Assert.That(_factory.MakeTestRunner(package), Is.TypeOf(expectedType));
        }
    }
}
