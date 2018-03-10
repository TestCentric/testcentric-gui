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
using NUnit.TestUtilities;

namespace NUnit.ProjectEditor.Tests.Views
{
	[TestFixture]
	public class AddConfigurationDialogTests : FormTester
	{
		private AddConfigurationDialog dlg;

		[SetUp]
		public void SetUp()
		{
            dlg = new AddConfigurationDialog();
            dlg.ConfigList = new string[] { "Debug", "Release" };
            this.Form = dlg;
		}

		[TearDown]
		public void TearDown()
		{
			dlg.Close();
		}

        [Test]
        public void CheckForControls()
        {
            AssertControlExists("configurationNameTextBox", typeof(TextBox));
            AssertControlExists("configurationComboBox", typeof(ComboBox));
            AssertControlExists("okButton", typeof(Button));
            AssertControlExists("cancelButton", typeof(Button));
        }

        [Test]
        public void TextBox_OnLoad_IsEmpty()
        {
            TextBox configBox = TextBoxes["configurationNameTextBox"];
            Assert.AreEqual("", configBox.Text);
        }

        [Test]
        public void ComboBox_OnLoad_IsInitializedCorrectly()
        {
            ComboBox combo = Combos["configurationComboBox"];
            dlg.Show();
            Assert.AreEqual(3, combo.Items.Count);
            Assert.AreEqual("<none>", combo.Items[0]);
            Assert.AreEqual("Debug", combo.Items[1]);
            Assert.AreEqual("Release", combo.Items[2]);
            Assert.AreEqual("<none>", combo.SelectedItem);
        }

        [Test]
        public void TestSimpleEntry()
        {
            dlg.Show();
            TextBox config = TextBoxes["configurationNameTextBox"];
            Button okButton = Buttons["okButton"];
            config.Text = "Super";
            okButton.PerformClick();
            Assert.AreEqual("Super", dlg.ConfigToCreate);
            Assert.AreEqual(null, dlg.ConfigToCopy);
        }

        [Test]
        public void TestComplexEntry()
        {
            dlg.Show();
            TextBox config = TextBoxes["configurationNameTextBox"];
            Button okButton = Buttons["okButton"];
            ComboBox combo = Combos["configurationComboBox"];

            config.Text = "Super";
            combo.SelectedIndex = combo.FindStringExact("Release");

            okButton.PerformClick();
            Assert.AreEqual("Super", dlg.ConfigToCreate);
            Assert.AreEqual("Release", dlg.ConfigToCopy);
        }
	}
}
