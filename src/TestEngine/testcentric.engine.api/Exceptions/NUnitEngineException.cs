// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
#if !NETSTANDARD1_6
using System.Runtime.Serialization;
#endif

namespace TestCentric.Engine
{
    /// <summary>
    /// NUnitEngineException is thrown when the engine has been
    /// called with improper values or when a particular facility
    /// is not available.
    /// </summary>
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class NUnitEngineException : Exception
    {
        /// <summary>
        /// Construct with a message
        /// </summary>
        public NUnitEngineException(string message) : base(message)
        {
        }

        /// <summary>
        /// Construct with a message and inner exception
        /// </summary>
        public NUnitEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }

#if !NETSTANDARD1_6
        /// <summary>
        /// Serialization constructor
        /// </summary>
        public NUnitEngineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
