// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using NUnit.Framework;
using TestCentric.Engine.Helpers;

namespace TestCentric.Engine.Runners
{
    // Functional tests of the NUnitFrameworkDriver calling into the framework.
    public abstract class NonRunnableAssemblyRunnerTests
    {
        protected string _expectedRunState;
        protected string _expectedReason;
        protected string _expectedResult;
        protected string _expectedLabel;

        [TestCase("junk.dll", "Assembly")]
        [TestCase("junk.exe", "Assembly")]
        [TestCase("junk.cfg", "Unknown")]
        public void Load(string filePath, string expectedType)
        {
            var package = new TestPackage(filePath);
            var runner = CreateRunner(package);
            var result = runner.Load().Xml;

            Assert.That(result.Name, Is.EqualTo("test-suite"));
            Assert.That(result.GetAttribute("type"), Is.EqualTo(expectedType));
            Assert.That(result.GetAttribute("id"), Is.EqualTo(package.ID));
            Assert.That(result.GetAttribute("name"), Is.EqualTo(filePath));
            Assert.That(result.GetAttribute("fullname"), Is.EqualTo(Path.GetFullPath(filePath)));
            Assert.That(result.GetAttribute("runstate"), Is.EqualTo(_expectedRunState));
            Assert.That(result.GetAttribute("testcasecount"), Is.EqualTo("0"));
            Assert.That(GetSkipReason(result), Is.EqualTo(_expectedReason));
            Assert.That(result.SelectNodes("test-suite").Count, Is.EqualTo(0), "Load result should not have child tests");
        }

        [TestCase("junk.dll", "Assembly")]
        [TestCase("junk.exe", "Assembly")]
        [TestCase("junk.cfg", "Unknown")]
        public void Explore(string filePath, string expectedType)
        {
            var package = new TestPackage(filePath);
            var runner = CreateRunner(package);
            var result = runner.Explore(TestFilter.Empty).Xml;

            Assert.That(result.Name, Is.EqualTo("test-suite"));
            Assert.That(result.GetAttribute("id"), Is.EqualTo(package.ID));
            Assert.That(result.GetAttribute("name"), Is.EqualTo(filePath));
            Assert.That(result.GetAttribute("fullname"), Is.EqualTo(Path.GetFullPath(filePath)));
            Assert.That(result.GetAttribute("type"), Is.EqualTo(expectedType));
            Assert.That(result.GetAttribute("runstate"), Is.EqualTo(_expectedRunState));
            Assert.That(result.GetAttribute("testcasecount"), Is.EqualTo("0"));
            Assert.That(GetSkipReason(result), Is.EqualTo(_expectedReason));
            Assert.That(result.SelectNodes("test-suite").Count, Is.EqualTo(0), "Result should not have child tests");
        }

        [TestCase("junk.dll")]
        [TestCase("junk.exe")]
        [TestCase("junk.cfg")]
        public void CountTestCases(string filePath)
        {
            ITestEngineRunner runner = CreateRunner(new TestPackage(filePath));
            Assert.That(runner.CountTestCases(TestFilter.Empty), Is.EqualTo(0));
        }

        [TestCase("junk.dll", "Assembly")]
        [TestCase("junk.exe", "Assembly")]
        [TestCase("junk.cfg", "Unknown")]
        public void Run(string filePath, string expectedType)
        {
            var package = new TestPackage(filePath);
            ITestEngineRunner runner = CreateRunner(package);
            var result = runner.Run(new NullListener(), TestFilter.Empty).Xml;
            Assert.That(result.Name, Is.EqualTo("test-suite"));
            Assert.That(result.GetAttribute("id"), Is.EqualTo(package.ID));
            Assert.That(result.GetAttribute("name"), Is.EqualTo(filePath));
            Assert.That(result.GetAttribute("fullname"), Is.EqualTo(Path.GetFullPath(filePath)));
            Assert.That(result.GetAttribute("type"), Is.EqualTo(expectedType));
            Assert.That(result.GetAttribute("runstate"), Is.EqualTo(_expectedRunState));
            Assert.That(result.GetAttribute("testcasecount"), Is.EqualTo("0"));
            Assert.That(GetSkipReason(result), Is.EqualTo(_expectedReason));
            Assert.That(result.SelectNodes("test-suite").Count, Is.EqualTo(0), "Load result should not have child tests");
            Assert.That(result.GetAttribute("result"), Is.EqualTo(_expectedResult));
            Assert.That(result.GetAttribute("label"), Is.EqualTo(_expectedLabel));
            Assert.That(result.SelectSingleNode("reason/message").InnerText, Is.EqualTo(_expectedReason));
        }

        protected abstract ITestEngineRunner CreateRunner(TestPackage package);

        private static string GetSkipReason(XmlNode result)
        {
            var propNode = result.SelectSingleNode(string.Format("properties/property[@name='{0}']", NUnit.Framework.Internal.PropertyNames.SkipReason));
            return propNode == null ? null : propNode.GetAttribute("value");
        }

        private class NullListener : ITestEventListener
        {
            public void OnTestEvent(string testEvent)
            {
                // No action
            }
        }
    }

    public class InvalidAssemblyRunnerTests : NonRunnableAssemblyRunnerTests
    {
        public InvalidAssemblyRunnerTests()
        {
            _expectedRunState = "NotRunnable";
            _expectedReason = "Assembly could not be found";
            _expectedResult = "Failed";
            _expectedLabel = "Invalid";
        }

        protected override ITestEngineRunner CreateRunner(TestPackage package)
        {
            return new InvalidAssemblyRunner(package, _expectedReason);
        }
    }

    public class SkippedAssemblyRunnerTests : NonRunnableAssemblyRunnerTests
    {
        public SkippedAssemblyRunnerTests()
        {
            _expectedRunState = "Runnable";
            _expectedReason = "Skipping non-test assembly";
            _expectedResult = "Skipped";
            _expectedLabel = "NoTests";
        }

        protected override ITestEngineRunner CreateRunner(TestPackage package)
        {
            return new SkippedAssemblyRunner(package);
        }
    }
}
