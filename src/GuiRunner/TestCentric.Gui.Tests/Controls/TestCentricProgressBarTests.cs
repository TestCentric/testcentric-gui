// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using NUnit.Framework;

namespace TestCentric.Gui.Controls
{
    [TestFixture]
    public class TestCentricProgressBarTests
    {
        [TestCase(0, 0)]
        [TestCase(50, 100)]
        [TestCase(100, 200)]
        public void GetProgressWidth_MaximumValueIsDefined_ReturnsExpectedValue(int progress, int expectedWidth)
        {
            TestCentricProgressBar progressBar = new TestCentricProgressBar();
            progressBar.Maximum = 100;
            progressBar.Value = progress;

            Rectangle rect = new Rectangle(0, 0, 200, 50);
            int width = progressBar.GetProgressWidth(rect);

            Assert.That(width, Is.EqualTo(expectedWidth));
        }

        [Test]
        public void GetProgressWidth_MaximumValueIsZero_ReturnsZero()
        {
            TestCentricProgressBar progressBar = new TestCentricProgressBar();
            progressBar.Maximum = 0;
            progressBar.Value = 0;

            Rectangle rect = new Rectangle(0, 0, 200, 50);
            int width = progressBar.GetProgressWidth(rect);

            Assert.That(width, Is.EqualTo(0));
        }

        [TestCase(0, 0)]
        [TestCase(50, 50)]
        [TestCase(100, 100)]
        public void GetPercentCompleted_MaximumValueIsDefined_ReturnsExpectedValue(int progress, int expectedPercent)
        {
            TestCentricProgressBar progressBar = new TestCentricProgressBar();
            progressBar.Maximum = 100;
            progressBar.Value = progress;

            int percent = progressBar.GetPercentCompleted();

            Assert.That(percent, Is.EqualTo(expectedPercent));
        }

        [Test]
        public void GetPercentCompleted_MaximumValueIsZero_ReturnsZero()
        {
            TestCentricProgressBar progressBar = new TestCentricProgressBar();
            progressBar.Maximum = 0;
            progressBar.Value = 0;

            int percent = progressBar.GetPercentCompleted();

            Assert.That(percent, Is.EqualTo(0));
        }
    }
}
