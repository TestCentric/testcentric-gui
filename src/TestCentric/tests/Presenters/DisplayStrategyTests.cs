// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters
{
    [TestFixture]
    internal class DisplayStrategyTests
    {
        [TestCase("Passed", true, TestTreeView.SuccessIndex)]
        [TestCase("Passed", false, TestTreeView.SuccessIndex_NotLatestRun)]
        [TestCase("Failed", true, TestTreeView.FailureIndex)]
        [TestCase("Failed", false, TestTreeView.FailureIndex_NotLatestRun)]
        [TestCase("Warning", true, TestTreeView.WarningIndex)]
        [TestCase("Warning", false, TestTreeView.WarningIndex_NotLatestRun)]
        [TestCase("Inconclusive", true, TestTreeView.InconclusiveIndex)]
        [TestCase("Inconclusive", false, TestTreeView.InconclusiveIndex_NotLatestRun)]
        [TestCase("Skipped", false, TestTreeView.SkippedIndex)]
        public void CalcImageIndex_ReturnsExpectedIndex(string result, bool isLatestRun, int expectedImageIndex)
        {
            // Arrange
            var resultNode = new ResultNode($"<test-case id='1' result='{result}'/>");
            resultNode.IsLatestRun = isLatestRun;

            // Act
            int imageIndex = DisplayStrategy.CalcImageIndex(resultNode);

            // Assert
            Assert.That(imageIndex, Is.EqualTo(expectedImageIndex));
        }

        [TestCase(true, TestTreeView.IgnoredIndex)]
        [TestCase(false, TestTreeView.IgnoredIndex_NotLatestRun)]
        public void CalcImageIndex_IgnoreTest_ReturnsExpectedIndex(bool isLatestRun, int expectedImageIndex)
        {
            // Arrange
            var resultNode = new ResultNode($"<test-case id='1' result='Skipped' label='Ignored' />");
            resultNode.IsLatestRun = isLatestRun;

            // Act
            int imageIndex = DisplayStrategy.CalcImageIndex(resultNode);

            // Assert
            Assert.That(imageIndex, Is.EqualTo(expectedImageIndex));
        }
    }
}
