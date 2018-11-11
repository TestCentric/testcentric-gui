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
using System.Windows.Forms;
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    public delegate bool ViewClosingDelegate();

    public partial class MainForm : Form, IMainView
    {
        #region Instance Variables

        private IMessageDisplay messageDisplay;
        private IDialogManager dialogManager;

        private ICommand newProjectCommand;
        private ICommand openProjectCommand;
        private ICommand closeProjectCommand;
        private ICommand saveProjectCommand;
        private ICommand saveProjectAsCommand;

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            this.messageDisplay = new MessageDisplay("Nunit Project Editor");
            this.dialogManager = new DialogManager("NUnit Project Editor");

            this.newProjectCommand = new MenuElement(newToolStripMenuItem);
            this.openProjectCommand = new MenuElement(openToolStripMenuItem);
            this.closeProjectCommand = new MenuElement(closeToolStripMenuItem);
            this.saveProjectCommand = new MenuElement(saveToolStripMenuItem);
            this.saveProjectAsCommand = new MenuElement(saveAsToolStripMenuItem);
        }

        #endregion

        #region IMainView Members

        #region Events

        public event ActiveViewChangingHandler ActiveViewChanging;
        public event ActiveViewChangedHandler ActiveViewChanged;

        #endregion

        #region Properties

        public IDialogManager DialogManager
        {
            get { return dialogManager; }
        }

        public ICommand NewProjectCommand
        {
            get { return newProjectCommand; }
        }

        public ICommand OpenProjectCommand
        {
            get { return openProjectCommand; }
        }

        public ICommand CloseProjectCommand
        {
            get { return closeProjectCommand; }
        }

        public ICommand SaveProjectCommand
        {
            get { return saveProjectCommand; }
        }

        public ICommand SaveProjectAsCommand
        {
            get { return saveProjectAsCommand; }
        }

        public IXmlView XmlView
        {
            get { return xmlView; }
        }

        public IPropertyView PropertyView
        {
            get { return propertyView; }
        }

        public SelectedView SelectedView
        {
            get { return (SelectedView)tabControl1.SelectedIndex; }
            set { tabControl1.SelectedIndex = (int)value; }
        }

        public IMessageDisplay MessageDisplay
        {
            get { return messageDisplay; }
        }

        #endregion

        #endregion

        #region Event Handlers

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog(this);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (ActiveViewChanging != null && !ActiveViewChanging())
                e.Cancel = true;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (ActiveViewChanged != null)
                ActiveViewChanged();
        }

        #endregion
    }
}
