// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public class DialogManager : IDialogManager
    {
        #region IDialogManager Members

        public IList<string> SelectMultipleFiles(string title, string filter)
        {
            OpenFileDialog dlg = CreateOpenFileDialog(title, filter);

            dlg.Multiselect = true;

            return dlg.ShowDialog() == DialogResult.OK
                ? dlg.FileNames
                : new string[0];
        }

        public string GetFileOpenPath(string title, string filter)
        {
            OpenFileDialog dlg = CreateOpenFileDialog(title, filter);

            dlg.Multiselect = false;

            return dlg.ShowDialog() == DialogResult.OK
                ? dlg.FileName
                : null;
        }

        public string GetFileSavePath(string title, string filter)
        {
            SaveFileDialog dlg = CreateSaveFileDialog(title, filter);

            return dlg.ShowDialog() == DialogResult.OK
                ? dlg.FileName
                : null;
        }

        public string GetFolderPath(string message, string initialPath)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = message;
            browser.SelectedPath = initialPath;
            return browser.ShowDialog() == DialogResult.OK
                ? browser.SelectedPath
                : null;
        }

        public Font SelectFont(Font currentFont)
        {
            FontDialog dlg = new FontDialog();
            dlg.FontMustExist = true;
            dlg.Font = currentFont;
            dlg.MinSize = 6;
            dlg.MaxSize = 12;
            dlg.AllowVectorFonts = false;
            dlg.ScriptsOnly = true;
            dlg.ShowEffects = false;
            dlg.ShowApply = true;
            dlg.Apply += (s, e) => ApplyFont?.Invoke(currentFont = dlg.Font);

            return dlg.ShowDialog() == DialogResult.OK ? dlg.Font : currentFont;
        }

        public event ApplyFontHandler ApplyFont;

        #endregion

        #region Helper Methods

        private static OpenFileDialog CreateOpenFileDialog(string title, string filter)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = title;
            dlg.Filter = filter;
            //if (initialDirectory != null)
            //    dlg.InitialDirectory = initialDirectory;
            dlg.FilterIndex = 1;
            dlg.FileName = "";
            return dlg;
        }

        private static SaveFileDialog CreateSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = title;
            dlg.Filter = filter;
            dlg.FilterIndex = 1;
            dlg.FileName = "";
            return dlg;
        }

        #endregion
    }
}
