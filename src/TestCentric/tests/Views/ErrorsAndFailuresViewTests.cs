// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;

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
            AssertControlExists("header", typeof(Label));
            AssertControlExists("panel1", typeof(Panel));

            var panelTester = new ControlTester(_control.Controls["panel1"]);
            panelTester.AssertControlExists("detailList", typeof(ListBox));
            panelTester.AssertControlExists("tabSplitter", typeof(Splitter));
            panelTester.AssertControlExists("errorBrowser", typeof(NUnit.UiException.Controls.ErrorBrowser));
        }

        //[Test]
        //public void ControlsArePositionedCorrectly()
        //{
        //    AssertControlsAreStackedVertically( "detailList", "tabSplitter", "errorBrowser" );
        //}
    }
}
