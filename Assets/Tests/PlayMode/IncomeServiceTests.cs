using Game.Core.Economy;
using Game.Core.Economy.UpgradeSystem;
using Game.Core.Telemetry;
using NUnit.Framework;

namespace Tests.PlayMode
{
    public class IncomeServiceTests
    {
        [Test]
        public void Income_Increases_WithUpgrades()
        {
            var currency = new CurrencyService();
            var analytics = new AnalyticsService();
            var remote = new RemoteConfigService();
            remote.InitializeAsync().Wait();
            var prestige = new PrestigeService(currency, analytics);
            var def = new UpgradeDefinition { Id = UpgradeIds.Income, BaseEffect = 1, EffectExponentA = 1 };
            var defs = new[] { def, new UpgradeDefinition { Id = UpgradeIds.Tap } };
            var upgrades = new UpgradeService(defs, currency);
            var income = new IncomeService(upgrades, currency, prestige, remote, analytics);

            var baseIncome = income.CalculateIncomePerSecond().RawValue;
            upgrades.SetLevel(UpgradeIds.Income, 10);
            var improvedIncome = income.CalculateIncomePerSecond().RawValue;
            Assert.Greater(improvedIncome, baseIncome);
        }
    }
}
