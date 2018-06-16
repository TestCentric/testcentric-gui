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
using System.Windows.Forms;

namespace TestCentric.Gui
{
    /// <summary>
    /// Utility class used to display a wait cursor
    /// while a long operation takes place and
    /// guarantee that it will be removed on exit.
    /// 
    /// Use as follows:
    /// 
    ///		using ( new WaitCursor() )
    ///		{
    ///			// Long running operation goes here
    ///		}
    ///		
    /// </summary>
    public class WaitCursor : IDisposable
    {
        private Cursor cursor;
        private Control control;

        public WaitCursor()
        {
            this.control = null;
            this.cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitCursor( Control control )
        {
            this.control = control;
            this.cursor = control.Cursor;
            control.Cursor = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            if ( control != null )
                control.Cursor = this.cursor;
            else
                Cursor.Current = this.cursor;
        }
    }
}
