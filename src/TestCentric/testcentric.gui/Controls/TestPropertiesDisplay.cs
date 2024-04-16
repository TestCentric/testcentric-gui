// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestCentric.Gui.Elements;

namespace TestCentric.Gui.Controls
{
    public partial class TestPropertiesDisplay : UserControl
    {
        public event CommandHandler DisplayHiddenPropertiesChanged;

        public TestPropertiesDisplay()
        {
            InitializeComponent();

            displayHiddenProperties.CheckedChanged += (s, e) =>
            {
                DisplayHiddenPropertiesChanged?.Invoke();
            };
        }

        public string TestType
        {
            get { return testType.Text; }
            set { this.InvokeIfRequired(() => { testType.Text = value; }); }
        }

        public string FullName
        {
            get { return fullName.Text; }
            set { this.InvokeIfRequired(() => { fullName.Text = value; }); }
        }

        public string Description
        {
            get { return description.Text; }
            set { this.InvokeIfRequired(() => { description.Text = value; }); }
        }

        public string Categories
        {
            get { return categories.Text; }
            set { this.InvokeIfRequired(() => { categories.Text = value; }); }
        }

        public string TestCount
        {
            get { return testCaseCount.Text; }
            set { this.InvokeIfRequired(() => { testCaseCount.Text = value; }); }
        }

        public string RunState
        {
            get { return runState.Text; }
            set { this.InvokeIfRequired(() => { runState.Text = value; }); }
        }

        public string SkipReason
        {
            get { return skipReason.Text; }
            set { this.InvokeIfRequired(() => { skipReason.Text = value; }); }
        }

        public bool DisplayHiddenProperties
        {
            get { return displayHiddenProperties.Checked; }
        }

        public string Properties
        {
            get { return properties.Text; }
            set { properties.Text = value; }
        }
    }
}
