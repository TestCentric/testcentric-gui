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
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Model;

	/// <summary>
	/// SettingsPage is the base class for all pages used
	/// in a tabbed or tree-structured SettingsDialog.
	/// </summary>
	public class SettingsPage : UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string _key;
		private string _title;

        private MessageDisplay _messageDisplay;

		// Constructor used by the Windows.Forms Designer
		public SettingsPage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		// Constructor we use in creating page for a Tabbed
		// or TreeBased dialog.
		public SettingsPage( string key) : this()
		{
			_key = key;
			_title = key;
			int dot = key.LastIndexOf( '.' );
			if ( dot >= 0 ) _title = key.Substring(dot+1);
            _messageDisplay = new MessageDisplay("NUnit Settings");
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Properties

		public string Key
		{
			get { return _key; }
		}

		public string Title
		{
			get { return _title; }
		}

		public bool SettingsLoaded
		{
			get { return Settings != null; }
		}

		public virtual bool HasChangesRequiringReload
		{
			get { return false; }
		}

        public IMessageDisplay MessageDisplay
        {
            get { return _messageDisplay; }
        }

        protected ISettings Settings { get; private set; }

		#endregion

		#region Public Methods
		public virtual void LoadSettings()
		{
		}

		public virtual void ApplySettings()
		{
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// SettingsPage
			// 
			Name = "SettingsPage";
			Size = new System.Drawing.Size(456, 336);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if ( !DesignMode )
			{
                Settings = (ParentForm as SettingsDialogBase)?.Settings;
				LoadSettings();
			}
		}
	}
}
