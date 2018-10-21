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

using NUnit.UiException.StackTraceAnalyzers;

namespace NUnit.UiException.StackTraceAnalysers
{
    /// <summary>
    /// This class is responsible for extracting a Windows like path value
    /// from a line of the given stack trace. This class bases its work
    /// on the following assumptions:
    /// - paths are supposed to be absolute,
    /// - paths are supposed to be made of two parts: [drive][path]
    /// Where [drive] refers to a sequence like: "C:\\"
    /// and [path] a non empty string of characters that extends to the
    /// trailing ':' (as given in stack trace).
    /// </summary>
    public class WindowsPathParser :
        IErrorParser
    {
        #region IErrorParser Membres

        /// <summary>
        /// Locates and fills RawError.Path property with the first
        /// Windows path values found from RawError.Input property.
        /// </summary>
        /// <param name="parser">The stack trace parser. This parameter
        /// must not be null.</param>
        /// <param name="args">The RawError from which retrieving and
        /// filling Input and Path properties. This parameter cannot not
        /// be null.</param>
        /// <returns>True if a match occured, false otherwise.</returns>
        public bool TryParse(StackTraceParser parser, RawError args)
        {
            int posDriveLetter;
            int posTrailingColon;
            string path;

            UiExceptionHelper.CheckNotNull(parser, "parser");
            UiExceptionHelper.CheckNotNull(args, "args");

            posDriveLetter = lookForDriveLetter(args.Input, 0);
            if (posDriveLetter == -1)
                return (false);

            posTrailingColon = PathCompositeParser.IndexOfTrailingColon(args.Input, posDriveLetter + 3);
            if (posTrailingColon == -1)
                return (false);

            path = args.Input.Substring(posDriveLetter, posTrailingColon - posDriveLetter);
            path = path.Trim();

            if (path.Length <= 3)
                return (false);

            args.Path = path;

            return (true);
        }

        #endregion

        private int lookForDriveLetter(string error, int startIndex)
        {
            int i;

            for (i = startIndex; i < error.Length - 2; ++i)
            {
                if (((error[i] >= 'a' && error[i] <= 'z') ||
                    (error[i] >= 'A' && error[i] <= 'Z')) &&
                    error[i + 1] == ':' &&
                    error[i + 2] == '\\')
                    return (i);
            }

            return (-1);
        }
    }
}
