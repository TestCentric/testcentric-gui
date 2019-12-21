// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.Engine
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
