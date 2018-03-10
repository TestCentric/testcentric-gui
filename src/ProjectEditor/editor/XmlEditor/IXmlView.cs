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
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// IXmlView is the interface implemented by the XmlView
    /// and consumed by the XmlPresenter.
    /// </summary>
    public interface IXmlView : IView
    {
        /// <summary>
        /// Gets or sets the XML text
        /// </summary>
        ITextElement Xml { get; }

        /// <summary>
        /// Display an error message at bottom of the view,
        /// adjusting the text to make edit box to make room.
        /// </summary>
        /// <param name="message">The message to display</param>
        void DisplayError(string message);

        /// <summary>
        /// Display an error message at bottom of the view,
        /// adjusting the text to make edit box to make room
        /// and highlighting the text that caused the error.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="lineNumber">The line number in which the error occured.</param>
        /// <param name="linePosition">The position in the line that caused the error.</param>
        void DisplayError(string message, int lineNumber, int linePosition);

        /// <summary>
        /// Remove any error message from the view, adjusting
        /// the edit box so it uses all the space.
        /// </summary>
        void RemoveError();

        
    }
}
