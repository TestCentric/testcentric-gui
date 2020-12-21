// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Views
{
    public partial class SettingsPage : UserControl
    {
        // Constructor used by the Windows.Forms Designer.
        // For some reason, this must be present for any
        // UserControl used as a base for another user control.
        public SettingsPage()
        {
            InitializeComponent();
        }

        // Constructor we use in creating page for SettingsDialog
        public SettingsPage(string key, UserSettings settings)
        {
            InitializeComponent();

            this.Key = key;
            this.Settings = settings;
            this.MessageDisplay = new MessageBoxDisplay("NUnit Settings");
        }

        #region Properties

        protected UserSettings Settings { get; private set; }

        protected IMessageDisplay MessageDisplay { get; private set; }

        public string Key { get; private set; }

        public bool SettingsLoaded { get; private set; }

        public virtual bool HasChangesRequiringReload
        {
            get { return false; }
        }

        #endregion

        #region Public Methods

        public virtual void LoadSettings()
        {
        }

        public virtual void ApplySettings()
        {
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                this.LoadSettings();
                SettingsLoaded = true;
            }
        }
    }
}
