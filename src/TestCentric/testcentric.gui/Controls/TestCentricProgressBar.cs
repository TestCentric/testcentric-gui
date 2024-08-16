// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    public enum ProgressBarStatus
    {
        Success = 0,
        Warning = 1,
        Failure = 2
    }

    public class TestCentricProgressBar : ProgressBar
    {
        private const int SUCCESS = 0;
        private const int WARNING = 1;
        private const int FAILURE = 2;

        public readonly static Color[][] BrushColors =
        {
              new Color[] { Color.FromArgb(32, 205, 32), Color.FromArgb(16, 64, 16) },  // Success
              new Color[] { Color.FromArgb(255, 255, 0), Color.FromArgb(242, 242, 0) }, // Warning
              new Color[] { Color.FromArgb(255, 0, 0), Color.FromArgb(150, 0, 0) }      // Failure
        };

        private Brush _brush;

        public TestCentricProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            CreateNewBrush();
        }

        #region Properties

        private ProgressBarStatus _status = ProgressBarStatus.Success;
        [Browsable(false)]
        public ProgressBarStatus Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;

                    CreateNewBrush();
                }
            }
        }

        [Browsable(true), Category("Appearance")]
        public bool ShowPercentComplete { get; set; }

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Maximum == 0)       // No progress can be painted in this case
                return;

            Rectangle rec = this.ClientRectangle;
            rec.Inflate(-2, -2);
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
            rec.Inflate(-1, -1);
            rec.Width = (int)(rec.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(_brush, rec); //2, 2, rec.Width, rec.Height);

            int percentComplete = Value * 100 / Maximum;
            if (ShowPercentComplete && percentComplete > 0)
            {
                string text = $"{(int)percentComplete}%";
                float fontSize = ClientRectangle.Height / 2;
                Font font = new Font( FontFamily.GenericMonospace, fontSize, FontStyle.Bold);
                int textWidth = e.Graphics.MeasureString(text, font).ToSize().Width;
                e.Graphics.DrawString(text, font, Brushes.Black, new Point((rec.Width - textWidth)/2, 2));
            }
        }

        private void CreateNewBrush()
        {
            Color[] colors = BrushColors[(int)_status];

            if (_brush != null)
                _brush.Dispose();

            _brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, this.ClientSize.Height - 3),
                colors[0],
                colors[1]);

            Invalidate();
        }

        #endregion
    }
}
