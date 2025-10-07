using Game.Core.Economy;
using Game.Core.Telemetry;
using NUnit.Framework;

namespace Tests.PlayMode
{
    public class PrestigeServiceTests
    {
        [Test]
        public void Shards_Calculation_UsesLog10()
        {
            var currency = new CurrencyService();
            var analytics = new AnalyticsService();
            var prestige = new PrestigeService(currency, analytics);
            prestige.TrackLifetimeEarnings(1_000_000);
            var shards = prestige.CalculateShardsToGain();
            Assert.AreEqual((int)System.Math.Floor(System.Math.Log10(1_000_000 + 1)), shards);
        }
    }
}
