// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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

        [TestCase("RunButton", true)]
        [TestCase("StopButton", false)]
        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", true)]
        [TestCase("AddTestFilesCommand", true)]
        [TestCase("ReloadTestsCommand", true)]
        [TestCase("RuntimeMenu", false)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllCommand", true)]
        [TestCase("RunSelectedCommand", true)]
        [TestCase("RunFailedCommand", false)]
        [TestCase("TestParametersCommand", true)]
        [TestCase("StopRunCommand", false)]
        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
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
