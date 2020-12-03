// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    using Model;

    public class LongRunningOperationDisplay : Form
    {
        private Label operation;
        private readonly Cursor originalCursor;
        private bool disposed = false;

        public LongRunningOperationDisplay(ITestModel model, string text)
            : this(text)
        {
            // The display doesn't understand the reason it's called.
            // It removes itself if any of the following events occurs.
            model.Events.TestLoaded += x => Dispose();
            model.Events.TestReloaded += x => Dispose();
            model.Events.TestLoadFailure += x => Dispose();
            model.Events.RunFinished += x => Dispose();
        }

        public LongRunningOperationDisplay(string text)
        {
            Owner = ActiveForm;
            originalCursor = Owner.Cursor;

            InitializeComponent();

            operation.Text = text;

            Show();
            Invalidate();
            Update();

            Application.DoEvents();
        }

        private void InitializeComponent()
        {
            this.operation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // operation
            // 
            this.operation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.operation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.operation.Location = new System.Drawing.Point(0, 0);
            this.operation.Name = "operation";
            this.operation.Size = new System.Drawing.Size(320, 60);
            this.operation.TabIndex = 0;
            this.operation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LongRunningOperationDisplay
            // 
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(320, 60);
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ClientSize = new Size(320, 60);
            var origin = Owner.Location;
            origin.Offset(
                (Owner.Size.Width - Size.Width) / 2,
                (Owner.Size.Height - Size.Height) / 2);

            Location = origin;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    Owner.Cursor = originalCursor;

                base.Dispose(disposing);

                disposed = true;
            }
        }
    }
}
