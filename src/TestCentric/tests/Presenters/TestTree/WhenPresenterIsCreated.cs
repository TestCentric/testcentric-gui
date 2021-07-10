// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenPresenterIsCreated : TreeViewPresenterTestBase
    {
        [TestCase("RunAllCommand")]
        [TestCase("RunSelectedCommand")]
        [TestCase("RunFailedCommand")]
        [TestCase("DebugAllCommand")]
        [TestCase("DebugSelectedCommand")]
        [TestCase("DebugFailedCommand")]
        [TestCase("TestParametersCommand")]
        [TestCase("StopRunCommand")]
        public void CommandIsDisabled(string propName)
        {
            ViewElement(propName).Received().Enabled = false;
            //_view.RunAllCommand.Received().Enabled = false;
            //_view.RunSelectedCommand.Received().Enabled = false;
            //_view.RunFailedCommand.Received().Enabled = false;
            //_view.DebugAllCommand.Received().Enabled = false;
            //_view.DebugSelectedCommand.Received().Enabled = false;
            //_view.DebugFailedCommand.Received().Enabled = false;
            //_view.TestParametersCommand.Received().Enabled = false;
            //_view.RunCheckedCommand.Received().Visible = false;
            //_view.DebugCheckedCommand.Received().Visible = false;
            //_view.StopRunCommand.Received().Enabled = false;
        }

        [TestCase("RunCheckedCommand")]
        [TestCase("DebugCheckedCommand")]
        public void ElementIsNotVisible(string propName)
        {
            ViewElement(propName).Received().Visible = false;
        }

        [Test]
        public void AlternateImageSetIsSet()
        {
            _view.Received().AlternateImageSet = "MyImageSet";
        }

        [Test]
        public void ShowCheckBoxesIsSet()
        {
            _view.ShowCheckBoxes.Received().Checked = true;
        }
    }
}
