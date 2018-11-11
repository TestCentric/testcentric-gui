// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace TestCentric.Gui.Views
{
    [TestFixture]
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
