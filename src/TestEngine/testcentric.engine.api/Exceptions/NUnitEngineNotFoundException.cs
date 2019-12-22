// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace NUnit.Engine
{
    /// <summary>
    /// The exception that is thrown if a valid test engine is not found
    /// </summary>
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class NUnitEngineNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitEngineNotFoundException"/> class.
        /// </summary>
        public NUnitEngineNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitEngineNotFoundException"/> class.
        /// </summary>
        /// <param name="minVersion">The minimum version.</param>
        internal NUnitEngineNotFoundException(Version minVersion)
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
