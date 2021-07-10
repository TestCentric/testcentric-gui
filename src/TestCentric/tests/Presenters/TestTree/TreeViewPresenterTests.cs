// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Views;
    using Model;
    using Elements;
    using System.Collections.Generic;

    public class TreeViewPresenterTests : TreeViewPresenterTestBase
    {
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


        [Test]
        public void WhenDisplayFormatChanges_TreeIsReloaded()
        {
            TestNode testNode = new TestNode(XmlHelper.CreateXmlNode("<test-run id='1'><test-suite id='42'/></test-run>"));
            _model.Tests.Returns(testNode);
            _view.DisplayFormat.SelectedItem.Returns("NUNIT_TREE");
            _view.DisplayFormat.SelectionChanged += Raise.Event<CommandHandler>();

            _view.Tree.Received().Add(Arg.Compat.Is<TreeNode>((tn) => ((TestNode)tn.Tag).Id == "42"));
        }
    }
}
