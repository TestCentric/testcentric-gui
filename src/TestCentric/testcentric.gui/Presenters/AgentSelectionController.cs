// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
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
            var package = _model.TestCentricProject;
            return package != null &&
                _model.GetAgentsForPackage(package).Count > 1;
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

            var agentsToEnable = _model.GetAgentsForPackage(_model.TestCentricProject);
            var selectedAgentName = _model.TestCentricProject.GetSetting(EnginePackageSettings.SelectedAgentName, "DEFAULT");
            
            foreach (var agentName in _model.AvailableAgents)
            {
                var menuItem = new ToolStripMenuItem(agentName)
                {
                    Tag = agentName,
                    Enabled = agentsToEnable.Contains(agentName)
                };

                agentMenu.MenuItems.Add(menuItem);
            }

            // Go through all menu items and check one
            bool isItemChecked = false;
            var packageSettings = _model.TestCentricProject.Settings;
            foreach (ToolStripMenuItem item in agentMenu.MenuItems)
            {
                if ((string)item.Tag == selectedAgentName)
                    item.Checked = isItemChecked = true;
                else
                    item.Click += (s, e) =>
                    {
                        item.Checked = true;
                        
                        if (item.Tag == null || item.Tag as string == "DEFAULT")
                            packageSettings.Remove(EnginePackageSettings.SelectedAgentName);
                        else
                            packageSettings[EnginePackageSettings.SelectedAgentName] = item.Tag;

                        // Even though the _model has a Reload method, we cannot use it because Reload
                        // does not re-create the Engine.  Since we just changed a setting, we must
                        // re-create the Engine by unloading/reloading the tests. We make a copy of
                        // __model.TestFiles because the method does an unload before it loads.
                        _model.TestCentricProject.LoadTests();
                    };
            }

            if (!isItemChecked)
            {
                defaultMenuItem.Checked = true;
                packageSettings.Remove(EnginePackageSettings.SelectedAgentName);
            }
        }
    }
}
