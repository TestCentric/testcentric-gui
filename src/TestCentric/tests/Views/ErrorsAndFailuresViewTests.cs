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
            panelTester.AssertControlExists("flowLayoutPanel", typeof(FlowLayoutPanel));
            panelTester.AssertControlExists("tabSplitter", typeof(Splitter));
            panelTester.AssertControlExists("errorBrowser", typeof(NUnit.UiException.Controls.ErrorBrowser));

            var flowLayoutPanelTester = new ControlTester(_control.Controls["panel1"].Controls["flowLayoutPanel"]);
            flowLayoutPanelTester.AssertControlExists("detailList", typeof(ListBox));
            flowLayoutPanelTester.AssertControlExists("testOutputSubView", typeof(TestOutputSubView));
            flowLayoutPanelTester.AssertControlExists("testResultSubView", typeof(TestResultSubView));
        }

        //[Test]
        //public void ControlsArePositionedCorrectly()
        //{
        //    AssertControlsAreStackedVertically( "detailList", "tabSplitter", "errorBrowser" );
        //}
    }
}
