// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace NUnit.Engine.Services.Fakes
{
    public class FakeRuntimeService : FakeService, IRuntimeFrameworkService
    {
        bool IRuntimeFrameworkService.IsAvailable(string framework)
        {
            return true;
        }

        string IRuntimeFrameworkService.SelectRuntimeFramework(TestPackage package)
        {
            return string.Empty;
        }
    }
}
