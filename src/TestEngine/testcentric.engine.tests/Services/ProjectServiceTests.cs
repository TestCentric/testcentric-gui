// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCOREAPP1_1
using System;
using NUnit.Framework;

namespace NUnit.Engine.Services
{
    public class ProjectServiceTests
    {
        private ProjectService _projectService;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            services.Add(new ExtensionService());
            _projectService = new ProjectService();
            services.Add(_projectService);
            services.ServiceManager.StartServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_projectService.Status, Is.EqualTo(ServiceStatus.Started));
        }
    }
}
#endif
