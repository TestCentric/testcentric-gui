// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    public class LongRunningOperationDisplay : Form
    {
        private readonly Label _operation;
        private readonly Cursor _originalCursor;

        public LongRunningOperationDisplay(Form owner, string text)
        {
            Owner = owner;
            _originalCursor = owner.Cursor;
            _operation = new Label()
            {
                Name = "operation", // Test uses this
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 10.2F, FontStyle.Italic, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text
            };

            InitializeComponent();

            Show();
            Invalidate();
            Update();

            Application.DoEvents();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.LightYellow;
            ClientSize = new Size(320, 40);
            ControlBox = false;
            Controls.Add(_operation);
            Cursor = Cursors.WaitCursor;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LongRunningOperationDisplay";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            ResumeLayout(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ClientSize = new Size(320, 40);
            var origin = Owner.Location;
            origin.Offset(
                (Owner.Size.Width - Size.Width) / 2,
                (Owner.Size.Height - Size.Height) / 2);

            Location = origin;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Owner.Cursor = _originalCursor;

            base.Dispose(disposing);
        }
    }
}
