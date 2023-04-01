// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric
{
    /// <summary>
    /// InternalTrace provides facilities for tracing the execution
    /// of the NUnit framework. Tests and classes under test may make use 
    /// of Console writes, System.Diagnostics.Trace or various loggers and
    /// NUnit itself traps and processes each of them. For that reason, a
    /// separate internal trace is needed.
    /// 
    /// Note:
    /// InternalTrace uses a global lock to allow multiple threads to write
    /// trace messages. This can easily make it a bottleneck so it must be 
    /// used sparingly. Keep the trace Level as low as possible and only
    /// insert InternalTrace writes where they are needed.
    /// TODO: add some buffering and a separate writer thread as an option.
    /// TODO: figure out a way to turn on trace in specific classes only.
    /// </summary>
    public static class InternalTrace
    {
        /// <summary>
        /// Gets a flag indicating whether the InternalTrace is initialized
        /// </summary>
        public static bool Initialized { get; private set; }

        /// <summary>
        /// TraceLevel as initially set
        /// </summary>
        public static InternalTraceLevel TraceLevel { get; private set; }

        /// <summary>
        /// The TraceWriter used for logging
        /// </summary>
        private static InternalTraceWriter TraceWriter { get; set; }

        /// <summary>
        /// Initialize the internal trace facility using the name of the log
        /// to be written to and the trace level.
        /// </summary>
        /// <param name="logName">The log name</param>
        /// <param name="level">The trace level</param>
        public static void Initialize(string logName, InternalTraceLevel level)
        {
            if (!Initialized)
            {
                TraceLevel = level;

                if (TraceWriter == null && TraceLevel > InternalTraceLevel.Off)
                {
                    TraceWriter = new InternalTraceWriter(logName);
                    TraceWriter.WriteLine("InternalTrace: Initializing at level {0}", TraceLevel);
                }

                Initialized = true;
            }
            else
                TraceWriter.WriteLine("InternalTrace: Ignoring attempted re-initialization at level {0}", level);
        }

        /// <summary>
        /// Get a named Logger
        /// </summary>
        /// <returns></returns>
        public static Logger GetLogger(string name)
        {
            return new Logger(name, TraceLevel, TraceWriter);
        }

        /// <summary>
        /// Get a logger named for a particular Type.
        /// </summary>
        public static Logger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }
    }
}
