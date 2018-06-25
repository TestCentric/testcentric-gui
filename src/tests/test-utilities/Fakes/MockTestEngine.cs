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

using System;
using NUnit.Engine;

namespace NUnit.TestUtilities.Fakes
{
    public class MockTestEngine : ITestEngine
    {
        #region Instance Variables

        private ServiceLocator _services = new ServiceLocator();

        // Simulate Engine internal services, which are always present
        private SettingsService _settings = new SettingsService();
        private RecentFilesService _recentFiles = new RecentFilesService();
        private ExtensionService _extensions = new ExtensionService();
        private ResultService _resultService = new ResultService();
        private AvailableRuntimesService _availableRuntimes = new AvailableRuntimesService();

        #endregion

        #region Constructor

        public MockTestEngine()
        {
            _services.AddService<ISettings>(_settings);
            _services.AddService<IRecentFiles>(_recentFiles);
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

        public MockTestEngine WithSetting(string key, object value)
        {
            _settings.SaveSetting(key, value);
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
