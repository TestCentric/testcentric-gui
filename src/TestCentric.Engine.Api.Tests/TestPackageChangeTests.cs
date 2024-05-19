// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestCentric.Engine
{
    public class TestPackageChangeTests
    {
        private TestPackage _package;
        private int _changeCount = 0;

        [SetUp]
        public void SetUp() 
        {
            _changeCount = 0;
            _package = new TestPackage("test1.dll", "test2.dll");
            _package.AddSetting("SomeSetting", 123);
            _package.Changed += (s,e) => _changeCount++;
        }

        [Test]
        public void AddSubPackage()
        {
            _package.AddSubPackage("test3.dll");
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        public void RemoveSubPackage()
        {
            _package.RemoveSubPackage("test2.dll");
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void AddSetting()
        {
            _package.AddSetting("NewSetting", "VALUE");
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void AddSettingDirectly()
        {
            _package.Settings.Add("NewSetting", "VALUE");
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void RemoveSetting()
        {
            _package.Settings.Remove("SomeSetting");
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void ChangeSetting()
        {
            _package.Settings["SomeSetting"] = 456;
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void ChangeSubPackageSetting()
        {
            _package.SubPackages[0].Settings["SomeSetting"] = 456;
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void AddSecondLevelSubPackage()
        {
            _package.SubPackages[0].AddSubPackage("another.dll");
            Assert.That(_changeCount, Is.EqualTo(2));
        }

#if !NET20 && !NET35
        [Test]
        public void AddSubPackageDirectly()
        {
            _package.SubPackages.Add(new TestPackage("test3.dll"));
            Assert.That(_changeCount, Is.EqualTo(1));
        }

        [Test]
        public void RemoveSubPackageDirectly()
        {
            _package.SubPackages.RemoveAt(1);
            Assert.That(_changeCount, Is.EqualTo(1));
        }
#endif
    }
}
