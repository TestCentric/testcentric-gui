// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
