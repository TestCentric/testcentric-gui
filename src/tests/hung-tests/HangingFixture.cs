using System.Threading;
using NUnit.Framework;

namespace NUnit.Tests
{
    public class HangingFixture
    {
        [Test]
        public void HangingTest()
        {
            while (true)
                Thread.Sleep(5000);
        }
    }
}
