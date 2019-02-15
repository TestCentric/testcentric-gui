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

using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    /// <summary>
    /// We store settings used by the engine using the same
    /// settings path that the console runner uses. We may
    /// want to change this in the future.
    /// </summary>
    public class EngineSettings : SettingsGroup
    {
        public EngineSettings(ISettings settings)
            : base(settings, "Engine.Options") { }

        public bool ShadowCopyFiles
        {
            get { return GetSetting(nameof(ShadowCopyFiles), true); }
            set { SaveSetting(nameof(ShadowCopyFiles), value); }
        }

        public string ProcessModel
        {
            get { return GetSetting(nameof(ProcessModel), "Default"); }
            set { SaveSetting(nameof(ProcessModel), value); }
        }

        public string DomainUsage
        {
            get { return GetSetting(nameof(DomainUsage), "Default"); }
            set { SaveSetting(nameof(DomainUsage), value); }
        }

        public int Agents
        {
            get { return GetSetting(nameof(Agents), 0); }
            set { SaveSetting(nameof(Agents), value); }
        }

        public bool SetPrincipalPolicy
        {
            get { return GetSetting(nameof(SetPrincipalPolicy), false); }
            set { SaveSetting(nameof(SetPrincipalPolicy), value); }
        }

        public string PrincipalPolicy
        {
            get { return GetSetting(nameof(PrincipalPolicy), nameof(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal)); }
            set { SaveSetting(nameof(PrincipalPolicy), value); }
        }
    }
}
