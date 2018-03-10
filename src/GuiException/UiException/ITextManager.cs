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

namespace NUnit.UiException
{
    /// <summary>
    /// Provides an abstract way to manipulate a text as a whole and as separate
    /// sequences that can randomly be accessed one line at a time.
    /// </summary>
    public interface ITextManager
    {
        /// <summary>
        /// Gets the number of line in text managed by this object.
        /// </summary>
        int LineCount { get; }

        /// <summary>
        /// Gets the character count of the longest line in the text managed
        /// by this object.
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Gets the complete text managed by this object.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a string filled with all characters in the line
        /// at the specified startingPosition without the trailing '\r\n' characters.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        string GetTextAt(int lineIndex);
    }
}
