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
    /// MessageBoxDisplay provides a simple implementation of IMessageDisplay using MessageBox.
    /// </summary>
    public class MessageBoxDisplay : IMessageDisplay
    {
        private static readonly string DEFAULT_CAPTION = "TestCentric";

        private readonly string caption;

        public MessageBoxDisplay() : this(DEFAULT_CAPTION) { }

        public MessageBoxDisplay(string caption)
        {
            this.caption = caption;
        }

        #region Public Methods

        public void Error(string message)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public void Error(string message, Exception exception)
        {
            MessageBox.Show(BuildMessage(message, exception), caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public void Info(string message)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool Ask(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

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

        private static string BuildMessage(string message, Exception exception)
        {
            return message + Environment.NewLine + Environment.NewLine + BuildMessage(exception);
        }

        #endregion
    }
}
