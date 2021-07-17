// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;

    public class WhenTestsAreReloaded : TreeViewPresenterTestBase
    {
        [SetUp]
        public void SimulateTestReload()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);

            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.Tests.Returns(testNode);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage("dummy.dll"));

            FireTestReloadedEvent(testNode);
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif
    }
}
