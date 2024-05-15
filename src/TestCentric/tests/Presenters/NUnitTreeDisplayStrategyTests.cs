// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using System.Collections.Generic;
    using Model;
    using Views;

    public abstract class DisplayStrategyTests
    {
        protected ITestTreeView _view;
        protected ITestModel _model;
        protected Model.Settings.UserSettings _settings;
        protected DisplayStrategy _strategy;

        [SetUp]
        public void CreateDisplayStrategy()
        {
            _view = Substitute.For<ITestTreeView>();
            _model = Substitute.For<ITestModel>();
            _settings = new Fakes.UserSettings();
            _model.Settings.Returns(_settings);

            // We can't construct a TreeNodeCollection, so we fake it
            var nodes = new TreeNode().Nodes;
            nodes.Add(new TreeNode("test.dll"));
            _view.Nodes.Returns(nodes);

            var project = new TestCentricProject(_model, "dummy.dll");
            _model.TestCentricProject.Returns(project);

            _strategy = GetDisplayStrategy();
        }

        protected abstract DisplayStrategy GetDisplayStrategy();

        [Test]
        public void WhenTestsAreLoaded_TreeViewIsLoaded()
        {
            _strategy.OnTestLoaded(
                new TestNode("<test-run id='1'><test-suite id='42'/><test-suite id='99'/></test-run>"),
                null);

            _view.Received().Clear();
            _view.Received().Add(Arg.Compat.Is<TreeNode>((tn) => (tn.Tag as TestNode).Id == "42"));
            _view.Received().Add(Arg.Compat.Is<TreeNode>((tn) => (tn.Tag as TestNode).Id == "99"));
        }

        [Test]
        public void WhenTestsAreUnloaded_TreeViewIsCleared()
        {
            _strategy.OnTestUnloaded();

            _view.Received().Clear();
        }
    }

    public class NUnitTreeDisplayStrategyTests : DisplayStrategyTests
    {
        protected override DisplayStrategy GetDisplayStrategy()
        {
            return new NUnitTreeDisplayStrategy(_view, _model);
        }
    }

    //public class NUnitTestListStrategyTests : DisplayStrategyTests
    //{
    //    protected override DisplayStrategy GetDisplayStrategy()
    //    {
    //        return new TestListDisplayStrategy(_view, _model);
    //    }
    //}
}
