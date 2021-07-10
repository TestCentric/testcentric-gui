// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenTestRunCompletes : TreeViewPresenterTestBase
    {
        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenTestRunCompletes_RunCommandIsEnabled()
        //{
        //    ClearAllReceivedCalls();
        //    FireRunFinishedEvent(new ResultNode("<test-run/>"));

        //    _view.RunCommand.Received().Enabled = true;
        //}

        [Test]
        public void RunSummaryButtonIsMadeVisible()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.HasResults.Returns(true);

            FireRunFinishedEvent(new ResultNode("<test-run id='XXX' result='Failed' />"));

            _view.RunSummaryButton.Received().Visible = true;
        }
    }
}
