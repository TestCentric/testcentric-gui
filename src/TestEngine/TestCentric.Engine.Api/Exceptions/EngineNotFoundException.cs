// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine
{
    /// <summary>
    /// The exception that is thrown if a valid test engine is not found
    /// </summary>
    [Serializable]
    public class EngineNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineNotFoundException"/> class.
        /// </summary>
        public EngineNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineNotFoundException"/> class.
        /// </summary>
        /// <param name="minVersion">The minimum version.</param>
        internal EngineNotFoundException(Version minVersion)
            :base(CreateMessage(minVersion))
        {
        }

        private static string CreateMessage(Version minVersion = null)
        {
            return string.Format("{0} with a version greater than or equal to {1} not found",
                TestEngineActivator.DefaultTypeName, minVersion ?? TestEngineActivator.DefaultMinimumVersion);
        }
    }
}