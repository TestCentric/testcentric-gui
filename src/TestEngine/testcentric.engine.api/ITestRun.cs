// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TestCentric.Engine
{
    /// <summary>
    /// The ITestRun class represents an ongoing test run.
    /// </summary>
    public interface ITestRun
    {
        /// <summary>
        /// Get the result of the test.
        /// </summary>
        /// <returns>An XmlNode representing the test run result</returns>
        XmlNode Result { get; }

        /// <summary>
        /// Blocks the current thread until the current test run completes
        /// or the timeout is reached
        /// </summary>
        /// <param name="timeout">A <see cref="T:System.Int32"/> that represents the number of milliseconds to wait or -1 milliseconds to wait indefinitely. </param>
        /// <returns>True if the run completed</returns>
        bool Wait(int timeout);
    }
}
