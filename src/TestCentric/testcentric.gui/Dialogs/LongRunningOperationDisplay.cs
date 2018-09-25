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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    /// <summary>
    /// LongRunningOperationDisplay shows an overlay message block 
    /// that describes the operation in progress.
    /// </summary>
    public class LongRunningOperationDisplay : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label operation;
        private Cursor _originalCursor;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public LongRunningOperationDisplay(Form owner, string text)
		{
			InitializeComponent();
            
			// Save the owner
			Owner = owner;

			// Save owner's current cursor and set it to the WaitCursor
            _originalCursor = owner.Cursor;
            owner.Cursor = Cursors.WaitCursor;

            // Display the text
			this.operation.Text = text;

            // Force immediate display upon construction
            Show();
            Invalidate();
            Update();

			Application.DoEvents();
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

				Owner.Cursor = _originalCursor;
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.operation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // operation
            // 
            this.operation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.operation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operation.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.operation.Location = new System.Drawing.Point(0, 0);
            this.operation.Name = "operation";
            this.operation.Size = new System.Drawing.Size(320, 40);
            this.operation.TabIndex = 0;
            this.operation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LongRunningOperationDisplay
            // 
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(320, 40);
            this.ControlBox = false;
            this.Controls.Add(this.operation);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LongRunningOperationDisplay";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad (e);
    
            // Set this again, see Mono Bug #82769
            this.ClientSize = new System.Drawing.Size(320, 40);
            Point origin = this.Owner.Location;
            origin.Offset( 
                (this.Owner.Size.Width - this.Size.Width) / 2,
                (this.Owner.Size.Height - this.Size.Height) / 2 );
            this.Location = origin;
        }
    }
}
