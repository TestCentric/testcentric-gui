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
using NUnit.UiException.CodeFormatters;

namespace NUnit.UiException.CodeFormatters
{
    /// <summary>
    /// ICodeFormatter is the interface to make the syntax
    /// coloring of a string for a specific developpment language.
    /// </summary>
    public interface ICodeFormatter
    {
        /// <summary>
        /// The language name handled by this formatter.
        /// Ex: "C#", "Java", "C++" and so on...
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Makes the coloring syntax of the given text.
        /// </summary>
        /// <param name="code">The text to be formatted. This
        /// parameter cannot be null.</param>
        /// <returns>A FormattedCode instance.</returns>
        FormattedCode Format(string code);
    }
}
