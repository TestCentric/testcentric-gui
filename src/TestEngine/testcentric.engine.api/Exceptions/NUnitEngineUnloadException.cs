// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
#if !NETSTANDARD1_6
using System.Runtime.Serialization;
#endif

namespace TestCentric.Engine
{

    /// <summary>
    /// NUnitEngineUnloadException is thrown when a test run has completed successfully
    /// but one or more errors were encountered when attempting to unload
    /// and shut down the test run cleanly.
    /// </summary>
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class NUnitEngineUnloadException : NUnitEngineException  //Inherits from NUnitEngineException for backwards compatibility of calling runners
    {
        private const string AggregatedExceptionsMsg =
            "Multiple exceptions encountered. Retrieve AggregatedExceptions property for more information";


        /// <summary>
        /// Construct with a message
        /// </summary>
        public NUnitEngineUnloadException(string message) : base(message)
        {
        }

        /// <summary>
        /// Construct with a message and inner exception
        /// </summary>
        public NUnitEngineUnloadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct with a message and a collection of exceptions.
        /// </summary>
        public NUnitEngineUnloadException(ICollection<Exception> aggregatedExceptions) : base(AggregatedExceptionsMsg)
        {
            AggregatedExceptions = aggregatedExceptions;
        }

#if !NETSTANDARD1_6
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        public NUnitEngineUnloadException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif

        /// <summary>
        /// Gets the collection of exceptions .
        /// </summary>
        public ICollection<Exception> AggregatedExceptions { get; }
    }
}
