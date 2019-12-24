// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    public interface IPropertyView : IView
    {
        #region Properties

        IDialogManager DialogManager { get; }
        IConfigurationEditorDialog ConfigurationEditorDialog { get; }

        #region Command Elements

        ICommand BrowseProjectBaseCommand { get; }
        ICommand EditConfigsCommand { get; }
        ICommand BrowseConfigBaseCommand { get; }

        ICommand AddAssemblyCommand { get; }
        ICommand RemoveAssemblyCommand { get; }
        ICommand BrowseAssemblyPathCommand { get; }

        #endregion

        #region Properties of the Model as a Whole

        ITextElement ProjectPath { get; }
        ITextElement ProjectBase { get; }
        ISelectionList ProcessModel { get; }
        ISelectionList DomainUsage { get; }
        ITextElement ActiveConfigName { get; }

        ISelectionList ConfigList { get; }

        #endregion

        #region Properties of the Selected Config

        ISelectionList Runtime { get; }
        IComboBox RuntimeVersion { get; }
        ITextElement ApplicationBase { get; }
        ITextElement ConfigurationFile { get; }

        ISelection BinPathType { get; }
        ITextElement PrivateBinPath { get; }

        ISelectionList AssemblyList { get; }
        ITextElement AssemblyPath { get; }

        #endregion

        #endregion
    }
}
