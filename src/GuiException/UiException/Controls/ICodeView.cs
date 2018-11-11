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

using NUnit.UiException.CodeFormatters;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// The interface through which SourceCodeDisplay interacts with the code display.
    /// 
    /// Direct implementation is:
    ///     - CodeBox
    /// </summary>
    public interface ICodeView
    {
        /// <summary>
        /// Gets or sets a text to display in the code display.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the language formatting of the code display.
        /// </summary>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the viewport location from a 0 based line index
        /// </summary>
        int CurrentLine { get; set; }

        /// <summary>
        /// Gives access to the underlying IFormatterCatalog.
        /// </summary>
        IFormatterCatalog Formatter { get; }
    }
}
