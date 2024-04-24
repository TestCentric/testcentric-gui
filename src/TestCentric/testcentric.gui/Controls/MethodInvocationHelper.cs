// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    static public class MethodInvocationHelper
    {
        public static void InvokeIfRequired(this Control ctrl, MethodInvoker _delegate)
        {
            if (ctrl.InvokeRequired)
                ctrl.BeginInvoke(_delegate);
            else
                _delegate();
        }
    }
}
