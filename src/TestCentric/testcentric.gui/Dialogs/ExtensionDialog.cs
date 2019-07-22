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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace TestCentric.Gui
{
    /// <summary>
    /// Summary description for AddinDialog.
    /// </summary>
    public class ExtensionDialog : System.Windows.Forms.Form
    {
        private IList<IExtensionPoint> _extensionPoints;
        private IList<IExtensionNode> _extensions;

        private System.Windows.Forms.TextBox extensionPointDescriptionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView extensionListView;
        private System.Windows.Forms.ColumnHeader extensionNameColumn;
        private System.Windows.Forms.ColumnHeader extensionStatusColumn;
        private ListBox extensionPointsListBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox extensionDescriptionTextBox;
        private Label label2;
        private Label label3;
        private TextBox propertiesTextBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private IExtensionService _extensionService;

        public ExtensionDialog(IExtensionService extensionService)
        {
            _extensionService = extensionService;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionDialog));
            this.extensionListView = new System.Windows.Forms.ListView();
            this.extensionNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.extensionStatusColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.extensionPointDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.extensionPointsListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.propertiesTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.extensionDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // extensionListView
            // 
            this.extensionListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.extensionNameColumn,
            this.extensionStatusColumn});
            this.extensionListView.FullRowSelect = true;
            this.extensionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.extensionListView.HideSelection = false;
            this.extensionListView.Location = new System.Drawing.Point(7, 23);
            this.extensionListView.MultiSelect = false;
            this.extensionListView.Name = "extensionListView";
            this.extensionListView.Size = new System.Drawing.Size(432, 59);
            this.extensionListView.TabIndex = 0;
            this.extensionListView.UseCompatibleStateImageBehavior = false;
            this.extensionListView.View = System.Windows.Forms.View.Details;
            this.extensionListView.SelectedIndexChanged += new System.EventHandler(this.extensionListView_SelectedIndexChanged);
            this.extensionListView.Resize += new System.EventHandler(this.extensionListView_Resize);
            // 
            // extensionNameColumn
            // 
            this.extensionNameColumn.Text = "Extension";
            this.extensionNameColumn.Width = 357;
            // 
            // extensionStatusColumn
            // 
            this.extensionStatusColumn.Text = "Status";
            this.extensionStatusColumn.Width = 71;
            // 
            // extensionPointDescriptionTextBox
            // 
            this.extensionPointDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionPointDescriptionTextBox.Location = new System.Drawing.Point(9, 128);
            this.extensionPointDescriptionTextBox.Multiline = true;
            this.extensionPointDescriptionTextBox.Name = "extensionPointDescriptionTextBox";
            this.extensionPointDescriptionTextBox.Size = new System.Drawing.Size(432, 31);
            this.extensionPointDescriptionTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(6, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description:";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(192, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // extensionPointsListBox
            // 
            this.extensionPointsListBox.FormattingEnabled = true;
            this.extensionPointsListBox.Location = new System.Drawing.Point(9, 19);
            this.extensionPointsListBox.Name = "extensionPointsListBox";
            this.extensionPointsListBox.Size = new System.Drawing.Size(432, 82);
            this.extensionPointsListBox.TabIndex = 6;
            this.extensionPointsListBox.SelectedIndexChanged += new System.EventHandler(this.extensionPointsListBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.extensionPointDescriptionTextBox);
            this.groupBox1.Controls.Add(this.extensionPointsListBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 165);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extension Points";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.propertiesTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.extensionDescriptionTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.extensionListView);
            this.groupBox2.Location = new System.Drawing.Point(5, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 230);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Installed Extensions";
            // 
            // propertiesTextBox
            // 
            this.propertiesTextBox.Location = new System.Drawing.Point(7, 179);
            this.propertiesTextBox.Multiline = true;
            this.propertiesTextBox.Name = "propertiesTextBox";
            this.propertiesTextBox.Size = new System.Drawing.Size(432, 41);
            this.propertiesTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Properties";
            // 
            // extensionDescriptionTextBox
            // 
            this.extensionDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionDescriptionTextBox.Location = new System.Drawing.Point(7, 115);
            this.extensionDescriptionTextBox.Multiline = true;
            this.extensionDescriptionTextBox.Name = "extensionDescriptionTextBox";
            this.extensionDescriptionTextBox.Size = new System.Drawing.Size(432, 31);
            this.extensionDescriptionTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(6, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description:";
            // 
            // ExtensionDialog
            // 
            this.ClientSize = new System.Drawing.Size(464, 449);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtensionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Engine Extensions";
            this.Load += new System.EventHandler(this.ExtensionDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void ExtensionDialog_Load(object sender, System.EventArgs e)
        {
            if (!DesignMode)
            {
                _extensionPoints = new List<IExtensionPoint>(_extensionService.ExtensionPoints);
                _extensions = new List<IExtensionNode>(_extensionService.Extensions);

                foreach (var ep in _extensionPoints)
                    extensionPointsListBox.Items.Add(ep.Path);

                if (extensionPointsListBox.Items.Count > 0)
                    extensionPointsListBox.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void extensionPointsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = extensionPointsListBox.SelectedIndex;
            if (index >= 0)
            {
                var ep = _extensionPoints[index];
                _extensions = new List<IExtensionNode>(ep.Extensions);
                extensionPointDescriptionTextBox.Text = ep.Description ?? "==None Provided==";

                extensionListView.Items.Clear();
                foreach (var extension in ep.Extensions)
                {
                    ListViewItem item = new ListViewItem(
                        new string[] { extension.TypeName, extension.Enabled ? "Enabled" : "Disabled" });
                    extensionListView.Items.Add(item);
                }

                if (extensionListView.Items.Count > 0)
                {
                    extensionListView.Items[0].Selected = true;
                    AutoSizeFirstColumnOfListView();
                }
                else
                {
                    extensionDescriptionTextBox.Text = "";
                    propertiesTextBox.Text = "";
                }
            }
        }

        private void extensionListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!DesignMode && extensionListView.SelectedIndices.Count > 0)
            {
                int index = extensionListView.SelectedIndices[0];
                var extension = _extensions[index];

                extensionDescriptionTextBox.Text = extension.Description ?? "==None Provided==";

                propertiesTextBox.Clear();
                foreach (string prop in extension.PropertyNames)
                {
                    var sb = new StringBuilder($"{prop} :");
                    foreach (string val in extension.GetValues(prop))
                        sb.Append(" " + val);

                    propertiesTextBox.AppendText(sb.ToString() + Environment.NewLine);
                }
            }
        }

        private void extensionListView_Resize(object sender, System.EventArgs e)
        {
            AutoSizeFirstColumnOfListView();
        }

        private void AutoSizeFirstColumnOfListView()
        {
            int width = extensionListView.ClientSize.Width;
            for (int i = 1; i < extensionListView.Columns.Count; i++)
                width -= extensionListView.Columns[i].Width;
            extensionListView.Columns[0].Width = width;
        }
    }
}
