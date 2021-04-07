// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;
    using Views;

    public class WhenTestsAreLoaded : TestTreePresenterTestBase
    {
        [SetUp]
        public void SimulateTestLoad()
        {
            _model.IsPackageLoaded.Returns(true);
            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);

            _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(new TestNode("<test-run/>")));
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
    }
}
