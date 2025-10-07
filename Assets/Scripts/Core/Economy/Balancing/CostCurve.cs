using System;

namespace Game.Core.Economy.Balancing
{
    /// <summary>
    /// Exponential cost curve used by upgrades and prestige calculations.
    /// </summary>
    [Serializable]
    public class CostCurve
    {
        public double StartCost = 1d;
        public double Growth = 1.1d;

        public double Evaluate(int level)
        {
            if (level < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(level));
            }

            return StartCost * Math.Pow(Growth, level);
        }
    }
}
