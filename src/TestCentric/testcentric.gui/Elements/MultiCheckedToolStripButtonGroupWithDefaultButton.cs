// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This class is a specialized class of class MultiCheckedToolStripButtonGroup
    /// Therefore this class supports also the selection of multiple buttons.
    /// But additionally there is one special default button that resets all other buttons when it is clicked.
    /// </summary>
    internal class MultiCheckedToolStripButtonGroupWithDefaultButton : MultiCheckedToolStripButtonGroup
    {
        public MultiCheckedToolStripButtonGroupWithDefaultButton(params ToolStripButton[] buttons) : base(buttons)
        {
            // By convention the default button is the first one in the list
            DefaultButton = buttons.First();
            IndividualButtons = buttons.Skip(1);

            foreach (ToolStripButton button in ToolStripButtons)
                button.CheckedChanged += OnButtonCheckedChanged;

            DefaultButton.Checked = true;
        }

        private ToolStripButton DefaultButton { get; }

        private IEnumerable<ToolStripButton> IndividualButtons { get; }

        protected override void OnButtonClicked(object sender, EventArgs e)
        {
            ToolStripButton clickedButton = sender as ToolStripButton;
            if (clickedButton == DefaultButton)
                DefaultButtonClicked(sender, e);
            else
                IndividualButtonClicked(sender, e);

            base.OnButtonClicked(sender, e);
        }

        private void DefaultButtonClicked(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in IndividualButtons)
                button.Checked = false;
        }

        private void IndividualButtonClicked(object sender, EventArgs e)
        {
            DefaultButton.Checked = false;
        }

        private void OnButtonCheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            var style = button.Checked ? FontStyle.Bold : FontStyle.Regular;
            button.Font = new Font(button.Font, style);
        }
    }
}
