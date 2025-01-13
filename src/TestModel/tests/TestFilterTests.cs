// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Framework;

namespace TestCentric.Gui.Model
{
    [TestFixture]
    internal class TestFilterTests
    {
        [Test]
        public void MakeVisibleIdFilter_NoSelectedNodes_ReturnsFilter()
        {
            // Arrange
            var testNodes = new List<TestNode>();

            // Act
            TestFilter filter = TestFilter.MakeVisibleIdFilter(testNodes);

            // Assert
            Assert.That(filter.XmlText, Is.EqualTo("<filter><or></or></filter>"));
        }

        [Test]
        public void MakeVisibleIdFilter_AllTestNodesVisible_ReturnsFilter()
        {
            // Arrange
            var testNodes = new List<TestNode>()
            {
                new TestNode("<test-case id='1' />"),
                new TestNode("<test-case id='2' />"),
            };

            // Act
            TestFilter filter = TestFilter.MakeVisibleIdFilter(testNodes);

            // Assert
            Assert.That(filter.XmlText, Is.EqualTo("<filter><or><id>1</id><id>2</id></or></filter>"));
        }

        [Test]
        public void MakeVisibleIdFilter_TestNodesNotVisible_ReturnsFilter()
        {
            // Arrange
            var testNodes = new List<TestNode>()
            {
                new TestNode("<test-case id='1' />") { IsVisible = false },
                new TestNode("<test-case id='2' />") { IsVisible = true },
            };

            // Act
            TestFilter filter = TestFilter.MakeVisibleIdFilter(testNodes);

            // Assert
            Assert.That(filter.XmlText, Is.EqualTo("<filter><or><id>2</id></or></filter>"));
        }

        private static object[] MakeVisibleIdFilter =
{
            new object[] { new[] { "3-1000", "3-1100", "3-1110", "3-1111" }, new[] { "3-1111" }, new[] { "3-1000", "3-1100", "3-1110", "3-1112" } },
            new object[] { new[] { "3-1000", "3-1200", "3-1210", "3-1211", "3-1212" }, new[] { "3-1211", "3-1211" }, new[] { "3-1000", "3-1100", "3-1111", "3-1112", "3-1210" } },
            new object[] { new[] { "3-1000", "3-1300", "3-1310", "3-1312", "3-1320", "3-1322" }, new[] { "3-1312", "3-1322" }, new[] { "3-1000", "3-1300", "3-1310", "3-1311", "3-1321" } },
            new object[] { new[] { "3-1000", "3-1300", "3-1320", "3-1322" }, new[] { "3-1322" }, new[] { "3-1000", "3-1300", "3-1310", "3-1311", "3-1312", "3-1321" } },
        };

        [Test]
        [TestCaseSource(nameof(MakeVisibleIdFilter))]
        public void MakeVisibleIdFilter_TestNodesHierarchie_ReturnsFilter(IList<string> visibleNodeIds, IList<string> expectedIds, IList<string> expectedIdsNotInFilter)
        {
            // Arrange
            TestNode testNode = new TestNode(
                CreateTestSuiteXml("3-1000", 
                    CreateTestSuiteXml("3-1100", 
                        CreateTestFixtureXml("3-1110", 
                            CreateTestcaseXml("3-1111"),
                            CreateTestcaseXml("3-1112"))) +
                    CreateTestSuiteXml("3-1200", 
                        CreateTestFixtureXml("3-1210", 
                            CreateTestcaseXml("3-1211"),
                            CreateTestcaseXml("3-1212"))),
                    CreateTestSuiteXml("3-1300", 
                        CreateTestFixtureXml("3-1310", 
                            CreateTestcaseXml("3-1311"),
                            CreateTestcaseXml("3-1312")) +
                        CreateTestFixtureXml("3-1320",
                            CreateTestcaseXml("3-1321"),
                            CreateTestcaseXml("3-1322")))));

            SetVisibleNodes(testNode, visibleNodeIds);

            // Act
            TestFilter filter = TestFilter.MakeVisibleIdFilter(new[] { testNode });

            // Assert
            string xmlText = filter.XmlText;

            foreach (string id in expectedIds)
                Assert.That(xmlText, Does.Contain($"<id>{id}</id>"));

            foreach (string id in expectedIdsNotInFilter)
                Assert.That(xmlText, Does.Not.Contain($"{id}"));
        }

        private void SetVisibleNodes(TestNode testNode, IList<string> visibleNodeIds)
        {
            testNode.IsVisible = visibleNodeIds.Contains(testNode.Id);

            foreach(TestNode childNode in testNode.Children)
                SetVisibleNodes(childNode, visibleNodeIds);
        }

        private string CreateTestcaseXml(string testId)
        {
            string str = $"<test-case id='{testId}' /> ";
            return str;
        }

        private string CreateTestFixtureXml(string testId, params string[] testCases)
        {
            string str = $"<test-suite type='TestFixture' id='{testId}' > ";

            foreach (string testCase in testCases)
                str += testCase;

            str += "</test-suite>";

            return str;
        }

        private string CreateTestSuiteXml(string testId, params string[] testSuites)
        {
            string str = $"<test-suite type='TestSuite' id='{testId}' > ";
            foreach (string testSuite in testSuites)
                str += testSuite;

            str += "</test-suite>";

            return str;
        }
    }
}
