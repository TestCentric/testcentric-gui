// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.UiException.Tests.data;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestErrorItemCollection
    {
        TestResource _resourceA;
        TestResource _resourceB;

        private ErrorItemCollection _items;
        private ErrorItem _itemA;
        private ErrorItem _itemB;

        [SetUp]
        public void SetUp()
        {
            _items = new InternalTraceItemCollection();

            _resourceA = new TestResource("HelloWorld.txt");
            _resourceB = new TestResource("TextCode.txt");

            _itemA = new ErrorItem(_resourceA.Path, 1);
            _itemB = new ErrorItem(_resourceB.Path, 2);

            return;
        }

        [TearDown]
        public void TearDown()
        {
            if (_resourceA != null)
            {
                _resourceA.Dispose();
                _resourceA = null;
            }

            if (_resourceB != null)
            {
                _resourceB.Dispose();
                _resourceB = null;
            }
        }

        [Test]
        public void Test_TraceItems()
        {
            List<ErrorItem> lst;

            Assert.That(_items.Count, Is.EqualTo(0));

            _items.Add(_itemA);
            _items.Add(_itemB);

            Assert.That(_items.Count, Is.EqualTo(2));

            Assert.That(_items[0], Is.EqualTo(_itemA));
            Assert.That(_items[1], Is.EqualTo(_itemB));

            lst = new List<ErrorItem>();
            foreach (ErrorItem item in _items)
                lst.Add(item);
            Assert.That(lst.Count, Is.EqualTo(2));
            Assert.That(lst[0], Is.EqualTo(_items[0]));
            Assert.That(lst[1], Is.EqualTo(_items[1]));

            return;
        }

        //[Test]
        //[ExpectedException(typeof(ArgumentNullException),
        //    ExpectedMessage = "item",
        //    MatchType = MessageMatch.Contains)]
        //public void Test_Add_Throws_NullItemException()
        //{
        //    _items.Add(null); // throws exception
        //}

        [Test]
        public void Test_Clear()
        {
            _items.Add(_itemA);

            Assert.That(_items.Count, Is.EqualTo(1));
            _items.Clear();
            Assert.That(_items.Count, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Contains()
        {
            Assert.That(_items.Contains(null), Is.False);
            Assert.That(_items.Contains(_itemA), Is.False);

            _items.Add(_itemA);

            Assert.That(_items.Contains(_itemA), Is.True);

            return;
        }

        #region InternalTraceItemCollection

        class InternalTraceItemCollection :
            ErrorItemCollection
        {
            public InternalTraceItemCollection()
            {
                // nothing to do
            }
        }

        #endregion
    }
}
