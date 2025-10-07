using System;
using Game.Core.SaveSystem;
using Game.Core.Telemetry;

namespace Game.Core.Economy
{
    public class PrestigeService
    {
        private readonly CurrencyService _currencyService;
        private readonly AnalyticsService _analyticsService;
        private int _totalShards;
        private double _lifetimeCoins;

        public PrestigeService(CurrencyService currencyService, AnalyticsService analyticsService)
        {
            _currencyService = currencyService;
            _analyticsService = analyticsService;
        }

        public void LoadFromSave(SaveData save)
        {
            _totalShards = save.Shards;
            _lifetimeCoins = save.TotalLifetimeCoins;
        }

        public void TrackLifetimeEarnings(double coins)
        {
            _lifetimeCoins += coins;
        }

        public double GetPrestigeMultiplier()
        {
            return 1d + 0.5d * _totalShards;
        }

        public int CalculateShardsToGain()
        {
            return (int)Math.Floor(Math.Log10(_lifetimeCoins + 1));
        }

        public void ApplyPrestige()
        {
            int shards = CalculateShardsToGain();
            _totalShards += shards;
            _currencyService.SetSoft(CurrencyType.Coins, 0);
            _analyticsService.LogPrestige(shards, _totalShards, _lifetimeCoins);
        }

        public int GetTotalShards() => _totalShards;
        public double GetLifetimeCoins() => _lifetimeCoins;
    }
}
