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

using System;
using System.Xml;

namespace NUnit.ProjectEditor
{
    public class XmlPresenter
    {
        private IProjectDocument doc;
        private IXmlView view;

        public XmlPresenter(IProjectDocument doc, IXmlView view)
        {
            this.doc = doc;
            this.view = view;

            view.Xml.Validated += delegate
            {
                UpdateModelFromView();

                if (!doc.IsValid)
                {
                    XmlException ex = doc.Exception as XmlException;
                    if (ex != null)
                        view.DisplayError(ex.Message, ex.LineNumber, ex.LinePosition);
                    else
                        view.DisplayError(doc.Exception.Message);
                }
            };

            doc.ProjectCreated += delegate
            {
                view.Visible = true;
                LoadViewFromModel();
            };

            doc.ProjectClosed += delegate
            {
                view.Xml.Text = null;
                view.Visible = false;
            };
        }

        public void LoadViewFromModel()
        {
            view.Xml.Text = doc.XmlText;

            if (doc.Exception != null)
            {
                XmlException ex = doc.Exception as XmlException;
                if (ex != null)
                    view.DisplayError(ex.Message, ex.LineNumber, ex.LinePosition);
                else
                    view.DisplayError(doc.Exception.Message);
            }
            else
                view.RemoveError();
        }

        private int GetOffset(int lineNumber, int charPosition)
        {
            int offset = 0;

            for (int lineCount = 1; lineCount < lineNumber; lineCount++ )
            {
                int next = doc.XmlText.IndexOf(Environment.NewLine, offset);
                if (next < 0) break;

                offset = next + Environment.NewLine.Length;
            }

            return offset - lineNumber + charPosition;
        }

        public void UpdateModelFromView()
        {
            doc.XmlText = view.Xml.Text;
        }
    }
}
