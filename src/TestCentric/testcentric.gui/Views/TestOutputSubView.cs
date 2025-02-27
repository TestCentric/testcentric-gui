// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public partial class TestOutputSubView : TestPropertiesView.SubView, ITestOutputSubView
    {
        public TestOutputSubView()
        {
            InitializeComponent();
        }

        public override int FullHeight => output.Top + HeightNeededForControl(output) + 4;

        public string Output
        {
            get { return output.Text; }
            set { this.InvokeIfRequired(() => { output.Text = value; }); }
        }
    }
}
