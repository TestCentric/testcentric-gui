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

using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// ConfigurationEditor form is designed for adding, deleting
    /// and renaming configurations from a doc.
    /// </summary>
    public partial class ConfigurationEditorDialog : System.Windows.Forms.Form, IConfigurationEditorDialog
    {
        #region Instance Variables

        private ICommand addCommand;
        private ICommand removeCommand;
        private ICommand renameCommand;
        private ICommand activeCommand;

        private ISelectionList configList;

        private IMessageDisplay messageDisplay;

        #endregion

        #region Constructor

        public ConfigurationEditorDialog()
        {
            InitializeComponent();

            addCommand = new ButtonElement(addButton);
            removeCommand = new ButtonElement(removeButton);
            renameCommand = new ButtonElement(renameButton);
            activeCommand = new ButtonElement(activeButton);

            configList = new ListBoxElement(configListBox);

            messageDisplay = new MessageDisplay("NUnit Configuration Editor");
        }

        #endregion

        #region IConfigurationEditorDialog Members

        #region Properties

        public ICommand AddCommand
        {
            get { return addCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return removeCommand; }
        }

        public ICommand RenameCommand
        {
            get { return renameCommand; }
        }

        public ICommand ActiveCommand
        {
            get { return activeCommand; }
        }

        public ISelectionList ConfigList
        {
            get { return configList; }
        }

        public IAddConfigurationDialog AddConfigurationDialog
        {
            get { return new AddConfigurationDialog(); }
        }

        public IMessageDisplay MessageDisplay
        {
            get { return messageDisplay; }
        }

        public IRenameConfigurationDialog RenameConfigurationDialog
        {
            get { return new RenameConfigurationDialog(); }
        }

        #endregion

        #endregion
    }
}
