// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("TestCentric")]
[assembly: AssemblyProduct("TestCentric Runner for NUnit")]
[assembly: AssemblyCopyright("Copyright (c) 2015-2018 Charlie Poole")]
[assembly: AssemblyTrademark("TestCentric is a trademark of TestCentric Software")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("tc-next")]
[assembly: AssemblyDescription("Experimental GUI runner for NUnit")]

// The following GUID is for the Id of the typelib if this project is exposed to COM
[assembly: Guid("6a34c3cc-e569-4349-a736-ad99ced6c195")]

// Common version information for local developer builds.
// Should be set to the NEXT planned version between releases.
// For CI builds, this info will be updated by GitVersion.
[assembly: AssemblyVersion("0.18.0.0")]
[assembly: AssemblyFileVersion("0.18.0.0")]
