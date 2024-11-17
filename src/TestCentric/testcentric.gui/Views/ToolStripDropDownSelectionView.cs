// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestCentric.Gui.Elements;

namespace TestCentric.Gui.Views
{
    internal class ToolStripDropDownSelectionView : Form
    {
        CheckedListBox categoryListBox;
        TextBox textBox;
        TableLayoutPanel panel;

        public delegate void OnDropDownClosedEventHandler(IList<string> selectedItems);

        public event OnDropDownClosedEventHandler OnDropDownClosed;

        public ToolStripDropDownSelectionView(IList<string> items, IList<string> selectedItems)
        {
            AllItems = items;

            this.FormBorderStyle = FormBorderStyle.Sizable; // .SizableToolWindow;
            ShowInTaskbar = false;
            // ControlBox = false;
            SizeGripStyle = SizeGripStyle.Show;
            Text = "Category filter";

            panel = new TableLayoutPanel();
            panel.RowCount = 3;
            panel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            panel.Dock = DockStyle.Fill;

            FlowLayoutPanel flowControlRow1 = new FlowLayoutPanel();
            Button buttonSelectAll = new Button();
            buttonSelectAll.AutoSize = true;
            buttonSelectAll.Text = "Select all";
            buttonSelectAll.Click += SelectAllClicked;

            Button buttonClear = new Button();
            buttonClear.AutoSize = true;
            buttonClear.Text = "Clear";
            buttonClear.Click += ClearClicked;
            buttonClear.Location = new System.Drawing.Point(buttonSelectAll.Location.X + buttonSelectAll.Size.Width + buttonSelectAll.Margin.Right, buttonSelectAll.Location.Y);

            textBox = new TextBox();
            textBox.Margin = new Padding(2);
            textBox.Padding = new Padding(10);
            textBox.AutoSize = false;
            textBox.Location = new System.Drawing.Point(buttonClear.Location.X + buttonClear.Size.Width + buttonClear.Margin.Right, buttonSelectAll.Location.Y);
            textBox.Height = buttonClear.Height;
            // textBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            // textBox.Dock = DockStyle.Right;
            // textBox.Height = buttonClear.Height;
            textBox.LostFocus += TextBoxLostFocus;
            textBox.GotFocus += TextBoxGotFocus;
            textBox.TextChanged += OnTextChanged;
            textBox.ForeColor = System.Drawing.Color.LightGray;
            TextBoxLostFocus(null, EventArgs.Empty);


            flowControlRow1.Controls.Add(buttonSelectAll);
            flowControlRow1.Controls.Add(buttonClear);
            flowControlRow1.Controls.Add(textBox);
            flowControlRow1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flowControlRow1.Dock = DockStyle.Fill;
            flowControlRow1.AutoSize = true;

            categoryListBox = new CheckedListBox();
            categoryListBox.CheckOnClick = true;

            foreach (string item in AllItems)
            {
                categoryListBox.Items.Add(item);
                if (selectedItems.IndexOf(item) > -1)
                    categoryListBox.SetItemChecked(categoryListBox.Items.Count - 1, true);
            }

            categoryListBox.Margin = new Padding(2);
            categoryListBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            categoryListBox.Dock = DockStyle.Fill;

            FlowLayoutPanel flowControl = new FlowLayoutPanel();
            flowControl.FlowDirection = FlowDirection.RightToLeft;
            Button applyButton = new Button();
            applyButton.Text = "Apply";
            applyButton.Click += OkButtonClicked;
            applyButton.AutoSize = true;

            //okButton.Margin = new Padding(20, 2, 20, 2);
            Button applyAndCloseButton = new Button();
            applyAndCloseButton.Text = "Apply + Close";
            applyAndCloseButton.Click += CancelButtonClicked;
            applyAndCloseButton.AutoSize = true;

            // cancelButton.Margin = new Padding(20, 2, 20, 2);

            flowControl.Controls.Add(applyAndCloseButton);
            flowControl.Controls.Add(applyButton);
            flowControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flowControl.Dock = DockStyle.Fill;
            flowControl.AutoSize = true;

            panel.Controls.Add(flowControlRow1, 0, 0);
            panel.Controls.Add(categoryListBox, 0, 1);
            panel.Controls.Add(flowControl, 0, 2);

            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Controls.Add(panel);
        }

        public IList<string> SelectedItems
        {
            get
            {
                var selectedItems = new List<string>();
                foreach (var item in categoryListBox.CheckedItems)
                {
                    selectedItems.Add(item.ToString());
                }

                return selectedItems;
            }
        }

        private IList<string> AllItems { get; set; }

        public void ShowDropDown(object parent)
        {
            StartPosition = FormStartPosition.Manual;
            //var button = parent as ToolStripDropDownButton;

            //Point p = parent.PointToScreen(button.Bounds.Location);
            //p.X -= 2 * this.Margin.Left;
            //p.Y += button.Bounds.Height;
            //Location = p;

            Show();
        }

        private void OkButtonClicked(object sender, EventArgs e)
        {
            OnDropDownClosed?.Invoke(SelectedItems);
            //Close();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            OnDropDownClosed?.Invoke(SelectedItems);
            Close();
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < categoryListBox.Items.Count; i++)
            {
                categoryListBox.SetItemChecked(i, false);
            }
        }

        private void SelectAllClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < categoryListBox.Items.Count; i++)
            {
                categoryListBox.SetItemChecked(i, true);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;

            const int HTLEFT = 10;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOMLEFT = 16;

            // Call base method 
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
            {
                int baseWndProcResult = m.Result.ToInt32();
                if (baseWndProcResult == HTLEFT || baseWndProcResult == HTTOP || baseWndProcResult == HTTOPLEFT || baseWndProcResult == HTTOPRIGHT || baseWndProcResult == HTBOTTOMLEFT)
                    m.Result = new IntPtr(0);
            }

        }

        private void TextBoxGotFocus(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
            {
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.Color.Black;
                PlaceHolderTextUsed = false;
            }
        }

        private void TextBoxLostFocus(object sender, EventArgs e)
        {
            string searchText = textBox.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                PlaceHolderTextUsed = true;
                textBox.Text = "Search...";
                textBox.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        private bool PlaceHolderTextUsed { get; set; }
        private Timer _typingTimer;

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
                return;

            if (_typingTimer == null)
            {
                _typingTimer = new Timer();
                _typingTimer.Interval = 600;
                _typingTimer.Tick += TypingTimerTimeout;
            }

            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private void TypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
                return;

            // The timer must be stopped!
            timer.Stop();

            string searchText = textBox.Text;
            categoryListBox.SuspendLayout();
            categoryListBox.Items.Clear();
            foreach (string item in AllItems)
            {
                if (string.IsNullOrEmpty(searchText) || item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1)
                    categoryListBox.Items.Add(item);
            }

            categoryListBox.ResumeLayout();
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    if (DialogResult == DialogResult.OK)
        //    {
        //        OnDropDownClosed?.Invoke(SelectedItems);
        //    }

        //    base.OnClosed(e);
        //}
    }
}
