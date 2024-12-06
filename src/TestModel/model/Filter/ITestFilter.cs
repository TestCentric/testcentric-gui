// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui.Model.Filter
{
    /// <summary>
    /// Interface for all test filters
    /// </summary>
    internal interface ITestFilter
    {
        /// <summary>
        /// Unqiue identifier of the filter
        /// </summary>
        string FilterId { get; }

        /// <summary>
        /// The filter condition
        /// </summary>
        IEnumerable<string> Condition { get; set; }

        /// <summary>
        /// Reset the filter condition to its default state
        /// </summary>
        void Reset();

        /// <summary>
        /// Init filter after a project is loaded
        /// </summary>
        void Init();

        /// <summary>
        /// Checks if the testNode matches the filter condition
        /// </summary>
        bool IsMatching(TestNode testNode);
    }
}
