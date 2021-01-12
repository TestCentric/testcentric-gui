// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
#if !NET20
using System.Linq;
#endif

namespace TestCentric.Engine
{
    /// <summary>
    /// Runtime class represents a specific Runtime, which may be
    /// available in one or more versions. To define new Runtimes,
    /// add a new member to the RuntimeType enum and then update
    /// the SetProperties method in this class.
    /// </summary>
    public abstract class Runtime
    {
        #region Instances

        // NOTE: The following are the only instances, which should
        // ever exist, since the nested classes are private.

        /// <summary>Microsoft .NET Framework</summary>
        public static Runtime Net { get; } = new NetFrameworkRuntime();

        /// <summary>Mono</summary>
        public static Runtime Mono { get; } = new MonoRuntime();

        /// <summary>NetCore</summary>
        public static Runtime NetCore { get; } = new NetCoreRuntime();

        #endregion

        public abstract string DisplayName { get; }

        public abstract string FrameworkIdentifier { get; }

        public abstract bool Matches(Runtime targetRuntime);

        public static Runtime Parse(string s)
        {
            switch (s.ToLower())
            {
                case "net":
                    return Runtime.Net;
                case "mono":
                    return Runtime.Mono;
                case "netcore":
                    return Runtime.NetCore;
                default:
                    throw new NUnitEngineException($"Invalid runtime specified: {s}");
            }
        }

        public static Runtime FromFrameworkIdentifier(string s)
        {
            switch (s)
            {
                case FrameworkIdentifiers.NetFramework:
                    return Runtime.Net;
                case FrameworkIdentifiers.NetCoreApp:
                    return Runtime.NetCore;
            }

            throw new NUnitEngineException("Unrecognized Target Framework Identifier: " + s);
        }

        private class NetFrameworkRuntime : Runtime
        {
            public override string DisplayName => ".NET";
            public override string FrameworkIdentifier => FrameworkIdentifiers.NetFramework;

            public override string ToString() => "Net";
            public override bool Matches(Runtime targetRuntime) => targetRuntime is NetFrameworkRuntime;
        }

        private class MonoRuntime : Runtime
        {
            public override string DisplayName => "Mono";
            public override string FrameworkIdentifier => FrameworkIdentifiers.NetFramework;

            public override string ToString() => "Mono";
            public override bool Matches(Runtime targetRuntime) => targetRuntime is NetFrameworkRuntime;
        }

        private class NetCoreRuntime : Runtime
        {
            public override string DisplayName => ".NETCore";
            public override string FrameworkIdentifier => FrameworkIdentifiers.NetCoreApp;

            public override string ToString() => "NetCore";
            public override bool Matches(Runtime targetRuntime) => targetRuntime is NetCoreRuntime;
        }
    }
}
