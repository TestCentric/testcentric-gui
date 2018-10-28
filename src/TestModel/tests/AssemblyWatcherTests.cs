// ***********************************************************************
// Copyright (c) 2011-2018 Charlie Poole
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
using System.IO;
using NUnit.Framework;


namespace TestCentric.Gui.Model
{
    [TestFixture]
    public class AssemblyWatcherTests
    {
        private AssemblyWatcher watcher;
        private CounterEventHandler handler;
        private static int watcherDelayMs = 100;
        private string fileName;
        private string tempFileName;

        [SetUp]
        public void CreateFile()
        {
            string tempDir = Path.GetTempPath();
            fileName = Path.Combine(tempDir, "temp.txt");
            tempFileName = Path.Combine(tempDir, "newTempFile.txt");

            StreamWriter writer = new StreamWriter(fileName);
            writer.Write("Hello");
            writer.Close();

            handler = new CounterEventHandler();
            watcher = new AssemblyWatcher();
            watcher.Setup(watcherDelayMs, fileName);
            watcher.AssemblyChanged += new AssemblyChangedHandler(handler.OnChanged);
            watcher.Start();
        }

        [TearDown]
        public void DeleteFile()
        {
            watcher.Stop();
            FileInfo fileInfo = new FileInfo(fileName);
            fileInfo.Delete();

            FileInfo temp = new FileInfo(tempFileName);
            if (temp.Exists) temp.Delete();
        }

        [Test]
        // TODO: Exclusion should really only apply to Mono on Windows
        [Platform(Exclude = "Mono")]
        public void MultipleCloselySpacedChangesTriggerWatcherOnlyOnce()
        {
            for (int i = 0; i < 3; i++)
            {
                StreamWriter writer = new StreamWriter(fileName, true);
                writer.WriteLine("Data");
                writer.Close();
                System.Threading.Thread.Sleep(20);
            }

            WaitForTimerExpiration();
            Assert.AreEqual(1, handler.Counter);
            Assert.AreEqual(Path.GetFullPath(fileName), handler.FileName);
        }

        [Test]
        // TODO: Exclusion should really only apply to Mono on Windows
        [Platform(Exclude = "Mono")]
        public void ChangingFileTriggersWatcher()
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write("Goodbye");
            writer.Close();

            WaitForTimerExpiration();
            Assert.AreEqual(1, handler.Counter);
            Assert.AreEqual(Path.GetFullPath(fileName), handler.FileName);
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Attribute change triggers watcher")]
        public void ChangingAttributesDoesNotTriggerWatcher()
        {
            FileInfo fi = new FileInfo(fileName);
            FileAttributes attr = fi.Attributes;
            fi.Attributes = FileAttributes.Hidden | attr;

            WaitForTimerExpiration();
            Assert.AreEqual(0, handler.Counter);
        }

        [Test]
        public void CopyingFileDoesNotTriggerWatcher()
        {
            FileInfo fi = new FileInfo(fileName);
            fi.CopyTo(tempFileName);
            fi.Delete();

            WaitForTimerExpiration();
            Assert.AreEqual(0, handler.Counter);
        }

        private static void WaitForTimerExpiration()
        {
            System.Threading.Thread.Sleep(watcherDelayMs * 10);
        }

        private class CounterEventHandler
        {
            int counter;
            String fileName;

            public int Counter
            {
                get { return counter; }
            }

            public String FileName
            {
                get { return fileName; }
            }

            public void OnChanged(String fullPath)
            {
                fileName = fullPath;
                counter++;
            }
        }
    }
}