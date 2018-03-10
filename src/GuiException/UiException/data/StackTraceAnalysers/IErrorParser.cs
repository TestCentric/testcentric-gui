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

namespace NUnit.UiException.StackTraceAnalyzers
{
    public class RawError
    {
        private string _function;
        private string _path;
        private int _line;
        private string _input;

        public RawError(string input)
        {
            UiExceptionHelper.CheckNotNull(input, "input");
            _input = input;

            return;
        }

        public string Input
        {
            get { return (_input); }
        }

        public string Function
        {
            get { return (_function); }
            set { _function = value; }
        }

        public string Path
        {
            get { return (_path); }
            set { _path = value; }
        }

        public int Line
        {
            get { return (_line); }
            set { _line = value; }
        }

        public ErrorItem ToErrorItem()
        {
            UiExceptionHelper.CheckTrue(
                _function != null,
                "Cannot create instance of ErrorItem without a valid value in Function",
                "Function");

            return (new ErrorItem(_path, _function, _line));
        }
    }

    public interface IErrorParser
    {
        bool TryParse(StackTraceParser parser, RawError args);
    }
}
