// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using NUnit.Engine;

#if !NETCOREAPP2_1
namespace TestCentric.Engine.Services
{
    public class TestPackageAnalyzerTests
    {
        // NOTE: Tests are a bit fragile, since we can't inject a 
        // current framework to the validator. Need to find a way
        // to fake current framework for better tests.
        private const string VALID_RUNTIME = "net-4.0";
        private const string INVALID_RUNTIME = "invalid-5.0";
        private static readonly string CURRENT_RUNTIME = RuntimeFramework.CurrentFramework.Id;

        private TestPackage _package;
        private TestPackageAnalyzer _analyzer;

        public interface ITestRuntimeService : IRuntimeFrameworkService, IService { }

        [SetUp]
        public void Initialize()
        {
            // Validation doesn't look at the files specified, only settings
            _package = new TestPackage("any.dll");

            var runtimeService = Substitute.For<ITestRuntimeService>();
            runtimeService.IsAvailable("net-2.0").Returns(true);
            runtimeService.IsAvailable("net-4.0").Returns(true);
            runtimeService.IsAvailable("net-4.5").Returns(true);
            runtimeService.IsAvailable(CURRENT_RUNTIME).Returns(true);
            runtimeService.IsAvailable("netcore-3.0").Returns(true); // Not actually available yet, but used to test

            var context = new ServiceContext();
            context.Add(runtimeService);
            context.Add(Substitute.For<ProjectService>());
            context.Add(Substitute.For<TestFrameworkService>());

            context.Add(new TestPackageAnalyzer());
            
            context.ServiceManager.StartServices();

            _analyzer = context.GetService<TestPackageAnalyzer>();
        }

        [Test]
        public void RequestedFrameworkNotSpecified()
        {
            Assert.That(() => Validate(), Throws.Nothing);
        }

        [Test]
        public void RequestedFrameworkInvalid()
        {
            _package.AddSetting(EnginePackageSettings.RequestedRuntimeFramework, INVALID_RUNTIME);

            var exception = Assert.Throws<NUnitEngineException>(() => Validate());

            CheckMessageContent(exception.Message, $"The requested framework {INVALID_RUNTIME} is unknown or not available.");
        }

        [TestCase("ProcessModel")]
        [TestCase("DomainUsage")]
        [TestCase("ProcessModel", "DomainUsage")]
        public void ObsoletePackageSetting(params string[] settings)
        {
            foreach (var setting in settings)
                _package.AddSetting(setting, "something"); // Test doesn't use the value

            var exception = Assert.Throws<NUnitEngineException>(() => Validate());

            var errors = new List<string>();
            foreach (var setting in settings)
                errors.Add($"The {setting} setting is no longer supported.");
            CheckMessageContent(exception.Message, errors.ToArray());
        }

        [Test]
        public void AllPossibleErrors()
        {
            _package.AddSetting(EnginePackageSettings.RequestedRuntimeFramework, INVALID_RUNTIME);
            _package.AddSetting("ProcessModel", "something"); // Test doesn't use the value
            _package.AddSetting("DomainUsage", "something"); // Test doesn't use the value

            var exception = Assert.Throws<NUnitEngineException>(() => Validate());

            CheckMessageContent(exception.Message,
                $"The requested framework {INVALID_RUNTIME} is unknown or not available.",
                "The ProcessModel setting is no longer supported.",
                "The DomainUsage setting is no longer supported.");
        }

        [Test]
        public void RequestedFrameworkValid()
        {
            _package.AddSetting(EnginePackageSettings.RequestedRuntimeFramework, VALID_RUNTIME);
            Assert.That(() => Validate(), Throws.Nothing);
        }

        private void Validate()
        {
            _analyzer.ValidatePackageSettings(_package);
        }

        private void CheckMessageContent(string message, params string[] errors)
        {
            Assert.That(message, Does.StartWith("The following errors were detected in the TestPackage:\n\n"));

            foreach (string error in errors)
                Assert.That(message, Contains.Substring($"\n* {error}\n"));
        }
    }
}
#endif
