// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;
    using Views;

    public class MainPresenterTestBase
    {
        protected IMainView View;
        protected ITestModel Model;
        protected MainPresenter Presenter;

        [SetUp]
        public void CreatePresenter()
        {
            View = Substitute.For<IMainView>();
            View.LongRunningOperation.Returns(Substitute.For<ILongRunningOperationDisplay>());
            Model = Substitute.For<ITestModel>();
            Model.Settings.Returns(new TestCentric.TestUtilities.Fakes.UserSettings());

            Presenter = new MainPresenter(View, Model, new CommandLineOptions());
        }

        [TearDown]
        public void RemovePresenter()
        {
            if (Presenter != null)
                Presenter.Dispose();

            Presenter = null;
        }
    }
}
