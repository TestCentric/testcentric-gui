// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;

    public class WhenTestRunBegins : TestTreePresenterTestBase
    {
        [SetUp]
        protected void SimulateTestRunStarting()
        {
            _model.IsPackageLoaded.Returns(true);
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(true);
            _model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(1234));
        }

        [Test]
        public void RunAllCommand_IsDisabled()
        {
            _view.RunAllCommand.Received().Enabled = false;
        }

        [Test]
        public void RunSelectedCommand_IsDisabled()
        {
            _view.RunSelectedCommand.Received().Enabled = false;
        }

        [Test]
        public void RunFailedCommand_IsDisabled()
        {
            _view.RunFailedCommand.Received().Enabled = false;
        }

        [Test]
        public void StopRunCommand_IsEnabled()
        {
            _view.StopRunCommand.Received().Enabled = true;
        }

        [Test]
        public void TestParametersCommand_IsDisabled()
        {
            _view.TestParametersCommand.Received().Enabled = false;
        }
    }
}
