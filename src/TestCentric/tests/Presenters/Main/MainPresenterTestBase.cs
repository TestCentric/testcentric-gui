// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.Main
{
    using Elements;
    using Model;
    using Views;

    public class MainPresenterTestBase : PresenterTestBase<IMainView>
    {
        protected TestCentricPresenter Presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _view.LongRunningOperation.Returns(Substitute.For<ILongRunningOperationDisplay>());

            Presenter = new TestCentricPresenter(_view, _model, new CommandLineOptions());
        }

        [TearDown]
        public void RemovePresenter()
        {
            Presenter = null;
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
