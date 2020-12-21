// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Text;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    /// <summary>
    /// MessageBoxDisplay provides a simple implementation of IMessageDisplay using
    /// a Windows MessageBox. Additionally, it provides static methods for a displaying
    /// a MessageBox without exposing implementation details to the caller.
    /// </summary>
    public class MessageBoxDisplay : IMessageDisplay
    {
        private static readonly string DEFAULT_CAPTION = "TestCentric";

        private readonly string _caption;

        public MessageBoxDisplay() : this(DEFAULT_CAPTION) { }

        public MessageBoxDisplay(string caption)
        {
            _caption = caption;
        }

        #region Static Methods to Display a MessageBox

        public static void Error(string message)
        {
            Error(message, DEFAULT_CAPTION);
        }

        public static void Error(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public static void Info(string message)
        {
            Info(message, DEFAULT_CAPTION);
        }

        public static void Info(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool YesNo(string message)
        {
            return YesNo(message, DEFAULT_CAPTION);
        }

        public static bool YesNo(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        #endregion

        #region IMessageDisplay Implementation

        void IMessageDisplay.Error(string message)
        {
            Error(message, _caption);
        }

        void IMessageDisplay.Info(string message)
        {
            Info(message, _caption);
        }

        bool IMessageDisplay.YesNo(string message)
        {
            return YesNo(message, _caption);
        }

        #endregion
    }
}
