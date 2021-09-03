// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Engine;

namespace TestCentric.Engine.Agents
{
    public class AgentOptions
    {
        public AgentOptions(params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                // NOTE: we can test these strings exactly since
                // they originate from the engine itself.
                if (arg.StartsWith("--agentId="))
                {
                    AgentId = new Guid(arg.Substring(10));
                }
                else if (arg.StartsWith("--agencyUrl="))
                {
                    AgencyUrl = arg.Substring(12);
                }
                else if (arg == "--debug-agent")
                {
                    DebugAgent = true;
                }
                else if (arg.StartsWith("--trace="))
                {
                    TraceLevel = (InternalTraceLevel)Enum.Parse(typeof(InternalTraceLevel), arg.Substring(8));
                }
                else if (arg.StartsWith("--pid="))
                {
                    AgencyPid = arg.Substring(6);
                }
                else if (arg.StartsWith("--work="))
                {
                    WorkDirectory = arg.Substring(7);
                }
            }
        }

        public Guid AgentId { get; } = Guid.Empty;
        public string AgencyUrl { get; } = string.Empty;
        public string AgencyPid { get; } = string.Empty;
        public bool RunAsX86 { get; } = false;
        public bool DebugTests { get; } = false;
        public bool DebugAgent { get; } = false;
        public bool LoadUserProfile { get; } = false;
        public InternalTraceLevel TraceLevel { get; } = InternalTraceLevel.Off;
        public string WorkDirectory { get; } = string.Empty;
    }
}
