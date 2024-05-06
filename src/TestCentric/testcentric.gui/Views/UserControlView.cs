// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    /// <summary>
    /// Base class for views implemented as a user control
    /// </summary>
    public class UserControlView : UserControl
    {
        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (InvokeRequired)
                BeginInvoke(_delegate);
            else
                _delegate();
        }

        public T InvokeIfRequired<T>(Func<T> func)
        {
            return InvokeRequired
                ? (T)Invoke(func)
                : (T)func();
        }
    }
}
