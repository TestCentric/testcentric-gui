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


namespace NUnit.ProjectEditor
{
    public class AddConfigurationPresenter
    {
        private IProjectModel model;
        private IAddConfigurationDialog dlg;

        public AddConfigurationPresenter(IProjectModel model, IAddConfigurationDialog dlg)
        {
            this.model = model;
            this.dlg = dlg;

            dlg.ConfigList = model.ConfigNames;

            dlg.OkButton.Execute += delegate
            {
                if (dlg.ConfigToCreate == string.Empty)
                {
                    dlg.MessageDisplay.Error("No configuration name provided");
                    return;
                }

                foreach (string config in model.ConfigNames)
                {
                    if (config == dlg.ConfigToCreate)
                    {
                        dlg.MessageDisplay.Error("A configuration with that name already exists");
                        return;
                    }
                }

                IProjectConfig newConfig = model.AddConfig(dlg.ConfigToCreate);

                if (dlg.ConfigToCopy != null)
                {
                    IProjectConfig copyConfig = model.Configs[dlg.ConfigToCopy];
                    if (copyConfig != null)
                    {
                        newConfig.BasePath = copyConfig.BasePath;
                        newConfig.BinPathType = copyConfig.BinPathType;
                        if (newConfig.BinPathType == BinPathType.Manual)
                            newConfig.PrivateBinPath = copyConfig.PrivateBinPath;
                        newConfig.ConfigurationFile = copyConfig.ConfigurationFile;
                        newConfig.RuntimeFramework = copyConfig.RuntimeFramework;

                        foreach (string assembly in copyConfig.Assemblies)
                            newConfig.Assemblies.Add(assembly);
                    }
                }

                dlg.Close();
            };
        }
    }
}
