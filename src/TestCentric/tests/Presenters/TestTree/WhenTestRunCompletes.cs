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
        [SetUp]
        public void SimulateTestRunCompletion()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.HasResults.Returns(true);

            FireRunFinishedEvent(new ResultNode("<test-run id='XXX' result='Failed' />"));
        }
    }
}
