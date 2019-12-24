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
using TestCentric.Engine;

namespace TestCentric.Gui
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class InternalTrace
    {
        private readonly static string TIME_FMT = "HH:mm:ss.fff";

        private static bool initialized;
        private static InternalTraceLevel level;
        private static string logName;

        private static InternalTraceWriter writer;
        public static InternalTraceWriter Writer
        {
            get { return writer; }
        }

        private static string LogName
        {
            get { return logName; }
            set { logName = value; }
        }

        public static InternalTraceLevel Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;

                    if (writer == null && Level > InternalTraceLevel.Off)
                    {
                        writer = new InternalTraceWriter(logName);
                        writer.WriteLine("InternalTrace: Initializing at level " + Level.ToString());
                    }
                }
            }
        }

        //public static void Initialize(string logName)
        //{
        //    int lev = (int) new System.Diagnostics.TraceSwitch("NTrace", "NUnit internal trace").Level;
        //    Initialize(logName, (InternalTraceLevel)lev);
        //}

        public static void Initialize(string logName, InternalTraceLevel level)
        {
            if (!initialized)
            {
                LogName = logName;
                Level = level;

                initialized = true;
            }
        }

        public static void Flush()
        {
            if (writer != null)
                writer.Flush();
        }

        public static void Close()
        {
            if (writer != null)
                writer.Close();

            writer = null;
        }

        public static Logger GetLogger(string name)
        {
            return new Logger(name);
        }

        public static Logger GetLogger(Type type)
        {
            return new Logger(type.FullName);
        }

        public static void Log(InternalTraceLevel level, string message, string category)
        {
            Log(level, message, category, null);
        }

        public static void Log(InternalTraceLevel level, string message, string category, Exception ex)
        {
            Writer.WriteLine("{0} {1,-5} [{2,2}] {3}: {4}",
                DateTime.Now.ToString(TIME_FMT),
                level == InternalTraceLevel.Verbose ? "Debug" : level.ToString(),
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                category,
                message);

            if (ex != null)
                Writer.WriteLine(ex.ToString());
        }
    }
}
