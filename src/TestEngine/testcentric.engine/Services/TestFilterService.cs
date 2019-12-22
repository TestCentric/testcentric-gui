// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.Engine.Services
{
    public class TestFilterService : Service, ITestFilterService
    {
        public ITestFilterBuilder GetTestFilterBuilder()
        {
            return new TestFilterBuilder();
        }
    }
}
