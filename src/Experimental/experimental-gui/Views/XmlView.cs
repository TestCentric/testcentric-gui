// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;
using System.Xml;

namespace TestCentric.Gui.Views
{
    using Elements;

    public partial class XmlView : UserControl, IXmlView
    {
        private XmlNode _testXml;
        public event CommandHandler SelectAllCommand;
        public event CommandHandler SelectionChanged;
        public event CommandHandler CopyCommand;
        public event CommandHandler WordWrapChanged;
        public event CommandHandler ViewGotFocus;

        public XmlView()
        {
            InitializeComponent();

            XmlPanel = new ControlElement(xmlPanel);
            CopyToolStripMenuItem = new ToolStripMenuElement(copyToolStripMenuItem);
            WordWrapToolStripMenuItem = new ToolStripMenuElement(wordWrapToolStripMenuItem);
            selectAllToolStripMenuItem.Click += (s, a) =>
            {
                if (SelectAllCommand != null)
                    SelectAllCommand();
            };

            xmlTextBox.SelectionChanged += (s, a) =>
            {
                if (SelectionChanged != null)
                    SelectionChanged();
            };

            copyToolStripMenuItem.Click += (s, a) =>
            {
                if (CopyCommand != null)
                    CopyCommand();
            };

            wordWrapToolStripMenuItem.CheckedChanged += (s, a) =>
            {
                if (WordWrapChanged != null)
                    WordWrapChanged();
            };
        }

        public string Header
        {
            get { return header.Text; }
            set { InvokeIfRequired(() => { header.Text = value; }); }
        }

        public IViewElement XmlPanel { get; private set; }

        public ICommand CopyToolStripMenuItem { get; private set; }

        public IChecked WordWrapToolStripMenuItem { get; private set; }

        public bool WordWrap
        {
            get { return xmlTextBox.WordWrap; }
            set { InvokeIfRequired(() => xmlTextBox.WordWrap = value ); }
        }

        public XmlNode TestXml
        {
            get { return _testXml; }
            set
            {
                _testXml = value;
                InvokeIfRequired(() => xmlTextBox.Rtf = _testXml != null ? new Xml2RtfConverter(2).Convert(_testXml) : "");
            }
        }

        public string SelectedText
        {
            get { return xmlTextBox.SelectedText; }
            set { InvokeIfRequired(() => xmlTextBox.SelectedText = value); }
        }

        public void SelectAll()
        {
            xmlTextBox.Focus();
            xmlTextBox.SelectAll();
        }

        public void Copy()
        {
            xmlTextBox.Copy();
        }

        public void InvokeFocus()
        {
            ViewGotFocus?.Invoke();
        }

        #region Helper Methods

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(_delegate);
            else
                _delegate();
        }

        #endregion

    }
}
