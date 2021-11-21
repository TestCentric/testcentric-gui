// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Engine;

namespace TestCentric.Engine.TestBed
{
    /// <summary>
    /// A simple test runner used for testing packages. The
    /// arguments are a list of tests to run, specified as
    /// a relative path to the test assembly from the location
    /// of the runner. No options are supported - all the
    /// tests in each assembly are run.
    /// </summary>
    class TestRunner
    {
        static readonly ITestEngine TestEngine = new TestEngine();

        static void Main(string[] args)
        {
            Console.WriteLine("TestCentric Engine Test Runner");
            Console.WriteLine();

            Console.WriteLine("Test Environment");
            Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
            Console.WriteLine($"  CLR Version: {Environment.Version}");
            Console.WriteLine();


            if (args.Contains("--list-extensions"))
            {
                ListExtensions();
                return;
            }

            var package = MakeTestPackageFromArguments(args);

            Console.WriteLine("Test Files");
            foreach (var file in args)
                    Console.WriteLine("  " + file);
            Console.WriteLine();

            var runner = TestEngine.GetRunner(package);

            XmlNode resultNode = runner.Run(null, TestFilter.Empty);

            string resultFile = "TestResult.xml";
            using (var stream = new FileStream(resultFile, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    xmlWriter.WriteStartDocument(false);
                    resultNode.WriteTo(xmlWriter);
                }

                Console.WriteLine();
                Console.WriteLine($"Saved results to {resultFile}");
            }

            Environment.Exit(0);
        }

        private static void ListExtensions()
        {
            var extensionService = TestEngine.Services.GetService<IExtensionService>();

            Console.WriteLine("Extension Points and Extensions");
            Console.WriteLine();

            foreach (var ep in extensionService.ExtensionPoints)
            {
                Console.WriteLine($"  {ep.Path}");
                foreach (var extension in ep.Extensions)
                    Console.WriteLine($"    {extension.TypeName}");
            }
        }

        private static TestPackage MakeTestPackageFromArguments(string[] args)
        {
            List<string> files = new List<string>();
            bool debugAgent = false;
            bool trace = false;

            foreach (string arg in args)
                if (arg.StartsWith("-"))
                    switch (arg)
                    {
                        case "--trace":
                            trace = true;
                            break;

                        case "--debug-agent":
                            debugAgent = true;
                            break;

                        default:
                            throw new ArgumentException($"Invalid option: '{arg}'");
                    }
                else
                    files.Add(arg);

            var package = new TestPackage(files);
            if (trace)
                package.AddSetting("InternalTraceLevel", "Debug");
            if (debugAgent)
                package.AddSetting("DebugAgent", true);

            return package;
        }
    }
}
