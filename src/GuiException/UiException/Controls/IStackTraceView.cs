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

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// This enum defines indicators telling how instances of IStackTraceView
    /// should deal with item order in their list.
    /// </summary>
    public enum ErrorListOrderPolicy
    {
        /// <summary>
        /// Tells IStackTraceView to order items in the list in the same
        /// order they appear in the stack trace.
        /// </summary>
        InitialOrder,

        /// <summary>
        /// Tells IStackTraceView to order items in the list in the reverse
        /// order they appear in the stack trace. At Test Driven Development time
        /// this value is useful to point out the location where a test is expected
        /// to fail.
        /// </summary>
        ReverseOrder,
    }

    /// <summary>
    /// The interface through which SourceCodeDisplay interacts with the error list.
    /// 
    /// Direct implementations are:
    ///     - ErrorList
    /// </summary>
    public interface IStackTraceView
    {
        event EventHandler SelectedItemChanged;

        string StackTrace { get; set; }
        ErrorItem SelectedItem { get; }
        bool AutoSelectFirstItem { get; set; }
        ErrorListOrderPolicy ListOrderPolicy { get; set; }
    }
}
