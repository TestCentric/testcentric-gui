// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
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
            set { InvokeIfRequired(() => { packageSettings.Text = value; }); }
        }

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(_delegate);
            else
                _delegate();
        }
    }
}
