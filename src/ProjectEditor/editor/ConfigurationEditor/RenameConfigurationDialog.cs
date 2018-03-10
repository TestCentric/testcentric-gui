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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
	/// <summary>
    /// Displays a dialog for entry of a new name for an
    /// existing configuration. This dialog collects and
    /// validates the name. The caller is responsible for
    /// actually renaming the cofiguration.
    /// </summary>
	public partial class RenameConfigurationDialog : System.Windows.Forms.Form, IRenameConfigurationDialog
    {
        #region Instance Variables

        private ITextElement configurationName;
        private ICommand okButtonWrapper;
        private IMessageDisplay messageDisplay;

        #endregion

        #region Constructor

        public RenameConfigurationDialog()
		{
			InitializeComponent();

            configurationName = new TextElement(configurationNameTextBox);
            okButtonWrapper = new ButtonElement(okButton);

            messageDisplay = new MessageDisplay("Rename Configuration");
        }

		#endregion

		#region IRenameConfigurationDialogMembers

		public ITextElement ConfigurationName 
        {
            get { return configurationName; }
        }

        public ICommand OkButton 
        {
            get { return okButtonWrapper; }
        }

		#endregion

        #region IView Members

        public IMessageDisplay MessageDisplay 
        {
            get { return messageDisplay; }
        }

        #endregion
    }

    public interface IRenameConfigurationDialog : IDialog
    {
        ITextElement ConfigurationName { get; }
        ICommand OkButton { get; }
    }
}
