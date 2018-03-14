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
using System.Drawing;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Controls
{
    using Model;
    using Model.Settings;

	/// <summary>
	/// Summary description for ResultTabs.
	/// </summary>
	public class ResultTabs : UserControl, IViewControl
	{
		static Logger log = InternalTrace.GetLogger(typeof(ResultTabs));

		private TabControl tabControl;

		private TabPage errorTab;
		private ErrorDisplay errorDisplay;

		private TabPage notRunTab;
		private NotRunTree notRunTree;

        private TabPage outputTab;
        private TextBoxDisplay textBoxDisplay;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public ResultTabs()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl = new System.Windows.Forms.TabControl();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.errorDisplay = new ErrorDisplay();
            this.notRunTab = new System.Windows.Forms.TabPage();
            this.notRunTree = new NotRunTree();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.textBoxDisplay = new TextBoxDisplay();
            this.tabControl.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.notRunTab.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.errorTab);
            this.tabControl.Controls.Add(this.notRunTab);
            this.tabControl.Controls.Add(this.outputTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(488, 280);
            this.tabControl.TabIndex = 3;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // errorTab
            // 
            this.errorTab.Controls.Add(this.errorDisplay);
            this.errorTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorTab.Location = new System.Drawing.Point(4, 4);
            this.errorTab.Name = "errorTab";
            this.errorTab.Size = new System.Drawing.Size(480, 254);
            this.errorTab.TabIndex = 0;
            this.errorTab.Text = "Errors and Failures";
            // 
            // errorDisplay
            // 
            this.errorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorDisplay.Location = new System.Drawing.Point(0, 0);
            this.errorDisplay.Name = "errorDisplay";
            this.errorDisplay.Size = new System.Drawing.Size(480, 254);
            this.errorDisplay.TabIndex = 0;
            // 
            // notRunTab
            // 
            this.notRunTab.Controls.Add(this.notRunTree);
            this.notRunTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.notRunTab.Location = new System.Drawing.Point(4, 4);
            this.notRunTab.Name = "notRunTab";
            this.notRunTab.Size = new System.Drawing.Size(480, 254);
            this.notRunTab.TabIndex = 1;
            this.notRunTab.Text = "Tests Not Run";
            this.notRunTab.Visible = false;
            // 
            // notRunTree
            // 
            this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notRunTree.Indent = 19;
            this.notRunTree.Location = new System.Drawing.Point(0, 0);
            this.notRunTree.Name = "notRunTree";
            this.notRunTree.Size = new System.Drawing.Size(480, 254);
            this.notRunTree.TabIndex = 0;
            // 
            // outputTab
            // 
            this.outputTab.Controls.Add(this.textBoxDisplay);
            this.outputTab.Location = new System.Drawing.Point(4, 4);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(480, 254);
            this.outputTab.TabIndex = 2;
            this.outputTab.Text = "Text Output";
            this.outputTab.UseVisualStyleBackColor = true;
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplay.Location = new System.Drawing.Point(3, 3);
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.ReadOnly = true;
            this.textBoxDisplay.Size = new System.Drawing.Size(474, 248);
            this.textBoxDisplay.TabIndex = 0;
            this.textBoxDisplay.Text = "";
            this.textBoxDisplay.WordWrap = false;
            // 
            // ResultTabs
            // 
            this.Controls.Add(this.tabControl);
            this.Name = "ResultTabs";
            this.Size = new System.Drawing.Size(488, 280);
            this.tabControl.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.notRunTab.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        #region Properties

        private UserSettings Settings { get; set; }

        #endregion

        public void Clear()
        {
            errorDisplay.Clear();
            notRunTree.Nodes.Clear();
            textBoxDisplay.Clear();
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            int index = tabControl.SelectedIndex;
            if (index >= 0 && index < tabControl.TabCount)
                Settings.Gui.ResultTabs.SelectedTab = index;
        }

        private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			bool selected = e.Index == tabControl.SelectedIndex;

			Font font = selected ? new Font( e.Font, FontStyle.Bold ) : e.Font;
			Brush backBrush = new SolidBrush( selected ? SystemColors.Control : SystemColors.Window );
			Brush foreBrush = new SolidBrush( SystemColors.ControlText );

			e.Graphics.FillRectangle( backBrush, e.Bounds );
			Rectangle r = e.Bounds;
			r.Y += 3; r.Height -= 3;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.DrawString( tabControl.TabPages[e.Index].Text, font, foreBrush, r, sf );

			foreBrush.Dispose();
			backBrush.Dispose();
			if ( selected )
				font.Dispose();
		}

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            tabControl.ItemSize = new Size(tabControl.ItemSize.Width, this.Font.Height + 7);
        }

        #region IViewControl Implementation

        public void InitializeView(ITestModel model, TestCentricPresenter presenter)
        {
            Settings = model.Services.UserSettings;

            tabControl.SelectedIndex = Settings.Gui.ResultTabs.SelectedTab;

            model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                Clear();
            };

            model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                Clear();
            };

            model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
            if (Settings.Options.TestLoader.ClearResultsOnReload)
                this.Clear();
            };

            model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Clear();
            };
        }

        #endregion
	}
}
