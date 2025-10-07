using System;
using Game.Core.Economy.Balancing;
using Game.Core.SaveSystem;
using Game.Core.Telemetry;

namespace Game.Core.Economy
{
    public class OfflineEarningsService
    {
        private readonly IncomeService _incomeService;
        private readonly TimeService _timeService;
        private readonly RemoteConfigService _remoteConfigService;
        private readonly AnalyticsService _analyticsService;

        public OfflineEarningsService(
            IncomeService incomeService,
            TimeService timeService,
            RemoteConfigService remoteConfigService,
            AnalyticsService analyticsService)
        {
            _incomeService = incomeService;
            _timeService = timeService;
            _remoteConfigService = remoteConfigService;
            _analyticsService = analyticsService;
        }

        public BigNumber CalculateOfflineEarnings(SaveData saveData)
        {
            var lastQuit = DateTimeOffset.FromUnixTimeSeconds(saveData.LastQuit).UtcDateTime;
            var lastUptime = TimeSpan.FromSeconds(saveData.LastSavedUptime);
            var tampered = _timeService.CheckForTampering(lastQuit, lastUptime, out var delta);

            if (tampered)
            {
                _analyticsService.LogOfflineEarnings(0, 0, true);
                return new BigNumber(0);
            }

            var hoursAway = delta.TotalHours;
            var maxHours = _remoteConfigService.GetDouble(RemoteConfigKeys.MaxOfflineHours, 10d);
            var factor = _remoteConfigService.GetDouble(RemoteConfigKeys.OfflineFactor, 0.7d);
            var clampedHours = Math.Min(hoursAway, maxHours);
            var earnings = _incomeService.CalculateIncomePerSecond().RawValue * clampedHours * 3600d * factor;
            _analyticsService.LogOfflineEarnings(hoursAway, earnings, clampedHours < hoursAway);
            return new BigNumber(earnings);
        }
    }
}
