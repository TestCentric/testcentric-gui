// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenTestsAreUnloaded : TreeViewPresenterTestBase
    {
        [SetUp]
        public void SimulateTestUnload()
        {
            _settings.Gui.TestTree.DisplayFormat = "NUNIT_TREE";

            ClearAllReceivedCalls();

            _model.HasTests.Returns(false);
            _model.IsTestRunning.Returns(false);
            FireTestUnloadedEvent();
        }

        [Test]
        public void TestUnloaded_CategoryFilter_IsClosed()
        {
            // Act: unload tests
            FireTestUnloadedEvent();

            // Assert
            _view.CategoryFilter.Received().Close();
        }

        [Test]
        public void TestUnloaded_TestFilters_AreReset()
        {
            // Act: unload tests
            FireTestUnloadedEvent();

            // Assert
            _view.TextFilter.Received().Text = "";
            _view.OutcomeFilter.ReceivedWithAnyArgs().SelectedItems = null;
            _view.CategoryFilter.ReceivedWithAnyArgs().SelectedItems = null;
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif
    }
}
