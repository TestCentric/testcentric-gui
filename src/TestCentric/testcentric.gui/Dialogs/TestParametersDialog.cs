using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCentric.Gui.Dialogs
{
    public partial class TestParametersDialog : Form
    {
        public TestParametersDialog()
        {
            InitializeComponent();
        }

        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new ParameterDialog())
            {
                dlg.Font = Font;
                dlg.Text = "New Test Parameter";
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var item = new ListViewItem(new string[] { dlg.ParameterName, dlg.ParameterValue });
                    listView1.Items.Add(item);
                    item.Selected = true;
                    saveButton.Focus();
                }
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];
            var name = item.SubItems[0].Text;
            var value = item.SubItems[1].Text;

            using (var dlg = new ParameterDialog(name, value))
            {
                dlg.Font = Font;
                dlg.Text = "Edit Test Parameter";
                dlg.StartPosition = FormStartPosition.CenterParent;
                var result = dlg.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    item.SubItems[0].Text = dlg.ParameterName;
                    item.SubItems[1].Text = dlg.ParameterValue;
                    saveButton.Focus();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeButton.Enabled = editButton.Enabled = listView1.SelectedIndices.Count > 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Parameters.Clear();
            foreach (ListViewItem item in listView1.Items)
                Parameters.Add(item.SubItems[0].Text, item.SubItems[1].Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RunParametersDialog_Load(object sender, EventArgs e)
        {
            foreach (var key in Parameters.Keys)
                listView1.Items.Add(new ListViewItem(new string[] { key, Parameters[key] }));

            AutoSizeValueColumn();
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            AutoSizeValueColumn();
        }

        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (e.ColumnIndex == 0)
                AutoSizeValueColumn();
        }

        private void AutoSizeValueColumn()
        {
            int width = listView1.ClientSize.Width - listView1.Columns[0].Width;
            listView1.Columns[1].Width = width;
        }
    }
}
