// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;
    using Elements;

    public class WhenTestRunCompletes : TestTreePresenterTestBase
    {
        [SetUp]
        public void SimulateTestRunFinish()
        {
            _model.IsPackageLoaded.Returns(true);
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);

            var resultNode = new ResultNode("<test-suite/>");

            _model.Events.RunFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));
        }


        [Test]
        public void RunAllCommand_IsEnabled()
        {
            _view.RunAllCommand.Received().Enabled = true;
        }

        [Test]
        public void RunSelectedCommand_IsEnabled()
        {
            _view.RunSelectedCommand.Received().Enabled = true;
        }

        [Test]
        public void RunFailedCommand_IsEnabled()
        {
            _view.RunFailedCommand.Received().Enabled = true;
        }

        [Test]
        public void StopRunCommand_IsDisabled()
        {
            _view.StopRunCommand.Received().Enabled = false;
        }

        [Test]
        public void TestParametersCommand_IsEnabled()
        {
            _view.TestParametersCommand.Received(1).Enabled = true;
        }

        [Test]
        public void RunAllMenuItemRunsAllTests()
        {
            _view.RunAllCommand.Execute += Raise.Event<CommandHandler>();
            _model.Received().RunAllTests();
        }
    }
}
