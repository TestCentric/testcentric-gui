// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric.TestUtilities.Fakes
{
    public class MockTestEngine : ITestEngine
    {
        #region Instance Variables

        private ServiceLocator _services = new ServiceLocator();

        // Simulate Engine internal services, which are always present
        private ExtensionService _extensions = new ExtensionService();
        private ResultService _resultService = new ResultService();
        private AvailableRuntimesService _availableRuntimes = new AvailableRuntimesService();

        #endregion

        #region Constructor

        public MockTestEngine()
        {
            _services.AddService<IExtensionService>(_extensions);
            _services.AddService<IResultService>(_resultService);
            _services.AddService<IAvailableRuntimes>(_availableRuntimes);
        }

        #endregion

        #region Fluent Engine Setup Methods

        public MockTestEngine WithService<TService>(TService service)
        {
            _services.AddService<TService>(service);
            return this;
        }

        public MockTestEngine WithExtension()
        {
            return this;
        }

        public MockTestEngine WithResultWriter(string name)
        {
            return this;
        }

        public MockTestEngine WithRuntimes(params RuntimeFramework[] runtimes)
        {
            _availableRuntimes.AddRuntimes(runtimes);
            return this;
        }

        #endregion

        #region ITestEngine Explicit Implementation

        InternalTraceLevel ITestEngine.InternalTraceLevel { get; set; }

        IServiceLocator ITestEngine.Services { get { return _services; } }

        string ITestEngine.WorkDirectory { get; set; }

        ITestRunner ITestEngine.GetRunner(TestPackage package)
        {
            throw new NotImplementedException();
        }

        void ITestEngine.Initialize()
        {
        }

        #endregion

        #region IDisposable explicit implementation

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
