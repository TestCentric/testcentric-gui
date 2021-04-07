// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Dialogs
{
    public class DialogCreationTests
    {
        /// <summary>
        /// Minimal creation test for each dialog, in order to ensure
        /// that no exception is thrown.
        /// </summary>
        /// <param name="dlgType"></param>
        // TODO: We can't test TreeBasedSettingsDialog easily because
        // it references the main presenter. This needs to be changed.
        [TestCase(typeof(AboutBox))]
        [TestCase(typeof(TestPropertiesDialog))]
        [TestCase(typeof(SettingsDialogBase))]
        [TestCase(typeof(ExtensionDialog), null)]
        [TestCase(typeof(ParameterDialog))]
        [TestCase(typeof(TestParametersDialog))]
        public void CreateDialog(Type dlgType, params object[] args)
        {
            Activator.CreateInstance(dlgType, args);
        }
    }
}
