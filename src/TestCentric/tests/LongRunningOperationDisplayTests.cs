// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    using Views;

    [TestFixture]
    [Platform(Exclude = "Linux", Reason = "Display issues")]
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
