// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Engine.Extensibility
{
    public class ExtensionAssemblyTests
    {
        private static readonly Assembly THIS_ASSEMBLY = Assembly.GetExecutingAssembly();
        private static readonly string THIS_ASSEMBLY_PATH = THIS_ASSEMBLY.Location;
        private static readonly string THIS_ASSEMBLY_FULL_NAME = THIS_ASSEMBLY.GetName().FullName;
        private static readonly string THIS_ASSEMBLY_NAME = THIS_ASSEMBLY.GetName().Name;
        private static readonly Version THIS_ASSEMBLY_VERSION = THIS_ASSEMBLY.GetName().Version;

        private ExtensionAssembly _ea;

        [OneTimeSetUp]
        public void CreateExtensionAssemblies()
        {
            _ea = new ExtensionAssembly(THIS_ASSEMBLY_PATH, false);
        }

        [Test]
        public void Name()
        {
            Assert.That(_ea.Name, Is.EqualTo(THIS_ASSEMBLY_NAME));
        }

        [Test]
        public void FullName()
        {
            Assert.That(_ea.FullName, Is.EqualTo(THIS_ASSEMBLY_FULL_NAME));
        }

        [Test]
        public void MainModule()
        {
            Assert.That(_ea.MainModule.Assembly.FullName, Is.EqualTo(THIS_ASSEMBLY_FULL_NAME));
        }

        [Test]
        public void AssemblyVersion()
        {
            Assert.That(_ea.AssemblyVersion, Is.EqualTo(THIS_ASSEMBLY_VERSION));
        }

        [Test]
        public void TargetFramework()
        {
#if NETCOREAPP2_1
            Assert.That(_ea.TargetFramework.ToString, Is.EqualTo("netcore-2.1"));
#else
            Assert.That(_ea.TargetFramework.ToString, Is.EqualTo("net-2.0"));
#endif
        }
    }
}
