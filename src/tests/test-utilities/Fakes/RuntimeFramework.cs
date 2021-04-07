// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;

namespace TestCentric.TestUtilities.Fakes
{
    public class RuntimeFramework : IRuntimeFramework
    {
        public RuntimeFramework(string id, Version version)
        {
            Id = id;
            FrameworkVersion = version.Build >= 0
                ? new Version(version.Major, version.Minor)
                : version;
            ClrVersion = version;
            DisplayName = id;
        }

        public string Id { get; }

        public Version FrameworkVersion { get; }

        public Version ClrVersion { get; }

        public string DisplayName { get; set; }

        public string Profile { get; set; }
    }
}
