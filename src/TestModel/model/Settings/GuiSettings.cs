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

using System.Drawing;
using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class GuiSettings : SettingsGroup
    {
        private static readonly Font DefaultFixedFont = new Font(FontFamily.GenericMonospace, 8.0F);

        public GuiSettings(ISettings settings)
             : base(settings, "Gui") { }

        public TestTreeSettings TestTree
        {
            get { return new TestTreeSettings(_settings); }
        }

        public ResultTabsSettings ResultTabs
        {
            get { return new ResultTabsSettings(_settings); }
        }

        public RecentProjectsSettings RecentProjects
        {
            get { return new RecentProjectsSettings(_settings); }
        }

        public MiniFormSettings MiniForm
        {
            get { return new MiniFormSettings(_settings); }
        }

        public MainFormSettings MainForm
        {
            get { return new MainFormSettings(_settings); }
        }

        public string DisplayFormat
        {
            get { return GetSetting("DisplayFormat", "Full"); }
            set { SaveSetting("DisplayFormat", value); }
        }

        public string InitialSettingsPage
        {
            get { return (string)GetSetting("Settings.InitialPage"); }
            set { SaveSetting("Settings.InitialPage", value); } // TODO: Handle null
        }

        public Font FixedFont
        {
            get { return GetSetting("FixedFont", DefaultFixedFont); }
            set { SaveSetting("FixedFont", value); }
        }
    }
}