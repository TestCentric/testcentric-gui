// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TestCentric.Engine.TestBed
{
    class CommandLineOptions
    {
        public CommandLineOptions(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                    ProcessOption(arg);
                else
                    Files.Add(arg);
            }
        }

        public List<String> Files { get; } = new List<string>();
        public bool DebugAgent { get; private set; }
        public bool Trace { get; private set; }
        public bool ListExtensions { get; private set; }
        public int Timeout { get; private set; } = 0;

        private void ProcessOption(string arg)
        {
            string opt = arg;
            string val = null;

            int eq = arg.IndexOf('=');
            if (eq > 0)
            {
                opt = arg.Substring(0, eq);
                val = arg.Substring(eq + 1);
            }

            switch (opt)
            {
                case "--trace":
                    Trace = true;
                    break;

                case "--debug-agent":
                    DebugAgent = true;
                    break;

                case "--list-extensions":
                    ListExtensions = true;
                    break;

                case "--timeout":
                    Timeout = int.Parse(val);
                    break;

                default:
                    throw new ArgumentException($"Invalid option: '{arg}'");
            }
        }
    }
}
