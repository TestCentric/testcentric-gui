// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The TestFilterService provides builders that can create TestFilters
    /// </summary>
    public interface ITestFilterService
    {
        /// <summary>
        /// Get an uninitialized TestFilterBuilder
        /// </summary>
        ITestFilterBuilder GetTestFilterBuilder();
    }
}
