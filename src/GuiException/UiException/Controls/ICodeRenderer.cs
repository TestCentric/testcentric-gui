// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Drawing;
using NUnit.UiException.CodeFormatters;

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
