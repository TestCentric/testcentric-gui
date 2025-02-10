// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui.Model.Filter
{
    /// <summary>
    /// Provides filter functionality: by outcome, by duration, by category...
    /// </summary>
    public interface ITestCentricTestFilter
    {
        /// <summary>
        /// Filters the loaded TestNodes by outcome (for example: 'Passed', 'Failed' or 'Not run')
        /// </summary>
        IEnumerable<string> OutcomeFilter { get; set; }

        /// <summary>
        /// Filters the loaded TestNodes by matching a text (for example: Namespace, Class name or test method name - filter is case insensitive)
        /// </summary>
        string TextFilter { get; set; }

        /// <summary>
        /// Filters the loaded TestNodes by test categories. Use item 'No category' to filter for tests without any test category.
        /// </summary>
        IEnumerable<string> CategoryFilter { get; set; }

        /// <summary>
        /// Returns the list of available test categories defined in the loaded TestNodes + item 'No category'
        /// </summary>
        IEnumerable<string> AllCategories { get; }

        /// <summary>
        /// Checks if any filter is active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Clear all actives filters and reset them to default
        /// </summary>
        void ResetAll(bool suppressFilterChangedEvent = false);

        /// <summary>
        /// Init filter after a project is loaded
        /// </summary>
        void Init();
    }
}
