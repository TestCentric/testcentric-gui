// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// ITestItem is the common interface shared by TestNodes
    /// and TestGroups and allows either to be selected in
    /// the tree.
    /// </summary>
    public interface ITestItem
    {
        /// <summary>
        /// The name of this item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get a TestFilter for use in selecting this item
        /// to be run by the engine.
        /// </summary>
        /// <returns></returns>
        TestFilter GetTestFilter();
    }
}
