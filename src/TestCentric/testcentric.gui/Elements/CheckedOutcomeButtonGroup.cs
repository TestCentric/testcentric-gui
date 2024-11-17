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
    internal class CheckedOutcomeButtonGroup : MultiCheckedToolStripButtonGroup
    {
        public CheckedOutcomeButtonGroup(params ToolStripButton[] buttons) : base(buttons)
        {
            DefaultOutcomeButton = buttons.First();
            IndividualOutcomeButtons = buttons.Skip(1);

            foreach (ToolStripButton button in ToolStripButtons)
                button.CheckedChanged += OnButtonCheckedChanged;

            DefaultOutcomeButton.Checked = true;
        }

        private ToolStripButton DefaultOutcomeButton { get; }

        private IEnumerable<ToolStripButton> IndividualOutcomeButtons { get; }

        protected override void OnButtonClicked(object sender, EventArgs e)
        {
            ToolStripButton clickedButton = sender as ToolStripButton;
            if (clickedButton == DefaultOutcomeButton)
                DefaultOutcomeButtonClicked(sender, e);
            else 
                IndividualOutcomeButtonClicked(sender, e);

            base.OnButtonClicked(sender, e);
        }

        private void DefaultOutcomeButtonClicked(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in IndividualOutcomeButtons)
                button.Checked = false;
        }

        private void IndividualOutcomeButtonClicked(object sender, EventArgs e)
        {
            DefaultOutcomeButton.Checked = false;
        }

        private void OnButtonCheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            var style = button.Checked ? FontStyle.Bold : FontStyle.Regular;
            button.Font = new Font(button.Font, style);
        }
    }
}
