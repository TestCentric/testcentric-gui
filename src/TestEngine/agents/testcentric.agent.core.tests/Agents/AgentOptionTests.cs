// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine.Agents
{
    public class AgentOptionTests
    {
        static TestCaseData[] DefaultSettings = new[]
        {
            new TestCaseData("AgentId", Guid.Empty),
            new TestCaseData("AgencyUrl", string.Empty),
            new TestCaseData("AgencyPid", string.Empty),
            new TestCaseData("DebugAgent", false),
            new TestCaseData("DebugTests", false),
            new TestCaseData("TraceLevel", InternalTraceLevel.Off),
            new TestCaseData("WorkDirectory", string.Empty)
        };

        [TestCaseSource(nameof(DefaultSettings))]
        public void DefaultOptionSettings<T>(string propertyName, T defaultValue)
        {
            var options = new AgentOptions();
            var prop = typeof(AgentOptions).GetProperty(propertyName);
            Assert.NotNull(prop, $"Property {propertyName} does not exist");
            Assert.That(prop.GetValue(options, new object[0]), Is.EqualTo(defaultValue));
        }

        static readonly Guid AGENT_GUID = Guid.NewGuid();
        static readonly TestCaseData[] ValidSettings = new[]
        {
            // Boolean options - no values provided
            new TestCaseData("--debug-agent", "DebugAgent", true),
            new TestCaseData("--debug-tests", "DebugTests", true),
            // Options with values - using '=' as delimiter
            new TestCaseData($"--agentId={AGENT_GUID}", "AgentId", AGENT_GUID),
            new TestCaseData("--agencyUrl=THEURL", "AgencyUrl", "THEURL"),
            new TestCaseData("--pid=1234", "AgencyPid", "1234"),
            new TestCaseData("--trace=Info", "TraceLevel", InternalTraceLevel.Info),
            new TestCaseData("--work=WORKDIR", "WorkDirectory", "WORKDIR"),
            // Options with values - using ':' as delimiter
            new TestCaseData("--trace:Error", "TraceLevel", InternalTraceLevel.Error),
            new TestCaseData("--work:WORKDIR", "WorkDirectory", "WORKDIR")
        };

        [TestCaseSource(nameof(ValidSettings))]
        public void ValidOptionSettings<T>(string option, string propertyName, T expectedValue)
        {
            var options = new AgentOptions(option);
            var prop = typeof(AgentOptions).GetProperty(propertyName);
            Assert.NotNull(prop, $"Property {propertyName} does not exist");
            Assert.That(prop.GetValue(options, new object[0]), Is.EqualTo(expectedValue));
        }
    }
}
