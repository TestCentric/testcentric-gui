// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Text;

namespace TestCentric.Common
{
    /// <summary>
    /// Static methods for creating messages
    /// </summary>
    public static class MessageBuilder
    {
        /// <summary>
        /// Build a message including all info about an exception
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>A string containing the message</returns>
        public static string FromException(Exception exception)
        {
            Exception ex = exception;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} : {1}", ex.GetType().ToString(), ex.Message);

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                sb.AppendFormat("\r----> {0} : {1}", ex.GetType().ToString(), ex.Message);
            }

            return sb.ToString();
        }
    }
}
