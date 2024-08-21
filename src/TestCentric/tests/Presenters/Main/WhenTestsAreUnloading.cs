// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    [TestFixture]
    public class WhenTestsAreUnloading : MainPresenterTestBase
    {
        [Test]
        public void UnloadingTests_SummaryResultView_IsHidden()
        {
            FireTestsUnloadingEvent();
            _view.RunSummaryDisplay.Received().Hide();
        }
    }
}
