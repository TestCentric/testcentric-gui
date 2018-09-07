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

    public class SimpleSettingsDialog : SettingsDialogBase
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.IContainer components = null;

        public static void Display( TestCentricPresenter presenter, ITestModel model, SettingsPage page )
        {
            using( SimpleSettingsDialog dialog = new SimpleSettingsDialog(presenter, model) )
            {
                dialog.Font = model.Services.UserSettings.Gui.Font;
                dialog.SettingsPages.Add( page ); 
                dialog.ShowDialog();
            }
        }

        public SimpleSettingsDialog(TestCentricPresenter presenter, ITestModel model) : base(presenter, model)
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(410, 392);
            this.cancelButton.Name = "cancelButton";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(322, 392);
            this.okButton.Name = "okButton";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(16, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 336);
            this.panel1.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(16, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 8);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // SimpleSettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(490, 426);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SimpleSettingsDialog";
            this.Load += new System.EventHandler(this.SimpleSettingsDialog_Load);
            this.Controls.SetChildIndex(this.okButton, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.ResumeLayout(false);

        }
        #endregion

        private void SimpleSettingsDialog_Load(object sender, System.EventArgs e)
        {
            SettingsPage page = this.SettingsPages[0];
            this.panel1.Controls.Add( page );
            this.Text = page.Title;
        }
    }
}

