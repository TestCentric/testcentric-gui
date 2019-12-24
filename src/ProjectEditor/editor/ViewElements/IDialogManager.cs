// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.ViewElements
{
    public interface IDialogManager
    {
        string GetFileOpenPath(string title, string filter, string initialDirectory);

        string GetSaveAsPath(string title, string filter);

        string GetFolderPath(string message, string initialPath);
    }
}
