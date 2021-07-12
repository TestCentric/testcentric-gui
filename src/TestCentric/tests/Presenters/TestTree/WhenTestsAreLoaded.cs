// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;

    public class WhenTestsAreLoaded : TreeViewPresenterTestBase
    {
        [SetUp]
        public void SimulateTestLoad()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(false);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage("dummy.dll"));

            TestNode testNode = new TestNode("<test-suite id='1'/>");
            _model.Tests.Returns(testNode);
            FireTestLoadedEvent(testNode);
        }

        [TestCase("RunAllCommand", true)]
        [TestCase("RunSelectedCommand", true)]
        [TestCase("DebugAllCommand", true)]
        [TestCase("DebugSelectedCommand", true)]
        [TestCase("TestParametersCommand", true)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
        public void CheckElementIsEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }

        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        public void CheckElementVisibility(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
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
