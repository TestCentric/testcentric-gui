// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

using NSubstitute;

using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Model;

    public class TestPropertiesPresenterTests
    {
        [TestCase("Assembly", "test.dll", "Runnable", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.dll", "NotRunnable", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.dll", "Ignored", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.dll", "Explicit", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.dll", "Skipped", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.dll", "Unknown", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "Runnable", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "NotRunnable", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "Ignored", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "Explicit", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "Skipped", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.exe", "Unknown", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.png", "Runnable", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.png", "NotRunnable", ExpectedResult = "Unknown")]
        [TestCase("Assembly", "test.png", "Ignored", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.png", "Explicit", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.png", "Skipped", ExpectedResult = "Assembly")]
        [TestCase("Assembly", "test.png", "Unknown", ExpectedResult = "Assembly")]
        [TestCase("SomethingElse", "test.png", "Runnable", ExpectedResult = "SomethingElse")]
        [TestCase("SomethingElse", "test.png", "NotRunnable", ExpectedResult = "SomethingElse")]
        [TestCase("SomethingElse", "test.dll", "Runnable", ExpectedResult = "SomethingElse")]
        [TestCase("SomethingElse", "test.dll", "NotRunnable", ExpectedResult = "SomethingElse")]
        public string GetTestTypeTest(string type, string fullname, string runstate)
        {
            TestNode testNode = new TestNode(XmlHelper.CreateXmlNode(string.Format("<test-suite id='1' type='{0}' fullname='{1}' runstate='{2}'><test-suite id='42'/></test-suite>", type, fullname, runstate)));

            return TestPropertiesPresenter.GetTestType(testNode);
        }

    }
}
