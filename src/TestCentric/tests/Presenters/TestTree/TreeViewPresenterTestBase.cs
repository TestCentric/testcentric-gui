// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    using System.Windows.Forms;
    using Elements;
    using Model;
    using Views;

    public class TreeViewPresenterTestBase : PresenterTestBase<ITestTreeView>
    {
        protected TreeViewPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _view.Tree.ContextMenuStrip.Returns(new ContextMenuStrip());
            _settings.Gui.TestTree.AlternateImageSet = "MyImageSet";
            _settings.Gui.TestTree.ShowCheckBoxes = true;

            _presenter = new TreeViewPresenter(_view, _model);

            // Make it look like the view loaded
            _view.Load += Raise.Event<System.EventHandler>(_view, new System.EventArgs());
        }

        [TearDown]
        public void RemovePresenter()
        {
            _presenter = null;
        }

        protected IViewElement ViewElement(string propName)
        {
            var prop = _view.GetType().GetProperty(propName);
            if (prop == null)
                Assert.Fail($"View has no property named {propName}.");

            var element = prop.GetValue(_view) as IViewElement;
            if (element == null)
                Assert.Fail($"Property {propName} is not an IViewElement. It is declared as {prop.PropertyType}.");

            return element;
        }
    }
}
