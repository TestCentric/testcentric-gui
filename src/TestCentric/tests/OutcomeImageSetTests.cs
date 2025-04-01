// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using NUnit.Framework;

namespace TestCentric.Gui
{
    public class OutcomeImageSetTests
    {
        private static readonly string DEFAULT_IMAGE_SET_DIR = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Tree", "Default");

        private readonly OutcomeImageSet DefaultImageSet;

        public OutcomeImageSetTests()
        {
            DefaultImageSet = new OutcomeImageSet(DEFAULT_IMAGE_SET_DIR);
        }

        [Test, Order(1)]
        public void GetAllImages()
        {
            Assert.Multiple(() =>
            {
                Assert.That(DefaultImageSet.LoadCount, Is.EqualTo(0));

                Assert.That(DefaultImageSet.LoadImage("Success"), Is.Not.Null);
                Assert.That(DefaultImageSet.LoadImage("Failure"), Is.Not.Null);
                Assert.That(DefaultImageSet.LoadImage("Ignored"), Is.Not.Null);
                Assert.That(DefaultImageSet.LoadImage("Skipped"), Is.Not.Null);
                Assert.That(DefaultImageSet.LoadImage("Inconclusive"), Is.Not.Null);

                Assert.That(DefaultImageSet.LoadCount, Is.EqualTo(5));
            });
        }

        [Test, Order(2)]
        public void ImagesAreOnlyLoadedOnce()
        {
            Assert.That(DefaultImageSet.LoadCount, Is.EqualTo(5), "Initial count should be 5");

            Assert.That(DefaultImageSet.LoadImage("Success"), Is.Not.Null);
            Assert.That(DefaultImageSet.LoadImage("Failure"), Is.Not.Null);
            Assert.That(DefaultImageSet.LoadImage("Ignored"), Is.Not.Null);
            Assert.That(DefaultImageSet.LoadImage("Skipped"), Is.Not.Null);
            Assert.That(DefaultImageSet.LoadImage("Inconclusive"), Is.Not.Null);

            Assert.That(DefaultImageSet.LoadCount, Is.EqualTo(5), "Count should remain 5 after accessing images again");
        }

        [Test]
        public void BaseDirectoryMustExist()
        {
            Assert.That(() => new OutcomeImageSet("not/a/directory"), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void DirectoryMustContainRequiredImages()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "TempImageSet");
            Directory.CreateDirectory(tempDir);
            Assert.That(() => new OutcomeImageSet(tempDir), Throws.TypeOf<ArgumentException>());
        }
    }
}
