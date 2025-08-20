// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Runtime.Serialization;

namespace TestCentric.Engine
{
    /// <summary>
    /// NUnitEngineException is thrown when the engine has been
    /// called with improper values or when a particular facility
    /// is not available.
    /// </summary>
    [Serializable]
    public class EngineException : Exception
    {
        /// <summary>
        /// Construct with a message
        /// </summary>
        public EngineException(string message) : base(message)
        {
        }

        /// <summary>
        /// Construct with a message and inner exception
        /// </summary>
        public EngineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        public EngineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
