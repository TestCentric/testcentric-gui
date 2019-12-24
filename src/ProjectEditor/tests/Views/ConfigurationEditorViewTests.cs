// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.Tests.Views
{
    public class ConfigurationEditorViewTests
    {
        [Test]
        public void AllViewElementsAreWrapped()
        {
            ConfigurationEditorDialog view = new ConfigurationEditorDialog();

            Assert.NotNull(view.AddCommand);
            Assert.NotNull(view.RemoveCommand);
            Assert.NotNull(view.RenameCommand);
            Assert.NotNull(view.ActiveCommand);

            Assert.NotNull(view.ConfigList);
        }
    }
}
