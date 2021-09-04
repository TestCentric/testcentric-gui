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
        static readonly char[] DELIMS = new[] { '=', ':' };
        public AgentOptions(params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (IsOption(arg))
                {
                    var opt = arg.Substring(2);
                    var delim = opt.IndexOfAny(DELIMS);
                    string val = null;
                    if (delim > 0)
                    {
                        val = opt.Substring(delim + 1);
                        opt = opt.Substring(0, delim);
                    }

                    switch (opt)
                    {
                        case "agentId":
                            AgentId = new Guid(val);
                            break;
                        case "agencyUrl":
                            AgencyUrl = val;
                            break;
                        case "debug-agent":
                            DebugAgent = true;
                            break;
                        case "debug-tests":
                            DebugTests = true;
                            break;
                        case "trace":
                            TraceLevel = (InternalTraceLevel)Enum.Parse(typeof(InternalTraceLevel), val);
                            break;
                        case "pid":
                            AgencyPid = val;
                            break;
                        case "work":
                            WorkDirectory = val;
                            break;
                    }
                }
            }
        }

        public Guid AgentId { get; } = Guid.Empty;
        public string AgencyUrl { get; } = string.Empty;
        public string AgencyPid { get; } = string.Empty;
        public bool DebugTests { get; } = false;
        public bool DebugAgent { get; } = false;
        public InternalTraceLevel TraceLevel { get; } = InternalTraceLevel.Off;
        public string WorkDirectory { get; } = string.Empty;

        private static bool IsOption(string arg)
        {
            return arg.StartsWith("--");
        }
    }
}
