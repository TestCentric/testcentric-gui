// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Engine;
using NUnit.Framework;
using NSubstitute;
using TestCentric.Gui.Model.Fakes;

#if false // Not yet working
namespace TestCentric.Gui.Model
{
    public class TestExecutionTests 
    {
        private static TestNode TEST_CASE_NODE = new TestNode(
            XmlHelper.CreateXmlNode("<test-case id='1234' name='SomeTest' />"));
        private static TestNode TEST_SUITE_NODE = new TestNode(XmlHelper.CreateXmlNode("<test-suite/>"));

        [Test]
        public void RunSingleTest()
        {
            var engine = Substitute.For<ITestEngine>();
            var runner = Substitute.For<ITestRunner>();
            engine.GetRunner(null).ReturnsForAnyArgs(runner);
            Assert.That(engine.GetRunner(new TestPackage()), Is.SameAs(runner));

            var model = new TestModel(engine);

            model.RunTests(TEST_CASE_NODE);
            //runner.Received().RunAsync(model.Events as ITestEventListener, NUnit.Engine.TestFilter.Empty);
        }
    }
}
#endif
