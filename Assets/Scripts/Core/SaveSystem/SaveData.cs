using System;
using System.Collections.Generic;

namespace Game.Core.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        [Serializable]
        public class UpgradeState
        {
            public string Id;
            public int Level;
        }

        public int Version = 1;
        public double Coins;
        public int Gems;
        public double TotalLifetimeCoins;
        public List<UpgradeState> Upgrades = new();
        public int Shards;
        public double DoubleIncomeRemaining;
        public long LastQuit;
        public long LastTick;
        public int AdShardsClaimedToday;
        public bool RemoveAdsOwned;
        public long MonthlyBoosterExpiryUtc;
        public long LastSavedTime;
        public double LastSavedUptime;
        public bool TimeTamperFlag;
    }
}
