using Game.Core;
using Game.Core.Economy;
using Game.Core.SaveSystem;
using Game.Core.Telemetry;
using NUnit.Framework;

namespace Tests.PlayMode
{
    public class OfflineEarningsTests
    {
        [Test]
        public void OfflineEarnings_AreCapped()
        {
            var currency = new CurrencyService();
            var analytics = new AnalyticsService();
            var remote = new RemoteConfigService();
            remote.InitializeAsync().Wait();
            var prestige = new PrestigeService(currency, analytics);
            var upgrades = new UpgradeService(System.Array.Empty<UpgradeDefinition>(), currency);
            var income = new IncomeService(upgrades, currency, prestige, remote, analytics);
            var time = new TimeService();
            var offline = new OfflineEarningsService(income, time, remote, analytics);
            var save = new SaveData { LastQuit = System.DateTimeOffset.UtcNow.AddHours(-100).ToUnixTimeSeconds() };
            var earnings = offline.CalculateOfflineEarnings(save);
            Assert.LessOrEqual(earnings.RawValue, remote.GetDouble(RemoteConfigKeys.MaxOfflineHours, 10) * 3600);
        }
    }
}
