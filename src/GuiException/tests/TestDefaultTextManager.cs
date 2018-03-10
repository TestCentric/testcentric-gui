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

using System.Collections.Generic;
using NUnit.Framework;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestDefaultTextManager
    {
        private DefaultTextManager _textBlocks;

        [SetUp]
        public void SetUp()
        {
            _textBlocks = new DefaultTextManager();            
        }       

        [Test]
        public void Test_Default()
        {
            Assert.That(_textBlocks.Text, Is.EqualTo(""));
            Assert.That(_textBlocks.LineCount, Is.EqualTo(0));
            Assert.That(_textBlocks.MaxLength, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_CodeBlockCollection()
        {
            List<string> lst;

            Assert.That(_textBlocks.LineCount, Is.EqualTo(0));

            _textBlocks.Text = "01\r\n02\r\n03\r\n";

            Assert.That(_textBlocks.Text, Is.EqualTo("01\r\n02\r\n03\r\n"));
            Assert.That(_textBlocks.LineCount, Is.EqualTo(3));
            Assert.That(_textBlocks.GetTextAt(0), Is.EqualTo("01"));
            Assert.That(_textBlocks.GetTextAt(1), Is.EqualTo("02"));
            Assert.That(_textBlocks.GetTextAt(2), Is.EqualTo("03"));

            _textBlocks.Text = "01";
            Assert.That(_textBlocks.LineCount, Is.EqualTo(1));
            Assert.That(_textBlocks.GetTextAt(0), Is.EqualTo("01"));

            _textBlocks.Text = "01\r\n02";
            Assert.That(_textBlocks.LineCount, Is.EqualTo(2));
            Assert.That(_textBlocks.GetTextAt(0), Is.EqualTo("01"));
            Assert.That(_textBlocks.GetTextAt(1), Is.EqualTo("02"));

            lst = new List<string>();
            foreach (string line in _textBlocks)
                lst.Add(line);
            Assert.That(lst.Count, Is.EqualTo(2));
            Assert.That(lst[0], Is.EqualTo(_textBlocks.GetTextAt(0)));
            Assert.That(lst[1], Is.EqualTo(_textBlocks.GetTextAt(1)));

            _textBlocks.Text = null;
            Assert.That(_textBlocks.Text, Is.EqualTo(""));            

            return;
        }

        [Test]
        public void Test_MaxLength()
        {
            _textBlocks.Text = null;
            Assert.That(_textBlocks.MaxLength, Is.EqualTo(0));

            _textBlocks.Text = "a\r\nabc\r\nab";
            Assert.That(_textBlocks.MaxLength, Is.EqualTo(3));

            _textBlocks.Text = "a\r\nab\r\nabc";
            Assert.That(_textBlocks.MaxLength, Is.EqualTo(3));

            return;
        }
    }
}
