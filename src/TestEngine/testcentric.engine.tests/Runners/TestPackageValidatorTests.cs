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
namespace TestCentric.Engine.Runners
{
    public class TestPackageValidatorTests
    {
        // NOTE: Tests are a bit fragile, since we can't inject a 
        // current framework to the validator. Need to find a way
        // to fake current framework for better tests.
        private const string VALID_RUNTIME = "net-4.0";
        private const string INVALID_RUNTIME = "invalid-5.0";
        private static readonly string CURRENT_RUNTIME = RuntimeFramework.CurrentFramework.Id;

        private TestPackage _package;
        private IRuntimeFrameworkService _runtimeService;
        private TestPackageValidator _validator;

        [SetUp]
        public void Initialize()
        {
            // Validation doesn't look at the files specified, only settings
            _package = new TestPackage("any.dll");

            _runtimeService = Substitute.For<IRuntimeFrameworkService>();
            _runtimeService.IsAvailable("net-2.0").Returns(true);
            _runtimeService.IsAvailable("net-4.0").Returns(true);
            _runtimeService.IsAvailable("net-4.5").Returns(true);
            _runtimeService.IsAvailable(CURRENT_RUNTIME).Returns(true);
            _runtimeService.IsAvailable("netcore-3.0").Returns(true); // Not actually available yet, but used to test

            _validator = new TestPackageValidator(_runtimeService);
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
            var exception = Assert.Catch<Exception>(() => Validate());
            Assert.That(exception.Message, Is.EqualTo($"The requested framework {INVALID_RUNTIME} is unknown or not available."));
        }

        [Test]
        public void RequestedFrameworkValid()
        {
            _package.AddSetting(EnginePackageSettings.RequestedRuntimeFramework, VALID_RUNTIME);
            Assert.That(() => Validate(), Throws.Nothing);
        }

        private void Validate()
        {
            _validator.Validate(_package);
        }
    }
}
#endif
