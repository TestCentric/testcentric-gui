// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// CheckBoxElement wraps a CheckBox as an IChecked.
    /// </summary>
    public class CheckBoxElement : ControlElement, IChecked
    {
        private CheckBox _checkBox;

        public CheckBoxElement(CheckBox checkBox) : base(checkBox)
        {
            _checkBox = checkBox;
            checkBox.CheckedChanged += (s, e) => CheckedChanged?.Invoke();
        }

        public bool Checked
        {
            get { return _checkBox.Checked; }
            set
            {
                if (_checkBox.Checked != value)
                    InvokeIfRequired(() => _checkBox.Checked = value);
            }
        }

        public event CommandHandler CheckedChanged;
    }
}
