// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TestCentric.Engine.Internal
{
	[TestFixture]
	public class PathUtilTests : PathUtils
	{
		[Test]
		public void CheckDefaults()
		{
			Assert.That(PathUtils.DirectorySeparatorChar, Is.EqualTo(Path.DirectorySeparatorChar));
			Assert.That(PathUtils.AltDirectorySeparatorChar, Is.EqualTo(Path.AltDirectorySeparatorChar));
		}
	}

	[TestFixture]
	public class PathUtilTests_Windows : PathUtils
	{
		[OneTimeSetUp]
		public static void SetUpUnixSeparators()
		{
			PathUtils.DirectorySeparatorChar = '\\';
			PathUtils.AltDirectorySeparatorChar = '/';
		}

		[OneTimeTearDown]
		public static void RestoreDefaultSeparators()
		{
			PathUtils.DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
			PathUtils.AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		}

		[Test]
		public void Canonicalize()
		{
			Assert.That(PathUtils.Canonicalize( @"C:\folder1\.\folder2\..\file.tmp" ), Is.EqualTo(@"C:\folder1\file.tmp"));
			Assert.That(PathUtils.Canonicalize( @"folder1\.\folder2\..\file.tmp" ), Is.EqualTo(@"folder1\file.tmp"));
			Assert.That(PathUtils.Canonicalize( @"folder1\folder2\.\..\file.tmp" ), Is.EqualTo(@"folder1\file.tmp"));
			Assert.That(PathUtils.Canonicalize( @"folder1\folder2\..\.\..\file.tmp" ), Is.EqualTo(@"file.tmp"));
			Assert.That(PathUtils.Canonicalize( @"folder1\folder2\..\..\..\file.tmp" ), Is.EqualTo(@"file.tmp"));
		}

        [Test]
        public void RelativePath()
		{
            bool windows = false;

#if NETCOREAPP1_1 || NETCOREAPP2_1
            windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            var platform = Environment.OSVersion.Platform;
            windows = platform == PlatformID.Win32NT;
#endif

            Assume.That(windows, Is.True);

			Assert.That(PathUtils.RelativePath(
				@"c:\folder1", @"c:\folder1\folder2\folder3" ), Is.EqualTo(@"folder2\folder3"));
			Assert.That(PathUtils.RelativePath(
				@"c:\folder1", @"c:\folder2\folder3" ), Is.EqualTo(@"..\folder2\folder3"));
			Assert.That(PathUtils.RelativePath(
				@"c:\folder1", @"bin\debug" ), Is.EqualTo(@"bin\debug"));
			Assert.IsNull( PathUtils.RelativePath( @"C:\folder", @"D:\folder" ),
				"Unrelated paths should return null" );
            Assert.IsNull(PathUtils.RelativePath(@"C:\", @"D:\"),
                "Unrelated roots should return null");
            Assert.IsNull(PathUtils.RelativePath(@"C:", @"D:"),
                "Unrelated roots (no trailing separators) should return null");
            Assert.That(PathUtils.RelativePath(@"C:\folder1", @"C:\folder1"), Is.EqualTo(string.Empty));
            Assert.That(PathUtils.RelativePath(@"C:\", @"C:\"), Is.EqualTo(string.Empty));

            // First filePath consisting just of a root:
            Assert.That(PathUtils.RelativePath(
                @"C:\", @"C:\folder1\folder2"), Is.EqualTo(@"folder1\folder2"));

            // Trailing directory separator in first filePath shall be ignored:
            Assert.That(PathUtils.RelativePath(
                @"c:\folder1\", @"c:\folder1\folder2\folder3"), Is.EqualTo(@"folder2\folder3"));

            // Case-insensitive behavior, preserving 2nd filePath directories in result:
            Assert.That(PathUtils.RelativePath(
                @"C:\folder1", @"c:\folder1\Folder2\Folder3"), Is.EqualTo(@"Folder2\Folder3"));
            Assert.That(PathUtils.RelativePath(
                @"c:\folder1", @"C:\Folder2\folder3"), Is.EqualTo(@"..\Folder2\folder3"));
        }

        [TestCase(@"C:\folder1\folder2\folder3", @"c:\folder1\.\folder2\junk\..\folder3", true)]
        [TestCase(@"C:\folder1\folder2\", @"c:\folder1\.\folder2\junk\..\folder3", true)]
        [TestCase(@"C:\folder1\folder2", @"c:\folder1\.\folder2\junk\..\folder3", true)]
        [TestCase(@"C:\folder1\folder2", @"c:\folder1\.\Folder2\junk\..\folder3", true)]
        [TestCase(@"C:\folder1\folder2", @"c:\folder1\.\folder22\junk\..\folder3", false)]
        [TestCase(@"C:\folder1\folder2ile.tmp", @"D:\folder1\.\folder2\folder3\file.tmp", false)]
        [TestCase(@"C:\", @"D:\", false)]
        [TestCase(@"C:\", @"c:\", true)]
		[TestCase(@"C:\", @"c:\bin\debug", true)]
        public void SamePathOrUnder(string path1, string path2, bool expected)
        {
            if (PathUtils.SamePathOrUnder(path1, path2) != expected)
            {
                string msg = expected
                   ? $"\r\n\tExpected: Same path or under <{path1}>\r\n\t but was: <{path2}>"
                   : $"\r\n\tExpected: Not same path or under <{path1}>\r\n\t but was: <{path2}>";
                Assert.Fail(msg);
            }
        }
	}

	[TestFixture]
	public class PathUtilTests_Unix : PathUtils
	{
		[OneTimeSetUp]
		public static void SetUpUnixSeparators()
		{
			PathUtils.DirectorySeparatorChar = '/';
			PathUtils.AltDirectorySeparatorChar = '\\';
		}

		[OneTimeTearDown]
		public static void RestoreDefaultSeparators()
		{
			PathUtils.DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
			PathUtils.AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		}

		[Test]
		public void Canonicalize()
		{
			Assert.That(PathUtils.Canonicalize( "/folder1/./folder2/../file.tmp" ), Is.EqualTo("/folder1/file.tmp"));
			Assert.That(PathUtils.Canonicalize( "folder1/./folder2/../file.tmp" ), Is.EqualTo("folder1/file.tmp"));
			Assert.That(PathUtils.Canonicalize( "folder1/folder2/./../file.tmp" ), Is.EqualTo("folder1/file.tmp"));
			Assert.That(PathUtils.Canonicalize( "folder1/folder2/.././../file.tmp" ), Is.EqualTo("file.tmp"));
			Assert.That(PathUtils.Canonicalize( "folder1/folder2/../../../file.tmp" ), Is.EqualTo("file.tmp"));
		}

		[Test]
		public void RelativePath()
		{
			Assert.That(PathUtils.RelativePath(	"/folder1", "/folder1/folder2/folder3" ), Is.EqualTo("folder2/folder3"));
			Assert.That(PathUtils.RelativePath( "/folder1", "/folder2/folder3" ), Is.EqualTo("../folder2/folder3"));
			Assert.That(PathUtils.RelativePath( "/folder1", "bin/debug" ), Is.EqualTo("bin/debug"));
			Assert.That(PathUtils.RelativePath( "/folder", "/other/folder" ), Is.EqualTo("../other/folder"));
			Assert.That(PathUtils.RelativePath( "/a/b/c", "/a/d" ), Is.EqualTo("../../d"));
            Assert.That(PathUtils.RelativePath("/a/b", "/a/b"), Is.EqualTo(string.Empty));
            Assert.That(PathUtils.RelativePath("/", "/"), Is.EqualTo(string.Empty));

            // First filePath consisting just of a root:
            Assert.That(PathUtils.RelativePath(
                "/", "/folder1/folder2"), Is.EqualTo("folder1/folder2"));

            // Trailing directory separator in first filePath shall be ignored:
            Assert.That(PathUtils.RelativePath(
                "/folder1/", "/folder1/folder2/folder3"), Is.EqualTo("folder2/folder3"));

            // Case-sensitive behavior:
            Assert.That(PathUtils.RelativePath("/folder1", "/Folder1/Folder2/folder3"), Is.EqualTo("../Folder1/Folder2/folder3"), "folders differing in case");
        }

        [TestCase( "/folder1/folder2/folder3", "/folder1/./folder2/junk/../folder3", true )]
        [TestCase( "/folder1/folder2/", "/folder1/./folder2/junk/../folder3", true )]
        [TestCase( "/folder1/folder2", "/folder1/./folder2/junk/../folder3", true )]
        [TestCase( "/folder1/folder2", "/folder1/./Folder2/junk/../folder3", false )]
        [TestCase( "/folder1/folder2", "/folder1/./folder22/junk/../folder3", false )]
        [TestCase( "/", "/", true )]
        [TestCase( "/", "/bin/debug", true )]
		public void SamePathOrUnder(string path1, string path2, bool expected)
		{
            if (PathUtils.SamePathOrUnder(path1, path2) != expected)
            {
                string msg = expected
                   ? $"\r\n\tExpected: Same path or under <{path1}>\r\n\t but was: <{path2}>"
                   : $"\r\n\tExpected: Not same path or under <{path1}>\r\n\t but was: <{path2}>";
                Assert.Fail(msg);
            }
		}
	}
}
