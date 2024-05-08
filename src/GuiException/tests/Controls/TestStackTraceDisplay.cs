// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests.Controls
{
    [TestFixture]
    public class TestStackTraceDisplay
    {
        private StackTraceDisplay _traceDisplay;

        [SetUp]
        public void SetUp()
        {
            _traceDisplay = new StackTraceDisplay();
        }

        [Test]
        public void DefaultState()
        {
            Assert.That(_traceDisplay.PluginItem, Is.Not.Null);
            Assert.That(_traceDisplay.PluginItem.Text, Is.EqualTo("Display actual stack trace"));
            Assert.That(_traceDisplay.OptionItems, Is.Not.Null);
            Assert.That(_traceDisplay.OptionItems.Length, Is.EqualTo(1));
            Assert.That(_traceDisplay.OptionItems[0].Text, Is.EqualTo("Copy stack trace to clipboard"));

            Assert.That(_traceDisplay.Content, Is.Not.Null);
            Assert.That(_traceDisplay.Content, Is.TypeOf(typeof(TextBox)));
            TextBox text = _traceDisplay.Content as TextBox;
            Assert.That(text.Text, Is.EqualTo(""));
            Assert.That(text.ReadOnly, Is.True);
            Assert.That(text.Multiline, Is.True);

            return;
        }

        [Test]
        public void OnStackTraceChanged()
        {
            string trace_1 =
                    "à System.ThrowHelper.ThrowArgumentException(ExceptionResource resource)\r\n" +
                    "à System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)\r\n" +
                    "à System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)\r\n" +
                    "à NUnit.UiException.Tests.MockHelper.Two_Mocks_In_Dictionary() dans C:\\folder\\file1.cs:ligne 87\r\n";

            string trace_2 = "";
            TextBox content = _traceDisplay.Content as TextBox;

            _traceDisplay.OnStackTraceChanged(trace_1);
            Assert.That(content.Text, Is.EqualTo(trace_1));

            _traceDisplay.OnStackTraceChanged(trace_2);
            Assert.That(content.Text, Is.EqualTo(trace_2));

            // passing null should not cause error

            _traceDisplay.OnStackTraceChanged(null);
            Assert.That(content.Text, Is.EqualTo(""));

            return;
        }

        [TestCase("hi, there!", true)]
        [TestCase("", false)]
        [Apartment(ApartmentState.STA)]
        public void CopyToClipBoard(string content, bool containsText)
        {
            Clipboard.Clear();

            _traceDisplay.OnStackTraceChanged(content);
            _traceDisplay.CopyToClipBoard();

            Assert.That(Clipboard.ContainsText(), Is.EqualTo(containsText));
            Assert.That(Clipboard.GetText(), Is.EqualTo(content));
        }

        [Test]
        public void FeedingDisplayWithGarbageDoesNotMakeItCrash()
        {
            _traceDisplay.OnStackTraceChanged(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\r\n" +
                "Nam at nisi ut neque sollicitudin ultrices. Sed rhoncus\r\n" +
                "rhoncus arcu. Morbi eu elit ut augue congue luctus. Nullam\r\n" +
                "eu eros. Nunc blandit varius orci. Mauris condimentum diam\r\n" +
                "ac ligula. Nullam ut metus. Maecenas sagittis nibh in nisl.\r\n" +
                "Phasellus rhoncus diam a nulla. Integer vestibulum.\r\n");

            TextBox text = _traceDisplay.Content as TextBox;
            Assert.That(text.Text, Is.Not.Null);

            return;
        }

    }
}
