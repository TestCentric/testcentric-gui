// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.ViewElements
{
    public interface IMessageDisplay
    {
        void Error(string message);

        bool AskYesNoQuestion(string question);
    }
}
