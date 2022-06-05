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
    class TestBed
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

            var options = new CommandLineOptions(args);

            if (options.ListExtensions)
            {
                ListExtensions();
                return;
            }

            Console.WriteLine("Test Files");
            foreach (var file in options.Files)
                    Console.WriteLine("  " + file);
            Console.WriteLine();

            var package = new TestPackage(options.Files);

            if (options.Trace)
            {
                package.AddSetting("InternalTraceLevel", "Debug");
                TestEngine.InternalTraceLevel = InternalTraceLevel.Debug;
            }

            if (options.DebugAgent)
                package.AddSetting("DebugAgent", true);

            var runner = TestEngine.GetRunner(package);

            XmlNode resultNode = runner.Run(new TestEventHandler(), TestFilter.Empty);

            ResultReporter.ReportResults(resultNode);

            SaveTestResults(resultNode); 

            Environment.Exit(0);
        }

        static void SaveTestResults(XmlNode resultNode)
        {
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
        }

        private static void ListExtensions()
        {
            var extensionService = TestEngine.Services.GetService<IExtensionService>();

            Console.WriteLine("Extension Points and Installed Extensions");
            Console.WriteLine();

            foreach (var ep in extensionService.ExtensionPoints)
            {
                Console.WriteLine($"  Extension Point: {ep.Path}");
                int extensionCount = 0;
                foreach (var extension in ep.Extensions)
                {
                    Console.WriteLine($"    Extension: {extension.TypeName}");
                    Console.WriteLine($"     Assembly: {extension.AssemblyPath}");
                    ++extensionCount;
                }
            }
        }
    }
}
