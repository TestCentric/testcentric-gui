// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// An object implementing ITestAgentProvider is able to provide
    /// test agents of a particular type, which satisfy the criteria
    /// specified in a TestPackage.
    /// </summary>
    /// <remarks>
    /// Currently, the only supported criteria are agent type,
    /// target runtime and bitness. Additional factors may be added.
    /// </remarks>
    public interface ITestAgentProvider
    {
        /// <summary>
        /// AvailableAgents returns a list of information about every
        /// agent available for use. It is used for display and for 
        /// selection of the appropriate agent.
        /// </summary>
        /// <remarks>
        /// The factory creates this list by combining info from
        /// each agent source available.
        /// </remarks>
        IList<TestAgentInfo> AvailableAgents { get; }

        /// <summary>
        /// Returns true if an agent can be found, which is suitable
        /// for running the provided test package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        bool IsAgentAvailable(TestPackage package);

        /// <summary>
        /// Return an agent, which best matches the criteria defined
        /// in a TestPackage.
        /// </summary>
        /// <param name="package">The test package to be run</param>
        /// <returns>An ITestAgent</returns>
        /// <exception cref="ArgumentException">If no agent is available.</exception>
        ITestAgent GetAgent(TestPackage package);

        /// <summary>
        /// Releases the test agent back to the supplier, which provided it.
        /// </summary>
        /// <param name="agent">An agent previously provided by a call to GetAgent.</param>
        /// <exception cref="InvalidOperationException">
        /// If agent was never provided by the factory or was previously released.
        /// </exception>
        /// <remarks>
        /// Disposing an agent also releases it. However, this should not
        /// normally be done by the client, but by the source that created
        /// the agent in the first place.
        /// </remarks>
        void ReleaseAgent(ITestAgent agent);
    }
}
