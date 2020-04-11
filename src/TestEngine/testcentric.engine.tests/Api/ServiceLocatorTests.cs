// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Api

{
    public class ServiceLocatorTests
    {
        private ITestEngine _testEngine;

        [OneTimeSetUp]
        public void CreateEngine()
        {
            _testEngine = new TestEngine();
            _testEngine.InternalTraceLevel = InternalTraceLevel.Off;
        }

        [TestCase(typeof(IDriverService))]
        public void CanAccessService(Type serviceType)
        {
            IService service = _testEngine.Services.GetService(serviceType) as IService;
            Assert.NotNull(service, "GetService(Type) returned null");
            Assert.That(service, Is.InstanceOf(serviceType));
            Assert.That(service.Status, Is.EqualTo(ServiceStatus.Started));
        }
    }
}
