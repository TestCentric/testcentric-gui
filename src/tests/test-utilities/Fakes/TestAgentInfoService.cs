// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;
using TestCentric.Engine;

namespace TestCentric.TestUtilities.Fakes
{
    public class TestAgentInfoService : ITestAgentInfo
    {
        public IList<TestAgentInfo> GetAvailableAgents()
        {
            return new TestAgentInfo[0];
        }

        public IList<TestAgentInfo> GetAvailableAgents(TestPackage package)
        {
            return new TestAgentInfo[0];
        }
    }
}
