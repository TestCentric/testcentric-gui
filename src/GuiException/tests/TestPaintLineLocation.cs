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
using NUnit.Framework;
using System.Drawing;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestPaintLineLocation
    {
        private PaintLineLocation _line;

        [SetUp]
        public void SetUp()
        {
            _line = new PaintLineLocation(42, "hello world!", new PointF(13, 42));
        }

        [Test]
        public void Test_PaintLineLocation()
        {
            Assert.That(_line.LineIndex, Is.EqualTo(42));
            Assert.That(_line.Text, Is.EqualTo("hello world!"));
            Assert.That(_line.Location, Is.EqualTo(new PointF(13, 42)));

            return;
        }

        //[Test]
        //[ExpectedException(typeof(ArgumentNullException),
        //    ExpectedMessage = "text",
        //    MatchType = MessageMatch.Contains)]
        //public void Test_SetText_Throws_NullTextException()
        //{
        //    new PaintLineLocation(0, null, new PointF(0, 0)); // throws exception
        //}

        [Test]
        public void Test_Equals()
        {
            Assert.That(_line.Equals(null), Is.False);
            Assert.That(_line.Equals("hello"), Is.False);
            Assert.That(_line.Equals(new PaintLineLocation(0, "", new PointF(0, 0))), Is.False);

            Assert.That(
                _line.Equals(new PaintLineLocation(_line.LineIndex, _line.Text, new PointF(0, 0))),
                Is.False);
            Assert.That(
                _line.Equals(new PaintLineLocation(_line.LineIndex, "", _line.Location)),
                Is.False);
            Assert.That(
                _line.Equals(new PaintLineLocation(0, _line.Text, _line.Location)),
                Is.False);

            Assert.That(_line.Equals(_line), Is.True);
            Assert.That(_line.Equals(
                new PaintLineLocation(_line.LineIndex, _line.Text, _line.Location)),
                Is.True);

            return;
        }
    }
}
