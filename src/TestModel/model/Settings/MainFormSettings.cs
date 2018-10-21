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

using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class MainFormSettings : SettingsGroup
    {
        public MainFormSettings(ISettings settings)
            : base(settings, "Gui.MainForm") { }

        public int Left
        {
            get { return GetSetting(nameof(Left), 10); }
            set { SaveSetting(nameof(Left), value); }
        }

        public int Top
        {
            get { return GetSetting(nameof(Top), 10); }
            set { SaveSetting(nameof(Top), value); }
        }

        public int Width
        {
            get { return GetSetting(nameof(Width), 700); }
            set { SaveSetting(nameof(Width), value); }
        }

        public int Height
        {
            get { return GetSetting(nameof(Height), 400); }
            set { SaveSetting(nameof(Height), value); }
        }

        public bool Maximized
        {
            get { return GetSetting(nameof(Maximized), false); }
            set { SaveSetting(nameof(Maximized), value); }
        }

        public int SplitPosition
        {
            get { return GetSetting(nameof(SplitPosition), 0); }
            set { SaveSetting(nameof(SplitPosition), value); }
        }
    }
}
