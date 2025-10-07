using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Telemetry
{
    /// <summary>
    /// Provides remote configuration values backed by Firebase Remote Config.
    /// Currently implemented as a stub with async simulation.
    /// </summary>
    public class RemoteConfigService
    {
        private readonly Dictionary<string, object> _values = new();
        private bool _initialized;

        public async Task InitializeAsync()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                await Task.Delay(200); // Simulate async fetch
                ApplyDefaults();
                _initialized = true;
                stopwatch.Stop();
                Debug.Log($"RemoteConfig initialized in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"RemoteConfig fetch failed: {ex.Message}");
            }
        }

        private void ApplyDefaults()
        {
            Set(RemoteConfigKeys.IncomeBase, 1d);
            Set(RemoteConfigKeys.GrowthIncome, 1.12d);
            Set(RemoteConfigKeys.GrowthTap, 1.10d);
            Set(RemoteConfigKeys.GrowthOffline, 1.13d);
            Set(RemoteConfigKeys.OfflineFactor, 0.7d);
            Set(RemoteConfigKeys.MaxOfflineHours, 10d);
            Set(RemoteConfigKeys.MilestoneEveryLevels, 25);
            Set(RemoteConfigKeys.MilestoneBonusPercent, 15d);
            Set(RemoteConfigKeys.RewardedCooldownMin, 10d);
            Set(RemoteConfigKeys.RewardedShardDailyCap, 3);
            Set(RemoteConfigKeys.GlobalIncomeMultiplier, 1d);
            Set(RemoteConfigKeys.AbFeatureInterstitials, false);
            Set(RemoteConfigKeys.AbBoosterPercent, 10d);
            Set(RemoteConfigKeys.OfferFirstSessionDelay, 420);
        }

        private void Set(string key, object value)
        {
            _values[key] = value;
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                ApplyDefaults();
            }
        }

        public double GetDouble(string key, double defaultValue)
        {
            EnsureInitialized();
            if (_values.TryGetValue(key, out var value) && value is double doubleValue)
            {
                return doubleValue;
            }

            if (_values.TryGetValue(key, out value) && value is float floatValue)
            {
                return floatValue;
            }

            if (_values.TryGetValue(key, out value) && value is int intValue)
            {
                return intValue;
            }

            return defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            EnsureInitialized();
            if (_values.TryGetValue(key, out var value))
            {
                if (value is int intValue)
                {
                    return intValue;
                }

                if (value is double doubleValue)
                {
                    return (int)Math.Round(doubleValue);
                }
            }

            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            EnsureInitialized();
            if (_values.TryGetValue(key, out var value) && value is bool boolValue)
            {
                return boolValue;
            }

            return defaultValue;
        }
    }
}
