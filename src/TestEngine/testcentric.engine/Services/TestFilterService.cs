// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    public class TestFilterService : Service, ITestFilterService
    {
        public ITestFilterBuilder GetTestFilterBuilder()
        {
            return new TestFilterBuilder();
        }
    }
}
