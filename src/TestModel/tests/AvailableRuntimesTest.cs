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
using NUnit.Engine;
using NUnit.Framework;
using NUnit.TestUtilities.Fakes;
using RuntimeFramework = NUnit.TestUtilities.Fakes.RuntimeFramework;

namespace TestCentric.Gui.Model
{
    public class AvailableRuntimesTest
    {
        [Test]
        public void RuntimesSupportedByEngineAreAvailable()
        {
            var mockEngine = new MockTestEngine().WithRuntimes(
                new RuntimeFramework("net-4.5", new Version(4, 5)),
                new RuntimeFramework("net-4.0", new Version(4, 0)));

            var model = new TestModel(mockEngine);

            Assert.That(model.AvailableRuntimes.Count, Is.EqualTo(2));
            Assert.That(model.AvailableRuntimes, Has.One.Property("Id").EqualTo("net-4.5"));
            Assert.That(model.AvailableRuntimes, Has.One.Property("Id").EqualTo("net-4.0"));
        }
    }
}
