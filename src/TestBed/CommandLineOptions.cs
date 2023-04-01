// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;

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
                else if (File.Exists(arg))
                    Files.Add(arg);
                else
                    throw new FileNotFoundException($"File not found: {arg}");
            }

            if (CancelTimeout > 0 && StopTimeout > CancelTimeout)
                throw new ArgumentException("Value of --stop may not be greater than that of --cancel");
        }

        public List<String> Files { get; } = new List<string>();
        public bool DebugAgent { get; private set; }
        public bool Trace { get; private set; }
        public bool ListExtensions { get; private set; }
        public int StopTimeout { get; private set; } = 0;
        public int CancelTimeout { get; private set; } = 0;

        public string WorkDirectory { get; private set; }

        private void ProcessOption(string arg)
        {
            string opt = arg;
            string val = null;

            int eq = arg.IndexOf('=');
            if (eq < 0)
                eq = arg.IndexOf(':');

            if (eq > 0)
            {
                opt = arg.Substring(0, eq);
                val = arg.Substring(eq + 1);
            }

            switch (opt)
            {
                case "--work":
                    WorkDirectory = val;
                    break;

                case "--trace":
                    Trace = true;
                    break;

                case "--debug-agent":
                    DebugAgent = true;
                    break;

                case "--list-extensions":
                    ListExtensions = true;
                    break;

                case "--stop":
                    StopTimeout = int.Parse(val);
                    break;

                case "--cancel":
                    CancelTimeout = int.Parse(val);
                    break;

                default:
                    throw new ArgumentException($"Invalid option: '{arg}'");
            }
        }
    }
}
