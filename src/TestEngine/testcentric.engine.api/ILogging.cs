// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.Engine
{
    /// <summary>
    /// Interface to abstract getting loggers
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name of the logger to get.</param>
        /// <returns></returns>
        ILogger GetLogger(string name);
    }
}
