using System;
using System.Threading.Tasks;
using Game.Core.Telemetry;
using UnityEngine;

namespace Game.Core.Monetization.Ads
{
    public enum RewardedAdType
    {
        DoubleIncome,
        Shard
    }

    public class AdService
    {
        private readonly AdConfigSO _config;
        private readonly AnalyticsService _analyticsService;
        private readonly RemoteConfigService _remoteConfigService;
        private DateTime _lastRewarded;
        private int _shardRewardsToday;

        public event Action<RewardedAdType> RewardGranted;

        public AdService(AdConfigSO config, AnalyticsService analyticsService, RemoteConfigService remoteConfigService)
        {
            _config = config;
            _analyticsService = analyticsService;
            _remoteConfigService = remoteConfigService;
        }

        public bool CanShow(RewardedAdType type)
        {
            var cooldown = TimeSpan.FromMinutes(_remoteConfigService.GetDouble(RemoteConfigKeys.RewardedCooldownMin, 10));
            if (type == RewardedAdType.Shard)
            {
                var cap = _remoteConfigService.GetInt(RemoteConfigKeys.RewardedShardDailyCap, 3);
                if (_shardRewardsToday >= cap)
                {
                    return false;
                }
            }

            return DateTime.UtcNow - _lastRewarded > cooldown;
        }

        public async Task ShowRewardedAsync(RewardedAdType type)
        {
            if (!CanShow(type))
            {
                Debug.Log("Ad not ready");
                return;
            }

            await Task.Delay(1000); // Simulate ad
            GrantReward(type);
        }

        private void GrantReward(RewardedAdType type)
        {
            _lastRewarded = DateTime.UtcNow;
            if (type == RewardedAdType.Shard)
            {
                _shardRewardsToday++;
            }

            RewardGranted?.Invoke(type);
            _analyticsService.LogAdReward(type.ToString().ToLowerInvariant(), 1, 0);
        }
    }
}
