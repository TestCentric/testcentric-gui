// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// MultiCheckedToolStripButtonGroup wraps a set of ToolStrip button items as an IMultiSelection.
    /// </summary>
    internal class MultiCheckedToolStripButtonGroup : IMultiSelection
    {
        public event CommandHandler SelectionChanged;

        public MultiCheckedToolStripButtonGroup(params ToolStripButton[] buttons)
        {
            ToolStripButtons = new List<ToolStripButton>();

            foreach (var button in buttons)
            {
                ToolStripButtons.Add(button);
                button.Click += OnButtonClicked;
            }
        }

        protected IList<ToolStripButton> ToolStripButtons { get; }

        public IEnumerable<string> SelectedItems
        {
            get
            {
                IList<string> result = new List<string>();
                foreach (ToolStripButton button in ToolStripButtons)
                    if (button.Checked)
                        result.Add(button.Tag as string);

                return result;
            }

            set
            {
                foreach (ToolStripButton button in ToolStripButtons)
                {
                    bool checkStatus = value.Contains(button.Tag as string);
                    button.Checked = checkStatus;
                }
            }
        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;

                foreach (ToolStripButton button in ToolStripButtons)
                    button.Enabled = value;
            }
        }

        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                foreach (ToolStripButton button in ToolStripButtons)
                    button.Visible = value;
            }
        }

        public string Text { get; set; }


        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void OnButtonClicked(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged();
        }
    }
}
