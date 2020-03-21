// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using System.Runtime.InteropServices;

//
// Assembly Information for the TestCentric Engine API
//
// NOTE: The API assembly does not use the common assembly info,
// which is used by other components of the engine.

[assembly: AssemblyProduct("TestCentric Engine")]

[assembly: AssemblyCompany("TestCentric Software")]
[assembly: AssemblyCopyright("Copyright (c) 2019-2020 Charlie Poole and TestCentric Engine contributors")]
[assembly: AssemblyTrademark("TestCentric is a trademark of TestCentric Software")]

[assembly: AssemblyTitle("NUnit Engine API")]
[assembly: AssemblyDescription("Defines the interfaces used to access the NUnit Engine")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c686a4e8-7c4b-4c9e-88e7-65cd1b90ba1e")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.3.3")]
[assembly: AssemblyInformationalVersion("1.3.3-ci00011-issue-551d")]
