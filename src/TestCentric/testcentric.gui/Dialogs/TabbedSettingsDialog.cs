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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    using Model;

	public class TabbedSettingsDialog : SettingsDialogBase
	{
		protected System.Windows.Forms.TabControl tabControl1;
		private System.ComponentModel.IContainer components = null;

		public static void Display( Form owner, ITestModel model, params SettingsPage[] pages )
		{
			using( TabbedSettingsDialog dialog = new TabbedSettingsDialog(model) )
			{
				owner.Site.Container.Add( dialog );
				dialog.Font = owner.Font;
				dialog.SettingsPages.AddRange( pages ); 
				dialog.ShowDialog();
			}
		}

		public TabbedSettingsDialog(ITestModel model) : base(model)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(394, 392);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(306, 392);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.ItemSize = new System.Drawing.Size(46, 18);
            this.tabControl1.Location = new System.Drawing.Point(10, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 376);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // TabbedSettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(474, 426);
            this.Controls.Add(this.tabControl1);
            this.Name = "TabbedSettingsDialog";
            this.Load += new System.EventHandler(this.TabbedSettingsDialog_Load);
            this.Controls.SetChildIndex(this.okButton, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.ResumeLayout(false);

		}
		#endregion

		private void TabbedSettingsDialog_Load(object sender, System.EventArgs e)
		{
            string initialPage = Settings.TestCentric.InitialSettingsPage;

            foreach ( SettingsPage page in SettingsPages )
			{
				TabPage tabPage = new TabPage(page.Title);
				tabPage.Controls.Add( page );
				page.Location = new Point(0, 16);
				tabControl1.TabPages.Add( tabPage );
                if (page.Name == initialPage)
                    tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
			}
		}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var page = SettingsPages[tabControl1.SelectedIndex];
            Settings.TestCentric.InitialSettingsPage = page.Name;
        }
    }
}

