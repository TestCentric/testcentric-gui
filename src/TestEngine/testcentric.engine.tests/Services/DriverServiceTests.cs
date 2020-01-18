// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using NUnit.Framework;
using TestCentric.Engine.Drivers;

namespace TestCentric.Engine.Services
{
    [TestFixture]
    public class DriverServiceTests
    {
        private DriverService _driverService;

        [SetUp]
        public void CreateDriverFactory()
        {
            var serviceContext = new ServiceContext();
#if !NETCOREAPP1_1
            serviceContext.Add(new ExtensionService());
#endif
            _driverService = new DriverService();
            serviceContext.Add(_driverService);
            serviceContext.ServiceManager.StartServices();
        }

        [Test]
        public void ServiceIsStarted()
        {
            Assert.That(_driverService.Status, Is.EqualTo(ServiceStatus.Started), "Failed to start service");
        }


#if NETCOREAPP1_1 || NETCOREAPP2_1
        [TestCase("mock-assembly.dll", false, typeof(NUnitNetStandardDriver))]
        [TestCase("mock-assembly.dll", true, typeof(NUnitNetStandardDriver))]
        [TestCase("notest-assembly.dll", false, typeof(NUnitNetStandardDriver))]
#else
        [TestCase("mock-assembly.dll", false, typeof(NUnit3FrameworkDriver))]
        [TestCase("mock-assembly.dll", true, typeof(NUnit3FrameworkDriver))]
        [TestCase("notest-assembly.dll", false, typeof(NUnit3FrameworkDriver))]
#endif
        [TestCase("mock-assembly.pdb", false, typeof(InvalidAssemblyFrameworkDriver))]
        [TestCase("mock-assembly.pdb", true, typeof(InvalidAssemblyFrameworkDriver))]
        [TestCase("junk.dll", false, typeof(InvalidAssemblyFrameworkDriver))]
        [TestCase("junk.dll", true, typeof(InvalidAssemblyFrameworkDriver))]
        [TestCase("testcentric.engine.core.dll", false, typeof(InvalidAssemblyFrameworkDriver))]
        [TestCase("testcentric.engine.core.dll", true, typeof(SkippedAssemblyFrameworkDriver))]
        [TestCase("notest-assembly.dll", true, typeof(SkippedAssemblyFrameworkDriver))]
        public void CorrectDriverIsUsed(string fileName, bool skipNonTestAssemblies, Type expectedType)
        {
            var package = new TestPackage(Path.Combine(TestContext.CurrentContext.TestDirectory, fileName));
            // HACK: Temporary till we finalize the driver service interface
            if (fileName == "notest-assembly.dll")
                package.Settings[InternalEnginePackageSettings.ImageNonTestAssembly] = true;
            package.Settings[EnginePackageSettings.SkipNonTestAssemblies] = skipNonTestAssemblies;

            var driver = _driverService.GetDriver(
#if !NETCOREAPP1_1
                AppDomain.CurrentDomain,
#endif
                package);

            Assert.That(driver, Is.InstanceOf(expectedType));
        }
    }
}
