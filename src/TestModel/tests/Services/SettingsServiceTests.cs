// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Gui.Model.Services
{
    public class SettingsServiceTests
    {
        private SettingsService _settingsService;

        [SetUp]
        public void CreateService()
        {
            _settingsService = new SettingsService(false);
        }

    }
}
