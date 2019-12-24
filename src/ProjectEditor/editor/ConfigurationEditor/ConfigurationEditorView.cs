// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
