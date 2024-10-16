// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TestCentric.Engine.Internal;
using TestCentric.Extensibility;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// The ExtensionService discovers ExtensionPoints and Extensions and
    /// maintains them in a database. It can return extension nodes or
    /// actual extension objects on request.
    /// </summary>
    class ExtensionService : Service, IExtensionService
    {
        static readonly Logger log = InternalTrace.GetLogger(typeof(ExtensionService));

        static readonly Assembly ENGINE_ASSEMBLY = Assembly.GetExecutingAssembly();
        static readonly Assembly ENGINE_API_ASSEMBLY = typeof(ITestEngine).Assembly;
        static readonly string ENGINE_DIRECTORY = Path.GetDirectoryName(AssemblyHelper.GetAssemblyPath(ENGINE_ASSEMBLY));

        private IExtensionManager _extensionManager;

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

        public string ExtensionBaseDirectory { get; set; } = ENGINE_DIRECTORY;

        public IEnumerable<IExtensionNode> GetExtensionNodes<T>(bool includeDisabled = false)
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
                _extensionManager = new ExtensionManager("/TestCentric/Engine/TypeExtensions/");
                _extensionManager.FindExtensionPoints(ENGINE_ASSEMBLY, ENGINE_API_ASSEMBLY);
                _extensionManager.FindExtensions(ENGINE_DIRECTORY);
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
