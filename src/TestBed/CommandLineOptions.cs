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
                    switch (arg)
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

                        default:
                            throw new ArgumentException($"Invalid option: '{arg}'");
                    }
                else
                    Files.Add(arg);
            }
        }

        public List<String> Files { get; } = new List<string>();
        public bool DebugAgent { get; }
        public bool Trace { get; }
        public bool ListExtensions { get; }
    }
}
