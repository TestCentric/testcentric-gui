// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenTestsAreUnloaded : TreeViewPresenterTestBase
    {
        [SetUp]
        public void SimulateTestUnload()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(false);
            _model.IsTestRunning.Returns(false);
            FireTestUnloadedEvent();
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif

        [TestCase("RunAllCommand", false)]
        [TestCase("RunSelectedCommand", false)]
        [TestCase("DebugAllCommand", false)]
        [TestCase("DebugSelectedCommand", false)]
        [TestCase("TestParametersCommand", false)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }

        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        public void CheckElementVisibility(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
        }
    }
}
