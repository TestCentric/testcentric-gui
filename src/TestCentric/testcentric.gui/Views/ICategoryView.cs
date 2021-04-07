// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    using Elements;

    public interface ICategoryView
    {
        IListBox AvailableList { get; }
        IListBox SelectedList { get; }
        IChecked ExcludeCategories { get; }
        ICommand AddButton { get; }
        ICommand RemoveButton { get; }

        void Clear();
    }
}
