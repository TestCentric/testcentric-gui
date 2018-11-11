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

using System;
using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    using Views;

    [TestFixture]
    public class LongRunningOperationDisplayTests
    {
        private IMainView _view;
        private LongRunningOperationDisplay _display;

        [OneTimeSetUp]
        public void CreateDisplay()
        {
            _view = new TestCentricMainView();
            _display = _view.LongOperationDisplay("Loading...");
        }

        [OneTimeTearDown]
        public void CloseDisplay()
        {
            _display.Close();
            _display = null;
        }

        [Test]
        public void DisplayIsOwnedByView()
        {
            Assert.That(_display.Owner, Is.EqualTo(_view));
        }

        [Test]
        public void DisplayShowsProperText()
        {
            Assert.That(GetOperationText(_display), Is.EqualTo("Loading..."));
        }

        private string GetOperationText(Control display)
        {
            foreach (Control control in display.Controls)
                if (control.Name == "operation")
                    return control.Text;

            return null;
        }
    }
}
