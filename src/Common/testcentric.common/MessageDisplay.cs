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
    /// Summary description for MessageDisplay.
    /// </summary>
    public class MessageDisplay : IMessageDisplay
    {
        private static readonly string DEFAULT_CAPTION = "TestCentric";

        private readonly string caption;

        public MessageDisplay() : this(DEFAULT_CAPTION) { }

        public MessageDisplay(string caption)
        {
            this.caption = caption;
        }

        #region Public Methods

        #region Display

        public DialogResult Display(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        #endregion

        #region Error

        public DialogResult Error(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public DialogResult Error(string message, Exception exception)
        {
            return MessageBox.Show(BuildMessage(message, exception, false), caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion

        #region Info

        public DialogResult Info(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Ask

        public DialogResult Ask(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        #endregion

        #endregion

        #region Helper Methods

        private static string BuildMessage(Exception exception)
        {
            Exception ex = exception;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} : {1}", ex.GetType().ToString(), ex.Message);

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                sb.AppendFormat("\r----> {0} : {1}", ex.GetType().ToString(), ex.Message);
            }

            return sb.ToString();
        }

        private static string BuildMessage(string message, Exception exception, bool isFatal)
        {
            string msg = message + Environment.NewLine + Environment.NewLine + BuildMessage(exception);

            return isFatal
                ? msg
                : msg + Environment.NewLine + Environment.NewLine + "For further information, use the Exception Details menu item.";
        }

        #endregion
    }
}
