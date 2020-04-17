// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// ITestServices extends IServiceLocator in order to
    /// conveniently cache commonly used services.
    /// </summary>
    public interface ITestServices : IServiceLocator
    {
        IExtensionService ExtensionService { get; }
        IResultService ResultService { get; }
        // TODO: Figure out how to handle this
        //IProjectService ProjectService { get; }
    }
}
