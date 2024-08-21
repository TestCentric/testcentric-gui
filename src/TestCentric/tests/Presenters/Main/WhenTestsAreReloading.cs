// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    [TestFixture]
    public class WhenTestsAreReloading : MainPresenterTestBase
    {
        [Test]
        public void ReloadingTests_SummaryResultView_IsHidden()
        {
            FireTestsReloadingEvent();
            _view.RunSummaryDisplay.Received().Hide();
        }
    }
}
