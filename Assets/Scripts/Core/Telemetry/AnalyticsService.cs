using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Telemetry
{
    /// <summary>
    /// Thin wrapper around Firebase Analytics. Stubs are used to keep compilation without the SDK.
    /// </summary>
    public class AnalyticsService
    {
        public void LogEvent(string key, Dictionary<string, object> parameters = null)
        {
            // TODO: Hook up Firebase Analytics
#if UNITY_EDITOR || DEV
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    Debug.Log($"Analytics Event {key} {kvp.Key}:{kvp.Value}");
                }
            }
            else
            {
                Debug.Log($"Analytics Event {key}");
            }
#endif
        }

        public void LogSessionStart()
        {
            LogEvent(AnalyticsKeys.SessionStart);
        }

        public void LogUpgradePurchase(string id, int level, double cost)
        {
            LogEvent(AnalyticsKeys.UpgradePurchase, new Dictionary<string, object>
            {
                {"id", id },
                {"level", level },
                {"cost", cost }
            });
        }

        public void LogTap(double value)
        {
            LogEvent(AnalyticsKeys.Tap, new Dictionary<string, object>
            {
                {"value", value }
            });
        }

        public void LogIncomeTick(double incomePerSecond)
        {
            LogEvent(AnalyticsKeys.IncomeTick, new Dictionary<string, object>
            {
                {"income_ps", incomePerSecond }
            });
        }

        public void LogPrestige(int shards, int totalShards, double lifetimeCoins)
        {
            LogEvent(AnalyticsKeys.Prestige, new Dictionary<string, object>
            {
                {"shards_gained", shards },
                {"total_shards", totalShards },
                {"lifetime_coins", lifetimeCoins }
            });
        }

        public void LogAdReward(string type, double value, double cooldown)
        {
            LogEvent(AnalyticsKeys.AdReward, new Dictionary<string, object>
            {
                {"type", type },
                {"value", value },
                {"cooldown_remaining", cooldown }
            });
        }

        public void LogIAPPurchase(string productId, string price, string currency)
        {
            LogEvent(AnalyticsKeys.IAPPurchase, new Dictionary<string, object>
            {
                {"product_id", productId },
                {"price", price },
                {"iso_currency", currency }
            });
        }

        public void LogOfflineEarnings(double hours, double earnings, bool clamped)
        {
            LogEvent(AnalyticsKeys.OfflineEarnings, new Dictionary<string, object>
            {
                {"hours", hours },
                {"earnings", earnings },
                {"clamped_by_cap", clamped }
            });
        }

        public void LogRemoteConfigFetch(bool success, long latencyMs)
        {
            LogEvent(AnalyticsKeys.RemoteConfigFetch, new Dictionary<string, object>
            {
                {"success", success },
                {"latency_ms", latencyMs }
            });
        }
    }
}
