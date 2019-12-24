// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;

    public class WhenTestsAreUnloaded : TestTreePresenterTestBase
    {
        [SetUp]
        public void SimulateTestUnload()
        {
            _model.IsPackageLoaded.Returns(true);
            _model.HasTests.Returns(false);
            _model.IsTestRunning.Returns(false);
            _model.Events.TestUnloaded += Raise.Event<TestEventHandler>(new TestEventArgs());
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
        public void StopRunCommand_IsDisabled()
        {
            _view.StopRunCommand.Received().Enabled = false;
        }

        [Test]
        public void TestParametersCommand_IsDisabled()
        {
            _view.TestParametersCommand.Received().Enabled = false;
        }

        [Test]
        public void TreeControl_IsCleared()
        {
            _view.Tree.Received().Clear();
        }
    }
}
