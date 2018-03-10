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
using System.Drawing;

namespace NUnit.UiException.Controls
{   
    /// <summary>
    /// The interface through which CodeBox interacts with a display to display itself.
    /// 
    /// Direct implementation is:
    ///     - DefaultCodeRenderer
    /// </summary>
    public interface ICodeRenderer
    {
        /// <summary>
        /// Draw the given code to be displayed in the actual viewport.
        /// </summary>
        /// <param name="code">The code to draw</param>
        /// <param name="args">Encapsulate graphic information about how to display the code</param>
        /// <param name="viewport">The portion of interest</param>
        void DrawToGraphics(FormattedCode code, CodeRenderingContext args, Rectangle viewport);

        /// <summary>
        /// Measures the code size in pixels.
        /// </summary>
        /// <param name="code">The code to measure</param>
        /// <param name="g">The target graphics object</param>
        /// <param name="font">The font with which displaying the code</param>
        /// <returns>The size in pixels</returns>
        SizeF GetDocumentSize(FormattedCode code, Graphics g, Font font);

        /// <summary>
        /// Converts a line index to its matching Y client coordinate.
        /// </summary>
        /// <param name="lineIndex">The line index to convert</param>
        /// <param name="g">The target graphics object</param>
        /// <param name="font">The font with which displaying the code</param>
        /// <returns>The Y client coordinate</returns>
        float LineIndexToYCoordinate(int lineIndex, Graphics g, Font font);
    }
}
