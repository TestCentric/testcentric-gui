// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This interface is specialized for the selection of multiple test categories
    /// </summary>
    public interface ICategoryFilterSelection : IMultiSelection
    {
        /// <summary>
        /// Init the filter selection 
        /// </summary>
        void Init(ITestModel testModel);

        /// <summary>
        /// Close the filter selection dialog
        /// </summary>
        void Close();
    }
}
