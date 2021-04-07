// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model.Settings
{
    /// <summary>
    /// We store settings used by the engine using the same
    /// settings path that the console runner uses. We may
    /// want to change this in the future.
    /// </summary>
    public class EngineSettings : SettingsGroup
    {
        public EngineSettings(ISettings settings, string prefix)
            : base(settings, prefix + "Engine") { }

        public bool ShadowCopyFiles
        {
            get { return GetSetting(nameof(ShadowCopyFiles), true); }
            set { SaveSetting(nameof(ShadowCopyFiles), value); }
        }

        public int Agents
        {
            get { return GetSetting(nameof(Agents), 0); }
            set { SaveSetting(nameof(Agents), value); }
        }

        public bool ReloadOnChange
        {
            get { return GetSetting(nameof(ReloadOnChange), true); }
            set { SaveSetting(nameof(ReloadOnChange), value); }
        }

        public bool RerunOnChange
        {
            get { return GetSetting(nameof(RerunOnChange), false); }
            set { SaveSetting(nameof(RerunOnChange), value); }
        }

        public bool ReloadOnRun
        {
            get { return GetSetting(nameof(ReloadOnRun), false); }
            set { SaveSetting(nameof(ReloadOnRun), value); }
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
