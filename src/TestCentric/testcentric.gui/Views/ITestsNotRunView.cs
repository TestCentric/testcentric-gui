// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    public interface ITestsNotRunView
    {
        void Clear();

        void AddResult(string name, string reason);
    }
}
