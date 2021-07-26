// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Elements;

    public partial class TestPropertiesView : UserControl, ITestPropertiesView
    {
        public event CommandHandler DisplayHiddenPropertiesChanged;

        public TestPropertiesView()
        {
            InitializeComponent();

            displayHiddenProperties.CheckedChanged += (s, e) =>
            {
                if (DisplayHiddenPropertiesChanged != null)
                    DisplayHiddenPropertiesChanged();
            };
        }

        public string Header
        {
            get { return header.Text; }
            set { InvokeIfRequired(() => { header.Text = value; }); }
        }

        public void ShowPackagePanel()
        {
            InvokeIfRequired(() =>
            {
                packagePanel.Visible = true;
                splitContainer1.Panel1Collapsed = false;
            });
        }
        public void HidePackagePanel()
        {
            InvokeIfRequired(() =>
            {
                packagePanel.Visible = false;
                splitContainer1.Panel1Collapsed = true;
            });
        }

        public void ShowTestPanel()
        {
            InvokeIfRequired(() =>
            {
                testPanel.Visible = true;
            });
        }
        public void HideTestPanel()
        {
            InvokeIfRequired(() =>
            {
                testPanel.Visible = false;
            });
        }

        public void ShowResultPanel()
        {
            InvokeIfRequired(() =>
            {
                resultPanel.Visible = true;
                splitContainer2.Panel2Collapsed = false;
                ClientSize = new Size(ClientSize.Width, splitContainer1.Bottom + 2);
            });
        }
        public void HideResultPanel()
        {
            InvokeIfRequired(() =>
            {
                resultPanel.Visible = false;
                splitContainer2.Panel2Collapsed = true;
                ClientSize = new Size(ClientSize.Width, splitContainer1.Bottom + 2);
            });
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
            get { return testCount.Text; }
            set { InvokeIfRequired(() => { testCount.Text = value; }); }
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

        public string Outcome
        {
            get { return outcome.Text; }
            set { InvokeIfRequired(() => { outcome.Text = value; }); }
        }

        public string ElapsedTime
        {
            get { return elapsedTime.Text; }
            set { InvokeIfRequired(() => { elapsedTime.Text = value; }); }
        }

        public string AssertCount
        {
            get { return assertCount.Text; }
            set { InvokeIfRequired(() => { assertCount.Text = value; }); }
        }

        public string Assertions
        {
            get { return assertions.Text; }
            set { InvokeIfRequired(() => { assertions.Text = value; }); }
        }

        public string Output
        {
            get { return output.Text; }
            set { InvokeIfRequired(() => { output.Text = value; }); }
        }
        public string PackageSettings
        {
            get { return packageSettings.Text; }
            set { InvokeIfRequired(() => { packageSettings.Text = value; }); }
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
