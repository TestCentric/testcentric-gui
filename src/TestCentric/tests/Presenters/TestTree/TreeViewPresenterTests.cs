// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Model;
using System;
using System.Runtime.InteropServices;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class TreeViewPresenterTests : TreeViewPresenterTestBase
    {
        [TestCase(true)]
        [TestCase(false)]
        public void WhenSettingsAreChanged_ShowCheckBoxes_NewSettingIsApplied(bool showCheckBoxSetting)
        {
            _model.Settings.Gui.TestTree.ShowCheckBoxes = showCheckBoxSetting;

            Assert.That(_view.ShowCheckBoxes.Checked, Is.EqualTo(showCheckBoxSetting));
        }

        [TestCase("Default")]
        [TestCase("VisualStudio")]
        public void WhenSettingsAreChanged_AlternateImageSet_NewSettingIsApplied(string imageSet)
        {
            _model.Settings.Gui.TestTree.AlternateImageSet = imageSet;

            Assert.That(_view.AlternateImageSet, Is.EqualTo(imageSet));
        }

        [Test]
        public void WhenContextMenuIsDisplayed_SelectedNodeIsGroup_ViewAsXmlContextMenu_IsDisabled()
        {
            // 1. Arrange
            TreeNode treeNode = new TreeNode("TreeNode");
            TestGroup testGroup = new TestGroup("MyGroup");
            treeNode.Tag = testGroup;

            _view.ContextNode.Returns(treeNode);

            // 2. Act
            _view.ContextMenuOpening += Raise.Event<EventHandler>();

            // 3. Assert
            Assert.That(_view.ViewAsXmlCommand.Enabled, Is.False);
        }

        [Test]
        public void WhenContextMenuIsDisplayed_SelectedNodeIsTestNode_ViewAsXmlContextMenu_IsEnabled()
        {
            // 1. Arrange
            TreeNode treeNode = new TreeNode("TreeNode");
            TestNode testNode = new TestNode("<test-suite id='1'/>");
            treeNode.Tag = testNode;

            _view.ContextNode.Returns(treeNode);

            // 2. Act
            _view.ContextMenuOpening += Raise.Event<EventHandler>();

            // 3. Assert
            Assert.That(_view.ViewAsXmlCommand.Enabled, Is.True);
        }

        [Test]
        public void WhenContextMenuIsDisplayed_NoSelectedNode_ViewAsXmlContextMenu_IsEnabled()
        {
            // 1. Arrange
            TreeNode treeNode = new TreeNode("TreeNode");
            treeNode.Tag = null;
            _view.ContextNode.Returns(treeNode);

            // 2. Act
            _view.ContextMenuOpening += Raise.Event<EventHandler>();

            // 3. Assert
            Assert.That(_view.ViewAsXmlCommand.Enabled, Is.True);
        }

        [Test]
        public void WhenContextMenuIsDisplayed_GuiMiniLayout_TestPropertiesContextMenu_IsVisible()
        {
            // 1. Arrange
            _model.Settings.Gui.GuiLayout = "Mini";

            // 2. Act
            _view.ContextMenuOpening += Raise.Event<EventHandler>();

            // 3. Assert
            Assert.That(_view.TestPropertiesCommand.Visible, Is.True);
        }

        [Test]
        public void WhenContextMenuIsDisplayed_GuiFullLayout_TestPropertiesContextMenu_IsNotVisible()
        {
            // 1. Arrange
            _model.Settings.Gui.GuiLayout = "Full";

            // 2. Act
            _view.ContextMenuOpening += Raise.Event<EventHandler>();

            // 3. Assert
            Assert.That(_view.TestPropertiesCommand.Visible, Is.False);
        }

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenContextNodeIsNotNull_RunCommandExecutesThatTest()
        //{
        //    var testNode = new TestNode("<test-case id='DUMMY-ID'/>");
        //    _view.ContextNode.Returns(new TestSuiteTreeNode(testNode));

        //    _view.RunCommand.Execute += Raise.Event<CommandHandler>();

        //    _model.Received().RunTests(testNode);
        //}

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenContextNodeIsNull_RunCommandExecutesSelectedTests()
        //{
        //    var testNodes = new[] { new TestNode("<test-case id='DUMMY-1'/>"), new TestNode("<test-case id='DUMMY-2'/>") };
        //    _view.SelectedTests.Returns(testNodes);

        //    _view.RunCommand.Execute += Raise.Event<CommandHandler>();

        //    _model.Received().RunTests(Arg.Compat.Is<TestSelection>((sel) => sel.Count == 2 && sel[0].Id == "DUMMY-1" && sel[1].Id == "DUMMY-2"));
        //}



        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenTestIsChanged_ReloadSettingsIsEnabled()
        //{
        //    _settings.Engine.ReloadOnChange = true;
        //    _model.Events.TestChanged += Raise.Event<TestEventHandler>(new TestEventArgs());
        //    _model.Received().ReloadTests();
        //}

        // TODO: Version 1 Test - Make it work if needed.
        //[Test]
        //public void WhenTestIsChanged_ReloadSettingsIsDisabled()
        //{
        //    _settings.Engine.ReloadOnChange = false;
        //    _model.Events.TestChanged += Raise.Event<TestEventHandler>(new TestEventArgs());
        //    _model.DidNotReceive().ReloadTests();
        //}

        //[Test]
        //public void WhenDisplayFormatChanges_TreeIsReloaded()
        //{
        //    TestNode testNode = new TestNode(XmlHelper.CreateXmlNode("<test-run id='1'><test-suite id='42'/></test-run>"));
        //    _model.Tests.Returns(testNode);

        //    _view.Tree.Received().Add(Arg.Compat.Is<TreeNode>((tn) => ((TestNode)tn.Tag).Id == "42"));
        //}

        //[Test, Combinatorial]
        //public void WhenContextMenuIsDisplayed_RunCheckedCommandVisibilityIsSet(
        //    [Values] bool showCheckBoxes)
        //{

        //    var testNode = new TestNode(XmlHelper.CreateXmlNode("<test-case/>"));
        //    var treeNode = new TreeNode()
        //    {
        //        Tag = testNode
        //    };

        //    _view.CheckBoxes.Returns(showCheckBoxes);
        //    _view.ShowCheckBoxes.Checked.Returns(showCheckBoxes);
        //    _view.ContextNode.Returns(treeNode);
        //    _view.TreeContextMenu.Returns(new ContextMenuStrip());

        //    _view.ClearReceivedCalls();
        //    _view.ContextMenuOpening += Raise.Event<EventHandler>();
        //    ViewElement("RunCheckedCommand").Received().Visible = showCheckBoxes;
        //}
    }
}
