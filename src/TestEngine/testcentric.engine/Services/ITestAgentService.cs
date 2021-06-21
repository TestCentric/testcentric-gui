// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;
using TestCentric.Common;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// ITestAgentService is provides information about all available
    /// test agents and allows runners to acquire and release them.
    /// Agents are provided according to specified criteria, which may
    /// be defined in the TestPackage
    /// </summary>
    /// <remarks>
    /// Currently, the only supported criteria are agent type, target
    /// runtime and bitness. Additional factors may be added in the future.
    /// </remarks>
    public interface ITestAgentService
    {
        /// <summary>
        /// AvailableAgents returns a list of information about every
        /// agent available for use. It is used for display and for 
        /// selection of the appropriate agent.
        /// </summary>
        IList<TestAgentInfo> AvailableAgents { get; }

        /// <summary>
        /// Returns true if a suitable agent can be found matching the
        /// criteria specified in the provided TestPackage.
        /// </summary>
        bool IsAgentAvailable(TestPackage package);

        /// <summary>
        /// Return an agent matching the criteria in a TestPackage.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no agent is available. Caller should check
        /// availability before trying to acquire the agent.
        /// </exception>
        ITestAgent GetAgent(TestPackage package);

        /// <summary>
        /// Select a specific agent from a list of those provided.
        /// </summary>
        /// <param name="name">The index of the agent in the list</param>
        /// <returns>An ITestAgent</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the index is out of range.
        /// </exception>
        /// <remarks>
        /// This function is intended for use by runners, which
        /// display a list of available agents and allow the user
        /// to select one of them. Its use assumes that the list
        /// of available agents remains constant while the
        /// application is running, which is currently the case.
        /// </remarks>
        ITestAgent SelectAgent(int index);

        /// <summary>
        /// Releases the test agent back to the supplier, which provided it.
        /// </summary>
        /// <param name="agent">An agent previously provided by a call to GetAgent.</param>
        /// <exception cref="InvalidOperationException">
        /// If agent was never provided by the factory or was previously released.
        /// </exception>
        /// <remarks>
        /// Disposing an agent also releases it. However, this should not
        /// normally be done by the client, which did not create it.
        /// </remarks>
        void ReleaseAgent(ITestAgent agent);
    }
}
