// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.IO;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Engine
{
    [NonParallelizable, Platform(Exclude = "netcore", Reason = "Can't locate engine in other directory under .NET Core")]
    class WorkingDirectoryTests
    {
        private string _origWorkingDir;

        [OneTimeSetUp]
        public void SetWorkingDirToTempDir()
        {
            _origWorkingDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetTempPath());
        }

        [OneTimeTearDown]
        public void ResetWorkingDir()
        {
            Directory.SetCurrentDirectory(_origWorkingDir);
        }

        [Test]
        public void EngineCanBeCreatedFromAnyWorkingDirectory()
        {
            Assert.That(() => new TestEngine(), Throws.Nothing);
        }
    }
}
