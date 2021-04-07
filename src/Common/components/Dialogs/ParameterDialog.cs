// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Dialogs
{
    public partial class ParameterDialog : Form
    {
        public ParameterDialog()
        {
            InitializeComponent();
        }

        public ParameterDialog(string name, string value)
        {
            InitializeComponent();

            ParameterName = name;
            ParameterValue = value;
        }

        public string ParameterName
        {
            get { return parameterNameTextBox.Text; }
            set { parameterNameTextBox.Text = value; }
        }

        public string ParameterValue
        {
            get { return parameterValueTextBox.Text; }
            set { parameterValueTextBox.Text = value; }
        }

        private void parameterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableDisableOKButton();
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableDisableOKButton();
        }

        private void EnableDisableOKButton()
        {
            okButton.Enabled = ParameterName.Length > 0 && ParameterValue.Length > 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
