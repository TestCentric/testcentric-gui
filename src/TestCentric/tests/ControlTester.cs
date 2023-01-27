// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui
{
    /// <summary>
    /// TestFixtures that test Controls inherit from this class.
    /// </summary>
    public class ControlTester
    {
        public ControlTester() { }

        public ControlTester(Control control)
        {
            this.control = control;
        }

        // TODO: Rewrite using generics when we move to .NET 2.0

        // The control we are testing
        private Control control;

        // Various ways of looking at this control's controls
        private ControlCollection controls;

        #region Properties

        /// <summary>
        /// Get and set the control to be tested
        /// </summary>
        public Control Control
        {
            get { return control; }
            set
            {
                control = value;
                controls = new ControlCollection(control.Controls);
            }
        }

        #endregion

        #region Assertions

        public void AssertControlExists(string expectedName, Type expectedType)
        {
            bool gotName = false;
            System.Type gotType = null;
            foreach (Control ctl in control.Controls)
            {
                if (ctl.Name == expectedName)
                {
                    gotName = true;
                    if (expectedType == null)
                        return;
                    gotType = ctl.GetType();
                    if (expectedType.IsAssignableFrom(gotType))
                        return;
                }
            }

            if (gotName)
                Assert.Fail("Expected control {0} to be a {1} but was {2}", expectedName, expectedType.Name, gotType.Name);
            else
                Assert.Fail("{0} does not contain {1} control", control.Name, expectedName);
        }

        #endregion

        #region Nested Collection Classes

        #region Enumerator used by all collections
        public class ControlEnumerator : IEnumerator
        {
            IEnumerator sourceEnum;
            System.Type typeFilter;

            public ControlEnumerator(Control.ControlCollection source, System.Type typeFilter)
            {
                this.sourceEnum = source.GetEnumerator();
                this.typeFilter = typeFilter;
            }

            #region IEnumerator Members

            public void Reset()
            {
                sourceEnum.Reset();
            }

            public object Current
            {
                get { return sourceEnum.Current; }
            }

            public bool MoveNext()
            {
                while (sourceEnum.MoveNext())
                {
                    if (typeFilter == null || typeFilter.IsAssignableFrom(Current.GetType()))
                        return true;
                }

                return false;
            }

            #endregion
        }
        #endregion

        #region Control Collection

        public class ControlCollection : IEnumerable
        {
            private Control.ControlCollection source;
            private System.Type typeFilter;

            public ControlCollection(Control.ControlCollection source)
                : this(source, null) { }

            public ControlCollection(Control.ControlCollection source, System.Type typeFilter)
            {
                this.source = source;
                this.typeFilter = typeFilter;
            }

            private Control this[string name]
            {
                get
                {
                    foreach (Control control in this)
                    {
                        if (control.Name == name)
                            return control;
                    }

                    return null;
                }
            }

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new ControlEnumerator(this.source, this.typeFilter);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
