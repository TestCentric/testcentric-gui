// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.TestUtilities
{
    using System;
    using System.IO;

    public class TempResourceFile : IDisposable
    {
        string path;

        public TempResourceFile(Type type, string name) : this(type, name, null) { }

        public TempResourceFile(Type type, string name, string filePath)
        {
            if (filePath == null)
                filePath = name;

            if (!System.IO.Path.IsPathRooted(filePath))
                filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), filePath);

            this.path = filePath;

            Stream stream = type.Assembly.GetManifestResourceStream(type, name);
            byte[] buffer = new byte[(int)stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            string dir = System.IO.Path.GetDirectoryName(this.path);
            if (dir != null && dir.Length != 0)
            {
                Directory.CreateDirectory(dir);
            }

            using (FileStream fileStream = new FileStream(this.path, FileMode.Create))
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Dispose()
        {
            File.Delete(this.path);

            string path = this.path;
            while (true)
            {
                path = System.IO.Path.GetDirectoryName(path);
                if (path == null || path.Length == 0 || Directory.GetFiles(path).Length > 0)
                {
                    break;
                }

                Directory.Delete(path);
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
        }
    }
}
