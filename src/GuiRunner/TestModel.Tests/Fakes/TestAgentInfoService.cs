// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using TestCentric.Engine;
using TestCentric.Engine.Services;

namespace TestCentric.Gui.Model.Fakes
{
    public class TestAgentInfoService : ITestAgentInfo
    {
        public IList<TestAgentInfo> GetAvailableAgents()
        {
            return new TestAgentInfo[0];
        }

        public IList<TestAgentInfo> GetAgentsForPackage(TestPackage package)
        {
            return new TestAgentInfo[0];
        }
    }
}
