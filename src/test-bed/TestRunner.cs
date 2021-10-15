// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
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
        static void Main(string[] args)
        {
            Console.WriteLine("TestCentric Engine Test Runner");
            Console.WriteLine();

            Console.WriteLine("Test Environment");
            Console.WriteLine($"   OS Version: {Environment.OSVersion.VersionString}");
            Console.WriteLine($"  CLR Version: {Environment.Version}");
            Console.WriteLine();

            var package = MakeTestPackageFromArguments(args);

            Console.WriteLine("Test Files");
            foreach (var file in args)
                    Console.WriteLine("  " + file);
            Console.WriteLine();

            var engine = new TestEngine();
            var runner = engine.GetRunner(package);

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

        private static TestPackage MakeTestPackageFromArguments(string[] args)
        {
            foreach (string arg in args)
                if (arg.StartsWith("-"))
                    throw new ArgumentException($"Options are not supported by this runner: '{arg}'");

            return new TestPackage(args);
        }
    }
}
