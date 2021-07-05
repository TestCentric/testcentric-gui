// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ControlElement is a generic wrapper for controls.
    /// </summary>
    public class ControlElement : IControlElement
    {
        private Control _control;

        public ControlElement(Control control)
        {
            _control = control;
        }

        public Point Location
        {
            get { return _control.Location; }
            set { InvokeIfRequired(() => { _control.Location = value; }); }
        }

        public Size Size
        {
            get { return _control.Size; }
            set { InvokeIfRequired(() => { _control.Size = value; }); }
        }

        public Size ClientSize
        {
            get { return _control.ClientSize; }
            set { InvokeIfRequired(() => { _control.ClientSize = value; }); }
        }

        public bool Enabled
        {
            get { return _control.Enabled; }
            set { InvokeIfRequired(() => { _control.Enabled = value; }); }
        }

        public bool Visible
        {
            get { return _control.Visible; }
            set { InvokeIfRequired(() => { _control.Visible = value; }); }
        }

        public string Text
        {
            get { return _control.Text; }
            set { InvokeIfRequired(() => { _control.Text = value; }); }
        }

        public void InvokeIfRequired(MethodInvoker del)
        {
            if (_control.InvokeRequired)
                _control.BeginInvoke(del, new object[0]);
            else
                del();
        }

        public ContextMenu ContextMenu
        {
            get { return _control.ContextMenu; }
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get { return _control.ContextMenuStrip; }
        }
    }
}
