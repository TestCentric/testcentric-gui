// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine;

namespace TestCentric.Gui.Model
{
    using Services;
    using Settings;

    /// <summary>
    /// ITestServices extends IServiceLocator in order to
    /// conveniently cache commonly used services.
    /// </summary>
    public interface ITestServices : IServiceLocator
    {
        IExtensionService ExtensionService { get; }
        IResultService ResultService { get; }
        IProjectService ProjectService { get; }
    }
}
