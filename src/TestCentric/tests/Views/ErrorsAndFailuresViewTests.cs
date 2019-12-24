// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using TestCentric.TestUtilities;

namespace TestCentric.Gui.Views
{
    [TestFixture]
    [Platform(Exclude = "Linux", Reason = "Uninitialized form causes an error in Travis-CI")]
    public class ErrorsAndFailuresViewTests : ControlTester
    {
        [OneTimeSetUp]
        public void CreateForm()
        {
            this.Control = new ErrorsAndFailuresView();
        }

        [OneTimeTearDown]
        public void CloseForm()
        {
            this.Control.Dispose();
        }

        [Test]
        public void ControlsExist()
        {
            AssertControlExists("detailList", typeof(ListBox));
            AssertControlExists("tabSplitter", typeof(Splitter));
            AssertControlExists("errorBrowser", typeof(NUnit.UiException.Controls.ErrorBrowser));
        }

        //[Test]
        //public void ControlsArePositionedCorrectly()
        //{
        //    AssertControlsAreStackedVertically( "detailList", "tabSplitter", "errorBrowser" );
        //}
    }
}
