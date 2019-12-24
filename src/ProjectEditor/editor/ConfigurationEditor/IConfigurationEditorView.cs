// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    public interface IConfigurationEditorDialog : IDialog
    {
        ICommand AddCommand { get; }
        ICommand RenameCommand { get; }
        ICommand RemoveCommand { get; }
        ICommand ActiveCommand { get; }

        ISelectionList ConfigList { get; }

        IAddConfigurationDialog AddConfigurationDialog { get; }
        IRenameConfigurationDialog RenameConfigurationDialog { get; }
    }
}
