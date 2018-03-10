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
using System.Windows.Forms;
using System.Xml;

namespace NUnit.ProjectEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            //MessageBox.Show("Attach to editor if desired", "Debug ProjectEditor?");
#endif

            // Set up main editor triad
            ProjectDocument doc = new ProjectDocument();
            MainForm view = new MainForm();
            MainPresenter presenter = new MainPresenter(doc, view);

            // TODO: Process arguments
            //    -new          = create new project
            //    -config=name  = create a new config (implies -new)
            //    assemblyName  = add assembly to the last config specified (or Default)

            if (args.Length == 1 && ProjectDocument.IsProjectFile(args[0]))
                doc.OpenProject(args[0]);
            else if (args.Length > 0)
            {
                doc.CreateNewProject();
                XmlNode configNode = XmlHelper.AddElement(doc.RootNode, "Config");
                XmlHelper.AddAttribute(configNode, "name", "Default");

                foreach (string fileName in args)
                {
                    if (PathUtils.IsAssemblyFileType(fileName))
                    {
                        XmlNode assemblyNode = XmlHelper.AddElement(configNode, "assembly");
                        XmlHelper.AddAttribute(assemblyNode, "path", fileName);
                    }
                }

                // Simulate view change so view gets updated
                presenter.ActiveViewChanged();
            }

            Application.Run(view);
        }
    }
}
