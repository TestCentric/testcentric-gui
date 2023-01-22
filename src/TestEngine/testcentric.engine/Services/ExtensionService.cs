// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Internal;
using TestCentric.Extensibility;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The ExtensionService discovers ExtensionPoints and Extensions and
    /// maintains them in a database. It can return extension nodes or
    /// actual extension objects on request.
    /// </summary>
    public class ExtensionService : Service, IExtensionService
    {
        static Logger log = InternalTrace.GetLogger(typeof(ExtensionService));

        static readonly Assembly ENGINE_ASSEMBLY = Assembly.GetExecutingAssembly();
        static readonly Assembly ENGINE_API_ASSEMBLY = typeof(ITestEngine).Assembly;
        static readonly Assembly TESTCENTRIC_API_ASSEMBLY = typeof(IAgentLauncher).Assembly;
        static readonly string ENGINE_DIRECTORY = Path.GetDirectoryName(AssemblyHelper.GetAssemblyPath(ENGINE_ASSEMBLY));

        private readonly IExtensionManager _extensionManager;

        public ExtensionService()
        {
            _extensionManager = new ExtensionManager(ENGINE_ASSEMBLY, ENGINE_API_ASSEMBLY, TESTCENTRIC_API_ASSEMBLY);
        }

        #region IExtensionService Implementation

        /// <summary>
        /// Gets an enumeration of all ExtensionPoints in the engine.
        /// </summary>
        public IEnumerable<IExtensionPoint> ExtensionPoints
        {
            get { return _extensionManager.ExtensionPoints; }
        }

        /// <summary>
        /// Gets an enumeration of all installed Extensions.
        /// </summary>
        public IEnumerable<IExtensionNode> Extensions
        {
            get { return _extensionManager.Extensions; }
        }

        /// <summary>
        /// Get an ExtensionPoint based on its unique identifying path.
        /// </summary>
        IExtensionPoint IExtensionService.GetExtensionPoint(string path)
        {
            return _extensionManager.GetExtensionPoint(path);
        }

        /// <summary>
        /// Get an enumeration of ExtensionNodes based on their identifying path.
        /// </summary>
        IEnumerable<IExtensionNode> IExtensionService.GetExtensionNodes(string path)
        {
            foreach (var node in _extensionManager.GetExtensionNodes(path))
                yield return node;
        }

        /// <summary>
        /// Enable or disable an extension
        /// </summary>
        public void EnableExtension(string typeName, bool enabled)
        {
            _extensionManager.EnableExtension(typeName, enabled);
        }

        #endregion

        #region Other Public Properties and Methods

        public IList<Assembly> RootAssemblies { get; } = new List<Assembly>();

        public IEnumerable<ExtensionNode> GetExtensionNodes<T>(bool includeDisabled = false)
        {
            return _extensionManager.GetExtensionNodes<T>(includeDisabled);
        }

        public IEnumerable<T> GetExtensions<T>()
        {
            return _extensionManager.GetExtensions<T>();
        }

        #endregion

        #region StartService Override

        public override void StartService()
        {
            try
            {
                _extensionManager.Initialize(ENGINE_DIRECTORY);
                Status = ServiceStatus.Started;
            }
            catch
            {
                Status = ServiceStatus.Error;
                throw;
            }
        }

        #endregion
    }
}
