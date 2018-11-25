// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Elements;
    using Model;
    using Views;

    public class TreeViewPresenterTests : PresenterTestBase<ITestTreeView>
    {
        private TreeViewPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _settings.Gui.TestTree.AlternateImageSet = "MyImageSet";
            _settings.Gui.TestTree.ShowCheckBoxes = true;
            _presenter = new TreeViewPresenter(_view, _model);
            _model.IsTestRunning.Returns(false);
        }

        [TearDown]
        public void RemovePresenter()
        {
            _presenter = null;
        }

        [Test]
        public void WhenPresenterIsCreated_RunCommandIsDisabled()
        {
            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        public void WhenPresenterIsCreated_AlternateImageSetIsSet()
        {
            _view.Received().AlternateImageSet = "MyImageSet";
        }

        [Test]
        public void WhenPresenterIsCreated_ShowCheckBoxesIsSet()
        {
            _view.Received().CheckBoxes = true;
            _view.ShowCheckBoxes.Received().Checked = true;
        }

        [Test]
        public void WhenTestLoadBegins_RunCommandIsDisabled()
        {
            ClearAllReceivedCalls();
            FireTestsLoadingEvent(new[] { "test.dll" });

            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Display issues")]
        public void WhenTestLoadCompletes_RunCommandIsEnabled()
        {
            ClearAllReceivedCalls();
            _model.TestFiles.Returns(new List<string>(new[] { "test.dll" }));
            FireTestLoadedEvent(new TestNode("<test-run id='2'/>"));

            _view.RunCommand.Received().Enabled = true;
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Display issues")]
        public void WhenTestLoadCompletes_PropertyDialogIsClosed()
        {
            ClearAllReceivedCalls();
            _model.TestFiles.Returns(new List<string>(new[] { "test.dll" }));
            FireTestLoadedEvent(new TestNode("<test-run id='2'/>"));

            _view.Received().CheckPropertiesDialog();
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Display issues")]
        public void WhenTestLoadCompletes_MultipleAssemblies_TopNodeIsTestRun()
        {
            TestNode testNode = new TestNode("<test-run id='2'><test-suite id='101' name='test.dll'/><test-suite id='102' name='another.dll'/></test-run>");
            ClearAllReceivedCalls();
            _model.TestFiles.Returns(new List<string>(new[] { "test.dll", "another.dll" }));
            FireTestLoadedEvent(testNode);

            _view.Tree.Received().Load(Arg.Is<TreeNode>((tn) => tn.Text == "TestRun" && tn.Nodes.Count == 2));
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Display issues")]
        public void WhenTestLoadCompletes_SingleAssembly_TopNodeIsAssembly()
        {
            TestNode testNode = new TestNode("<test-run><test-suite id='1' name='another.dll'/></test-run>");
            ClearAllReceivedCalls();
            _model.TestFiles.Returns(new List<string>(new[] { "test.dll" }));
            FireTestLoadedEvent(testNode);

            _view.Tree.Received().Load(Arg.Is<TreeNode>(tn => tn.Text == "another.dll"));
        }


        [Test]
        public void WhenTestReloadBegins_RunCommandIsDisabled()
        {
            ClearAllReceivedCalls();
            FireTestsReloadingEvent();

            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Display issues")]
        public void WhenTestReloadCompletes_RunCommandIsEnabled()
        {
            ClearAllReceivedCalls();
            FireTestReloadedEvent(new TestNode("<test-run id='2'/>"));

            _view.RunCommand.Received().Enabled = true;
        }

        [Test]
        public void WhenTestUnloadBegins_RunCommandIsDisabled()
        {
            ClearAllReceivedCalls();
            FireTestsUnloadingEvent();

            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        public void WhenTestUnloadCompletes_RunCommandIsDisabled()
        {
            ClearAllReceivedCalls();
            FireTestUnloadedEvent();

            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        public void WhenTestRunStarts_RunCommandIsDisabled()
        {
            ClearAllReceivedCalls();
            FireRunStartingEvent(1234);

            _view.RunCommand.Received().Enabled = false;
        }

        [Test]
        public void WhenTestRunCompletes_RunCommandIsEnabled()
        {
            ClearAllReceivedCalls();
            FireRunFinishedEvent(new ResultNode("<test-run/>"));

            _view.RunCommand.Received().Enabled = true;
        }

        [Test]
        public void WhenContextNodeIsNotNull_RunCommandExecutesThatTest()
        {
            var testNode = new TestNode("<test-case id='DUMMY-ID'/>");
            _view.ContextNode.Returns(new TestSuiteTreeNode(testNode));

            _view.RunCommand.Execute += Raise.Event<CommandHandler>();

            _model.Received().RunTests(testNode);
        }

        [Test]
        public void WhenContextNodeIsNull_RunCommandExecutesSelectedTests()
        {
            var testNodes = new[] { new TestNode("<test-case id='DUMMY-1'/>"), new TestNode("<test-case id='DUMMY-2'/>") };
            _view.SelectedTests.Returns(testNodes);

            _view.RunCommand.Execute += Raise.Event<CommandHandler>();

            _model.Received().RunTests(Arg.Is<TestSelection>((sel) => sel.Count == 2 && sel[0].Id == "DUMMY-1" && sel[1].Id == "DUMMY-2"));
        }

        [Test]
        public void WhenTestCaseCompletes_ResultIsPosted()
        {
            var test = new TestNode("<test-case id='100' name='DummyTest'/>");
            var treeNode = new TestSuiteTreeNode(test);
            _presenter.TreeMap["100"] = treeNode;

            var result = new ResultNode("<test-case id='100' name='DummyTest' result='Passed'/>");

            _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            Assert.That(treeNode.Result, Is.EqualTo(result));
        }

        [Test]
        public void WhenTestSuiteCompletes_ResultIsPosted()
        {
            var suite = new TestNode("<test-suite id='100' name='DUMMY'/>");
            var treeNode = new TestSuiteTreeNode(suite);
            _presenter.TreeMap["100"] = treeNode;

            var result = new ResultNode("<test-suite id='100' name='DUMMY' result='Passed'/>");

            _model.Events.SuiteFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(result));

            Assert.That(treeNode.Result, Is.EqualTo(result));
        }

        //[Test]
        //public void WhenTestRunStarts_ResultsAreCleared()
        //{
        //    _settings.RunStarting += Raise.Event<TestEventHandler>(new TestEventArgs(TestAction.RunStarting, "Dummy", 1234));

        //    _view.Received().ClearResults();
        //}

        //static object[] resultData = new object[] {
        //    new object[] { ResultState.Success, TestTreeView.SuccessIndex },
        //    new object[] { ResultState.Ignored, TestTreeView.WarningIndex },
        //    new object[] { ResultState.Failure, TestTreeView.FailureIndex },
        //    new object[] { ResultState.Inconclusive, TestTreeView.InconclusiveIndex },
        //    new object[] { ResultState.Skipped, TestTreeView.SkippedIndex },
        //    new object[] { ResultState.NotRunnable, TestTreeView.FailureIndex },
        //    new object[] { ResultState.Error, TestTreeView.FailureIndex },
        //    new object[] { ResultState.Cancelled, TestTreeView.FailureIndex }
        //};

        //[TestCaseSource("resultData")]
        //public void WhenTestCaseCompletes_NodeShowsProperResult(ResultState resultState, int expectedIndex)
        //{
        //    _model.IsPackageLoaded.Returns(true);
        //    _model.HasTests.Returns(true);
        //    _view.DisplayFormat.SelectedItem.Returns("NUNIT_TREE");

        //    var result = resultState.Status.ToString();
        //    var label = resultState.Label;

        //    var testNode = new TestNode("<test-run id='1'><test-case id='123'/></test-run>");
        //    var resultNode = new ResultNode(string.IsNullOrEmpty(label)
        //        ? string.Format("<test-case id='123' result='{0}'/>", result)
        //        : string.Format("<test-case id='123' result='{0}' label='{1}'/>", result, label));
        //    _model.Tests.Returns(testNode);

        //    //var treeNode = _adapter.MakeTreeNode(result);
        //    //_adapter.NodeIndex[suiteResult.Id] = treeNode;
        //    _model.Events.TestLoaded += Raise.Event<TestNodeEventHandler>(new TestNodeEventArgs(TestAction.TestLoaded, testNode));
        //    _model.Events.TestFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(TestAction.TestFinished, resultNode));

        //    _view.Tree.Received().SetImageIndex(Arg.Any<TreeNode>(), expectedIndex);
        //}

        //[Test]
        //public void WhenDisplayFormatChanges_TreeIsReloaded()
        //{
        //    TestNode testNode = new TestNode(XmlHelper.CreateXmlNode("<test-run id='1'><test-suite id='42'/></test-run>"));
        //    _model.Tests.Returns(testNode);
        //    _view.DisplayFormat.SelectedItem.Returns("NUNIT_TREE");
        //    _view.DisplayFormat.SelectionChanged += Raise.Event<CommandHandler>();

        //    _view.Tree.Received().Add(Arg.Is<TreeNode>((tn) => ((TestNode)tn.Tag).Id == "42"));
        //}
    }
}
