// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Reflection;

//
// Common Information for the TestCentric Runner 
//

[assembly: AssemblyProduct("TestCentric Runner for NUnit")]

[assembly: AssemblyCompany("TestCentric Software")]
[assembly: AssemblyCopyright("Copyright (C) 2018-2021 Charlie Poole and TestCentric contributors")]
[assembly: AssemblyTrademark("TestCentric is a trademark of TestCentric Software")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0-ci00030-issue-753")]
