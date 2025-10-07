using Game.Core.Economy.Balancing;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class CostCurveTests
    {
        [Test]
        public void Evaluate_IsMonotonic()
        {
            var curve = new CostCurve { StartCost = 10, Growth = 1.1 };
            Assert.Greater(curve.Evaluate(2), curve.Evaluate(1));
        }

        [Test]
        public void Evaluate_LevelZero_EqualsStart()
        {
            var curve = new CostCurve { StartCost = 5, Growth = 1.2 };
            Assert.AreEqual(5, curve.Evaluate(0));
        }
    }
}
