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
using NUnit.Engine;

namespace TestCentric.Gui
{
    public class Logger
    {
        public Logger(string name)
        {
            FullName = Name = name;
            int index = FullName.LastIndexOf('.');
            if (index >= 0)
                Name = FullName.Substring(index + 1);
        }

        #region Properties

        public string Name { get; }

        public string FullName { get; }

        #endregion

        #region Error

        public void Error(string message)
        {
            Log(InternalTraceLevel.Error, message);
        }

        public void Error(string message, params object[] args)
        {
            Log(InternalTraceLevel.Error, message, args);
        }

        public void Error(string message, Exception ex)
        {
            Log(message, ex);
        }

        #endregion

        #region Warning
        public void Warning(string message)
        {
            Log(InternalTraceLevel.Warning, message);
        }

        public void Warning(string message, params object[] args)
        {
            Log(InternalTraceLevel.Warning, message, args);
        }
        #endregion

        #region Info
        public void Info(string message)
        {
            Log(InternalTraceLevel.Info, message);
        }

        public void Info(string message, params object[] args)
        {
            Log(InternalTraceLevel.Info, message, args);
        }
        #endregion

        #region Debug
        public void Debug(string message)
        {
            Log(InternalTraceLevel.Verbose, message);
        }

        public void Debug(string message, params object[] args)
        {
            Log(InternalTraceLevel.Verbose, message, args);
        }
        #endregion

        #region Helper Methods
        public void Log(InternalTraceLevel level, string message)
        {
            if (InternalTrace.Level >= level)
                InternalTrace.Log(level, message, Name);
        }

        private void Log(InternalTraceLevel level, string format, params object[] args)
        {
            if (InternalTrace.Level >= level)
                Log(level, string.Format(format, args));
        }

        public void Log(string message, Exception ex)
        {
            if (InternalTrace.Level >= InternalTraceLevel.Error)
            {
                InternalTrace.Log(InternalTraceLevel.Error, message, Name, ex);
            }
        }

        #endregion
    }
}
