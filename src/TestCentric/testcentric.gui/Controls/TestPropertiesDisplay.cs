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
                if (DisplayHiddenPropertiesChanged != null)
                    DisplayHiddenPropertiesChanged();
            };
        }

        public string TestType
        {
            get { return testType.Text; }
            set { InvokeIfRequired(() => { testType.Text = value; }); }
        }

        public string FullName
        {
            get { return fullName.Text; }
            set { InvokeIfRequired(() => { fullName.Text = value; }); }
        }

        public string Description
        {
            get { return description.Text; }
            set { InvokeIfRequired(() => { description.Text = value; }); }
        }

        public string Categories
        {
            get { return categories.Text; }
            set { InvokeIfRequired(() => { categories.Text = value; }); }
        }

        public string TestCount
        {
            get { return testCaseCount.Text; }
            set { InvokeIfRequired(() => { testCaseCount.Text = value; }); }
        }

        public string RunState
        {
            get { return runState.Text; }
            set { InvokeIfRequired(() => { runState.Text = value; }); }
        }

        public string SkipReason
        {
            get { return skipReason.Text; }
            set { InvokeIfRequired(() => { skipReason.Text = value; }); }
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

        #region Helper Methods

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(_delegate);
            else
                _delegate();
        }

        #endregion
    }
}
