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
    /// TestServices caches commonly used services.
    /// </summary>
    public class TestServices : ITestServices
    {
        private IServiceLocator _services;
        private ITestEngine _testEngine;

        public TestServices(ITestEngine testEngine)
        {
            _testEngine = testEngine;
            _services = testEngine.Services;

            ExtensionService = GetService<IExtensionService>();
            ResultService = GetService<IResultService>();
            ProjectService = GetService<IProjectService>();
        }

        #region ITestServices Implementation

        public IExtensionService ExtensionService { get; }

        public IResultService ResultService { get; }

        public IProjectService ProjectService { get; }

        #endregion

        #region IServiceLocator Implementation

        public T GetService<T>() where T : class
        {
            return _services.GetService<T>();
        }

        public object GetService(Type serviceType)
        {
            return _services.GetService(serviceType);
        }

        #endregion
    }
}
