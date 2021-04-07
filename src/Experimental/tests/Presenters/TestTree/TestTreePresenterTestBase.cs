// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using Model;
    using Views;

    public class TestTreePresenterTestBase
    {
        protected ITestTreeView _view;
        protected ITestModel _model;

        [SetUp]
        public void CreatePresenter()
        {
            _view = Substitute.For<ITestTreeView>();
            _model = Substitute.For<ITestModel>();
            _model.Settings.Returns(new TestCentric.TestUtilities.Fakes.UserSettings());

            new TreeViewPresenter(_view, _model);

            // Make it look like the view loaded
            _view.Load += Raise.Event<System.EventHandler>(null, new System.EventArgs());
        }
    }
}
