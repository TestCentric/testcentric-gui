// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.UiException.StackTraceAnalyzers;

namespace NUnit.UiException.StackTraceAnalysers
{
    /// <summary>
    /// This class is responsible for extracting a Unix like path value
    /// from a line of the given stack trace. This class bases its work
    /// on the following assumptions:
    /// - paths are supposed to be absolute,
    /// - paths are supposed to be made of two parts: [/][path]
    /// Where [/] refers to the Unix mount point 
    /// and [path] a non empty string of characters that extends to the
    /// trailing ':' (as given in stack trace).
    /// </summary>
    class UnixPathParser :
        IErrorParser
    {
        #region IErrorParser Membres

        /// <summary>
        /// Locates and fills RawError.Path property with the first
        /// Unix path values found from RawError.Input property.
        /// </summary>
        /// <param name="parser">The stack trace parser. This parameter
        /// must not be null.</param>
        /// <param name="args">The RawError from which retrieving and
        /// filling Input and Path properties. This parameter cannot not
        /// be null.</param>
        /// <returns>True if a match occured, false otherwise.</returns>
        public bool TryParse(StackTraceParser parser, RawError args)
        {
            int posSlash;
            int posColon;
            string path;

            UiExceptionHelper.CheckNotNull(parser, "parser");
            UiExceptionHelper.CheckNotNull(args, "args");

            if ((posSlash = indexOfFirstSlash(args.Input, 0)) == -1)
                return (false);

            if ((posColon = PathCompositeParser.IndexOfTrailingColon(args.Input, posSlash + 1)) == -1)
                return (false);

            path = args.Input.Substring(posSlash, posColon - posSlash);
            path = path.Trim();

            if (path.Length <= 1)
                return (false);

            args.Path = path;

            return (true);
        }

        #endregion

        private int indexOfFirstSlash(string error, int startIndex)
        {
            for (; startIndex < error.Length; startIndex++)
                if (error[startIndex] == '/')
                {
                    if (startIndex == 0 ||
                        startIndex > 0 && error[startIndex - 1] == ' ')
                        return (startIndex);
                }

            return (-1);
        }
    }
}
