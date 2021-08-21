// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using TestCentric.Common;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters
{
    public class AgentSelectionController
    {
        private ITestModel _model;
        private IMainView _view;

        public AgentSelectionController(ITestModel model, IMainView view)
        {
            _model = model;
            _view = view;           
        }

        public bool AllowAgentSelection()
        {
            var package = _model.TestPackage;
            return package != null &&
                _model.GetAvailableAgents(package).Count > 1;
        }

        public void PopulateMenu()
        {
            var agentMenu = _view.SelectAgentMenu;

            agentMenu.MenuItems.Clear();

            var defaultMenuItem = new ToolStripMenuItem("Default")
            {
                Name = "defaultMenuItem",
                Tag = "DEFAULT"
            };

            agentMenu.MenuItems.Add(defaultMenuItem);

            var agentsToEnable = _model.GetAvailableAgents(_model.TestPackage).Select(info => info.AgentName);
            var selectedAgentName = _model.TestPackage.GetSetting(EnginePackageSettings.SelectedAgentName, "DEFAULT");
            
            foreach (var info in _model.AvailableAgents)
            {
                var menuItem = new ToolStripMenuItem(info.AgentName)
                {
                    Tag = info.AgentName,
                    Enabled = agentsToEnable.Contains(info.AgentName)
                };

                agentMenu.MenuItems.Add(menuItem);
            }

            // Go through all menu items and check one
            bool isItemChecked = false;
            foreach (ToolStripMenuItem item in agentMenu.MenuItems)
            {
                if ((string)item.Tag == selectedAgentName)
                    item.Checked = isItemChecked = true;
                else
                    item.Click += (s, e) =>
                    {
                        item.Checked = true;
                        if (item.Tag == null || item.Tag as string == "DEFAULT")
                            _model.PackageOverrides.Remove(EnginePackageSettings.SelectedAgentName);
                        else
                            _model.PackageOverrides[EnginePackageSettings.SelectedAgentName] = item.Tag;

                        // Even though the _model has a Reload method, we cannot use it because Reload
                        // does not re-create the Engine.  Since we just changed a setting, we must
                        // re-create the Engine by unloading/reloading the tests. We make a copy of
                        // __model.TestFiles because the method does an unload before it loads.
                        _model.LoadTests(new List<string>(_model.TestFiles));
                    };
            }

            if (!isItemChecked)
            {
                defaultMenuItem.Checked = true;
                _model.TestPackage.Settings.Remove(EnginePackageSettings.SelectedAgentName);
                _model.PackageOverrides.Remove(EnginePackageSettings.SelectedAgentName);
            }
        }
    }
}
