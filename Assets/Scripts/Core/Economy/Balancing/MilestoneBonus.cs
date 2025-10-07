using System;

namespace Game.Core.Economy.Balancing
{
    /// <summary>
    /// Defines milestone bonus thresholds and percentage boost.
    /// </summary>
    [Serializable]
    public class MilestoneBonus
    {
        public int EveryLevels = 25;
        public double PercentBonus = 15d;

        public double EvaluateMultiplier(int level)
        {
            if (EveryLevels <= 0)
            {
                return 1d;
            }

            var milestonesReached = level / EveryLevels;
            return 1d + milestonesReached * (PercentBonus / 100d);
        }
    }
}
