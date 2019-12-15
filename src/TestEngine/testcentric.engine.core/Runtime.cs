// ***********************************************************************
// Copyright (c) 2018 Charlie Poole, Rob Prouse
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

namespace NUnit.Engine
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
                case RuntimeType.Any:
                    DisplayName = "Any";
                    // FrameworkIdentifier is left as null
                    break;
                case RuntimeType.Net:
                    DisplayName = ".NET";
                    FrameworkIdentifier = ".NETFramework";
                    break;
                case RuntimeType.Mono:
                    DisplayName = "Mono";
                    FrameworkIdentifier = ".NETFramework";
                    break;
                case RuntimeType.NetCore:
                    DisplayName = ".NETCore";
                    FrameworkIdentifier = ".NETCoreApp";
                    break;
            }
        }

        /// <summary>Any supported runtime framework</summary>
        public static Runtime Any { get; } = new Runtime(RuntimeType.Any);

        /// <summary>Microsoft .NET Framework</summary>
        public static Runtime Net { get; } = new Runtime(RuntimeType.Net);

        /// <summary>Mono</summary>
        public static Runtime Mono { get; } = new Runtime(RuntimeType.Mono);

        /// <summary>NetCore</summary>
        public static Runtime NetCore { get; } = new Runtime(RuntimeType.NetCore);

        public static string[] KnownRuntimes { get; }

        public string DisplayName { get; private set; }

        public string FrameworkIdentifier { get; private set; }

        public bool Matches(Runtime targetRuntime)
        {
            var targetType = targetRuntime._runtimeType;

            if (_runtimeType == RuntimeType.Any || targetType == RuntimeType.Any)
                return true;

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
            foreach (string item in KnownRuntimes)
                if (item.ToLower() == name.ToLower())
                    return true;

            return false;
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
                case ".NETFramework":
                    return Runtime.Net;
                case ".NETCoreApp":
                    return Runtime.NetCore;
                default:
                    return null;
            }
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

    public static class FrameworkIdentifiers
    {
        public const string Net = ".NETFramework";
        public const string NetCoreApp = ".NETCoreApp";
    }

}
