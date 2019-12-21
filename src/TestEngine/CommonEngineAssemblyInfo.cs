// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;

//
// Common Information for the NUnit assemblies
//
[assembly: AssemblyTrademark("NUnit is a trademark of NUnit Software")]
[assembly: AssemblyCompany("NUnit Software")]
[assembly: AssemblyCopyright("Copyright (c) 2019 Charlie Poole, Rob Prouse")]

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
