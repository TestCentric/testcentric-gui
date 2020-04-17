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
    public class Runtime
    {
        private RuntimeType _runtimeType;

        static Runtime()
        {
            KnownRuntimes = Enum.GetNames(typeof(RuntimeType));
        }

        private Runtime(RuntimeType runtimeType)
        {
            SetProperties(runtimeType);
        }

        private void SetProperties(RuntimeType runtimeType)
        {
            _runtimeType = runtimeType;

            // Properties are completely determined by the RuntimeType
            switch(runtimeType)
            {
                case RuntimeType.Net:
                    DisplayName = ".NET";
                    FrameworkIdentifier = FrameworkIdentifiers.Net;
                    TargetFrameworkMoniker = "net";
                    break;
                case RuntimeType.Mono:
                    DisplayName = "Mono";
                    FrameworkIdentifier = FrameworkIdentifiers.Net;
                    break;
                case RuntimeType.NetCore:
                    DisplayName = ".NETCore";
                    FrameworkIdentifier = FrameworkIdentifiers.NetCoreApp;
                    break;
            }
        }

        /// <summary>Microsoft .NET Framework</summary>
        public static Runtime Net { get; } = new Runtime(RuntimeType.Net);

        /// <summary>Mono</summary>
        public static Runtime Mono { get; } = new Runtime(RuntimeType.Mono);

        /// <summary>NetCore</summary>
        public static Runtime NetCore { get; } = new Runtime(RuntimeType.NetCore);

        public static string[] KnownRuntimes { get; }

        public string DisplayName { get; private set; }

        public string FrameworkIdentifier { get; private set; }

        public string TargetFrameworkMoniker { get; private set; }

        public bool Matches(Runtime targetRuntime)
        {
            var targetType = targetRuntime._runtimeType;

            if (_runtimeType == targetType)
                return true;

            if (_runtimeType == RuntimeType.Net && targetType == RuntimeType.Mono)
                return true;

            if (_runtimeType == RuntimeType.Mono && targetType == RuntimeType.Net)
                return true;

            return false;
        }

        /// <summary>
        /// Check to see if a Runtime of the supplied name exists
        /// </summary>
        /// <param name="name">Name of the Runtime to check</param>
        /// <returns>True if name represents a known runtime, otherwise false</returns>
        public static bool IsKnownRuntime(string name)
        {
#if !NET20
            return KnownRuntimes.Any(r => String.Equals(r, name, StringComparison.OrdinalIgnoreCase));
#else
            foreach (string item in KnownRuntimes)
                if (item.ToLower() == name.ToLower())
                    return true;

            return false;
#endif
        }

        public static Runtime Parse(string s)
        {
            var runtimeType = (RuntimeType)System.Enum.Parse(typeof(RuntimeType), s, true);
            return new Runtime(runtimeType);
        }

        public static Runtime FromFrameworkIdentifier(string s)
        {
            switch(s)
            {
                case FrameworkIdentifiers.Net:
                    return Runtime.Net;
                case FrameworkIdentifiers.NetCoreApp:
                    return Runtime.NetCore;
            }

            throw new NUnitEngineException("Unrecognized Target Framework Identifier: " + s);
        }

        public override string ToString()
        {
            return _runtimeType.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Runtime)
            {
                var other = obj as Runtime;
                return _runtimeType.Equals(other._runtimeType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _runtimeType.GetHashCode();
        }

        public static bool operator ==(Runtime left, Runtime right)
        {
            if (left is null)
                return right is null;

            if (right is null)
                return false;

            return left._runtimeType == right._runtimeType;
        }

        public static bool operator !=(Runtime left, Runtime right)
        {
            return left._runtimeType != right._runtimeType;
        }
    }
}
