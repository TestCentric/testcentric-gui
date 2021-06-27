// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    using Fakes;
    using NUnit.Engine;

    public class TestAgencyTests
    {
        private TestAgency _testAgency;

        [SetUp]
        public void CreateTestAgency()
        {
            _testAgency = new TestAgency("TestAgencyTest", 0);
            _testAgency.StartService();
        }

        [TearDown]
        public void StopServices()
        {
            _testAgency.StopService();
        }
    }
}
#endif
