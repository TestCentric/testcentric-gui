// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// The TestStatus enum indicates the suiteResult of running a test
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// The test was inconclusive
        /// </summary>
        Inconclusive,

        /// <summary>
        /// The test has skipped 
        /// </summary>
        Skipped,

        /// <summary>
        /// The test succeeded
        /// </summary>
        Passed,

        /// <summary>
        /// There was a warning
        /// </summary>
        Warning,

        /// <summary>
        /// The test failed
        /// </summary>
        Failed
    }
}
