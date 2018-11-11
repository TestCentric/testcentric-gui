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
using NUnit.UiException.StackTraceAnalysers;
using NUnit.UiException.StackTraceAnalyzers;

namespace NUnit.UiException.Tests.StackTraceAnalyzers
{
    [TestFixture]
    public class TestIErrorParser
    {
        protected StackTraceParser _stack;
        protected IErrorParser[] _array;

        public TestIErrorParser()
        {
            _stack = new StackTraceParser();

            return;
        }

        [SetUp]
        public void SetUp()
        {
            PathCompositeParser pathParser;

            pathParser = new PathCompositeParser();

            _array = new IErrorParser[]  {
                pathParser.UnixPathParser,
                pathParser.WindowsPathParser,
                new FunctionParser(),
                new PathCompositeParser(),
                new LineNumberParser()
            };

            return;
        }

        [Test]
        public void Test_IErrorParser_Can_Throw_ParserNullException()
        {
            bool hasRaisedException;

            foreach (IErrorParser item in _array)
            {
                hasRaisedException = false;

                try
                {
                    item.TryParse(null, new RawError("test")); // throws exception
                }
                catch (Exception e)
                {
                    Assert.That(e.Message.Contains("parser"), Is.True);
                    hasRaisedException = true;
                }

                Assert.That(hasRaisedException, Is.True,
                    item.ToString() + " failed to raise exception");
            }

            return;
        }

        [Test]
        public void Test_IErrorParser_Can_Throw_ArgsNullException()
        {
            bool hasRaisedException;

            foreach (IErrorParser item in _array)
            {
                hasRaisedException = false;

                try
                {
                    item.TryParse(_stack, null); // throws exception
                }
                catch (Exception e)
                {
                    Assert.That(e.Message.Contains("args"), Is.True);
                    hasRaisedException = true;
                }

                Assert.That(hasRaisedException, Is.True,
                    item.ToString() + " failed to raise exception");
            }

            return;
        }

        public RawError AcceptValue(IErrorParser parser, string error)
        {
            RawError res;

            res = new RawError(error);
            Assert.That(parser.TryParse(_stack, res), Is.True, "Failed to parse \"{0}\"", error);

            return (res);
        }

        public void RejectValue(IErrorParser parser, string error)
        {
            Assert.That(parser.TryParse(_stack, new RawError(error)), Is.False);
        }
    }
}
