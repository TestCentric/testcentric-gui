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

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenTestRunStarts_ResultsAreCleared()
        //{
        //    _settings.RunStarting += Raise.Event<TestEventHandler>(new TestEventArgs(TestAction.RunStarting, "Dummy", 1234));

        //    _view.Received().ClearResults();
        //}
    }
}
