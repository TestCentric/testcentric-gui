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
using System.Security.Principal;
using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class TestLoaderSettings : SettingsGroup
    {
        public TestLoaderSettings(ISettings settings)
            : base(settings, "Options.TestLoader") { }

        public bool ClearResultsOnReload
        {
            get { return GetSetting("ClearResultsOnReload", false); }
            set { SaveSetting("ClearResultsOnReload", value); }
        }

        public bool ReloadOnChange
        {
            get { return GetSetting("ReloadOnChange", true); }
            set { SaveSetting("ReloadOnChange", value); }
        }

        public bool RerunOnChange
        {
            get { return GetSetting("RerunOnChange", false); }
            set { SaveSetting("RerunOnChange", value); }
        }

        public bool ReloadOnRun
        {
            get { return GetSetting("ReloadOnRun", false); }
            set { SaveSetting("ReloadOnRun", value); }
        }

        public bool ShadowCopyFiles
        {
            get { return GetSetting("ShadowCopyFiles", true); }
            set { SaveSetting("ShadowCopyFiles", value); }
        }

        public string ProcessModel
        {
            get { return GetSetting("ProcessModel", "Multiple"); }
            set { SaveSetting("ProcessModel", value); }
        }

        public string DomainUsage
        {
            get { return GetSetting("DomainUsage", "Multiple"); }
            set { SaveSetting("DomainUsage", value); }
        }

        public int Agents
        {
            get { return GetSetting("Agents", 0); }
            set { SaveSetting("Agents", value); }
        }

        public bool SetPrincipalPolicy
        {
            get { return GetSetting("SetPrincipalPolicy", false); }
            set { SaveSetting("SetPrincipalPolicy", value); }
        }

        public PrincipalPolicy PrincipalPolicy
        {
            get { return GetSetting("PrincipalPolicy", PrincipalPolicy.UnauthenticatedPrincipal); }
            set { SaveSetting("PrincipalPolicy", value); }
        }
    }
}
