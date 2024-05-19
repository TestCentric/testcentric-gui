// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCentric.Engine
{
    public class SettingsDictionaryChangeTests
    {
        private SettingsDictionary _settings;
        private int _changeCount = 0;

        [SetUp]
        public void SetUp() 
        {
            _changeCount = 0;
            _settings = new SettingsDictionary();
            _settings.Add("SomeKey", 123);
            _settings.Changed += (s, e) => _changeCount++;
        }

        [Test]
        public void Add()
        {
            _settings.Add("NewKey", "VALUE");
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void AddKeyValuePair()
        {
            _settings.Add(new KeyValuePair<string, object>("NewKey", "VALUE"));
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void Remove()
        {
            _settings.Remove("SomeKey");
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void RemoveKeyValuePair()
        {
            _settings.Remove(new KeyValuePair<string, object>("SomeKey", 123));
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void SetNewItem()
        {
            _settings["NewKey"] = "VALUE";
            Assert.That(_changeCount == 1);
        }

        [Test]
        public void SetItemToNewValue()
        {
            _settings["SomeKey"] = 456;
            Assert.That(_changeCount == 1);
        }
    }
}
