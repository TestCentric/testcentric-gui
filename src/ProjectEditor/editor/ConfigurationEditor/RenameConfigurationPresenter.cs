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

namespace NUnit.ProjectEditor
{
    public class RenameConfigurationPresenter
    {
        private IProjectModel model;
        private IRenameConfigurationDialog dlg;
        private string originalName;

        public RenameConfigurationPresenter(IProjectModel model, IRenameConfigurationDialog dlg, string originalName)
        {
            this.model = model;
            this.dlg = dlg;
            this.originalName = originalName;

            dlg.ConfigurationName.Text = originalName;
            dlg.ConfigurationName.Select(0, originalName.Length);

            dlg.ConfigurationName.Changed += delegate
            {
                string text = dlg.ConfigurationName.Text;
                dlg.OkButton.Enabled = text != string.Empty && text != originalName;
            };

            dlg.OkButton.Execute += delegate
            {
                string newName = dlg.ConfigurationName.Text;

                foreach (string existingName in model.ConfigNames)
                {
                    if (existingName == newName)
                    {
                        dlg.MessageDisplay.Error("A configuration with that name already exists");
                        return;
                    }
                }

                model.Configs[originalName].Name = newName;
            };
        }
    }
}
