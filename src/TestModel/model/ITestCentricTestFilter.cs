// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// Provides filter functionality: by outcome, by duration, by category...
    /// </summary>
    public interface ITestCentricTestFilter
    {
        /// <summary>
        /// Filters the loaded TestNodes by outcome
        /// </summary>
        IEnumerable<string> OutcomeFilter { get; set; }
    }
}
