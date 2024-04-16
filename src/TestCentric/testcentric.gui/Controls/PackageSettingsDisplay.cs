// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    public partial class PackageSettingsDisplay : UserControl
    {
        public PackageSettingsDisplay()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get { return packageSettings.Text; }
            set { this.InvokeIfRequired(() => { packageSettings.Text = value; }); }
        }
    }
}
