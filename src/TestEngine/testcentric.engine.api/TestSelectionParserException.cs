// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
#if !NETSTANDARD1_6
using System.Runtime.Serialization;
#endif

namespace NUnit.Engine
{
    /// <summary>
    /// TestSelectionParserException is thrown when an error
    /// is found while parsing the selection expression.
    /// </summary>
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class TestSelectionParserException : Exception
    {
        /// <summary>
        /// Construct with a message
        /// </summary>
        public TestSelectionParserException(string message) : base(message) { }

        /// <summary>
        /// Construct with a message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TestSelectionParserException(string message, Exception innerException) : base(message, innerException) { }

#if !NETSTANDARD1_6
        /// <summary>
        /// Serialization constructor
        /// </summary>
        public TestSelectionParserException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
