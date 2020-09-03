// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;

//
// Common Information for the TestCentric Engine 
//

[assembly: AssemblyProduct("TestCentric Engine")]

[assembly: AssemblyCompany("TestCentric Software")]
[assembly: AssemblyCopyright("Copyright (c) 2019-2020 Charlie Poole and TestCentric Engine contributors")]
[assembly: AssemblyTrademark("TestCentric is a trademark of TestCentric Software")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("1.4.1.0")]
[assembly: AssemblyFileVersion("1.4.1")]
[assembly: AssemblyInformationalVersion("1.4.1-ci00016-issue-585a")]
