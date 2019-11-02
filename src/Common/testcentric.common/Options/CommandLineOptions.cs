// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Mono.Options;

namespace TestCentric.Gui
{
    /// <summary>
    /// CommandLineOptions encapsulates the option settings for TestCentric.
    /// It inherits from the Mono.Options OptionSet class and provides a 
    /// central location for defining and parsing options.
    /// </summary>
    public class CommandLineOptions : OptionSet
    {
        private bool validated;

        #region Constructor

        public CommandLineOptions(params string[] args)
        {
            //this.Add("config=", "Project {CONFIG} to load (e.g.: Debug).",
            //    v => ActiveConfig = v);

            this.Add("noload", "Suppress loading of the most recent test file.",
                v => NoLoad = v != null);

            this.Add("run", "Automatically run the loaded tests.",
                v => RunAllTests = v != null);

            this.Add("process=", "{PROCESS} isolation for test assemblies. Values: InProcess, Separate, Multiple. If not specified, defaults to Separate for a single assembly or Multiple for more than one.",
                v =>
                {
                    if (CheckRequiredValue(v, "--process", "InProcess", "Separate", "Multiple"))
                        ProcessModel = v;
                });

            this.Add("x86", "Run tests in an X86 process on 64-bit systems.",
                v => RunAsX86 = v != null);

            this.Add("agents=", "Specify maximum {NUMBER} of test assembly agents to run at one time. If not specified, there is no limit.",
                v =>
                {
                    if (CheckRequiredInt(v, "--agents", out int val))
                        MaxAgents = val;
                });

            this.Add("inprocess", "Synonym for --process.InProcess.",
                v => ProcessModel = "Single");

            this.Add("domain=", "{DOMAIN} isolation for test assemblies. Values: None, Single, Multiple. If not specified, defaults to Single for a single assembly or Multiple for more than one.",
                v =>
                {
                    if (CheckRequiredValue(v, "--domain", "None", "Single", "Multiple"))
                        DomainUsage = v;
                });

            this.Add("work=", "{PATH} of the directory to use for output files. If not specified, defaults to the current directory.",
                v =>
                {
                    if (CheckRequiredValue(v, "--work"))
                        WorkDirectory = v;
                });

            //this.Add("runselected", "Automatically run last selected tests.",
            //    v => RunSelectedTests = v != null);

            this.Add("trace=", "Set internal trace {LEVEL}. Valid values are Off, Error, Warning, Info or Debug. Verbose is a synonym for Debug.",
                v =>
                {
                if (CheckRequiredValue(v, "--trace", "Off", "Error", "Warning", "Info", "Verbose", "Debug"))
                    InternalTraceLevel = v;
                });

#if DEBUG
            this.Add("debug-agent", "Launch debugger in nunit-agent when it starts.",
                v => DebugAgent = v != null);
#endif

            this.Add("help|h", "Display the help message and exit.",
                v => ShowHelp = v != null);

            // Default
            this.Add("<>", v =>
            {
                if (v.StartsWith("-") || v.StartsWith("/") && Path.DirectorySeparatorChar != '/')
                    ErrorMessages.Add("Invalid argument: " + v);
                else
                    InputFiles.Add(v);
            });

            if (args != null)
                this.Parse(args);
        }

#endregion

#region Properties

        // Action to Perform

        public bool ShowHelp { get; private set; }
        public bool NoLoad { get; private set; }
        public bool RunAllTests { get; private set; }

        // Select tests

        private List<string> inputFiles = new List<string>();
        public IList<string> InputFiles { get { return inputFiles; } }

        public string ActiveConfig { get; private set; }

        // How to Run Tests

        public string ProcessModel { get; private set; }
        public string DomainUsage { get; private set; }
        public bool RunAsX86 { get; private set; }
        public int MaxAgents { get; private set; }
        public string InternalTraceLevel { get; private set; }
        public string WorkDirectory { get; private set; }
        public bool DebugAgent { get; private set; }

        // Error Processing

        private List<string> errorMessages = new List<string>();
        public IList<string> ErrorMessages { get { return errorMessages; } }

#endregion

#region Public Methods

        public bool Validate()
        {
            if (!validated)
            {
                // Additional Checks here

                validated = true;
            }

            return ErrorMessages.Count == 0;
        }

        public string GetHelpText()
        {
            var writer = new StringWriter();
            writer.WriteLine("TESTCENTRIC [inputfiles] [options]");
            writer.WriteLine();
            writer.WriteLine("Starts the TestCentric Runner, optionally loading and running a set of NUnit tests. You may specify any combination of assemblies and supported project files as arguments.");
            writer.WriteLine();
            writer.WriteLine("InputFiles:");
            writer.WriteLine("   One or more assemblies or test projects of a recognized type.");
            writer.WriteLine("   If no input files are given, the tests contained in the most");
            writer.WriteLine("   recently used project or assembly are loaded, unless the");
            writer.WriteLine("   --noload option is specified");
            writer.WriteLine();
            writer.WriteLine("Options:");

            WriteOptionDescriptions(writer);

            return writer.ToString();
        }

#endregion

#region Helper Methods

        private bool CheckRequiredValue(string val, string option, params string[] validValues)
        {
            if (validValues == null || validValues.Length == 0)
                return true;

            foreach (string valid in validValues)
                if (string.Compare(valid, val, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;

            ErrorMessages.Add(string.Format("The value '{0}' is not valid for option '{1}'.", val, option));
            return false;
        }

        private bool CheckRequiredInt(string val, string option, out int result)
        {
            // We have to return something even though the val will 
            // be ignored if an error is reported. The -1 val seems
            // like a safe bet in case it isn't ignored due to a bug.
            result = -1;

            if (int.TryParse(val, out result))
                return true;

            ErrorMessages.Add("An int value was expected for option '{0}' but a value of '{1}' was used");
            return false;
        }

#endregion
    }
}
