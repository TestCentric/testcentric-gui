// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// Summary description for ResultSummary.
    /// </summary>
    public class ResultSummary
    {
        /// <summary>
        /// Returns the number of test cases actually run.
        /// </summary>
        public int RunCount => PassCount + FailureCount + ErrorCount + InconclusiveCount;

        /// <summary>
        /// Returns the number of test cases not run for any reason.
        /// </summary>
        public int NotRunCount => IgnoreCount + ExplicitCount + InvalidCount + SkipCount;

        /// <summary>
        /// Returns the number of failed test cases (including errors and invalid tests)
        /// </summary>
        public int FailedCount => FailureCount + InvalidCount + ErrorCount;

        /// <summary>
        /// Returns the sum of skipped test cases, including ignored and explicit tests
        /// </summary>
        public int TotalSkipCount => SkipCount + IgnoreCount + ExplicitCount;

        /// <summary>
        /// overall result of the test run as returned by the engine
        /// </summary>
        public string OverallResult { get; set; }

        /// <summary>
        /// Total time in fractional seconds how long the test run lasted as returned by the engine
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Time the test run was started
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Time the test run ended
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets the number of test cases for which results
        /// have been summarized. Any tests excluded by use of
        /// Category or Explicit attributes are not counted.
        /// </summary>
        public int TestCount { get; set; }

        /// <summary>
        /// Total number of test cases for which results were marked as warning.
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// Total number of test cases for which results were marked as passed.
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// Gets the count of failed tests, excluding errors and invalid tests
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Returns the number of test cases that had an error.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Gets the count of inconclusive tests
        /// </summary>
        public int InconclusiveCount { get; set; }

        /// <summary>
        /// Returns the number of test cases that were not runnable
        /// due to errors in the signature of the class or method.
        /// Such tests are also counted as Errors.
        /// </summary>
        public int InvalidCount { get; set; }

        /// <summary>
        /// Gets the count of skipped tests, excluding ignored and explicit tests
        /// </summary>
        public int SkipCount { get; set; }

        /// <summary>
        /// Gets the count of ignored tests
        /// </summary>
        public int IgnoreCount { get; set; }

        /// <summary>
        /// Gets the count of tests not run because the are Explicit
        /// </summary>
        public int ExplicitCount { get; set; }

        /// <summary>
        /// Gets the count of invalid assemblies
        /// </summary>
        public int InvalidAssemblies { get; set; }

        /// <summary>
        /// An Unexpected error occurred
        /// </summary>
        public bool UnexpectedError { get; set; }

        /// <summary>
        /// Invalid test fixture(s) were found
        /// </summary>
        public int InvalidTestFixtures { get; set; }
    }
}
