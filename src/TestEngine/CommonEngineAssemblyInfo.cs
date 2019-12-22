// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;

//
// Common Information for the TestCentric Engine assemblies
//
[assembly: AssemblyTrademark("TestCentric is a trademark of TestCentric Software")]
[assembly: AssemblyCompany("TestCentric")]
[assembly: AssemblyCopyright("Copyright (c) 2019 Charlie Poole and TestCentric Engine contributors")]

#if DEBUG
#if NET35
[assembly: AssemblyConfiguration(".NET 3.5 Debug")]
#elif NET20
[assembly: AssemblyConfiguration(".NET 2.0 Debug")]
#elif NETSTANDARD1_6 || NETCOREAPP1_1
[assembly: AssemblyConfiguration(".NET Standard 1.6 Debug")]
#elif NETSTANDARD2_0 || NETCOREAPP2_1
[assembly: AssemblyConfiguration(".NET Standard 2.0 Debug")]
#else
[assembly: AssemblyConfiguration("Debug")]
#endif
#else
#if NET35
[assembly: AssemblyConfiguration(".NET 3.5")]
#elif NET20
[assembly: AssemblyConfiguration(".NET 2.0")]
#elif NETSTANDARD1_6 || NETCOREAPP1_1
[assembly: AssemblyConfiguration(".NET Standard 1.6")]
#elif NETSTANDARD2_0 || NETCOREAPP2_1
[assembly: AssemblyConfiguration(".NET Standard 2.0")]
#else
[assembly: AssemblyConfiguration("")]
#endif
#endif
