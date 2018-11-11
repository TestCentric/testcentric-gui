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


namespace NUnit.ProjectEditor.ViewElements
{
    /// <summary>
    /// IComboBox is implemented by view elements that associate
    /// an editable TextBox with a SelectionList. The classic
    /// implementation is System.Windows.Forms.ComboBox. This 
    /// interface is only intended for use when the TextBox
    /// is editable. Otherwise, ISelectionList provides all
    /// the necessary functionality.
    /// </summary>
    public interface IComboBox : ISelectionList
    {
        /// <summary>
        /// Gets or sets the value of the TextBox associated
        /// with this ComboBox.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Event that is raised when the text has changed
        /// and the focus is moved away.
        /// </summary>
        event ActionDelegate TextValidated;
    }
}
