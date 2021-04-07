// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenPresenterIsCreated : TestTreePresenterTestBase
    {
        [Test]
        public void RunAllCommand_IsDisabled()
        {
            _view.RunAllCommand.Received(1).Enabled = false;
        }

        [Test]
        public void RunSelectedCommand_IsDisabled()
        {
            _view.RunSelectedCommand.Received(1).Enabled = false;
        }

        [Test]
        public void RunFailedCommand_IsDisabled()
        {
            _view.RunFailedCommand.Received(1).Enabled = false;
        }

        [Test]
        public void StopRunCommand_IsDisabled()
        {
            _view.StopRunCommand.Received(1).Enabled = false;
        }

        [Test]
        public void TestParametersCommand_IsDisabled()
        {
            _view.TestParametersCommand.Received(1).Enabled = false;
        }
    }
}
