// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    public class SettingsServiceTests
    {
        private SettingsService _settingsService;

        [SetUp]
        public void CreateServiceContext()
        {
            var services = new ServiceContext();
            _settingsService = new SettingsService(false);
            services.Add(_settingsService);
            services.ServiceManager.StartServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_settingsService.Status, Is.EqualTo(ServiceStatus.Started));
        }
    }
}
