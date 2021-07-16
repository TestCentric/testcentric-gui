// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;

    public class WhenTestsAreLoaded : MainPresenterTestBase
    {
        [SetUp]
        public void SimulateTestLoad()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage("dummy.dll"));

            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.Tests.Returns(testNode);
            FireTestLoadedEvent(testNode);
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif

        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", true)]
        [TestCase("AddTestFilesCommand", true)]
        [TestCase("ReloadTestsCommand", true)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllMenuCommand", true)]
        [TestCase("RunSelectedMenuCommand", true)]
        [TestCase("RunFailedMenuCommand", false)]
        [TestCase("StopRunMenuCommand", false)]
        [TestCase("ForceStopMenuCommand", false)]
        [TestCase("TestParametersMenuCommand", true)]
        [TestCase("RunAllCommand", true)]
        [TestCase("RunSelectedCommand", true)]
        [TestCase("DebugAllCommand", true)]
        [TestCase("DebugSelectedCommand", true)]
        [TestCase("TestParametersCommand", true)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
        [TestCase("SaveResultsCommand", false)]
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

#if NYI
        [Test]
        public void View_Receives_TestAssembliesLoaded()
        {
            View.Received().OnTestAssembliesLoaded();
        }
#endif
    }
}
