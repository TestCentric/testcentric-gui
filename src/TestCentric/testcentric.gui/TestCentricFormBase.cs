// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    public class TestCentricFormBase : Form
    {
        private IMessageDisplay messageDisplay;
        private string caption;

        public TestCentricFormBase() { }

        public TestCentricFormBase(string caption)
        {
            this.caption = caption;
        }

        public IMessageDisplay MessageDisplay
        {
            get
            {
                if (messageDisplay == null)
                    messageDisplay = new MessageBoxDisplay(caption == null ? Text : caption);

                return messageDisplay;
            }
        }
    }
}
