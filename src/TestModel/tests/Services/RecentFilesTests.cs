// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Model.Services
{
    /// <summary>
    /// This fixture is used to test both RecentProjects and
    /// its base class RecentFiles.  If we add any other derived
    /// classes, the tests should be refactored.
    /// </summary>
    [TestFixture]
    public class RecentFilesTests
    {
        RecentFiles _recentFiles;

        [SetUp]
        public void SetUp()
        {
            _recentFiles = new RecentFiles(new SettingsStore(), "Testing.");
        }

        [Test]
        public void EmptyList()
        {
            Assert.IsNotNull(  _recentFiles.Entries, "Entries should never be null" );
            Assert.That(_recentFiles.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddSingleItem()
        {
            CheckAddItems( 1 );
        }

        [Test]
        public void AddTenItems()
        {
            CheckAddItems(10);
        }

        [Test]
        public void AddMaxItems()
        {
            CheckAddItems( _recentFiles.MaxFiles );
        }

        [Test]
        public void AddTooManyItems()
        {
            CheckAddItems( _recentFiles.MaxFiles + 5 );
        }

        [Test]
        public void AddMixedItems()
        {
            SetMockValues(5);
            _recentFiles.Latest = "30";
            _recentFiles.Latest = "20";
            _recentFiles.Latest = "10";
            CheckListContains( 10, 20, 30, 1, 2, 3, 4, 5 );
        }

        [Test]
        public void ReorderLastProject()
        {
            SetMockValues( 5 );
            _recentFiles.Latest = "5";
            CheckListContains( 5, 1, 2, 3, 4 );
        }

        [Test]
        public void ReorderSingleProject()
        {
            SetMockValues( 5 );
            _recentFiles.Latest = "3";
            CheckListContains( 3, 1, 2, 4, 5 );
        }

        [Test]
        public void ReorderMultipleProjects()
        {
            SetMockValues( 5 );
            _recentFiles.Latest = "3";
            _recentFiles.Latest = "5";
            _recentFiles.Latest = "2";
            CheckListContains( 2, 5, 3, 1, 4 );
        }

        [Test]
        public void ReorderSameProject()
        {
            SetMockValues( 5 );
            _recentFiles.Latest = "1";
            CheckListContains( 1, 2, 3, 4, 5 );
        }

        [Test]
        public void RemoveFirstProject()
        {
            SetMockValues( 3 );
            _recentFiles.Remove("1");
            CheckListContains( 2, 3 );
        }

        [Test]
        public void RemoveOneProject()
        {
            SetMockValues( 4 );
            _recentFiles.Remove("2");
            CheckListContains( 1, 3, 4 );
        }

        [Test]
        public void RemoveMultipleProjects()
        {
            SetMockValues( 5 );
            _recentFiles.Remove( "3" );
            _recentFiles.Remove( "1" );
            _recentFiles.Remove( "4" );
            CheckListContains( 2, 5 );
        }
        
        [Test]
        public void RemoveLastProject()
        {
            SetMockValues( 5 );
            _recentFiles.Remove("5");
            CheckListContains( 1, 2, 3, 4 );
        }

        // Set RecentFiles to a list of known values up
        // to a maximum. Most recent will be "1", next 
        // "2", and so on...
        private void SetMockValues(int count)
        {
            for (int num = count; num > 0; --num)
                _recentFiles.Latest = num.ToString();
        }

        // Check that the list is set right: 1, 2, ...
        private void CheckMockValues(int count)
        {
            var files = _recentFiles.Entries;
            Assert.That(files.Count, Is.EqualTo(count), "Count");

            for (int index = 0; index < count; index++)
                Assert.That(files[index], Is.EqualTo((index + 1).ToString()), "Item");
        }

        // Check that we can add count items correctly
        private void CheckAddItems(int count)
        {
            SetMockValues(count);
            Assert.That(_recentFiles.Entries[0], Is.EqualTo("1"), "RecentFile");

            CheckMockValues(Math.Min(count, _recentFiles.MaxFiles));
        }

        // Check that the list contains a set of entries
        // in the order given and nothing else.
        private void CheckListContains(params int[] item)
        {
            var files = _recentFiles.Entries;
            Assert.That(files.Count, Is.EqualTo(item.Length), "Count");

            for (int index = 0; index < files.Count; index++)
                Assert.That(files[index], Is.EqualTo(item[index].ToString()), "Item");
        }
    }
}
