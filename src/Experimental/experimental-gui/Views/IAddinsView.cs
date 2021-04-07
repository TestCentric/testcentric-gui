// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Engine.Extensibility;

namespace TestCentric.Gui.Views
{
    public interface IAddinsView : IDialog
    {
        void AddExtensionPoint(IExtensionPoint extensionPoint);
    }
}
