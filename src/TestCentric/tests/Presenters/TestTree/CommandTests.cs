// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Elements;
    using Views;

    public class CommandTests : TreeViewPresenterTestBase
    {
        private static string[] NO_FILES_SELECTED = new string[0];
        private static string NO_FILE_PATH = null;

        [Test]
        public void RunAllCommand_RunsAllTests()
        {
            _view.RunAllCommand.Execute += Raise.Event<CommandHandler>();
            _model.Received().RunAllTests();
        }

        [Test]
        public void RunSelectedCommand_RunsSelectedTests()
        {
            _view.RunSelectedCommand.Execute += Raise.Event<CommandHandler>();
            _model.Received().RunSelectedTests();
        }

        [Test]
        public void StopRunButton_StopsTestsAndChangesMenu()
        {
            _view.StopRunButton.Execute += Raise.Event<CommandHandler>();
            _model.Received().StopTestRun(false);
            _view.StopRunButton.Received().Visible = false;
            _view.ForceStopButton.Received().Visible = true;
        }

        [Test]
        public void ForceStopCommand_ForcesTestsToStopAndDisablesForceStop()
        {
            _view.ForceStopButton.Execute += Raise.Event<CommandHandler>();
            _model.Received().StopTestRun(true);
            _view.ForceStopButton.Received().Enabled = false;
        }
    }
}
