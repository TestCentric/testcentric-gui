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
using System.Drawing;
using System.Windows.Forms;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// This interface describes a feature that can be added to the ErrorWindow
    /// in order to show relevant information about failures/errors after a
    /// test suite run.
    ///     Clients who wants to add their own display should implement this
    /// interface and register their instance to ErrorBrowser at run-time.
    /// 
    /// Direct known implementations are:
    ///     StackTraceDisplay
    ///     SourceCodeDisplay
    /// </summary>
    public interface IErrorDisplay
    {
        /// <summary>
        /// Gives access to the ToolStripButton that enables this display.
        /// </summary>
        ToolStripButton PluginItem { get; }

        /// <summary>
        /// Gives access to a possibly null collection of option controls that will
        /// be shown when this display has the focus.
        /// </summary>
        ToolStripItem[] OptionItems { get; }

        /// <summary>
        /// Gives access to the content control of this display.
        /// </summary>
        Control Content { get; }

        /// <summary>
        /// Called whenever the user changes the error selection in the detail list.
        /// This method is called to allow the display to update its content according
        /// the given stack trace.
        /// </summary>
        /// <param name="stackTrace"></param>
        void OnStackTraceChanged(string stackTrace);
    }
}
