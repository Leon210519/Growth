using Game.Core.Economy.Balancing;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class BigNumberTests
    {
        [Test]
        public void Format_ThousandBoundary_RoundsUp()
        {
            var result = BigNumber.Format(999900, 1);
            Assert.AreEqual("1.0M", result);
        }

        [Test]
        public void Compare_Works()
        {
            var a = new BigNumber(5);
            var b = new BigNumber(10);
            Assert.IsTrue(b > a);
        }
    }
}
