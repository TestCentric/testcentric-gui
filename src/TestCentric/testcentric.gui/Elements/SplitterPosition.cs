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

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
	/// <summary>
    /// A SplitterPosition element represents the position of
	/// a particular Splitter that is part of the view.
    /// </summary>
	public class SplitterPosition : IViewParameter<int>
    {
		private Splitter _splitter;

		public event CommandHandler Changed;

        public SplitterPosition(Splitter splitter)
        {
			_splitter = splitter;

			_splitter.SplitterMoved += (s, e) =>
			{
				Changed?.Invoke();
			};
        }

        public int Value
		{
			get { return _splitter.SplitPosition; }
			set { _splitter.SplitPosition = value; }
		}
    }
}
