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

using System.Windows.Forms;
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    public delegate bool ActiveViewChangingHandler();
    public delegate void ActiveViewChangedHandler();

    /// <summary>
    /// IMainView represents the top level view for the
    /// Project editor. It provides a menu commands and several
    /// utility methods used in opening and saving files. It
    /// aggregates the property and xml views.
    /// </summary>
    public interface IMainView : IView
    {
        IDialogManager DialogManager { get; }

        ICommand NewProjectCommand { get; }
        ICommand OpenProjectCommand { get; }
        ICommand CloseProjectCommand { get; }
        ICommand SaveProjectCommand { get; }
        ICommand SaveProjectAsCommand { get; }

        event ActiveViewChangingHandler ActiveViewChanging;
        event ActiveViewChangedHandler ActiveViewChanged;

        event FormClosingEventHandler FormClosing;

        IPropertyView PropertyView { get; }
        IXmlView XmlView { get; }

        SelectedView SelectedView { get; set; }
    }

    public enum SelectedView
    {
        PropertyView = 0,
        XmlView = 1
    }
}
