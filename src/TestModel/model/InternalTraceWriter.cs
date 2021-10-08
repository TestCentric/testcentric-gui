// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Diagnostics;
using System.IO;

namespace TestCentric.Gui
{
    /// <summary>
    /// A trace listener that writes to a separate file per domain
    /// and process using it.
    /// </summary>
    public class InternalTraceWriter : TextWriter
    {
        StreamWriter writer;

        public InternalTraceWriter(string logName)
        {
            int pId = Process.GetCurrentProcess().Id;
            string domainName = AppDomain.CurrentDomain.FriendlyName;

            string fileName = logName
                .Replace("%p", pId.ToString())
                .Replace("%a", domainName);

            string logDirectory = Environment.CurrentDirectory;
            //if (!Directory.Exists(logDirectory))
            //    Directory.CreateDirectory(logDirectory);

            string logPath = Path.Combine(logDirectory, fileName);
            this.writer = new StreamWriter(logPath, true);
            this.writer.AutoFlush = true;
        }

        public override System.Text.Encoding Encoding
        {
            get { return writer.Encoding; }
        }

        public override void Write(char value)
        {
            writer.Write(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            writer.WriteLine(value);
        }

        public override void Close()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer = null;
            }
        }

        public override void Flush()
        {
            if (writer != null)
                writer.Flush();
        }
    }
}
