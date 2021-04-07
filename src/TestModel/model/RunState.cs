// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// The RunState enum indicates whether a test can be executed. 
    /// </summary>
    public enum RunState
    {
        /// <summary>
        /// We don't know the RunState
        /// </summary>
        Unknown,

        /// <summary>
        /// The test is not runnable.
        /// </summary>
        NotRunnable,

        /// <summary>
        /// The test is runnable. 
        /// </summary>
        Runnable,

        /// <summary>
        /// The test can only be run explicitly
        /// </summary>
        Explicit,

        /// <summary>
        /// The test has been skipped. This val may
        /// appear on a Test when certain attributes
        /// are used to skip the test.
        /// </summary>
        Skipped,

        /// <summary>
        /// The test has been ignored. May appear on
        /// a Test, when the IgnoreAttribute is used.
        /// </summary>
        Ignored
    }
}
