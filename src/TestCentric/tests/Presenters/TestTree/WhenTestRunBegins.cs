// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenTestRunBegins : TreeViewPresenterTestBase
    {
        // Use dedicated test file name; Used for VisualState file too
        const string TestFileName = "TreeViewPresenterTestRunBegin.dll";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _treeDisplayStrategyFactory = new TreeDisplayStrategyFactory();
        }

        [TearDown]
        public void TearDown()
        {
            // Delete VisualState file to prevent any unintended side effects
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        [Test]
        public void WhenTestRunStarts_TreeNodeImagesAreReset()
        {
            // Arrange
            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            var project = new TestCentricProject(_model, TestFileName);
            _model.TestCentricProject.Returns(project);
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Act
            FireRunStartingEvent(1234);

            // Assert
            _view.Received().ResetAllTreeNodeImages();
        }

        [Test]
        public void WhenTestRunStarts_TestModel_ClearResults()
        {
            // Arrange
            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            var project = new TestCentricProject(_model, TestFileName);
            _model.TestCentricProject.Returns(project);
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Act
            FireRunStartingEvent(1234);

            // Assert
            _model.Received().ClearResults();
        }

        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void WhenTestRunStarts_CurrentDisplayFormat_IsSaved_InVisualFile(string displayFormat)
        {
            // Arrange
            _settings.Gui.TestTree.DisplayFormat = displayFormat;
            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            var project = new TestCentricProject(_model, TestFileName);
            _model.TestCentricProject.Returns(project);
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Act
            FireRunStartingEvent(1234);

            // Assert
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            VisualState visualState = VisualState.LoadFrom(fileName);
            Assert.That(visualState.DisplayStrategy, Is.EqualTo(displayFormat));
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void WhenTestRunStarts_CurrentGroupBy_IsSaved_InVisualFile(string groupBy)
        {
            // Arrange
            _settings.Gui.TestTree.DisplayFormat = "FIXTURE_LIST";
            _settings.Gui.TestTree.FixtureList.GroupBy = groupBy;

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            var project = new TestCentricProject(_model, TestFileName);
            _model.TestCentricProject.Returns(project);
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Act
            FireRunStartingEvent(1234);

            // Assert
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            VisualState visualState = VisualState.LoadFrom(fileName);
            Assert.That(visualState.GroupBy, Is.EqualTo(groupBy));
        }

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenTestRunStarts_ResultsAreCleared()
        //{
        //    _settings.RunStarting += Raise.Event<TestEventHandler>(new TestEventArgs(TestAction.RunStarting, "Dummy", 1234));

        //    _view.Received().ClearResults();
        //}
    }
}
