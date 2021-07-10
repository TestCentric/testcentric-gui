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
        [TestCase("DebugAllCommand")]
        [TestCase("DebugSelectedCommand")]
        [TestCase("TestParametersCommand")]
        [TestCase("StopRunButton")]
        public void CommandIsDisabled(string propName)
        {
            ViewElement(propName).Received().Enabled = false;
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
