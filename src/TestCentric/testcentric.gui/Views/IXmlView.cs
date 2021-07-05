// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using System.Xml;

namespace TestCentric.Gui.Views
{
    using Elements;

    public interface IXmlView : IView
    {
        bool Visible { get; set; }
        bool WordWrap { get; set; }

        string Header { get; set; }
        IViewElement XmlPanel { get; }

        ICommand CopyToolStripMenuItem { get; }
        IChecked WordWrapToolStripMenuItem { get; }

        XmlNode TestXml { get; set; }

        string SelectedText { get; set; }
        
        void SelectAll();
        void Copy();

        event CommandHandler SelectAllCommand;
        event CommandHandler SelectionChanged;
        event CommandHandler CopyCommand;
        event CommandHandler WordWrapChanged;
        event CommandHandler ViewGotFocus;
    }
}
