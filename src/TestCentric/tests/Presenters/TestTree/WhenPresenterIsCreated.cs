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
        [TestCase("ForceStopButton")]
        public void CommandIsDisabled(string propName)
        {
            ViewElement(propName).Received().Enabled = false;
        }

        [TestCase("RunCheckedCommand", false)]
        [TestCase("DebugCheckedCommand", false)]
        [TestCase("RunSummaryButton", false)]
        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        public void CheckElementVisibility(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
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
