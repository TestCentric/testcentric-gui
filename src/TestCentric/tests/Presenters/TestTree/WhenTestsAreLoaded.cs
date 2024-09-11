// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;
    using System.IO;
    using System.Windows.Forms;

    public class WhenTestsAreLoaded : TreeViewPresenterTestBase
    {
        // Use dedicated test file name; Used for VisualState file too
        const string TestFileName = "TreeViewPresenterTestsLoaded.dll";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _treeDisplayStrategyFactory = new TreeDisplayStrategyFactory();
        }

        [SetUp]
        public void SimulateTestLoad()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);
            var project = new TestCentricProject(_model, TestFileName);
            _model.TestCentricProject.Returns(project);

            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);
        }

        [TearDown]
        public void TearDown()
        {
            // Delete VisualState file to prevent any unintended side effects
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestLoaded_NoVisualState_ShowCheckBox_IsAppliedFromSettings(bool showCheckBoxSetting)
        {
            // Arrange: adapt settings
            _model.Settings.Gui.TestTree.ShowCheckBoxes = showCheckBoxSetting;

            // Act: load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_view.ShowCheckBoxes.Checked, Is.EqualTo(showCheckBoxSetting));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestLoaded_WithVisualState_ShowCheckBox_IsAppliedFromVisualState(bool showCheckBox)
        {
            // Arrange: Create and save VisualState file
            VisualState visualState = new VisualState();
            visualState.ShowCheckBoxes = showCheckBox;
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            visualState.Save(fileName);

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_view.ShowCheckBoxes.Checked, Is.EqualTo(showCheckBox));
        }

        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void TestLoaded_WithVisualState_TreeStrategy_IsCreatedFromVisualState(string displayFormat)
        {
            // Arrange: Create and save VisualState file
            VisualState visualState = new VisualState();
            visualState.DisplayStrategy = displayFormat;
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            visualState.Save(fileName);

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_presenter.Strategy.StrategyID, Is.EqualTo(displayFormat));
        }

        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void TestLoaded_NoVisualState_TreeStrategy_IsCreatedFromSettings(string displayFormat)
        {
            // Arrange: adapt settings
            _model.Settings.Gui.TestTree.DisplayFormat = displayFormat;

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_presenter.Strategy.StrategyID, Is.EqualTo(displayFormat));
        }

        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void TestLoaded_WithVisualState_DisplayFormatSetting_IsUpdatedFromVisualState(string displayFormat)
        {
            // Arrange: Create and save VisualState file
            VisualState visualState = new VisualState();
            visualState.DisplayStrategy = displayFormat;
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            visualState.Save(fileName);

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_model.Settings.Gui.TestTree.DisplayFormat, Is.EqualTo(displayFormat));
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void TestLoaded_WithVisualState_FixtureListGroupBySetting_IsUpdatedFromVisualState(string groupBy)
        {
            // Arrange: Create and save VisualState file
            VisualState visualState = new VisualState();
            visualState.DisplayStrategy = "FIXTURE_LIST";
            visualState.GroupBy = groupBy;
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            visualState.Save(fileName);

            _model.Settings.Gui.TestTree.TestList.GroupBy = "DURATION";

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_model.Settings.Gui.TestTree.FixtureList.GroupBy, Is.EqualTo(groupBy));
            Assert.That(_model.Settings.Gui.TestTree.TestList.GroupBy, Is.EqualTo("DURATION"));     // Assert that testList groupBy was not changed accidently
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void TestLoaded_WithVisualState_TestListGroupBySetting_IsUpdatedFromVisualState(string groupBy)
        {
            // Arrange: Create and save VisualState file
            VisualState visualState = new VisualState();
            visualState.DisplayStrategy = "TEST_LIST";
            visualState.GroupBy = groupBy;
            string fileName = VisualState.GetVisualStateFileName(TestFileName);
            visualState.Save(fileName);

            _model.Settings.Gui.TestTree.FixtureList.GroupBy = "DURATION";

            var tv = new TreeView();
            _view.TreeView.Returns(tv);

            // Act: Load tests
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.LoadedTests.Returns(testNode);
            FireTestLoadedEvent(testNode);

            // Assert
            Assert.That(_model.Settings.Gui.TestTree.TestList.GroupBy, Is.EqualTo(groupBy));
            Assert.That(_model.Settings.Gui.TestTree.FixtureList.GroupBy, Is.EqualTo("DURATION"));     // Assert that fixtureList groupBy was not changed accidently
        }

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //[Platform(Exclude = "Linux", Reason = "Display issues")]
        //public void WhenTestLoadCompletes_PropertyDialogIsClosed()
        //{
        //    ClearAllReceivedCalls();
        //    _model.TestFiles.Returns(new List<string>(new[] { "test.dll" }));
        //    FireTestLoadedEvent(new TestNode("<test-run id='2'/>"));

        //    _view.Received().CheckPropertiesDialog();
        //}

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //[Platform(Exclude = "Linux", Reason = "Display issues")]
        //public void WhenTestLoadCompletes_MultipleAssemblies_TopNodeIsTestRun()
        //{
        //    TestNode testNode = new TestNode("<test-run id='2'><test-suite id='101' name='test.dll'/><test-suite id='102' name='another.dll'/></test-run>");
        //    ClearAllReceivedCalls();
        //    _model.TestFiles.Returns(new List<string>(new[] { "test.dll", "another.dll" }));
        //    FireTestLoadedEvent(testNode);

        //    _view.Tree.Received().Load(Arg.Compat.Is<TreeNode>((tn) => tn.Text == "TestRun" && tn.Nodes.Count == 2));
        //}

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //[Platform(Exclude = "Linux", Reason = "Display issues")]
        //public void WhenTestLoadCompletes_SingleAssembly_TopNodeIsAssembly()
        //{
        //    TestNode testNode = new TestNode("<test-run><test-suite id='1' name='another.dll'/></test-run>");
        //    ClearAllReceivedCalls();
        //    _model.TestFiles.Returns(new List<string>(new[] { "test.dll" }));
        //    FireTestLoadedEvent(testNode);

        //    _view.Tree.Received().Load(Arg.Compat.Is<TreeNode>(tn => tn.Text == "another.dll"));
        //}
    }
}
