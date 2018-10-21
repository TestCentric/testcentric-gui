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

using System.Drawing;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// The interface through which ErrorList interacts with a painter to paint itself.
    /// 
    /// Direct implementation is:
    ///     - DefaultErrorListRenderer
    /// </summary>
    public interface IErrorListRenderer
    {
        /// <summary>
        /// Draws the list on the given graphics.
        /// </summary>
        /// <param name="items">The item collection to paint on the graphics object</param>
        /// <param name="selected">The item to paint with selection feature</param>
        /// <param name="g">The target graphics object</param>
        /// <param name="viewport">The viewport location</param>
        void DrawToGraphics(ErrorItemCollection items, ErrorItem selected, Graphics g, Rectangle viewport);

        /// <summary>
        /// Draw the given item on the given graphics object.
        /// </summary>
        /// <param name="item">The item to be painted</param>
        /// <param name="index">The item's index</param>
        /// <param name="hovered">If true, this item can display hover feature</param>
        /// <param name="selected">If true, this item can display selection feature</param>
        /// <param name="g">The target graphics object</param>
        /// <param name="viewport">The current viewport</param>
        void DrawItem(ErrorItem item, int index, bool hovered, bool selected, Graphics g, Rectangle viewport);

        /// <summary>
        /// Given a collection of items and a graphics object, this method
        /// measures in pixels the size of the collection.
        /// </summary>
        /// <param name="items">The collection</param>
        /// <param name="g">The target graphics object</param>
        /// <returns>The size in pixels of the collection</returns>
        Size GetDocumentSize(ErrorItemCollection items, Graphics g);

        /// <summary>
        /// Gets the Item right under point.
        /// </summary>
        /// <param name="items">A collection of items</param>
        /// <param name="g">The target graphics object</param>
        /// <param name="point">Some client coordinate values</param>
        /// <returns>One item in the collection or null the location doesn't match any item</returns>
        ErrorItem ItemAt(ErrorItemCollection items, Graphics g, Point point);

        /// <summary>
        /// Gets and sets the font for this renderer
        /// </summary>
        Font Font { get; set; }
    }
}
