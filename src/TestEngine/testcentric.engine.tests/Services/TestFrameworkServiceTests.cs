// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Engine.Services
{
    public class TestFrameworkServiceTests
    {
        private static readonly string THIS_ASSEMBLY = Assembly.GetExecutingAssembly().Location;
        private static readonly string NON_TEST_ASSEMBLY = typeof(TestFrameworkService).Assembly.Location;

        // Use same service for all tests, since that's how service is normally used
        private ITestFrameworkService _service = new TestFrameworkService();

        [Test]
        public void KnownFrameworks()
        {
            Assert.That(_service.KnownFrameworks, Is.EqualTo(new[] { "nunit.framework", "nunitlite" }));
        }

        [Test]
        public void GetFrameworkReferenced()
        {
            var framework = _service.GetFrameworkReference(THIS_ASSEMBLY);
            Assert.NotNull(framework);
            Assert.NotNull(framework.FrameworkReference);
            Assert.That(framework.Name, Is.EqualTo("nunit.framework"));
        }

        [Test]
        public void GetFrameworkReferenceReturnsNull()
        {
            var framework = _service.GetFrameworkReference(NON_TEST_ASSEMBLY);
            Assert.Null(framework);
        }
    }
}
